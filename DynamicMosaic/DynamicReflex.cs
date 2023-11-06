using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynamicParser;
using DynamicProcessor;
using Processor = DynamicParser.Processor;
using Region = DynamicParser.Region;

namespace DynamicMosaic
{
    /// <summary>
    ///     Служит для поиска данных на карте и сохранения результатов поиска.
    /// </summary>
    public sealed class DynamicReflex
    {
        /// <summary>
        ///     Служит для контроля содержимого поисковых запросов.
        /// </summary>
        /// <remarks>
        ///     Контроль нужен для того, чтобы запросы, содержащие символы, которые нельзя сопоставить с картами, содержащимися в
        ///     текущем экземпляре, не были выполнены.
        ///     Поле НЕ является потокобезопасным.
        /// </remarks>
        readonly HashSet<char> _setChars = new HashSet<char>();

        /// <summary>
        ///     Карты, с помощью которых производится выполнение поискового запроса.
        /// </summary>
        /// <remarks>
        ///     Содержит результаты выполнения последнего поискового запроса, которые будут использованы при следующем запросе.
        ///     Поле НЕ является потокобезопасным.
        /// </remarks>
        ProcessorContainer _seaProcessors;

        /// <summary>
        ///     Инициализирует текущий экземпляр <see cref="DynamicReflex" /> картами из указанного экземпляра <see cref="ProcessorContainer" />.
        /// </summary>
        /// <param name="processors">Карты, которые будут использованы при выполнении первого поискового запроса.</param>
        /// <remarks>
        ///     Внутри класса будет создана полная копия указанной коллекции карт.
        ///     В случае, если аргумент <paramref name="processors" /> равен <see langword="null" />, будет выдано исключение <see cref="ArgumentNullException" />.
        /// </remarks>
        /// <exception cref="ArgumentNullException" />
        /// <seealso cref="ProcessorContainer" />
        public DynamicReflex(ProcessorContainer processors)
        {
            if (processors == null)
                throw new ArgumentNullException(nameof(processors),
                    $"{nameof(DynamicReflex)}: Карты должны быть добавлены в контекст (null).");

            Processor[] procs = new Processor[processors.Count];

            for (int k = 0; k < procs.Length; k++)
            {
                Processor p = processors[k];
                _setChars.Add(char.ToUpper(p.Tag[0]));
                procs[k] = p;
            }

            _seaProcessors = new ProcessorContainer(procs);
        }

        /// <summary>
        ///     Возвращает карты, содержащиеся в текущем экземпляре <see cref="DynamicReflex" />.
        /// </summary>
        /// <remarks>
        ///     Карты возвращает "как есть". Копии не создаёт.
        ///     Свойство НЕ потокобезопасно.
        /// </remarks>
        /// <seealso cref="Processor" />
        public IEnumerable<Processor> Processors
        {
            get
            {
                for (int k = 0; k < _seaProcessors.Count; k++)
                    yield return _seaProcessors[k];
            }
        }

        /// <summary>
        ///     Создаёт копию фрагмента исследуемой карты <paramref name="p" /> в местах, где указывает объект <paramref name="registered" />.
        /// </summary>
        /// <param name="p">Карта, с которой надо скопировать указанный фрагмент.</param>
        /// <param name="registered">Указывает координаты, размеры и <see cref="Processor.Tag" /> найденной карты.</param>
        /// <returns>Возвращает карту, содержащую копию фрагмента карты <paramref name="p" />, по указанным координатам.</returns>
        /// <remarks>
        ///     Значение свойства <see cref="Processor.Tag" /> берётся из названия найденной карты.
        ///     Метод потокобезопасен. Без блокировок.
        /// </remarks>
        /// <seealso cref="Registered" />
        static Processor GetProcessorFromField(Processor p, Registered registered)
        {
            SignValue[,] values = new SignValue[registered.Region.Width, registered.Region.Height];
            for (int y = registered.Y, y1 = 0; y < registered.Bottom; y++, y1++)
            for (int x = registered.X, x1 = 0; x < registered.Right; x++, x1++)
                values[x1, y1] = p[x, y];

            return new Processor(values, registered.Register.SelectedProcessor.Tag);
        }

