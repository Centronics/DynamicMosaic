using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynamicParser;

namespace DynamicMosaic
{
    /// <summary>
    ///     Предназначен для связывания карт.
    /// </summary>
    public sealed class Reflex : ICloneable
    {
        /// <summary>
        /// Слова, поиск которых производится при каждом запросе.
        /// </summary>
        readonly List<string> _lstWords = new List<string>();

        /// <summary>
        /// Карты, с помощью которых производится поиск запрашиваемых данных.
        /// </summary>
        ProcessorContainer _seaProcessors;

        /// <summary>
        /// Рефлексы, которые система испытывала при предыдущих запросах.
        /// </summary>
        readonly List<Reflex> _lstReflexs = new List<Reflex>();

        /// <summary>
        /// Получает карту, поиск которой производится при каждом запросе поиска слова.
        /// </summary>
        /// <param name="index">Индекс карты.</param>
        /// <returns>Возвращает карту, поиск которой производится при каждом запросе поиска слова.</returns>
        public Processor GetProcessorContainerAt(int index) => _seaProcessors[index];

        /// <summary>
        /// Получает слово, по которому производится поиск при каждом запросе поиска слова.
        /// </summary>
        /// <param name="index">Индекс слова.</param>
        /// <returns>Возвращает слово, поиск которого производится при каждом запросе поиска слова.</returns>
        public string GetCompareString(int index) => _lstWords[index];

        /// <summary>
        /// Получает количество карт в контексте.
        /// </summary>
        public int CountProcessor => _seaProcessors.Count;

        /// <summary>
        /// Получает количество слов в контексте.
        /// </summary>
        public int CountWords => _lstWords.Count;

        /// <summary>
        /// Получает количество рефлексов в контексте.
        /// </summary>
        public int CountReflexs => _lstReflexs.Count;

        /// <summary>
        /// Получает <see cref="Processor"/>, поле <see cref="Processor.Tag"/> которого начинается указанным символом.
        /// Поиск производится без учёта регистра.
        /// </summary>
        /// <param name="c">Искомый символ.</param>
        /// <returns>Возвращает <see cref="Processor"/>, поле <see cref="Processor.Tag"/> которого начинается указанным символом.</returns>
        public Processor GetMapByChar(char c)
        {
            for (int k = 0; k < _seaProcessors.Count; k++)
                if (char.ToUpper(_seaProcessors[k].Tag[0]) == c)
                    return _seaProcessors[k];
            return null;
        }

        /// <summary>
        /// Добавляет слова, поиск которых будет производиться при каждом запросе поиска слова.
        /// </summary>
        /// <param name="words">Добавляемые слова.</param>
        public void Add(IList<string> words)
        {
            if (words == null || words.Count <= 0)
                return;
            if (!VerifyWords(words))
                throw new ArgumentException($"{nameof(Add)}: В словах не должно быть повторяющихся букв.");
            _lstWords.AddRange(words.Select(t => t.ToUpper()));
        }

        /// <summary>
        /// Добавляет слова, поиск которых будет производиться при каждом запросе поиска слова.
        /// </summary>
        /// <param name="words">Добавляемые слова.</param>
        public void Add(params string[] words) => Add((IList<string>)words);

