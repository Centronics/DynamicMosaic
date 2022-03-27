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
    ///     Предназначен для связывания карт <see cref="DynamicParser.Processor" />.
    ///     Служит для поиска данных на карте и сохранения результатов поиска в виде генерации нового экземпляра
    ///     <see cref="DynamicReflex" />.
    /// </summary>
    public sealed class DynamicReflex
    {
        /// <summary>
        ///     Карты <see cref="DynamicParser.Processor" />, с помощью которых производится поиск запрашиваемых данных.
        /// </summary>
        ProcessorContainer _seaProcessors;

        readonly HashSet<char> _setChars = new HashSet<char>();

        /// <summary>
        ///     Инициализирует текущий экземпляр <see cref="DynamicReflex" /> картами из указанного экземпляра
        ///     <see cref="ProcessorContainer" />.
        ///     Карты предназначены для вызова <see cref="DynamicParser.Processor.GetEqual(ProcessorContainer)" />.
        ///     Этот список невозможно изменить вручную, в процессе работы с классом.
        /// </summary>
        /// <param name="processors">
        ///     Карты, которые необходимо добавить в текущий экземпляр <see cref="DynamicReflex" />.
        ///     С помощью них будет проводиться поиск запрашиваемых данных.
        /// </param>
        public DynamicReflex(ProcessorContainer processors)
        {
            if (processors == null)
                throw new ArgumentNullException(nameof(processors), $"{nameof(DynamicReflex)}: Карты должны быть добавлены в контекст (null).");

            Processor[] procs = new Processor[processors.Count];

            for (int k = 0; k < procs.Length; k++)
            {
                Processor p = processors[k];
                _setChars.Add(char.ToUpper(p.Tag[0]));
                procs[k] = p;
            }

            _seaProcessors = new ProcessorContainer(procs);
        }

        public IEnumerable<Processor> Processors
        {
            get
            {
                for (int k = 0; k < _seaProcessors.Count; k++)
                    yield return _seaProcessors[k];
            }
        }

        /// <summary>
        ///     Определяет, содержит ли текущий экземпляр <see cref="DynamicReflex" /> карту с подобным содержимым или нет.
        ///     В случае нахождения карты с совпадающим содержимым, метод возвращает значение <see langword="null" />, в противном
        ///     случае -
        ///     массив данных, сведения о котором указаны в параметре <see cref="Registered" />.
        /// </summary>
        /// <param name="processor">Карта, из которой необходимо получить целевую карту.</param>
        /// <param name="registered">Сведения о получаемой карте.</param>
        /// <returns>
        ///     В случае нахождения карты с совпадающим содержимым, метод возвращает значение <see langword="null" />, в противном
        ///     случае -
        ///     массив данных, сведения о котором указаны в параметре <see cref="Registered" />.
        /// </returns>
        static Processor GetProcessorFromField(Processor processor, Registered registered)
        {
            SignValue[,] values = new SignValue[registered.Region.Width, registered.Region.Height];
            for (int y = registered.Y, y1 = 0; y < registered.Bottom; y++, y1++)
                for (int x = registered.X, x1 = 0; x < registered.Right; x++, x1++)
                    values[x1, y1] = processor[x, y];

            return new Processor(values, registered.Register.SelectedProcessor.Tag);
        }

        public bool FindRelation(params (Processor p, string q)[] queryPairs)
        {
            if (queryPairs == null)
                throw new ArgumentNullException(nameof(queryPairs), $@"{nameof(FindRelation)}: Должен быть задан хотя бы один запрос (null).");
            if (!queryPairs.Any())
                throw new ArgumentException($@"{nameof(FindRelation)}: Должен быть задан хотя бы один запрос (<пусто>).", nameof(queryPairs));

            queryPairs = queryPairs.Where(t =>
            {
                if (t.p == null)
                    return false;

                if (t.p.Width < _seaProcessors.Width)
                    return false;

                if (t.p.Height < _seaProcessors.Height)
                    return false;

                return !string.IsNullOrWhiteSpace(t.q);

            }).Select(t => (t.p, new string(new HashSet<char>(t.q.Select(char.ToUpper)).ToArray()))).Where(t => _setChars.IsSupersetOf(t.Item2)).ToArray();

            if (!queryPairs.Any())
                return false;

            ConcurrentBag<(Processor[] pc, string q)> completedQueries = new ConcurrentBag<(Processor[], string)>();
            Exception exThrown = null;

            Parallel.ForEach(queryPairs, (pq, state) =>
            {
                try
                {
                    Processor[] pc = FindWord(pq.p, pq.q).ToArray();//как вызвать исключение???

                    if (pc.Any())
                        completedQueries.Add((pc, pq.q));
                    else
                        state.Stop();
                }
                catch (Exception ex)
                {
                    exThrown = ex;
                    state.Stop();
                }
            });

            if (exThrown != null)
                throw exThrown;

            if (completedQueries.Count != queryPairs.Length)
                return false;

            HashSet<char> allQueries = new HashSet<char>(Processors.Select(t => char.ToUpper(t.Tag[0])));
            allQueries.ExceptWith(completedQueries.SelectMany(t => t.q));

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

            _seaProcessors = new ProcessorContainer(GetResultContainer().ToArray());

            return true;
        }

        IEnumerable<Processor> GetProcessorsBySymbols(IEnumerable<char> symbols) => Processors.Where(p => new HashSet<char>(symbols).Contains(char.ToUpper(p.Tag[0])));

        /// <summary>
        ///     Получает список коллекций областей, позволяющих выполнить поиск требуемого слова, т.е. искомое слово можно
        ///     составить из первых букв свойств
        ///     <see cref="Processor.Tag" /> карт, при условии отсутствия наложений одной карты на другую.
        /// </summary>
        /// <param name="regs">Список обрабатываемых карт, из которых требуется получить список коллекций областей.</param>
        /// <param name="word">Искомое слово.</param>
        /// <param name="mapSize">Размер поля результатов поиска, в которых требуется выполнить поиск требуемого слова.</param>
        /// <returns>Возвращает список коллекций областей, позволяющих выполнить поиск требуемого слова.</returns>
        IEnumerable<Processor> FindWord(Processor p, string word)
        {
            IList<Reg> regs = FindSymbols(word, p.GetEqual(_seaProcessors)).ToArray();

            if (regs.Count <= 0)
                yield break;

            int[] counting = null;
            Reg[] regsCounting = new Reg[word.Length];
            Region region = new Region(p.Width, p.Height);
            StringBuilder mapString = new StringBuilder(word.Length);
            TagSearcher mapSearcher = new TagSearcher(word);
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
                    mapString.Append(pp.SelectedProcessor.Tag[0]);
                }

                if (result && mapSearcher.IsEqual(mapString.ToString()))
                    foreach (Registered r in region.Elements)
                        yield return GetProcessorFromField(p, r);

                region.Clear();
                mapString.Clear();
            }
        }

        /// <summary>
        ///     Увеличивает значение младших разрядов счётчика, если это возможно.
        ///     Возвращаются только случаи без совпадающих значений в различных разрядах.
        ///     Если увеличение было произведено, возвращается номер позиции, на которой произошло изменение, в противном случае
        ///     -1.
        /// </summary>
        /// <param name="count">Массив, представляющий состояние разрядов счётчика.</param>
        /// <param name="counter">Количество элементов в массиве "count".</param>
        /// <param name="maxCount">Максимальное значение каждого разряда.</param>
        /// <returns>Возвращается номер позиции, на которой произошло изменение, в противном случае -1.</returns>
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
        ///     соответствуют указанному символу.
        ///     Поиск производится без учёта регистра.
        /// </summary>
        /// <param name="procName">Искомое название карты.</param>
        /// <param name="searchResults">Результаты поиска, в которых необходимо найти указанные карты.</param>
        /// <returns>Возвращает сведения о найденных картах.</returns>
        static List<Reg> FindSymbols(IEnumerable<char> procName, SearchResults searchResults)
        {
            HashSet<char> pNames = new HashSet<char>(procName);
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