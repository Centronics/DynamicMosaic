using System;
using System.Collections.Generic;
using System.Linq;
using DynamicParser;
using System.Drawing;
using DynamicProcessor;
using Processor = DynamicParser.Processor;

namespace DynamicMosaic
{
    /// <summary>
    ///     Предназначен для связывания карт.
    /// </summary>
    public sealed class Reflex : ICloneable
    {
        /// <summary>
        /// Карты, с помощью которых производится поиск запрашиваемых данных.
        /// </summary>
        readonly ProcessorContainer _seaProcessors;

        /// <summary>
        /// Карты, изначально загруженные в текущий экземпляр <see cref="Reflex"/> и предназначенные для клонирования текущего экземпляра <see cref="Reflex"/>.
        /// </summary>
        readonly ProcessorContainer _seaMaps;

        /// <summary>
        /// Получает карту, поиск которой производится при каждом запросе поиска слова.
        /// </summary>
        /// <param name="index">Индекс карты.</param>
        /// <returns>Возвращает карту, поиск которой производится при каждом запросе поиска слова.</returns>
        public Processor this[int index] => _seaProcessors[index];

        /// <summary>
        /// Предоставляет доступ к картам, загруженным в текущий контекст изначально, т.е. до вызова метода <see cref="FindRelation(Processor, string, int, int)"/>.
        /// </summary>
        public IEnumerable<Processor> ProcessorsBase
        {
            get
            {
                for (int k = 0; k < _seaMaps.Count; k++)
                    yield return _seaMaps[k];
            }
        }

        /// <summary>
        /// Получает количество карт, с которыми был инициализирован текущий контекст <see cref="Reflex"/>.
        /// </summary>
        public int CountProcessorsBase => _seaMaps.Count;

        /// <summary>
        /// Получает все карты контекста <see cref="Reflex"/>.
        /// </summary>
        public IEnumerable<Processor> Processors
        {
            get
            {
                for (int k = 0; k < _seaProcessors.Count; k++)
                    yield return _seaProcessors[k];
            }
        }

        /// <summary>
        /// Получает количество карт в контексте.
        /// </summary>
        public int CountProcessor => _seaProcessors.Count;

        /// <summary>
        /// Инициализирует текущий контекст указанными картами.
        /// </summary>
        /// <param name="processors">Карты, которые необходимо добавить в контекст.</param>
        public Reflex(ProcessorContainer processors)
        {
            if (processors == null)
                throw new ArgumentNullException(nameof(processors), $"{nameof(Reflex)}: Карты должны быть добавлены в контекст (null).");
            if (processors.Count <= 0)
                throw new ArgumentException($"{nameof(Reflex)}: Карты должны присутствовать в контексте (Count = 0).");
            Processor[] procs = new Processor[processors.Count];
            for (int k = 0; k < procs.Length; k++)
                procs[k] = processors[k];
            _seaProcessors = new ProcessorContainer(procs);
            _seaMaps = new ProcessorContainer(procs);
        }

        /// <summary>
        /// Получает <see cref="Processor"/>, поле <see cref="Processor.Tag"/> которых начинается указанным символом.
        /// Поиск производится без учёта регистра.
        /// </summary>
        /// <param name="name">Искомая строка.</param>
        /// <param name="startIndex">Стартовый индекс позиции, с которой необходимо начать анализ поля <see cref="Processor.Tag"/> карты.</param>
        /// <returns>Возвращает <see cref="Processor"/>, поле <see cref="Processor.Tag"/> которых начинается указанным символом.</returns>
        public IEnumerable<Processor> GetMap(string name, int startIndex = 0)
        {
            for (int k = 0; k < _seaProcessors.Count; k++)
                if (_seaProcessors[k].IsProcessorName(name, startIndex))
                    yield return _seaProcessors[k];
        }

