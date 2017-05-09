using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DynamicParser;
using System.Drawing;
using DynamicProcessor;
using Processor = DynamicParser.Processor;

namespace DynamicMosaic
{
    /// <summary>
    ///     Предназначен для связывания карт.
    /// </summary>
    public sealed class Reflex
    {
        /// <summary>
        /// Карты, с помощью которых производится поиск запрашиваемых данных.
        /// </summary>
        readonly ProcessorContainer _seaProcessors;

        /// <summary>
        /// Получает карту, поиск которой производится при каждом запросе поиска слова.
        /// </summary>
        /// <param name="index">Индекс карты.</param>
        /// <returns>Возвращает карту, поиск которой производится при каждом запросе поиска слова.</returns>
        public Processor GetProcessor(int index) => _seaProcessors[index];

        /// <summary>
        /// Получает количество карт в контексте.
        /// </summary>
        public int CountProcessor => _seaProcessors.Count;

        /// <summary>
        /// Инициализирует текущий контекст указанными картами.
        /// </summary>
        /// <param name="processors">Карты, которые необходимо добавить в контекст.</param>
        public Reflex(IList<Processor> processors)
        {
            if (processors == null)
                throw new ArgumentNullException(nameof(processors), $"{nameof(Reflex)}: Карты должны быть добавлены в контекст (null).");
            if (processors.Count <= 0)
                throw new ArgumentException($"{nameof(Reflex)}: Карты должны присутствовать в контексте (Count = 0).");
            _seaProcessors = new ProcessorContainer(processors);
        }

        /// <summary>
        /// Получает <see cref="Processor"/>, поле <see cref="Processor.Tag"/> которых начинается указанным символом.
        /// Поиск производится без учёта регистра.
        /// </summary>
        /// <param name="c">Искомый символ.</param>
        /// <returns>Возвращает <see cref="Processor"/>, поле <see cref="Processor.Tag"/> которых начинается указанным символом.</returns>
        public IEnumerable<Processor> GetMap(char c)
        {
            char ch = char.ToUpper(c);
            for (int k = 0; k < _seaProcessors.Count; k++)
                if (char.ToUpper(_seaProcessors[k].Tag[0]) == ch)
                    yield return _seaProcessors[k];
        }

        /// <summary>
        /// Получает карту из целевой карты по указанным координатам.
        /// </summary>
        /// <param name="processor">Карта, из которой необходимо получить целевую карту.</param>
        /// <param name="registered">Информация о получаемой карте.</param>
        /// <returns>Возвращает карту по указанным координатам целевой карты.</returns>
        public static Processor GetMap(Processor processor, Registered registered)
        {
            if (processor == null)
                throw new ArgumentNullException(nameof(processor), $@"{nameof(GetMap)}: Исходная карта должна быть указана.");
            if (registered == null)
                throw new ArgumentNullException(nameof(registered), $@"{nameof(GetMap)}: Информация о получаемой карте должна быть указана.");
            SignValue[,] values = new SignValue[registered.Region.Width, registered.Region.Height];
            for (int y = registered.Y, y1 = 0; y < registered.Bottom; y++, y1++)
                for (int x = registered.X, x1 = 0; x < registered.Right; x++, x1++)
                    values[x1, y1] = processor[x, y];
            return new Processor(values, $"{processor.Tag}0");
        }