        /// <summary>
        ///     Выполняет указанные запросы одновременно, выполняя поиск указанных слов на указанных картах.
        /// </summary>
        /// <param name="queryPairs">Пары запросов (карта и запрос к ней).</param>
        /// <returns>
        ///     В случае, когда выполнены все запросы, возвращает значение <see langword="true" />. В противном случае -
        ///     <see langword="false" />. В этом случае состояние текущего объекта остаётся прежним.
        /// </returns>
        /// <remarks>
        ///     Найденные карты заменяют карты, имеющиеся в текущем экземпляре. Они будут скопированы по координатам, где были
        ///     найдены искомые карты.
        ///     Если необходимо сохранить какую-либо карту в текущем экземпляре, необходимо подать её на распознавание в виде
        ///     запроса.
        ///     Процесс выполнения поискового запроса использует только первую букву названия каждой искомой карты (хранящейся
        ///     внутри текущего экземпляра), остальные буквы названия карты предназначены для случая, когда необходимо искать
        ///     несколько карт с одним названием, но разным содержимым.
        ///     Таким образом, этот класс <see cref="DynamicReflex" /> представляет собой "перекрестие" вертикальной и
        ///     горизонтальной моделей.
        ///     Он являет собой "снимок" выбранного момента времени. Следовательно, он позволяет осуществлять переход из состояния
        ///     в состояние посредством осуществления количественно-качественного перехода содержимого текущего экземпляра.
        ///     Таким образом, он может "накапливать" "заряд" (набирать карты с одним названием, т.е. первой буквой), а так же
        ///     сбрасывать его посредством задания соответствующего запроса, содержащего одну или несколько карт из всех
        ///     существующих "дублей" (когда первая буква совпадает).
        ///     Таким же образом, есть возможность "переименовывать" карту, т.е. можно дать запрос, где есть карта, которая
        ///     подходит двум и более картам из текущего экземпляра.
        ///     Задав им соответствующие названия в соответствующих запросах, можно получить эффект "переименовывания" карт.
        ///     Если какой-либо запрос (или запросы) содержит "недопустимые" символы, т.е. те, которые отсутствуют в текущем
        ///     экземпляре, эти запросы будут игнорированы.
        ///     В случае, если такими окажутся все запросы, метод вернёт значение <see langword="false" />.
        ///     Карты будут сохранены в текущий экземпляр при условии отсутствия наложений одной карты на другую, и при условии,
        ///     что из них можно составить искомое слово (запрос), дубликаты карт (карты с одинаковой первой буквой в названии и
        ///     идентичные по содержимому) будут исключены, при помощи класса <see cref="ProcessorHandler" />.
        ///     Если содержимое карт(ы), на которых выполняется поиск, полностью идентично содержимому текущего экземпляра, то,
        ///     после выполнения запроса, содержимое текущего экземпляра не будет изменено.
        ///     Регистр символов запросов и названий карт не важен.
        ///     Метод НЕ потокобезопасен.
        ///     <paramref name="queryPairs" /> не может быть равен <see langword="null" />, т.к. в этом случае будет выброшено
        ///     исключение <see cref="ArgumentNullException" />.
        ///     Если <paramref name="queryPairs" /> не содержит запросы, будет выброшено исключение
        ///     <see cref="ArgumentException" />.
        /// </remarks>
        /// <exception cref="ArgumentNullException" />
        /// <exception cref="ArgumentException" />
        public bool FindRelation(params (Processor p, string q)[] queryPairs)
        {
            if (queryPairs == null)
                throw new ArgumentNullException(nameof(queryPairs),
                    $"{nameof(FindRelation)}: Должен быть задан хотя бы один запрос (null).");
            if (!queryPairs.Any())
                throw new ArgumentException($"{nameof(FindRelation)}: Должен быть задан хотя бы один запрос (<пусто>).",
                    nameof(queryPairs));

            queryPairs = queryPairs.Where(t =>
                {
                    if (t.p == null)
                        return false;

                    if (t.p.Width < _seaProcessors.Width)
                        return false;

                    if (t.p.Height < _seaProcessors.Height)
                        return false;

                    return !string.IsNullOrWhiteSpace(t.q);
                }).Select(t => (t.p, new string(new HashSet<char>(t.q.Select(char.ToUpper)).ToArray())))
                .Where(t => _setChars.IsSupersetOf(t.Item2)).ToArray();

            if (!queryPairs.Any())
                return false;

            ConcurrentBag<(Processor[] pc, string q)> completedQueries = new ConcurrentBag<(Processor[], string)>();

            Parallel.ForEach(queryPairs, (pq, state) =>
            {
                Processor[]
                    pc = FindWord(pq.p, pq.q)
                        .ToArray();

                if (pc.Any())
                    completedQueries.Add((pc, pq.q));
                else
                    state.Stop();
            });

            if (completedQueries.Count != queryPairs.Length)
                return false;

            HashSet<char> allQueries = new HashSet<char>(_setChars);
            allQueries.ExceptWith(completedQueries.SelectMany(t => t.q));

            _seaProcessors = new ProcessorContainer(GetResultContainer().ToArray());

            return true;

            IEnumerable<Processor> GetResultContainer()
            {
                foreach (Processor p in GetProcessorsBySymbols(allQueries))
                    yield return p;

                ProcessorHandler ph = new ProcessorHandler();

                foreach (Processor p in completedQueries.SelectMany(t => t.pc))
                    ph.Add(p);

                foreach (Processor p in ph.Processors)
                    yield return p;
            }
        }

