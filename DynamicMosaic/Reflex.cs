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
    /// Служит для поиска данных на карте и сохранения результатов поиска для последующих вызовов.
    /// Они позволят системе лучше ориентиоваться при поиске данных, сопоставляя каждую новую карту с тем, что она уже обрабатывала.
    /// </summary>
    public sealed class Reflex
    {
        /// <summary>
        /// Карты, с помощью которых производится поиск запрашиваемых данных.
        /// </summary>
        readonly ProcessorContainer _seaProcessors;

        /// <summary>
        /// Инициализирует текущий контекст указанными картами. Карты предназначены для поиска запрашиваемых данных.
        /// Нужны для вызова <see cref="Processor.GetEqual(ProcessorContainer)"/>. Этот список невозможно изменить вручную в процессе работы с классом.
        /// </summary>
        /// <param name="processors">Карты, которые необходимо добавить в контекст <see cref="Reflex"/>.
        /// С помощью них будет проводиться поиск запрашиваемых данных.</param>
        public Reflex(ProcessorContainer processors)
        {
            if (processors == null)
                throw new ArgumentNullException(nameof(processors), $"{nameof(Reflex)}: Карты должны быть добавлены в контекст (null).");
            if (processors.Count < 2)
                throw new ArgumentException($"{nameof(Reflex)}: В контексте должны присутствовать минимум две карты ({nameof(processors.Count)} = {processors.Count}).");
            Processor[] procs = new Processor[processors.Count];
            for (int k = 0; k < procs.Length; k++)
                procs[k] = processors[k];
            _seaProcessors = new ProcessorContainer(procs);
        }

        /// <summary>
        /// Получает карту из целевой карты по указанным координатам, копируя данные карты в массив карт для поиска <see cref="_seaProcessors"/>.
        /// </summary>
        /// <param name="processor">Карта, из которой необходимо получить целевую карту.</param>
        /// <param name="registered">Информация о получаемой карте.</param>
        void GetMap(Processor processor, Registered registered)
        {
            if (processor == null)
                throw new ArgumentNullException(nameof(processor), $@"{nameof(GetMap)}: Исходная карта должна быть указана.");
            if (registered == null)
                throw new ArgumentNullException(nameof(registered), $@"{nameof(GetMap)}: Информация о получаемой карте должна быть указана.");
            if (registered.X < 0)
                throw new ArgumentException($"{nameof(GetMap)}: Координата {nameof(Registered.X)} получаемой карты меньше ноля ({registered.X}).", nameof(registered));
            if (registered.Y < 0)
                throw new ArgumentException($"{nameof(GetMap)}: Координата {nameof(Registered.Y)} получаемой карты меньше ноля ({registered.Y}).", nameof(registered));
            if (registered.Right > processor.Width)
                throw new ArgumentException($"{nameof(GetMap)}: Данные о получаемой карте некорректны: выход за предел карты по ширине.", nameof(registered));
            if (registered.Bottom > processor.Height)
                throw new ArgumentException($"{nameof(GetMap)}: Данные о получаемой карте некорректны: выход за предел карты по высоте.", nameof(registered));
            SignValue[,] values = AddMap(processor, registered);
            if (values == null)
                return;
            string tag = registered.Register.SelectedProcessor.Tag;
            while (_seaProcessors.ContainsTag(tag))
                tag += '0';
            _seaProcessors.Add(new Processor(values, tag));
        }

        /// <summary>
        /// Определяет, содержит ли текущий экземпляр <see cref="Reflex"/> карту с совпадающим содержимым или нет.
        /// В случае нахождения карт с совпадающим содержимым, метод возвращает значение <see langword="null"/>, в противном случае -
        /// массив данных, который указан в параметре <see cref="Registered"/>.
        /// </summary>
        /// <param name="processor">Карта, из которой необходимо получить целевую карту.</param>
        /// <param name="registered">Данные о получаемой карте.</param>
        /// <returns>В случае нахождения карты с совпадающим содержимым возвращает значение <see langword="true"/>, в противном случае - <see langword="false"/>.</returns>
        SignValue[,] AddMap(Processor processor, Registered registered)
        {
            if (processor == null)
                throw new ArgumentNullException(nameof(processor), $"{nameof(AddMap)}: Карта, из которой необходимо получить целевую карту, должна быть указана.");
            if (registered == null)
                throw new ArgumentNullException(nameof(registered), $"{nameof(AddMap)}: Информация о получаемой карте должна присутствовать.");
            if (registered.X < 0)
                throw new ArgumentException($"{nameof(AddMap)}: Координата {nameof(Registered.X)} получаемой карты меньше ноля ({registered.X}).", nameof(registered));
            if (registered.Y < 0)
                throw new ArgumentException($"{nameof(AddMap)}: Координата {nameof(Registered.Y)} получаемой карты меньше ноля ({registered.Y}).", nameof(registered));
            if (registered.Right > processor.Width)
                throw new ArgumentException($"{nameof(AddMap)}: Данные о получаемой карте некорректны: выход за предел карты по ширине.", nameof(registered));
            if (registered.Bottom > processor.Height)
                throw new ArgumentException($"{nameof(AddMap)}: Данные о получаемой карте некорректны: выход за предел карты по высоте.", nameof(registered));
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
        /// Производит поиск слова в текущем контексте <see cref="Reflex"/>.
        /// Возвращает значение <see langword="true"/> в случае нахождения указанного слова на карте, в противном случае - <see langword="false"/>.
        /// </summary>
        /// <param name="processor">Карта, на которой будет производиться поиск.</param>
        /// <param name="word">Искомое слово.</param>
        /// <returns>
        /// Возвращает значение <see langword="true"/> в случае нахождения указанного слова на карте, в противном случае - <see langword="false"/>.
        /// </returns>
        public bool FindRelation(Processor processor, string word)
        {
            if (processor == null)
                throw new ArgumentNullException(nameof(processor), $"{nameof(FindWord)}: Карта для поиска не указана (null).");
            if (word == null)
                throw new ArgumentNullException(nameof(word), $"{nameof(FindWord)}: Искомое слово равно null.");
            if (word == string.Empty)
                throw new ArgumentException($"{nameof(FindWord)}: Искомое слово не указано.", nameof(word));
            if (processor.Width < _seaProcessors.Width || processor.Height < _seaProcessors.Height)
                return false;
            if (!IsMapsWord(word))
                return false;
            SearchResults searchResults = processor.GetEqual(_seaProcessors);
            if (!searchResults.FindRelation(word))
                return false;
            List<Reg> lstRegs = new List<Reg>();
            foreach (List<Reg> lstReg in word.Select(c => FindSymbols(c, searchResults)))
                lstRegs.AddRange(lstReg);
            foreach (Registered r in FindWord(lstRegs, word.Length, searchResults.ResultSize).SelectMany(regs => regs))
                GetMap(processor, r);
            return true;
        }

        /// <summary>
        ///     Получает массив карт, представляющих заданное слово, т.е. искомое слово можно составить из первых букв названий карт.
        /// </summary>
        /// <param name="regs">Список обрабатываемых карт.</param>
        /// <param name="wordLength">Искомое слово.</param>
        /// <param name="mapSize">Размер поля результатов поиска, в которых планируется выполнить поиск требуемого слова.</param>
        /// <returns>Возвращает <see cref="WordSearcher" />, который позволяет выполнить поиск требуемого слова.</returns>
        static IEnumerable<IEnumerable<Registered>> FindWord(IList<Reg> regs, int wordLength, Size mapSize)
        {
            if (regs == null)
                throw new ArgumentNullException(nameof(regs), $"{nameof(FindWord)}: Список обрабатываемых карт равен null.");
            if (regs.Count <= 0)
                throw new ArgumentException($"{nameof(FindWord)}: Количество обрабатываемых карт должно быть больше ноля ({regs.Count}).", nameof(regs));
            if (wordLength <= 0)
                throw new ArgumentException($"{nameof(FindWord)}: Длина искомого слова должна быть указана.", nameof(wordLength));
            Size searchSize = regs[0].SelectedProcessor.Size;
            if (mapSize.Height < searchSize.Height)
                throw new ArgumentException(
                    $"{nameof(FindWord)}: Высота поля результатов поиска должна быть больше или равна высоте искомых карт.", nameof(mapSize));
            if (mapSize.Width < searchSize.Width)
                throw new ArgumentException(
                    $"{nameof(FindWord)}: Ширина поля результатов поиска должна быть больше или равна ширине искомых карт.", nameof(mapSize));
            int[] counting = new int[wordLength];
            Reg[] regsCounting = new Reg[wordLength];
            DynamicParser.Region region = new DynamicParser.Region(mapSize.Width, mapSize.Height);
            if (counting.Length > 1)
                if (ChangeCount(counting, regs.Count) < 0)
                    yield break;
            do
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
                }
                if (result)
                    yield return region.Elements;
                region.Clear();
            } while (ChangeCount(counting, regs.Count) >= 0);
        }

        /// <summary>
        ///     Увеличивает значение старших разрядов счётчика букв, если это возможно.
        ///     Возвращаются только случаи без совпадающих значений в различных разрядах.
        ///     Если увеличение было произведено, возвращается номер позиции, на которой произошло изменение, в противном случае
        ///     -1.
        /// </summary>
        /// <param name="count">Массив-счётчик.</param>
        /// <param name="maxCount">Максимальное значение счётчика.</param>
        /// <returns>Возвращается номер позиции, на которой произошло изменение, в противном случае -1.</returns>
        static int ChangeCount(IList<int> count, int maxCount)
        {
            if (count == null)
                throw new ArgumentNullException(nameof(count), $"{nameof(ChangeCount)}: Массив-счётчик не указан (null).");
            if (count.Count <= 0)
                throw new ArgumentException($"{nameof(ChangeCount)}: Длина массива-счётчика должна быть больше ноля ({count.Count}).", nameof(count));
            if (maxCount <= 0)
                throw new ArgumentOutOfRangeException(nameof(maxCount), $@"{nameof(ChangeCount)}: Максимальное значение счётчика меньше или равно нолю ({maxCount}).");
            for (int k = count.Count - 1, mc = maxCount - 1; k >= 0; k--)
            {
                if (count[k] >= mc) continue;
                count[k]++;
                for (int x = k + 1; x < count.Count; x++)
                    count[x] = 0;
                if (!NumberRepeat(count))
                    return k;
            }
            return -1;
        }

        /// <summary>
        /// В случае обнаружения повторяющихся значений в массиве возвращает значение <see langword="true"/>, в противном случае - <see langword="false"/>.
        /// </summary>
        /// <param name="count">Проверяемый массив.</param>
        /// <returns>В случае обнаружения повторяющихся значений в массиве возвращает значение <see langword="true"/>, в противном случае - <see langword="false"/>.</returns>
        static bool NumberRepeat(IList<int> count)
        {
            if (count == null)
                throw new ArgumentNullException(nameof(count), $"{nameof(NumberRepeat)}: Проверяемый массив должен быть указан.");
            return count.Where((element, k) => count.Where((t, j) => j != k).Any(t => t == element)).Any();
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
        ///     Находит карты в результатах поиска, поля <see cref="Processor.Tag" /> которых по первому символу соответствуют указанному символу.
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