        /// <summary>
        /// Проверяет, встречаются ли в словах одинаковые буквы. Проверка производится без учёта регистра.
        /// В случае нахождения повторяющихся букв, возвращает значение false, в противном случае - true.
        /// </summary>
        /// <param name="words">Проверяемые слова.</param>
        /// <returns>В случае нахождения повторяющихся букв, возвращает значение false, в противном случае - true.</returns>
        public static bool VerifyWords(IList<string> words)
        {
            for (int k = 0; k < words.Count; k++)
            {
                if (words[k] == null)
                    throw new ArgumentNullException($"{nameof(VerifyWords)}: В коллекции слов найдена позиция, которая ничего не содержит (null).");
                if (words[k] == string.Empty)
                    throw new ArgumentException($"{nameof(VerifyWords)}: В коллекции слов найдена позиция, которая ничего не содержит.");
                if (words.Where((t, j) => j != k).Any(t => words[k].Any(c => SearchLetter(c, t))))
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Выполняет поиск заданного символа в заданном слове, без учёта регистра.
        /// </summary>
        /// <param name="ch">Искомый символ.</param>
        /// <param name="str">Строка, в которой необходимо произвести поиск.</param>
        /// <returns>Возвращает значение true в случае, когда искомый символ содержится в указанном слове, в противном случае возвращает значение false.</returns>
        static bool SearchLetter(char ch, string str)
        {
            if (str == null)
                throw new ArgumentNullException(nameof(str), $"{nameof(SearchLetter)}: Строка поиска должна содержать значение.");
            return str.ToUpper().Contains(char.ToUpper(ch));
        }

        /// <summary>
        /// Добавляет указанную карту.
        /// Поиск по добавленным картам производится при каждом запросе.
        /// </summary>
        /// <param name="processor">Добавляемая карта.</param>
        public void Add(Processor processor)
        {
            if (processor == null)
                return;
            if (_seaProcessors == null)
            {
                _seaProcessors = new ProcessorContainer(processor);
                return;
            }
            _seaProcessors.Add(processor);
        }

        /// <summary>
        /// Добавляет указанные карты.
        /// Поиск по добавленным картам производится при каждом запросе.
        /// </summary>
        /// <param name="processors">Добавляемые карты.</param>
        public void Add(IList<Processor> processors)
        {
            if (processors == null || processors.Count <= 0)
                return;
            if (_seaProcessors == null)
            {
                _seaProcessors = new ProcessorContainer(processors);
                return;
            }
            _seaProcessors.AddRange(processors);
        }

        /// <summary>
        /// Добавляет указанные карты.
        /// Поиск по добавленным картам производится при каждом запросе.
        /// </summary>
        /// <param name="processors">Добавляемые карты.</param>
        public void Add(params Processor[] processors)
        {
            if (processors == null || processors.Length <= 0)
                return;
            if (_seaProcessors == null)
            {
                _seaProcessors = new ProcessorContainer(processors);
                return;
            }
            _seaProcessors.AddRange((IList<Processor>)processors);
        }

        /// <summary>
        /// Проверяет, присутствуют ли одинаковые буквы в словарных запасах указанных <see cref="Reflex"/> или нет.
        /// </summary>
        /// <param name="reflex">Проверяемые контексты.</param>
        /// <returns>Возвращает значение true в случае пересечения контекстов, в противном случае возвращает значение false.</returns>
        public static bool IsConflict(params Reflex[] reflex)
        {
            if (reflex == null || reflex.Length <= 0)
                return false;
            List<string> lstStrings = new List<string>();
            foreach (Reflex r in reflex.Where(r => r != null))
                lstStrings.AddRange(r._lstWords);
            return VerifyWords(lstStrings);
        }

        /// <summary>
        /// Получает слова для проверки их существования в контексте класса <see cref="Reflex"/>.
        /// </summary>
        /// <param name="word">Искомое слово.</param>
        /// <returns>Возвращает слова для проверки их существования в контексте класса <see cref="Reflex"/>.</returns>
        public static IEnumerable<string> GetWords(string word)
        {
            if (string.IsNullOrEmpty(word))
                yield break;
            for (int k = 1; k <= word.Length; k++)
                for (int j = 0, max = word.Length - k; j <= max; j++)
                    yield return word.Substring(j, k);
        }

        /// <summary>
        /// Возвращает результат, обозначающий, находится ли заданное слово в текущем контексте класса <see cref="Reflex"/>.
        /// </summary>
        /// <param name="word">Искомое слово.</param>
        /// <returns>Возвращает слова в случае присутствия их в текущем контексте <see cref="Reflex"/>, в противном случае - null.</returns>
        public ConcurrentBag<string> FindWord(string word) => string.IsNullOrEmpty(word) ? null : FindWord(GetWords(word).ToArray());

        /// <summary>
        /// Возвращает результат, обозначающий, находятся ли заданные слова в текущем контексте класса <see cref="Reflex"/>.
        /// </summary>
        /// <param name="words">Искомые слова.</param>
        /// <returns>Возвращает слова в случае присутствия указанных слов в текущем контексте <see cref="Reflex"/>, в противном случае - null.</returns>
        public ConcurrentBag<string> FindWord(IList<string> words)
        {
            if (words == null)
                return null;
            WordSearcher ws = new WordSearcher(words);
            ConcurrentBag<string> lstResult = new ConcurrentBag<string>();
            string errString = string.Empty, errStopped = string.Empty;
            bool exThrown = false, exStopped = false;
            Parallel.ForEach(_lstWords, (k, state) =>
            {
                try
                {
                    if (ws.IsEqual(k))
                        lstResult.Add(k);
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
            return lstResult;
        }

        /// <summary>
        /// Производит поиск слова в имеющихся картах.
        /// Возвращает <see cref="Reflex"/>, который так или иначе связан с указанным словом или <see langword="null"/>, если связи нет.
        /// </summary>
        /// <param name="processor">Анализируемая карта, на которой будет производиться поиск.</param>
        /// <param name="word">Искомое слово.</param>
        /// <returns>Возвращает <see cref="Reflex"/>, который так или иначе связан с указанным словом или <see langword="null"/>, если связи нет.</returns>
        public Reflex FindWord(Processor processor, string word)
        {
            if (processor == null || processor.Length <= 0 || string.IsNullOrEmpty(word) ||
                _lstWords == null || _lstWords.Count <= 0 || _seaProcessors == null || _seaProcessors.Count <= 0 || !IsAllMaps(word))
                return null;
            char[] chars = (from c in word select c).Select(char.ToUpper).ToArray();
            List<string> lstFindStrings = new List<string>();
            object thisLock = new object();
            StringBuilder sb = new StringBuilder();
            string errString = string.Empty, errStopped = string.Empty;
            bool exThrown = false, exStopped = false;
            Parallel.ForEach(processor.GetEqual(_seaProcessors).FindRelation((IList<string>)_lstWords), (k, state) =>
            {
                try
                {
                    if (string.IsNullOrEmpty(k))
                        throw new Exception($"{nameof(FindWord)}: При чтении данных из внутреннего массива произошла ошибка.");
                    if (!IsCharsInWord(chars, k))
                        return;
                    lock (thisLock)
                    {
                        sb.Append(k);
                        lstFindStrings.Add(k);
                    }
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
            if (lstFindStrings.Count <= 0)
                return null;
            if (IsAllMaps(sb.ToString()))
                return this;
            Reflex reflex = new Reflex();
            foreach (char c in lstFindStrings.SelectMany(str => str))
                for (int k = 0; k < _seaProcessors.Count; k++)
                    if (char.ToUpper(_seaProcessors[k].Tag[0]) == c)
                        reflex.Add(_seaProcessors[k]);
            if (lstFindStrings.Count <= 0)
                return null;
            reflex.Add(lstFindStrings);
            Reflex r = FindSimilar(reflex);
            if (r != null) return r.FindWord(processor, word);
            if (IsSimilar(reflex))
                return this;
            ConcurrentBag<Reflex> lstReflexs = FindWordLst(word, processor);
            if (lstReflexs != null)
                reflex._lstReflexs.AddRange(lstReflexs);
            if (_lstReflexs.All(refl => !refl.IsSimilar(reflex)))
                _lstReflexs.Add(reflex);
            return reflex;
        }

        /// <summary>
        /// Находит указанное слово, используя карты, находящиеся в коллекции.
        /// </summary>
        /// <param name="word">Искомое слово.</param>
        /// <param name="processor">Карта, на которой производится поиск искомого слова.</param>
        /// <returns>Возвращает коллекцию подходящих карт.</returns>
        ConcurrentBag<Reflex> FindWordLst(string word, Processor processor)
        {
            if (_lstReflexs.Count <= 0)
                return null;
            if (word == null)
                throw new ArgumentNullException(nameof(word), $"{nameof(FindWordLst)}: Искомое слово должно быть указано.");
            if (word == string.Empty)
                throw new ArgumentException($"{nameof(FindWordLst)}: Искомое слово должно быть указано.", nameof(word));
            if (processor == null)
                throw new ArgumentNullException(nameof(processor), $"{nameof(FindWordLst)}: Карта, на которой производится поиск, не указана.");
            ConcurrentBag<Reflex> lstReflexs = new ConcurrentBag<Reflex>();
            string errString = string.Empty, errStopped = string.Empty;
            bool exThrown = false, exStopped = false;
            Parallel.ForEach(_lstReflexs, (k, state) =>
            {
                try
                {
                    Reflex reflex = k.FindWord(processor, word);
                    if (reflex == null)
                        return;
                    lstReflexs.Add(reflex);
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
            return lstReflexs;
        }

        /// <summary>
        /// Возвращает результат, говорящий о том, присутствует ли в текущем контексте <see cref="Reflex"/> с такими же поисковыми картами,
        /// как указано в запросе.
        /// </summary>
        /// <param name="reflex">Искомый контекст <see cref="Reflex"/>.</param>
        /// <returns>Возвращает результат true в случае нахождения подобной карты, false - в противном случае.</returns>
        bool IsSimilar(Reflex reflex)
        {
            if (reflex == null)
                throw new ArgumentNullException(nameof(reflex), $"{nameof(IsSimilar)}: Искомый контекст должен быть указан.");
            if (_seaProcessors.Count != reflex._seaProcessors.Count || _lstWords.Count != reflex._lstWords.Count)
                return false;
            for (int k = 0; k < _seaProcessors.Count; k++)
                if (!reflex._seaProcessors.ContainsTag(_seaProcessors[k].Tag))
                    return false;
            return _lstWords.All(t => reflex._lstWords.All(source => string.Compare(source, t, StringComparison.OrdinalIgnoreCase) != 0));
        }

        /// <summary>
        /// Проверяет, содержат ли карты в свойстве <see cref="Processor.Tag"/> все буквы указанного слова.
        /// </summary>
        /// <param name="word">Проверяемое слово.</param>
        /// <returns>Возвращает значение true в случае, если текущий экземпляр полностью описывает указанное слово, в противном случае - false.</returns>
        bool IsAllMaps(string word)
        {
            if (string.IsNullOrEmpty(word))
                throw new ArgumentNullException(nameof(word), $"{nameof(IsAllMaps)}: Искомое слово должно быть задано.");
            bool[] maps = new bool[_seaProcessors.Count];
            foreach (char c in word)
                for (int k = 0; k < _seaProcessors.Count; k++)
                    if (char.ToUpper(_seaProcessors[k].Tag[0]) == c)
                        maps[k] = true;
            return maps.All(b => b);
        }

        /// <summary>
        /// Позволяет узнать, состоит ли запрашиваемое слово из указанных символов.
        /// </summary>
        /// <param name="chars">Набор символов для проверки.</param>
        /// <param name="word">Проверяемое слово.</param>
        /// <returns>Возвращает значение true в случае успешной проверки, в противном случае - false.</returns>
        static bool IsCharsInWord(ICollection<char> chars, string word)
        {
            if (chars == null || chars.Count <= 0)
                return false;
            return word.All(chars.Contains);
        }

        /// <summary>
        /// Находит контекст, полностью соответствующий заданному по своему потенциалу.
        /// </summary>
        /// <param name="reflex">Искомый потенциал.</param>
        /// <returns>Возвращает контекст, полностью соответствующий заданному по своему потенциалу.</returns>
        Reflex FindSimilar(Reflex reflex) => _lstReflexs.FirstOrDefault(r => r.IsEqual(reflex));

        /// <summary>
        /// Сравнивает контексты по их потенциалам.
        /// </summary>
        /// <param name="reflex">Сопоставляемый контекст.</param>
        /// <returns>В случае соответствия возвращает значение true, в противном случае - false.</returns>
        public bool IsEqual(Reflex reflex)
        {
            if (reflex?.CountWords != CountWords || reflex.CountProcessor != CountProcessor)
                return false;
            if ((from str in _lstWords from s in reflex._lstWords where string.Compare(s, str, StringComparison.OrdinalIgnoreCase) != 0 select str).Any())
                return false;
            for (int k = 0; k < reflex.CountProcessor; k++)
                for (int j = 0; j < CountProcessor; j++)
                    if (string.Compare(reflex.GetProcessorContainerAt(k).Tag, GetProcessorContainerAt(j).Tag, StringComparison.OrdinalIgnoreCase) != 0)
                        return false;
            return true;
        }

        /// <summary>
        /// Создаёт полную копию текущего контекста.
        /// </summary>
        /// <returns>Возвращает полную копию текущего контекста.</returns>
        public object Clone()
        {
            Reflex reflex = new Reflex();
            for (int k = 0; k < _seaProcessors.Count; k++)
                reflex._seaProcessors.Add(_seaProcessors[k]);
            reflex._lstWords.AddRange(_lstWords);
            foreach (Reflex r in _lstReflexs)
                reflex._lstReflexs.Add((Reflex)r.Clone());
            return reflex;
        }
    }
}