        /// <summary>
        ///     Получает карты, соответствующие по первой букве названия <see cref="Processor.Tag" /> заданным символам <paramref name="symbols" />.
        /// </summary>
        /// <param name="symbols">Набор символов, карты которых нужно получить. Регистр символов должен быть UPPER.</param>
        /// <returns>Возвращает набор карт, сопоставленный с указанными символами <paramref name="symbols" />.</returns>
        /// <remarks>
        ///     Возвращает карты "как есть", не создавая копий.
        ///     Метод получает карты из свойства <see cref="Processors" />.
        ///     Метод НЕ потокобезопасен.
        /// </remarks>
        IEnumerable<Processor> GetProcessorsBySymbols(IEnumerable<char> symbols)
        {
            HashSet<char> s = new HashSet<char>(symbols);
            return Processors.Where(p => s.Contains(char.ToUpper(p.Tag[0])));
        }

        /// <summary>
        ///     Получает карты, позволяющие выполнить поиск требуемого слова.
        /// </summary>
        /// <param name="p">Карта, с которой необходимо скопировать фрагменты.</param>
        /// <param name="word">Искомое слово.</param>
        /// <returns>Возвращает карты, позволяющие выполнить поиск требуемого слова.</returns>
        /// <remarks>
        ///     Искомое слово можно составить из первых букв свойств <see cref="Processor.Tag" /> карт, при условии отсутствия
        ///     наложений одной карты на другую.
        ///     Метод потокобезопасен.
        /// </remarks>
        IEnumerable<Processor> FindWord(Processor p, string word)
        {
            IList<Reg> regs = FindSymbols(word, p.GetEqual(_seaProcessors)).ToArray();

            if (regs.Count <= 0)
                yield break;

            int[] counting = null;
            Reg[] regsCounting = new Reg[word.Length];
            Region region = new Region(p.Width, p.Height);
            StringBuilder tagString = new StringBuilder(word.Length);
            TagSearcher tagSearcher = new TagSearcher(word);

            while (ChangeCount(ref counting, word.Length, regs.Count) >= 0)
            {
                bool result = true;

                for (int k = 0; k < counting.Length; k++)
                    regsCounting[k] = regs[counting[k]];

                foreach (Reg pp in regsCounting)
                {
                    Rectangle rect = new Rectangle(pp.Position, new Size(_seaProcessors.Width, _seaProcessors.Height));

                    if (region.IsConflict(rect))
                    {
                        result = false;
                        break;
                    }

                    region.Add(rect, pp.SelectedProcessor, pp.Percent);
                    tagString.Append(pp.SelectedProcessor.Tag[0]);
                }

                if (result && tagSearcher.IsEqual(tagString.ToString()))
                    foreach (Registered r in region.Elements)
                        yield return GetProcessorFromField(p, r);

                region.Clear();
                tagString.Clear();
            }
        }

