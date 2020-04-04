using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using DynamicParser;
using DynamicProcessor;
using Processor = DynamicParser.Processor;
using Region = DynamicParser.Region;

namespace DynamicMosaic
{
    /// <summary>
    ///     Предназначен для связывания карт <see cref="DynamicParser.Processor" />.
    ///     Служит для поиска данных на карте и сохранения результатов поиска в виде генерации нового экземпляра
    ///     <see cref="Reflex" />.
    /// </summary>
    public sealed class Reflex
    {
        /// <summary>
        ///     Карты <see cref="DynamicParser.Processor" />, с помощью которых производится поиск запрашиваемых данных.
        /// </summary>
        readonly ProcessorContainer _seaProcessors;

        /// <summary>
        ///     Инициализирует текущий экземпляр <see cref="Reflex" /> картами из указанного экземпляра
        ///     <see cref="ProcessorContainer" />.
        ///     Карты предназначены для вызова <see cref="DynamicParser.Processor.GetEqual(ProcessorContainer)" />.
        ///     Этот список невозможно изменить вручную, в процессе работы с классом.
        /// </summary>
        /// <param name="processors">
        ///     Карты, которые необходимо добавить в текущий экземпляр <see cref="Reflex" />.
        ///     С помощью них будет проводиться поиск запрашиваемых данных.
        /// </param>
        public Reflex(ProcessorContainer processors)
        {
            if (processors == null)
                throw new ArgumentNullException(nameof(processors),
                    $"{nameof(Reflex)}: Карты должны быть добавлены в контекст (null).");
            if (processors.Count < 2)
                throw new ArgumentException(
                    $"{nameof(Reflex)}: В контексте должны присутствовать минимум две карты ({nameof(processors.Count)} = {processors.Count}).");
            Processor[] procs = new Processor[processors.Count];
            for (int k = 0; k < procs.Length; k++)
                procs[k] = processors[k];
            _seaProcessors = new ProcessorContainer(procs);
        }

        /// <summary>
        ///     Инициализирует текущий экземпляр <see cref="Reflex" /> картами, взятыми из указанного экземпляра
        ///     <see cref="Reflex" />.
        ///     Карты предназначены для вызова <see cref="DynamicParser.Processor.GetEqual(ProcessorContainer)" />.
        ///     Этот список невозможно изменить вручную, в процессе работы с классом.
        /// </summary>
        /// <param name="reflex">
        ///     Экземпляр <see cref="Reflex" />, карты из которого необходимо добавить в текущий экземпляр <see cref="Reflex" />.
        ///     С помощью них будет проводиться поиск запрашиваемых данных.
        ///     Состояние данного экземпляра <see cref="Reflex" /> останется прежним.
        /// </param>
        public Reflex(Reflex reflex)
        {
            if (reflex == null)
                throw new ArgumentNullException(nameof(reflex),
                    $"{nameof(Reflex)}: Необходимо указать экземпляр класса {nameof(Reflex)}.");
            Processor[] procs = new Processor[reflex._seaProcessors.Count];
            for (int k = 0; k < procs.Length; k++)
                procs[k] = reflex._seaProcessors[k];
            _seaProcessors = new ProcessorContainer(procs);
        }

        /// <summary>
        ///     Получает количество карт, содержащихся в текущем экземпляре <see cref="Reflex" />.
        /// </summary>
        public int Count => _seaProcessors.Count;

        /// <summary>
        ///     Получает карту по заданному индексу, который отсчитывается от ноля.
        /// </summary>
        /// <param name="index">Индекс карты, начинающийся с ноля, которую необходимо получить.</param>
        /// <returns>Возвращает карту по заданному индексу.</returns>
        public Processor this[int index] => _seaProcessors[index];

        /// <summary>
        ///     Получает карту из целевой карты по указанным координатам, копируя часть указанной карты в массив карт для поиска
        ///     <see cref="_seaProcessors" />.
        /// </summary>
        /// <param name="processor">Карта, из которой необходимо получить целевую карту.</param>
        /// <param name="registered">Сведения о получаемой карте.</param>
        void GetMap(Processor processor, Registered registered)
        {
            if (processor == null)
                throw new ArgumentNullException(nameof(processor),
                    $@"{nameof(GetMap)}: Исходная карта должна быть указана.");
            if (registered == null)
                throw new ArgumentNullException(nameof(registered),
                    $@"{nameof(GetMap)}: Сведения о получаемой карте должны присутствовать.");
            if (registered.X < 0)
                throw new ArgumentException(
                    $"{nameof(GetMap)}: Координата {nameof(Registered.X)} получаемой карты меньше ноля ({registered.X}).",
                    nameof(registered));
            if (registered.Y < 0)
                throw new ArgumentException(
                    $"{nameof(GetMap)}: Координата {nameof(Registered.Y)} получаемой карты меньше ноля ({registered.Y}).",
                    nameof(registered));
            if (registered.Right > processor.Width)
                throw new ArgumentException(
                    $"{nameof(GetMap)}: Данные о получаемой карте некорректны: выход за предел карты по ширине.",
                    nameof(registered));
            if (registered.Bottom > processor.Height)
                throw new ArgumentException(
                    $"{nameof(GetMap)}: Данные о получаемой карте некорректны: выход за предел карты по высоте.",
                    nameof(registered));
            SignValue[,] values = MapCopy(processor, registered);
            if (values == null)
                return;
            string tag = registered.Register.SelectedProcessor.Tag;
            while (_seaProcessors.ContainsTag(tag))
                tag += '0';
            _seaProcessors.Add(new Processor(values, tag));
        }