        /// <summary>
        /// Производит поиск слова в имеющихся картах.
        /// Возвращает <see cref="Reflex"/>, который так или иначе связан с указанным словом или <see langword="null"/>, если связи нет.
        /// </summary>
        /// <param name="processor">Анализируемая карта, на которой будет производиться поиск.</param>
        /// <param name="words">Искомое слово.</param>
        /// <returns>Возвращает <see cref="Reflex"/>, который так или иначе связан с указанным словом или <see langword="null"/>, если связи нет.</returns>
        public WordSearcher FindWord(Processor processor, IList<string> words)
        {
            if (processor == null || processor.Length <= 0 || words == null || words.Count <= 0)
                return null;
            if (_seaProcessors == null || _seaProcessors.Count <= 0)
                throw new ArgumentException($"{nameof(FindWord)}: Карты для поиска искомого слова должны присутствовать.");
            object thisLock = new object(), thisLock1 = new object();
            List<string> lstProcessors = new List<string>();
            SearchResults searchResults = processor.GetEqual(_seaProcessors);
            bool exThrown = false, exStopping = false;
            string exString = string.Empty, exStoppingString = string.Empty;
            Parallel.ForEach(words, (word, state) =>
            {
                try
                {
                    if (!IsMapsWord(word))
                        return;
                    List<Reg> lstRegs = new List<Reg>();
                    foreach (List<Reg> lstReg in word.Select(c => FindSymbols(c, searchResults)))
                    {
                        if (lstReg.Count <= 0)
                            return;
                        lstRegs.AddRange(lstReg);
                    }
                    Registered[] lstProcs = FindWord(lstRegs, word, searchResults);
                    if (lstProcs == null || lstProcs.Length <= 0)
                        return;
                    lock (thisLock)
                        _seaProcessors.AddRange(lstProcs.Select(p => GetMap(processor, p)).ToArray());
                    lock (thisLock1)
                        lstProcessors.Add(word);
                }
                catch (Exception ex)
                {
                    try
                    {
                        exThrown = true;
                        exString = ex.Message;
                        state.Stop();
                    }
                    catch (Exception ex1)
                    {
                        exStopping = true;
                        exStoppingString = ex1.Message;
                    }
                }
            });
            if (exThrown)
                throw new Exception(exStopping ? $"{exString}{Environment.NewLine}{exStoppingString}" : exString);
            return new WordSearcher(lstProcessors);
        }

        /// <summary>
        ///     Получает значение true в случае нахождения искомого слова, в противном случае - false.
        /// </summary>
        /// <param name="regs">Список обрабатываемых карт.</param>
        /// <param name="word">Искомое слово.</param>
        /// <param name="searchResults">Поле результатов поиска, в которых планируется выполнить поиск требуемого слова.</param>
        /// <returns>Возвращает <see cref="WordSearcher" />, который позволяет выполнить поиск требуемого слова.</returns>
        static Registered[] FindWord(IList<Reg> regs, string word, SearchResults searchResults)
        {
            if (regs == null)
                throw new ArgumentNullException(nameof(regs),
                    $"{nameof(FindWord)}: Список обрабатываемых карт равен null.");
            if (searchResults == null)
                throw new ArgumentException($@"{nameof(FindWord)}: Поле результатов поиска отсутствует.", nameof(searchResults));
            if (regs.Count <= 0)
                return null;
            int[] counting = new int[word.Length];
            Reg[] regsCounting = new Reg[word.Length];
            DynamicParser.Region region = new DynamicParser.Region(searchResults.Width, searchResults.Height);
            for (int counter = word.Length - 1; counter >= 0;)
            {
                bool result = true;
                for (int k = 0; k < counting.Length; k++)
                    regsCounting[k] = regs[counting[k]];
                foreach (Reg pp in regsCounting)
                {
                    if (region.Contains(pp.SelectedProcessor.GetProcessorName(0, 1), 0))
                        continue;
                    Rectangle rect = new Rectangle(pp.Position, searchResults.MapSize);
                    if (region.IsConflict(rect))
                    {
                        result = false;
                        break;
                    }
                    region.Add(rect, pp.SelectedProcessor, pp.Percent);
                }
                if (result)
                {
                    Registered[] procs = GetProcessorsFromRegion(region, word).ToArray();
                    if (procs.Length > 0)
                        return procs;
                }
                if ((counter = ChangeCount(counting, regs.Count)) < 0)
                    return null;
                region.Clear();
            }
            return null;
        }

