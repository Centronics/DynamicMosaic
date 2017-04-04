using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DynamicParser;

namespace DynamicMosaic
{
    /// <summary>
    ///     Предназначен для связывания карт.
    /// </summary>
    public sealed class RegionMemory
    {
        /// <summary>
        /// 
        /// </summary>
        readonly List<string> _lstWords = new List<string>();

        /// <summary>
        /// 
        /// </summary>
        readonly List<ProcessorContainer> _lstProcessors = new List<ProcessorContainer>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="words"></param>
        public void Add(IList<string> words)
        {
            if (words == null || words.Count <= 0)
                return;
            if (!VerifyWords(words))
                throw new ArgumentException($"{nameof(Add)}: В словах не должно быть повторяющихся букв.");
            _lstWords.AddRange(words);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="words"></param>
        /// <returns></returns>
        public static bool VerifyWords(IList<string> words)
        {
            for (int k = 0; k < words.Count; k++)
            {
                if (string.IsNullOrEmpty(words[k]))
                    throw new ArgumentNullException($"{nameof(VerifyWords)}: В коллекции слов найдено {nameof(string.IsNullOrEmpty)}.");
                if (words.Where((t, j) => j != k).Any(t => words[k].Any(c => SearchLetter(c, t))))
                    return false;
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ch"></param>
        /// <param name="str"></param>
        /// <returns></returns>
        static bool SearchLetter(char ch, string str)
        {
            if (str == null)
                throw new ArgumentNullException(nameof(str), $"{nameof(SearchLetter)}: Строка для поиска должна содержать значение.");
            return str.ToUpper().Contains(char.ToUpper(ch));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="processors"></param>
        public void Add(IList<Processor> processors)
        {
            if (processors == null || processors.Count <= 0)
                return;
            foreach (Processor processor in processors)
                _lstProcessors.Add(new ProcessorContainer(processor));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="processor"></param>
        public void Add(Processor processor)
        {
            if (processor == null || processor.Length <= 0)
                return;
            _lstProcessors.Add(new ProcessorContainer(processor));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public IEnumerable<string> FindRelation(string word)
        {
            for (int j = 0; j < word.Length; j++)
                for (int k = 1; k <= word.Length; k++)
                    if (j + k >= word.Length)
                        yield return word.Substring(j, k);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="processor"></param>
        /// <returns></returns>
        public ConcurrentBag<string> FindRelation(Processor processor)
        {
            if (_lstWords == null || _lstWords.Count <= 0)
                return new ConcurrentBag<string>();
            SearchResults[] lstStrings = processor.GetEqual(_lstProcessors);
            ConcurrentBag<string> bag = new ConcurrentBag<string>();
            string errString = string.Empty, errStopped = string.Empty;
            bool exThrown = false, exStopped = false;
            Parallel.ForEach(_lstWords, (word, state) =>
            {
                try
                {
                    if (IsEqualWord(word, lstStrings))
                        bag.Add(word);
                }
                catch (Exception ex)
                {
                    try
                    {
                        errString = ex.Message;
                        exThrown = true;
                        state.Stop();
                    }
                    catch (Exception ex1)
                    {
                        errStopped = ex1.Message;
                        exStopped = true;
                    }
                }
            });
            if (exThrown)
                throw new Exception(exStopped ? $@"{errString}{Environment.NewLine}{errStopped}" : errString);
            return bag;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="word"></param>
        /// <param name="sr"></param>
        /// <returns></returns>
        static bool IsEqualWord(string word, IEnumerable<SearchResults> sr)
        {
            string errString = string.Empty, errStopped = string.Empty;
            bool exThrown = false, exStopped = false, result = false;
            Parallel.ForEach(sr, (k, state) =>
            {
                try
                {
                    if (!k.FindRelation(word))
                        return;
                    result = true;
                    state.Stop();
                }
                catch (Exception ex)
                {
                    try
                    {
                        errString = ex.Message;
                        exThrown = true;
                        state.Stop();
                    }
                    catch (Exception ex1)
                    {
                        errStopped = ex1.Message;
                        exStopped = true;
                    }
                }
            });
            if (exThrown)
                throw new Exception(exStopped ? $@"{errString}{Environment.NewLine}{errStopped}" : errString);
            return result;
        }

        /// <summary>
        /// Сортирует все карты по возрастанию.
        /// </summary>
        //void SortLayers()
        //{
        //    for (int k = 0; k < _lstProcessors.Count; k++)
        //    {
        //        for (int j = k + 1; j < _lstProcessors.Count; j++)
        //        {
        //            if (_lstProcessors[j].Length >= _lstProcessors[k].Length) continue;
        //            Processor processor = _lstProcessors[j];
        //            _lstProcessors[j] = _lstProcessors[k];
        //            _lstProcessors[k] = processor;
        //        }
        //    }
        //}









        /// <summary>
        ///     Текущие связываемые карты.
        /// </summary>
        readonly ProcessorContainer _currentProcessors;

        /// <summary>
        ///     Список классов для сопоставления. Чем больше индекс сопоставления, тем выше его уровень.
        /// </summary>
        readonly List<RegionMemory> _nextLinkedRegion = new List<RegionMemory>();

        /// <summary>
        ///     Инициализирует текущий экземпляр указанными картами.
        /// </summary>
        /// <param name="pc">Карты, которые необходимо ассоциировать.</param>
        public RegionMemory(ProcessorContainer pc)
        {
            if (pc == null)
                throw new ArgumentNullException(nameof(pc),
                    $"{nameof(RegionMemory)}: Сопоставляемые карты должны быть указаны.");
            if (pc.Count <= 0)
                throw new ArgumentException(
                    $"{nameof(RegionMemory)}: Количество сопоставляемых карт должно быть больше ноля.", nameof(pc));
            _currentProcessors = new ProcessorContainer(pc[0]);
            for (int k = 1; k < pc.Count; k++)
                _currentProcessors.Add(pc[k]);
            Width = pc.Width;
            Height = pc.Height;
        }

        /// <summary>
        ///     Размер связываемых карт по ширине.
        /// </summary>
        public int Width { get; }

        /// <summary>
        ///     Размер связываемых карт по высоте.
        /// </summary>
        public int Height { get; }

        /// <summary>
        /// Получает длину (<see cref="Width"/> * <see cref="Height"/>) связываемых карт.
        /// </summary>
        public int Length => Width * Height;

        /// <summary>
        /// Хранит значение, показывающее, требуется ли перед началом работы выполнить сортировку или нет.
        /// </summary>
        bool _sorted;

        /// <summary>
        /// Получает требуемый класс для сопоставления.
        /// </summary>
        /// <param name="index">Индекс требуемой ассоциации.</param>
        public RegionMemory GetAssociation(int index) => _nextLinkedRegion[index];

        /// <summary>
        /// Получает требуемую связываемую карту.
        /// </summary>
        public Processor GetRelative(int index) => _currentProcessors[index];

        /// <summary>
        /// Получает количество классов для сопоставления.
        /// </summary>
        public int CountAssociation => _nextLinkedRegion.Count;

        /// <summary>
        /// Получает количество связываемых карт.
        /// </summary>
        public int CountRelation => _nextLinkedRegion.Count;

        /// <summary>
        ///     Добавляет новые карты, которые необходимо ассоциировать с текущим объектом.
        ///     В случае необходимости создать слой для добавления карты, слой создаётся автоматически.
        /// </summary>
        /// <param name="pc">Карты, которые необходимо ассоциировать.</param>
        public void AddAssociation(ProcessorContainer pc)
        {
            if (pc == null)
                throw new ArgumentNullException(nameof(pc),
                    $"{nameof(AddAssociation)}: Ассоциируемые карты должны быть указаны.");
            if (pc.Count <= 0)
                return;
            _sorted = false;
            if (pc.Width == Width && pc.Height == Height)
            {
                for (int k = 0; k < pc.Count; k++)
                    _currentProcessors.Add(pc[k]);
                return;
            }
            RegionMemory rm = FindRegion(pc.Width, pc.Height);
            if (rm == null)
            {
                _nextLinkedRegion.Add(new RegionMemory(pc));
                return;
            }
            rm.AddAssociation(pc);
        }

        /// <summary>
        /// Предназначен для анализа указанной карты с применением карт текущего уровня.
        /// </summary>
        /// <param name="processor">Исследуемая карта.</param>
        /// <param name="word">Искомое слово.</param>
        /// <param name="startIndex">Индекс, начиная с которого будет сформирована строка названия карты.</param>
        /// <param name="charCount">Количество символов для выборки из названия карты, оно должно быть кратно длине искомого слова.</param>
        /// <param name="interrupter">Функция, обрабатывающая состояние перехода от одного этапа обработки данных к следующему.</param>
        /// <returns>Возвращает экземпляр класса <see cref="WordSearcher"/> в случае нахождения искомого слова, в противном случае - null.</returns>
        public WordSearcher Recognize(Processor processor, string word, int startIndex, int charCount, Interrupter interrupter)
        {
            if (processor == null)
                throw new ArgumentNullException(nameof(processor), $"{nameof(Recognize)}: Анализируемая карта отсутствует.");
            if (interrupter == null)
                throw new ArgumentNullException(nameof(interrupter), $"{nameof(Recognize)}: Функция обработки состояния перехода отсутствует (null).");
            SortLayers();
            List<string> strs = new List<string>();
            RegionInfo? ri = new RegionInfo { CharCount = charCount, CurrentProcessor = processor, StartIndex = startIndex, Word = word };
            bool res = ri.Value.CurrentProcessor.GetEqual(_currentProcessors).FindRelation(ri.Value.Word, ri.Value.StartIndex, ri.Value.CharCount);
            strs.Add(ri.Value.Word);
            ri = interrupter(ri.Value.Word, ri.Value.CurrentProcessor, ri.Value.StartIndex, ri.Value.CharCount, res);
            if (ri == null || !res)
                return null;
            // ReSharper disable once LoopCanBePartlyConvertedToQuery
            foreach (RegionMemory regionMemory in _nextLinkedRegion)
            {
                bool result = ri.Value.CurrentProcessor.GetEqual(regionMemory._currentProcessors)
                    .FindRelation(ri.Value.Word, ri.Value.StartIndex, ri.Value.CharCount);
                ri = interrupter(ri.Value.Word, ri.Value.CurrentProcessor, ri.Value.StartIndex, ri.Value.CharCount, result);
                if (ri == null || !result)
                    return null;
                if (ri.Value.CurrentProcessor == null)
                    throw new ArgumentNullException(nameof(interrupter), $"{nameof(Recognize)}: Обрабатываемая карта отсутствует (null).");
                strs.Add(ri.Value.Word);
            }
            return new WordSearcher(strs);
        }

        /// <summary>
        /// Суммирует ассоциации указанных классов при условии равенства всех уровней по ширине и высоте.
        /// </summary>
        /// <param name="rm">Суммируемый регион.</param>
        public void Add(RegionMemory rm)
        {
            if (rm == null)
                throw new ArgumentNullException(nameof(rm), $"{nameof(Add)}: Суммируемый регион равен null.");
            SortLayers();
            rm.SortLayers();
            if (_nextLinkedRegion.Count != rm._nextLinkedRegion.Count)
                throw new ArgumentException($@"{nameof(Add)}: Количество уровней связываемых должно быть одинаково: ({_nextLinkedRegion.Count} связывается с {
                    rm._nextLinkedRegion.Count}).");
            if (_currentProcessors.Width != rm._currentProcessors.Width)
                throw new ArgumentException($@"{nameof(Add)}: Ассоциируемые классы должны быть равны по размерам содержащихся в них карт (текущая ширина:{
                    _currentProcessors.Width}; сопоставляемая ширина: {rm._currentProcessors.Width}).", nameof(rm));
            if (_currentProcessors.Height != rm._currentProcessors.Height)
                throw new ArgumentException($@"{nameof(Add)}: Ассоциируемые классы должны быть равны по размерам содержащихся в них карт (текущая высота:{
                    _currentProcessors.Height}; сопоставляемая высота: {rm._currentProcessors.Height}).", nameof(rm));
            for (int k = 0; k < _nextLinkedRegion.Count; k++)
            {
                if (_nextLinkedRegion[k].Width != rm._nextLinkedRegion[k].Width)
                    throw new ArgumentException($@"{nameof(Add)}: Ассоциируемые классы должны быть равны по размерам содержащихся в них карт (текущая ширина:{
                    _nextLinkedRegion[k].Width}; сопоставляемая ширина: {rm._nextLinkedRegion[k].Width}).", nameof(rm));
                if (_nextLinkedRegion[k].Height != rm._nextLinkedRegion[k].Height)
                    throw new ArgumentException($@"{nameof(Add)}: Ассоциируемые классы должны быть равны по размерам содержащихся в них карт (текущая высота:{
                    _nextLinkedRegion[k].Height}; сопоставляемая высота: {rm._nextLinkedRegion[k].Height}).", nameof(rm));
            }
            for (int k = 0; k < _nextLinkedRegion.Count; k++)
                _nextLinkedRegion[k].Add(rm._nextLinkedRegion[k]);
        }

        /// <summary>
        /// Сортирует все слои по возрастанию.
        /// </summary>
        //void SortLayers()
        //{
        //    if (_sorted)
        //        return;
        //    for (int k = 0; k < _nextLinkedRegion.Count; k++)
        //    {
        //        for (int j = k + 1; j < _nextLinkedRegion.Count; j++)
        //        {
        //            if (_nextLinkedRegion[j].Length >= _nextLinkedRegion[k].Length) continue;
        //            RegionMemory regionMemory = _nextLinkedRegion[j];
        //            _nextLinkedRegion[j] = _nextLinkedRegion[k];
        //            _nextLinkedRegion[k] = regionMemory;
        //        }
        //    }
        //    _sorted = true;
        //}

        /// <summary>
        ///     Выполняет поиск и возвращает <see cref="RegionMemory" /> или null в зависимости от того, содержится указанный
        ///     уровень или нет.
        /// </summary>
        /// <param name="width">Искомая ширина.</param>
        /// <param name="height">Искомая высота.</param>
        /// <returns>Возвращает <see cref="RegionMemory" /> или null в зависимости от того, содержится указанный уровень или нет.</returns>
        RegionMemory FindRegion(int width, int height)
        {
            if (width <= 0)
                throw new ArgumentException($"{nameof(FindRegion)}: Ширина меньше или равна нолю ({width}).",
                    nameof(width));
            if (height <= 0)
                throw new ArgumentException($"{nameof(FindRegion)}: Высота меньше или равна нолю ({height}).",
                    nameof(height));
            if (Width == width && Height == height)
                return this;
            return _nextLinkedRegion.FirstOrDefault(rm => rm.Width == width && rm.Height == height);
        }

        /// <summary>
        ///     Выполняет поиск и возвращает значение, показывающее, содержится ли такой уровень или нет.
        /// </summary>
        /// <param name="width">Искомая ширина.</param>
        /// <param name="height">Искомая высота.</param>
        /// <returns>Возвращает значение true в случае нахождения указанного уровня, в противном случае - false.</returns>
        public bool Contains(int width, int height) => FindRegion(width, height) != null;
    }
}