        /// <summary>
        /// Получает карту из целевой карты по указанным координатам.
        /// </summary>
        /// <param name="processor">Карта, из которой необходимо получить целевую карту.</param>
        /// <param name="registered">Информация о получаемой карте.</param>
        /// <returns>Возвращает карту по указанным координатам целевой карты.</returns>
        void GetMap(Processor processor, Registered registered)
        {
            if (processor == null)
                throw new ArgumentNullException(nameof(processor), $@"{nameof(GetMap)}: Исходная карта должна быть указана.");
            if (registered == null)
                throw new ArgumentNullException(nameof(registered), $@"{nameof(GetMap)}: Информация о получаемой карте должна быть указана.");
            SignValue[,] values = new SignValue[registered.Region.Width, registered.Region.Height];
            for (int y = registered.Y, y1 = 0; y < registered.Bottom; y++, y1++)
                for (int x = registered.X, x1 = 0; x < registered.Right; x++, x1++)
                    values[x1, y1] = processor[x, y];
            string s = registered.Register.SelectedProcessor.Tag;
            while (_seaProcessors.ContainsTag(s))
                s += '0';
            _seaProcessors.Add(new Processor(values, s));
        }

        /// <summary>
        /// Производит поиск слова в имеющихся картах.
        /// Возвращает <see cref="Reflex"/>, который так или иначе связан с указанным словом или <see langword="null"/>, если связи нет.
        /// </summary>
        /// <param name="processor">Карта, на которой будет производиться поиск.</param>
        /// <param name="word">Искомое слово.</param>
        /// <param name="startIndex">Индекс, с которого необходимо начать поиск в названии карт.</param>
        /// <param name="count">Количество символов, которое необходимо взять из названия карты для определения их соответствия указанному слову.</param>
        /// <returns>Возвращает <see cref="Reflex"/>, который так или иначе связан с указанным словом или <see langword="null"/>, если связи нет.</returns>
        public bool FindRelation(Processor processor, string word, int startIndex = 0, int count = 1)
        {
            if (processor == null)
                throw new ArgumentNullException(nameof(processor), $"{nameof(FindWord)}: Карта для поиска не указана (null).");
            if (word == null)
                throw new ArgumentNullException(nameof(word), $"{nameof(FindWord)}: Искомое слово равно null.");
            if (word == string.Empty)
                throw new ArgumentException($"{nameof(FindWord)}: Искомое слово не указано.", nameof(word));
            if (startIndex < 0)
                throw new ArgumentException($"{nameof(FindWord)}: Индекс начала поиска имеет некорректное значение: {startIndex}.", nameof(startIndex));
            if (count <= 0)
                throw new ArgumentException($"{nameof(FindWord)}: Количество символов поиска задано неверно: {count}.", nameof(count));
            if (!IsMapsWord(word))
                return false;
            SearchResults searchResults = processor.GetEqual(_seaProcessors);
            if (!searchResults.FindRelation(word, startIndex, count))
                return false;
            List<Reg> lstRegs = new List<Reg>();
            if (count == 1)
            {
                foreach (List<Reg> lstReg in word.Select(c => FindSymbols(c, searchResults, startIndex)).Where(lstReg => lstReg.Count > 0))
                    lstRegs.AddRange(lstReg);
            }
            else
            {
                for (int p = 0, mx = word.Length - count; p <= mx; p++)
                    lstRegs.AddRange(FindSymbols(word.Substring(p, count), searchResults, startIndex));
            }
            foreach (Registered r in FindWord(lstRegs, word, searchResults).Where(lstProcs => lstProcs != null).SelectMany(regs => regs))
                GetMap(processor, r);
            return true;
        }