        /// <summary>
        ///     Генерирует <see cref="Processor" /> из <see cref="Processor.GetProcessorName" />.
        /// </summary>
        /// <param name="region"><see cref="DynamicParser.Region" />, из которого требуется получить <see cref="WordSearcher" />.</param>
        /// <param name="word">Искомое слово.</param>
        /// <returns>
        ///     Возвращает <see cref="Processor" /> из первых букв названия (<see cref="Processor.Tag" />) объектов <see cref="DynamicParser.Region" />.
        /// </returns>
        static IEnumerable<Registered> GetProcessorsFromRegion(DynamicParser.Region region, string word)
        {
            if (region == null)
                throw new ArgumentNullException(nameof(region), $@"{nameof(GetProcessorsFromRegion)}: Регион для получения карт должен быть указан.");
            if ((from c in word from r in region.Elements where char.ToUpper(r.Register.SelectedProcessor.Tag[0]) != char.ToUpper(c) select c).Any())
                return null;
            return from r in region.Elements where r != null select r;
        }

        /// <summary>
        ///     Увеличивает значение старших разрядов счётчика букв, если это возможно.
        ///     Если увеличение было произведено, возвращается номер позиции, на которой произошло изменение, в противном случае
        ///     -1.
        /// </summary>
        /// <param name="count">Массив-счётчик.</param>
        /// <param name="maxCount">Максимальное значение счётчика.</param>
        /// <returns>Возвращается номер позиции, на которой произошло изменение, в противном случае -1.</returns>
        static int ChangeCount(IList<int> count, int maxCount)
        {
            if (count == null)
                throw new ArgumentNullException(nameof(count),
                    $"{nameof(ChangeCount)}: Массив-счётчик не указан или его длина некорректна (null).");
            if (count.Count <= 0)
                throw new ArgumentException(
                    $"{nameof(ChangeCount)}: Длина массива-счётчика должна быть больше ноля ({count.Count}).",
                    nameof(count));
            if (maxCount <= 0)
                throw new ArgumentOutOfRangeException(nameof(maxCount),
                    $@"{nameof(ChangeCount)}: Максимальное значение счётчика меньше или равно нолю ({maxCount
                        }).");
            for (int k = count.Count - 1, mc = maxCount - 1; k >= 0; k--)
            {
                if (count[k] >= mc) continue;
                count[k]++;
                for (int x = k + 1; x < count.Count; x++)
                    count[x] = 0;
                return k;
            }
            return -1;
        }

        /// <summary>
        /// Позволяет выяснить, содержит ли текущий контекст достаточное количество карт для составления указанного слова.
        /// </summary>
        /// <param name="word">Проверяемое слово.</param>
        /// <returns>В случае успешной проверки возвращается значение <see langword="true"/>, иначе <see langword="false"/>.</returns>
        bool IsMapsWord(string word)
        {
            if (string.IsNullOrEmpty(word))
                return false;
            return word.All(c =>
            {
                for (int k = 0; k < _seaProcessors.Count; k++)
                    if (char.ToUpper(_seaProcessors[k].Tag[0]) != c)
                        return true;
                return false;
            });
        }

        /// <summary>
        ///     Находит карты в результатах поиска, поля <see cref="Processor.Tag" /> которых по указанной позиции соответствуют
        ///     указанной строке.
        /// </summary>
        /// <param name="procName">Искомая строка.</param>
        /// <param name="searchResults">Результаты поиска, в которых необходимо найти указанные карты.</param>
        /// <returns>Возвращает информацию о найденных картах.</returns>
        static List<Reg> FindSymbols(char procName, SearchResults searchResults)
        {
            if (searchResults == null)
                throw new ArgumentNullException(nameof(searchResults),
                    $@"{nameof(FindSymbols)}: Результаты поиска должны присутствовать.");
            procName = char.ToUpper(procName);
            List<Reg> lstRegs = new List<Reg>();
            for (int y = 0; y < searchResults.Height; y++)
                for (int x = 0; x < searchResults.Width; x++)
                {
                    Processor[] processors = searchResults[x, y].Procs?.Where(pr => pr != null).Where(pr => char.ToUpper(pr.Tag[0]) == procName).ToArray();
                    if ((processors?.Length ?? 0) <= 0)
                        continue;
                    double percent = searchResults[x, y].Percent;
                    Point point = new Point(x, y);
                    lstRegs.AddRange(from pr in processors
                                     where pr != null
                                     select new Reg(point)
                                     {
                                         SelectedProcessor = pr,
                                         Percent = percent
                                     });
                }
            return lstRegs;
        }
    }
}