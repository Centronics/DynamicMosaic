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
    ///     Предназначен для связывания карт <see cref="Processor"/>.
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
        /// Получает размер загруженных карт в текущий экземпляр <see cref="Reflex"/>.
        /// </summary>
        public Size MapSize => new Size(_seaMaps[0].Width, _seaMaps[0].Height);

        /// <summary>
        /// Инициализирует текущий контекст указанными картами.
        /// </summary>
        /// <param name="processors">Карты, которые необходимо добавить в контекст.</param>
        public Reflex(ProcessorContainer processors)
        {
            if (processors == null)
                throw new ArgumentNullException(nameof(processors), $"{nameof(Reflex)}: Карты должны быть добавлены в контекст (null).");
            if (processors.Count < 2)
                throw new ArgumentException($"{nameof(Reflex)}: В контексте должны присутствовать минимум две карты ({nameof(processors.Count)} = {processors.Count}).");
            Processor[] procs = new Processor[processors.Count];
            for (int k = 0; k < procs.Length; k++)
                procs[k] = processors[k].GetMapClone();
            _seaProcessors = new ProcessorContainer(procs);
            _seaMaps = new ProcessorContainer(procs);
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
            SignValue[,] values = AddMap(registered, processor);
            if (values == null)
                return;
            string s = registered.Register.SelectedProcessor.Tag;
            while (_seaProcessors.ContainsTag(s))
                s += '0';
            _seaProcessors.Add(new Processor(values, s));
        }

        /// <summary>
        /// Определяет, содержит ли текущий экземпляр <see cref="Reflex"/> карту с совпадающим содержимым или нет.
        /// В случае нахождения карты с совпадающим содержимым возвращает значение <see langword="true"/>, в противном случае - <see langword="false"/>.
        /// </summary>
        /// <param name="registered">Информация о получаемой карте.</param>
        /// <param name="processor">Карта, из которой необходимо получить целевую карту.</param>
        /// <returns>В случае нахождения карты с совпадающим содержимым возвращает значение <see langword="true"/>, в противном случае - <see langword="false"/>.</returns>
        SignValue[,] AddMap(Registered registered, Processor processor)
        {
            if (registered == null)
                throw new ArgumentNullException(nameof(registered), $"{nameof(AddMap)}: Информация о получаемой карте должна присутствовать.");
            if (processor == null)
                throw new ArgumentNullException(nameof(processor), $"{nameof(AddMap)}: Карта, из которой необходимо получить целевую карту, должна быть указана.");
            for (int k = 0; k < _seaProcessors.Count; k++)
            {
                Processor p = _seaProcessors[k];
                if (p.Width != registered.Region.Width || p.Height != registered.Region.Height)
                    throw new ArgumentException($"{nameof(AddMap)}: Карты не соответствуют по размерам.", nameof(registered));
                bool res = true;
                for (int y = registered.Y, y1 = 0; y < registered.Bottom; y++, y1++)
                    for (int x = registered.X, x1 = 0; x < registered.Right; x++, x1++)
                    {
                        if (p[x1, y1] == processor[x, y])
                            continue;
                        res = false;
                        goto exit;
                    }
                exit:
                if (res)
                    return null;
            }
            SignValue[,] values = new SignValue[registered.Region.Width, registered.Region.Height];
            for (int y = registered.Y, y1 = 0; y < registered.Bottom; y++, y1++)
                for (int x = registered.X, x1 = 0; x < registered.Right; x++, x1++)
                    values[x1, y1] = processor[x, y];
            return values;
        }

        /// <summary>
        /// Производит поиск слова в имеющихся картах.
        /// Возвращает <see cref="Reflex"/>, который так или иначе связан с указанным словом или <see langword="null"/>, если связи нет.
        /// </summary>
        /// <param name="processor">Карта, на которой будет производиться поиск.</param>
        /// <param name="word">Искомое слово.</param>
        /// <returns>Возвращает <see cref="Reflex"/>, который так или иначе связан с указанным словом или <see langword="null"/>, если связи нет.</returns>
        public bool FindRelation(Processor processor, string word)
        {
            if (processor == null)
                throw new ArgumentNullException(nameof(processor), $"{nameof(FindWord)}: Карта для поиска не указана (null).");
            if (word == null)
                throw new ArgumentNullException(nameof(word), $"{nameof(FindWord)}: Искомое слово равно null.");
            if (word == string.Empty)
                throw new ArgumentException($"{nameof(FindWord)}: Искомое слово не указано.", nameof(word));
            if (!IsMapsWord(word))
                return false;
            SearchResults searchResults = processor.GetEqual(_seaProcessors);
            if (!searchResults.FindRelation(word, 0, word.Length))
                return false;
            List<Reg> lstRegs = new List<Reg>();
            foreach (List<Reg> lstReg in word.Select(c => FindSymbols(c, searchResults)).Where(lstReg => lstReg.Count > 0))
                lstRegs.AddRange(lstReg);
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
                    if (region.Contains(pp.SelectedProcessor.Tag, 0))
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
            return from r in region.Elements.Where(r => r?.Register.SelectedProcessor?.IsProcessorName(word, 0) ?? false) select r;
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
        /// <returns>В случае успешной проверки возвращается значение <see langword="true"/>, иначе <see langword="false"/>.</returns>
        public bool IsMapsWord(string word)
        {
            if (string.IsNullOrEmpty(word))
                return false;
            List<char> lstCh = word.Select(char.ToUpper).ToList();
            for (int k = 0; k < _seaProcessors.Count; k++)
            {
                for (int j = 0; j < lstCh.Count; j++)
                    if (lstCh[j] == char.ToUpper(_seaProcessors[k].Tag[0]))
                        lstCh.RemoveAt(j--);
                if (lstCh.Count <= 0)
                    return true;
            }
            return lstCh.Count <= 0;
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
                    Processor[] procs = searchResults[x, y].Procs;
                    if (procs == null)
                        continue;
                    lstRegs.AddRange(from p in procs
                                     where char.ToUpper(p.Tag[0]) == procName
                                     select new Reg(new Point(x, y))
                                     {
                                         Percent = searchResults[x, y].Percent,
                                         SelectedProcessor = p
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

        /// <summary>
        /// Выполняет операцию сравнения двух экземпляров объекта <see cref="Reflex"/>.
        /// Возвращает значение <see langword="true"></see> в случае равенства, <see langword="false"></see> в противном случае.
        /// </summary>
        /// <param name="one">Первый сопоставляемый экземпляр.</param>
        /// <param name="two">Второй сопоставляемый экземпляр.</param>
        /// <returns>Возвращает значение <see langword="true"></see> в случае равенства, <see langword="false"></see> в противном случае.</returns>
        public static bool operator ==(Reflex one, Reflex two)
        {
            return Compare(one, two);
        }

        /// <summary>
        /// Выполняет операцию сравнения двух экземпляров объекта <see cref="Reflex"/>.
        /// Возвращает значение <see langword="true"></see> в случае неравенства, <see langword="false"></see> в противном случае.
        /// </summary>
        /// <param name="one">Первый сопоставляемый экземпляр.</param>
        /// <param name="two">Второй сопоставляемый экземпляр.</param>
        /// <returns>Возвращает значение <see langword="true"></see> в случае неравенства, <see langword="false"></see> в противном случае.</returns>
        public static bool operator !=(Reflex one, Reflex two)
        {
            return !Compare(one, two);
        }

        /// <summary>
        /// Сравнивает два экземпляра <see cref="Reflex"/>. Сопоставляются все карты, которые есть в наличии.
        /// В случае равенства возвращается значение <see langword="true"/>, в противном случае - <see langword="false"/>.
        /// </summary>
        /// <param name="reflex1">Первый сопоставляемый экземпляр <see cref="Reflex"/>.</param>
        /// <param name="reflex2">Второй сопоставляемый экземпляр <see cref="Reflex"/>.</param>
        /// <returns>В случае равенства возвращается значение <see langword="true"/>, в противном случае - <see langword="false"/>.</returns>
        static bool Compare(Reflex reflex1, Reflex reflex2)
        {
            if ((object)reflex1 == null && (object)reflex2 == null)
                return true;
            if ((object)reflex1 != null && (object)reflex2 == null)
                return false;
            if ((object)reflex1 == null)
                return false;
            if (reflex1._seaProcessors.Count != reflex2._seaProcessors.Count)
                return false;
            for (int k = 0; k < reflex1._seaProcessors.Count; k++)
            {
                bool res = false;
                for (int j = 0; j < reflex2._seaProcessors.Count; j++)
                {
                    if (!reflex1._seaProcessors[k].ProcessorCompare(reflex2._seaProcessors[j]))
                        continue;
                    res = true;
                    break;
                }
                if (!res)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Определяет, равен ли заданный объект текущему объекту.
        /// </summary>
        /// <param name="other">Объект, который требуется сравнить с текущим объектом.</param>
        /// <returns>Значение <see langword="true"/>, если указанный объект равен текущему объекту, в противном случае — значение <see langword="false"/>.</returns>
        bool Equals(Reflex other) => Equals(_seaProcessors, other._seaProcessors) && Equals(_seaMaps, other._seaMaps);

        /// <summary>
        /// Определяет, равен ли заданный объект текущему объекту.
        /// </summary>
        /// <param name="obj">Объект, который требуется сравнить с текущим объектом.</param>
        /// <returns>Значение <see langword="true"/>, если указанный объект равен текущему объекту, в противном случае — значение <see langword="false"/>.</returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            Reflex a = obj as Reflex;
            return a != null && Equals(a);
        }

        /// <summary>
        /// Получает хеш-код текущего экземпляра <see cref="Reflex"/>.
        /// </summary>
        /// <returns>Возвращает хеш-код текущего экземпляра <see cref="Reflex"/>.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                return ((_seaProcessors != null ? _seaProcessors.GetHashCode() : 0) * 397) ^ (_seaMaps != null ? _seaMaps.GetHashCode() : 0);
            }
        }
    }
}