        /// <summary>
        ///     Генерирует уникальную комбинацию значений элементов указанного массива.
        /// </summary>
        /// <param name="count">Массив, представляющий комбинацию разрядов. Допускает значение <see langword="null" />.</param>
        /// <param name="counter">Количество элементов в массиве <paramref name="count" />.</param>
        /// <param name="maxCount">
        ///     Максимальное значение каждого разряда. Не может быть меньше значения параметра <paramref name="counter" />.
        /// </param>
        /// <returns>Возвращает номер позиции, на которой произошло изменение, в противном случае -1.</returns>
        /// <remarks>
        ///     Если для параметра <paramref name="count" /> указать значение <see langword="null" />, будет создан и
        ///     инициализирован новый массив разрядов.
        ///     Это рекомендуется сделать для инициализации массива первоначальной комбинацией значений перед использованием.
        ///     Параметр <paramref name="counter" /> необходим только для создания и инициализации массива.
        ///     Метод возвращает только случаи без совпадающих значений в разрядах.
        ///     Метод потокобезопасен.
        /// </remarks>
        static int ChangeCount(ref int[] count, int counter, int maxCount)
        {
            if (count == null)
            {
                count = new int[counter];

                for (int k = 1; k < count.Length; k++)
                    count[k] = k;

                return counter - 1;
            }

            int? j = count.Length - 1;

            for (; j >= 0; j--)
            {
                int h = maxCount - count[j.Value];

                if (h > count.Length - j && h > 1)
                    break;

                if (j > 0)
                    continue;

                j = null;

                break;
            }

            if (j == null)
                return -1;

            int x = j.Value;
            count[x++]++;

            for (; x < count.Length; x++)
                count[x] = count[x - 1] + 1;

            return j.Value;
        }

        /// <summary>
        ///     Находит карты в <see cref="SearchResults" />, свойства <see cref="Processor.Tag" /> которых по первому символу
        ///     соответствуют указанным символам.
        /// </summary>
        /// <param name="symbols">Искомые карты.</param>
        /// <param name="searchResults">Результаты поиска, в которых необходимо найти указанные карты.</param>
        /// <returns>Возвращает сведения о найденных картах.</returns>
        /// <remarks>
        ///     Поиск производится без учёта регистра.
        ///     Метод потокобезопасен.
        /// </remarks>
        static List<Reg> FindSymbols(IEnumerable<char> symbols, SearchResults searchResults)
        {
            HashSet<char> pNames = new HashSet<char>(symbols);
            HashSet<char> pNameEx = new HashSet<char>(pNames);
            List<Reg> lstResult = new List<Reg>(pNames.Count);

            for (int y = 0, my = searchResults.Height - searchResults.MapHeight; y <= my; y++)
            for (int x = 0, mx = searchResults.Width - searchResults.MapWidth; x <= mx; x++)
            {
                ProcPerc pp = searchResults[x, y];

                lstResult.AddRange(pp.Procs.Where(p =>
                    {
                        char c = char.ToUpper(p.Tag[0]);
                        pNameEx.Remove(c);
                        return pNames.Contains(c);
                    })
                    .Select(p => new Reg(new Point(x, y)) { Percent = pp.Percent, SelectedProcessor = p }));
            }

            return pNameEx.Count <= 0 ? lstResult : new List<Reg>();
        }
    }
}