        /// <summary>
        ///     Определяет, содержит ли текущий экземпляр <see cref="Reflex" /> карту с подобным содержимым или нет.
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
        SignValue[,] MapCopy(Processor processor, Registered registered)
        {
            if (processor == null)
                throw new ArgumentNullException(nameof(processor),
                    $"{nameof(MapCopy)}: Карта, из которой необходимо получить целевую карту, должна быть указана.");
            if (registered == null)
                throw new ArgumentNullException(nameof(registered),
                    $"{nameof(MapCopy)}: Сведения о получаемой карте должны присутствовать.");
            if (registered.X < 0)
                throw new ArgumentException(
                    $"{nameof(MapCopy)}: Координата {nameof(Registered.X)} получаемой карты меньше ноля ({registered.X}).",
                    nameof(registered));
            if (registered.Y < 0)
                throw new ArgumentException(
                    $"{nameof(MapCopy)}: Координата {nameof(Registered.Y)} получаемой карты меньше ноля ({registered.Y}).",
                    nameof(registered));
            if (registered.Right > processor.Width)
                throw new ArgumentException(
                    $"{nameof(MapCopy)}: Данные о получаемой карте некорректны: выход за предел карты по ширине.",
                    nameof(registered));
            if (registered.Bottom > processor.Height)
                throw new ArgumentException(
                    $"{nameof(MapCopy)}: Данные о получаемой карте некорректны: выход за предел карты по высоте.",
                    nameof(registered));
            for (int k = 0; k < _seaProcessors.Count; k++)
            {
                Processor p = _seaProcessors[k];
                if (p.Width != registered.Region.Width || p.Height != registered.Region.Height)
                    throw new ArgumentException($"{nameof(MapCopy)}: Карты не соответствуют по размерам.",
                        nameof(registered));
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
        ///     Производит поиск указанного слова <see cref="string" /> на указанной карте <see cref="Processor" />.
        ///     Возвращает новый экземпляр <see cref="Reflex" /> в случае нахождения указанного слова на карте, в противном случае
        ///     - <see langword="null" />.
        ///     Возвращаемый экземпляр <see cref="Reflex" />, помимо карт текущего экземпляра, включает в себя карты, которые
        ///     скопированы с рабочей области.
        ///     Карты скопированы с координат, где были найдены искомые карты.
        ///     Таким образом, вызов этого метода не изменяет состояние текущего экземпляра <see cref="Reflex" />.
        ///     Скопированные карты добавлены в конец внутренней коллекции, количество карт в порождающем объекте равняется номеру
        ///     первой добавленной карты в порождённом объекте.
        ///     Карты содержатся во внутренней коллекции искомых карт <see cref="ProcessorContainer" />.
        ///     Карты сохраняются в новый экземпляр объекта при условии отсутствия наложений одной карты на другую, и при условии,
        ///     что из них можно составить
        ///     искомое слово с помощью метода <see cref="SearchResults.FindRelation(string,int,int)" />. При поиске слова
        ///     используется только
        ///     первая буква свойства <see cref="Processor.Tag" /> каждой искомой карты (хранящейся внутри текущего экземпляра
        ///     класса
        ///     <see cref="Reflex" />),
        ///     остальные буквы свойства <see cref="Processor.Tag" /> предназначены для случая, когда необходимо искать несколько
        ///     вариантов одной и той же карты. Поиск указанного слова производится без учёта регистра.
        ///     Этот метод работает как связка методов <see cref="Processor.GetEqual(ProcessorContainer)" /> и
        ///     <see cref="SearchResults.FindRelation(string,int,int)" />. Эту функцию безопасно могут использовать несколько
        ///     потоков одновременно, без блокировки.
        /// </summary>
        /// <param name="processor">Карта, на которой будет производиться поиск.</param>
        /// <param name="word">Искомое слово.</param>
        /// <returns>
        ///     Возвращает новый экземпляр <see cref="Reflex" /> в случае нахождения указанного слова на карте, в противном случае
        ///     -
        ///     <see langword="null" />.
        /// </returns>
        public Reflex FindRelation(Processor processor, string word)
        {
            if (processor == null)
                throw new ArgumentNullException(nameof(processor),
                    $"{nameof(FindRelation)}: Карта для поиска не указана (null).");
            if (word == null)
                throw new ArgumentNullException(nameof(word), $"{nameof(FindRelation)}: Искомое слово равно null.");
            if (word == string.Empty)
                throw new ArgumentException($"{nameof(FindRelation)}: Искомое слово не указано.", nameof(word));
            if (processor.Width < _seaProcessors.Width || processor.Height < _seaProcessors.Height)
                return null;
            string w = StripString(word);
            if (!IsMapsWord(w))
                return null;
            SearchResults searchResults = processor.GetEqual(_seaProcessors);
            if (!searchResults.FindRelation(w))
                return null;
            Reflex reflex = new Reflex(this);
            List<Reg> lstRegs = new List<Reg>();
            foreach (List<Reg> lstReg in w.Select(c => FindSymbols(c, searchResults)))
                lstRegs.AddRange(lstReg);
            foreach (Registered r in FindWord(lstRegs, w, searchResults.ResultSize).SelectMany(regs => regs))
                reflex.GetMap(processor, r);
            return reflex;
        }

        /// <summary>
        ///     Получает список коллекций областей, позволяющих выполнить поиск требуемого слова, т.е. искомое слово можно
        ///     составить из первых букв свойств
        ///     <see cref="Processor.Tag" /> карт, при условии отсутствия наложений одной карты на другую.
        /// </summary>
        /// <param name="regs">Список обрабатываемых карт, из которых требуется получить список коллекций областей.</param>
        /// <param name="word">Искомое слово.</param>
        /// <param name="mapSize">Размер поля результатов поиска, в которых требуется выполнить поиск требуемого слова.</param>
        /// <returns>Возвращает список коллекций областей, позволяющих выполнить поиск требуемого слова.</returns>
        static IEnumerable<IEnumerable<Registered>> FindWord(IList<Reg> regs, string word, Size mapSize)
        {
            if (regs == null)
                throw new ArgumentNullException(nameof(regs),
                    $"{nameof(FindWord)}: Список обрабатываемых карт равен null.");
            if (regs.Count <= 0)
                throw new ArgumentException(
                    $"{nameof(FindWord)}: Количество обрабатываемых карт должно быть больше ноля ({regs.Count}).",
                    nameof(regs));
            if (word == null)
                throw new ArgumentNullException(nameof(word),
                    $"{nameof(FindWord)}: Искомое слово должно быть указано (null).");
            if (word == string.Empty)
                throw new ArgumentException($"{nameof(FindWord)}: Искомое слово должно быть указано.", nameof(word));
            Size searchSize = regs[0].SelectedProcessor.Size;
            if (mapSize.Height < searchSize.Height)
                throw new ArgumentException(
                    $"{nameof(FindWord)}: Высота поля результатов поиска должна быть больше или равна высоте искомых карт.",
                    nameof(mapSize));
            if (mapSize.Width < searchSize.Width)
                throw new ArgumentException(
                    $"{nameof(FindWord)}: Ширина поля результатов поиска должна быть больше или равна ширине искомых карт.",
                    nameof(mapSize));
            int[] counting = null;
            Reg[] regsCounting = new Reg[word.Length];
            Region region = new Region(mapSize.Width, mapSize.Height);
            StringBuilder mapString = new StringBuilder(word.Length);
            TagSearcher mapSearcher = new TagSearcher(word);
            while (ChangeCount(ref counting, word.Length, regs.Count) >= 0)
            {
                bool result = true;
                for (int k = 0; k < counting.Length; k++)
                    regsCounting[k] = regs[counting[k]];
                foreach (Reg pp in regsCounting)
                {
                    Rectangle rect = new Rectangle(pp.Position, searchSize);
                    if (region.IsConflict(rect))
                    {
                        result = false;
                        break;
                    }

                    region.Add(rect, pp.SelectedProcessor, pp.Percent);
                    mapString.Append(pp.SelectedProcessor.Tag[0]);
                }

                if (result && mapSearcher.IsEqual(mapString.ToString()))
                    yield return region.Elements;
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
            if (counter <= 0)
                throw new ArgumentException(
                    $"{nameof(ChangeCount)}: Количество элементов в массиве-счётчике \"count\" должно быть больше ноля.",
                    nameof(counter));
            if (maxCount <= 0)
                throw new ArgumentOutOfRangeException(nameof(maxCount),
                    $@"{
                            nameof(ChangeCount)
                        }: Максимальное значение каждого разряда меньше или равно нолю ({maxCount}).");
            if (count != null)
            {
                if (count.Length != counter)
                    throw new ArgumentException(
                        $@"{nameof(ChangeCount)}: Количество элементов, указанных в параметре {nameof(counter)} ({
                                counter
                            }), не соответствует количеству элементов массива-счётчика {nameof(count)} ({
                                count.Length
                            }).",
                        nameof(counter));
                if (count.Length <= 0)
                    throw new ArgumentException(
                        $"{nameof(ChangeCount)}: Длина массива-счётчика должна быть больше ноля ({count.Length}).",
                        nameof(count));
                if (count.Length > maxCount)
                    throw new ArgumentException(
                        $@"{nameof(ChangeCount)}: Длина массива-счётчика ({
                                count.Length
                            }) должна быть меньше или равна максимальному значению каждого разряда ({maxCount}).");
            }

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
        ///     Удаляет из строки совпадающие символы.
        ///     Без учёта регистра.
        ///     Возвращает новую строку в верхнем регистре.
        /// </summary>
        /// <param name="str">Анализируемая строка.</param>
        /// <returns>Возвращает новую строку в верхнем регистре.</returns>
        static string StripString(string str)
        {
            if (str == null)
                throw new ArgumentNullException(nameof(str), $"{nameof(StripString)}: Искомое слово равно null.");
            if (str == string.Empty)
                throw new ArgumentException($"{nameof(StripString)}: Искомое слово не указано.", nameof(str));
            str = str.ToUpper();
            if (str.Length == 1)
                return str;
            StringBuilder sb = new StringBuilder(str);
            for (int k = 0; k < sb.Length; k++)
            for (int j = 0; j < sb.Length; j++)
                if (j != k && sb[j] == sb[k])
                    sb.Remove(j--, 1);
            return sb.ToString();
        }

        /// <summary>
        ///     Позволяет выяснить, содержит ли текущий экземпляр <see cref="Reflex" /> достаточное количество карт с
        ///     соответствующими значениями
        ///     первых символов свойств <see cref="Processor.Tag" /> для составления указанного слова.
        ///     Проверка производится без учёта регистра.
        ///     В случае успешной проверки возвращается значение <see langword="true" />, иначе <see langword="false" />.
        /// </summary>
        /// <param name="word">Проверяемое слово.</param>
        /// <returns>В случае успешной проверки возвращается значение <see langword="true" />, иначе <see langword="false" />.</returns>
        bool IsMapsWord(string word)
        {
            if (string.IsNullOrEmpty(word))
                return false;
            List<char> lstCh = word.Select(char.ToUpper).ToList();
            for (int k = 0; k < _seaProcessors.Count; k++)
            {
                char c = char.ToUpper(_seaProcessors[k].Tag[0]);
                for (int j = 0; j < lstCh.Count; j++)
                    if (lstCh[j] == c)
                        lstCh.RemoveAt(j--);
                if (lstCh.Count <= 0)
                    return true;
            }

            return lstCh.Count <= 0;
        }

        /// <summary>
        ///     Находит карты в <see cref="SearchResults" />, свойства <see cref="Processor.Tag" /> которых по первому символу
        ///     соответствуют указанному символу.
        ///     Поиск производится без учёта регистра.
        /// </summary>
        /// <param name="procName">Искомое название карты.</param>
        /// <param name="searchResults">Результаты поиска, в которых необходимо найти указанные карты.</param>
        /// <returns>Возвращает сведения о найденных картах.</returns>
        static List<Reg> FindSymbols(char procName, SearchResults searchResults)
        {
            if (searchResults == null)
                throw new ArgumentNullException(nameof(searchResults),
                    $@"{nameof(FindSymbols)}: Результаты поиска должны присутствовать.");
            procName = char.ToUpper(procName);
            List<Reg> lstRegs = new List<Reg>();
            for (int y = 0, my = searchResults.Height - searchResults.MapHeight; y <= my; y++)
            for (int x = 0, mx = searchResults.Width - searchResults.MapWidth; x <= mx; x++)
            {
                ProcPerc pp = searchResults[x, y];
                lstRegs.AddRange(from p in pp.Procs
                    where char.ToUpper(p.Tag[0]) == procName
                    select new Reg(new Point(x, y))
                    {
                        Percent = pp.Percent,
                        SelectedProcessor = p
                    });
            }

            return lstRegs;
        }
    }
}