        /// <summary>
        ///     Получает значение true в случае нахождения искомого слова, в противном случае - false.
        /// </summary>
        /// <param name="regs">Список обрабатываемых карт.</param>
        /// <param name="word">Искомое слово.</param>
        /// <param name="searchResults">Поле результатов поиска, в которых планируется выполнить поиск требуемого слова.</param>
        /// <returns>Возвращает <see cref="WordSearcher" />, который позволяет выполнить поиск требуемого слова.</returns>
        static IEnumerable<IEnumerable<Registered>> FindWord(IList<Reg> regs, string word, SearchResults searchResults)
        {
            if (regs == null)
                throw new ArgumentNullException(nameof(regs),
                    $"{nameof(FindWord)}: Список обрабатываемых карт равен null.");
            if (searchResults == null)
                throw new ArgumentException($@"{nameof(FindWord)}: Поле результатов поиска отсутствует.", nameof(searchResults));
            if (regs.Count <= 0)
                yield break;
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
                    yield return GetProcessorsFromRegion(region, word);
                if ((counter = ChangeCount(counting, regs.Count)) < 0)
                    yield break;
                region.Clear();
            }
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
                    $@"{nameof(ChangeCount)}: Максимальное значение счётчика меньше или равно нолю ({maxCount}).");
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
        /// <param name="startIndex">Стартовый индекс позиции, с которой необходимо начать анализ поля <see cref="Processor.Tag"/> карты.</param>
        /// <returns>В случае успешной проверки возвращается значение <see langword="true"/>, иначе <see langword="false"/>.</returns>
        public bool IsMapsWord(string word, int startIndex = 0)
        {
            if (string.IsNullOrEmpty(word))
                return false;
            word = word.ToUpper();
            return word.All(c =>
            {
                for (int k = 0; k < _seaProcessors.Count; k++)
                    if (_seaProcessors[k].IsProcessorName(word, startIndex))
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
        /// <param name="startIndex">Стартовый индекс позиции, с которой необходимо начать анализ поля <see cref="Processor.Tag"/> карты.</param>
        /// <returns>Возвращает информацию о найденных картах.</returns>
        static List<Reg> FindSymbols(string procName, SearchResults searchResults, int startIndex)
        {
            if (procName == null)
                throw new ArgumentNullException(nameof(procName), $"{nameof(FindSymbols)}: Искомая строка не может быть равна null.");
            if (procName == string.Empty)
                throw new ArgumentException($"{nameof(FindSymbols)}: Искомая строка не может быть пустой.", nameof(procName));
            if (searchResults == null)
                throw new ArgumentNullException(nameof(searchResults), $@"{nameof(FindSymbols)}: Результаты поиска должны присутствовать.");
            if (startIndex < 0)
                throw new ArgumentException($"{nameof(FindSymbols)}: Стартовый индекс должен быть больше ноля ({startIndex}).", nameof(startIndex));
            List<Reg> lstRegs = new List<Reg>();
            for (int y = 0; y < searchResults.Height; y++)
                for (int x = 0; x < searchResults.Width; x++)
                {
                    Processor[] processors = searchResults[x, y].Procs?.Where(pr => pr != null).Where(pr => pr.IsProcessorName(procName, startIndex)).ToArray();
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

        /// <summary>
        ///     Находит карты в результатах поиска, поля <see cref="Processor.Tag" /> которых по указанной позиции соответствуют
        ///     указанной строке.
        /// </summary>
        /// <param name="procName">Искомая строка.</param>
        /// <param name="searchResults">Результаты поиска, в которых необходимо найти указанные карты.</param>
        /// <param name="startIndex">Стартовый индекс позиции, с которой необходимо начать анализ поля <see cref="Processor.Tag"/> карты.</param>
        /// <returns>Возвращает информацию о найденных картах.</returns>
        static List<Reg> FindSymbols(char procName, SearchResults searchResults, int startIndex)
        {
            if (searchResults == null)
                throw new ArgumentNullException(nameof(searchResults),
                    $@"{nameof(FindSymbols)}: Результаты поиска должны присутствовать.");
            if (startIndex < 0)
                throw new ArgumentException($"{nameof(FindSymbols)}: Стартовый индекс должен быть больше ноля ({startIndex}).", nameof(startIndex));
            procName = char.ToUpper(procName);
            List<Reg> lstRegs = new List<Reg>();
            for (int y = 0; y < searchResults.Height; y++)
                for (int x = 0; x < searchResults.Width; x++)
                {
                    Processor[] processors = searchResults[x, y].Procs?.Where(pr => pr != null).Where(pr =>
                        startIndex < pr.Tag.Length && char.ToUpper(pr.Tag[startIndex]) == procName).ToArray();
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

        /// <summary>
        /// Создаёт неполную копию текущего экземпляра.
        /// Копируются только изначальные значения текущего экземпляра <see cref="Reflex"/>.
        /// </summary>
        /// <returns>Возвращает неполную копию текущего экземпляра.</returns>
        public object Clone() => new Reflex(_seaMaps);
    }
}