using System;
using System.Collections.Generic;
using System.Linq;
using DynamicParser;
using System.Drawing;
using System.Text;
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
        /// Производит поиск указанного слова <see cref="string"/> на указанной карте <see cref="Processor"/>.
        /// Запоминает все предыдущие вызовы посредством обучения на основе сведений, которые ему удалось получить.
        /// Сведениями, которые запоминает класс <see cref="Reflex"/>, являются фрагменты распознаваемой карты, которые были с неё скопированы
        /// в процессе распознавания. Это места, в которых были найдены искомые карты.
        /// При этом карты сохраняются во внутреннюю коллекцию искомых карт <see cref="ProcessorContainer"/>.
        /// Карты сохраняются при условии отсутствия конфликтов между ними, т.е. при условии того, что из них можно составить искомое слово
        /// и отыскать его методом <see cref="SearchResults.FindRelation(string,int,int)"/>. При поиске слова используется только первая буква
        /// свойства <see cref="Processor.Tag"/> каждой искомой карты (хранящейся внутри экземпляра класса <see cref="Reflex"/>),
        /// остальные буквы в названии <see cref="Processor.Tag"/> служат для случая, когда необходимо искать несколько вариантов одной и той же карты.
        /// Поиск указанного слова производится без учёта регистра.
        /// В остальном, метод работает так же, как связка <see cref="Processor.GetEqual(DynamicParser.Processor[])"/> и
        /// <see cref="SearchResults.FindRelation(string,int,int)"/>. Таким образом, результаты всех последующих вызовов зависят от предыдущих вызовов,
        /// в том числе от того, какое слово и на какой карте искалось, так же важна последовательность этих вызовов.
        /// Возвращает значение <see langword="true"/> в случае нахождения указанного слова на карте, в противном случае - <see langword="false"/>.
        /// </summary>
        /// <param name="processor">Карта, на которой будет производиться поиск.</param>
        /// <param name="word">Искомое слово.</param>
        /// <returns>Возвращает значение <see langword="true"/> в случае нахождения указанного слова на карте, в противном случае - <see langword="false"/>.</returns>
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
            string w = StripString(word);
            if (!IsMapsWord(w))
                return false;
            SearchResults searchResults = processor.GetEqual(_seaProcessors);
            if (!searchResults.FindRelation(w))
                return false;
            List<Reg> lstRegs = new List<Reg>();
            foreach (List<Reg> lstReg in w.Select(c => FindSymbols(c, searchResults)))
                lstRegs.AddRange(lstReg);
            foreach (Registered r in FindWord(lstRegs, w.Length, searchResults.ResultSize).SelectMany(regs => regs))
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
            int[] counting = null;
            Reg[] regsCounting = new Reg[wordLength];
            DynamicParser.Region region = new DynamicParser.Region(mapSize.Width, mapSize.Height);
            while (ChangeCount(ref counting, wordLength, regs.Count) >= 0)
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
            }
        }

        /// <summary>
        ///     Увеличивает значение старших разрядов счётчика букв, если это возможно.
        ///     Возвращаются только случаи без совпадающих значений в различных разрядах.
        ///     Если увеличение было произведено, возвращается номер позиции, на которой произошло изменение, в противном случае
        ///     -1.
        /// </summary>
        /// <param name="count">Массив-счётчик.</param>
        /// <param name="counter">Количество элементов в массиве "count".</param>
        /// <param name="maxCount">Максимальное значение счётчика.</param>
        /// <returns>Возвращается номер позиции, на которой произошло изменение, в противном случае -1.</returns>
        static int ChangeCount(ref int[] count, int counter, int maxCount)
        {
            if (counter <= 0)
                throw new ArgumentException($"{nameof(ChangeCount)}: Количество элементов в массиве-счётчике \"count\" должно быть больше ноля.", nameof(counter));
            if (maxCount <= 0)
                throw new ArgumentOutOfRangeException(nameof(maxCount), $@"{nameof(ChangeCount)}: Максимальное значение счётчика меньше или равно нолю ({maxCount}).");
            if (count != null)
            {
                if (count.Length != counter)
                    throw new ArgumentException($@"{nameof(ChangeCount)}: Количество элементов, указанных в параметре {nameof(counter)
                        } ({counter}), не соответствует количеству элементов массива-счётчика {nameof(count)} ({count.Length}).", nameof(counter));
                if (count.Length <= 0)
                    throw new ArgumentException($"{nameof(ChangeCount)}: Длина массива-счётчика должна быть больше ноля ({count.Length}).", nameof(count));
                if (count.Length > maxCount)
                    throw new ArgumentException($@"{nameof(ChangeCount)}: Длина массива-счётчика ({count.Length
                        }) должна быть меньше или равна максимальному значению счётчика ({maxCount}).");
            }
            int maxPosition = counter - 1;
            if (count == null)
            {
                count = new int[counter];
                for (int k = 1; k < count.Length; k++)
                    count[k] = k;
                return maxPosition;
            }
            for (int k = count.Length - 1, mc = maxCount - 1; k >= 0; k--)
            {
                if (count[k] >= mc)
                    continue;
                count[k]++;
                for (int x = k + 1; x < count.Length; x++)
                {
                    int t = count[x - 1];
                    if (t >= mc)
                        return -1;
                    count[x] = t + 1;
                }
                return k;
            }
            return -1;
        }

        /// <summary>
        /// Удаляет из строки совпадающие символы.
        /// Без учёта регистра.
        /// Возвращает новую строку в верхнем регистре.
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