using System;
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
    public sealed class Reflex
    {
        /// <summary>
        /// Содержит вложенные карты.
        /// </summary>
        readonly List<Reflex> _lstNested = new List<Reflex>();

        /// <summary>
        /// Слова, поиск которых производится при каждом запросе.
        /// </summary>
        readonly List<string> _lstWords = new List<string>();

        /// <summary>
        /// Карты, с помощью которых производится поиск запрашиваемых данных.
        /// </summary>
        readonly ProcessorContainer _lstProcessors = new ProcessorContainer();

        /// <summary>
        /// Получает карту, поиск которой производится при каждом запросе поиска слова.
        /// </summary>
        /// <param name="index">Индекс карты.</param>
        /// <returns>Возвращает карту, поиск которой производится при каждом запросе поиска слова.</returns>
        public Processor GetProcessorContainerAt(int index) => _lstProcessors[index];

        /// <summary>
        /// Получает слово, по которому производится поиск при каждом запросе поиска слова.
        /// </summary>
        /// <param name="index">Индекс слова.</param>
        /// <returns>Возвращает слово, поиск которого производится при каждом запросе поиска слова.</returns>
        public string GetCompareString(int index) => _lstWords[index];

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
            for (int k = 0; k < words.Count; k++)
                words[k] = words[k].ToUpper();
            _lstWords.AddRange(words);
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
            _lstProcessors.Add(processor);
        }

        /// <summary>
        /// Добавляет указанные карты.
        /// Поиск по добавленным картам производится при каждом запросе.
        /// </summary>
        /// <param name="processors">Добавляемые карты.</param>
        public void AddRange(IList<Processor> processors)
        {
            if (processors == null || processors.Count <= 0)
                return;
            _lstProcessors.AddRange(processors);
        }

        /// <summary>
        /// Добавляет указанные карты.
        /// Поиск по добавленным картам производится при каждом запросе.
        /// </summary>
        /// <param name="processors">Добавляемые карты.</param>
        public void AddRange(params Processor[] processors)
        {
            if (processors == null || processors.Length <= 0)
                return;
            _lstProcessors.AddRange((IList<Processor>)processors);
        }

        /// <summary>
        /// Производит поиск слова в имеющихся картах.
        /// Возвращает строку, которая так или иначе связана с указанным словом.
        /// </summary>
        /// <param name="processor">Анализируемая карта, на которой будет производиться поиск.</param>
        /// <param name="word">Искомое слово.</param>
        /// <returns>Возвращает строку, которая так или иначе связана с указанным словом.</returns>
        public string FindWord(Processor processor, string word)
        {
            if (processor == null || processor.Length <= 0 || string.IsNullOrEmpty(word) ||
                _lstWords == null || _lstWords.Count <= 0 || _lstProcessors == null || _lstProcessors.Count <= 0)
                return null;
            char[] chars = (from c in word select c).Select(char.ToUpper).ToArray();
            object locker = new object();
            StringBuilder bigStrings = new StringBuilder();
            List<string> lstFindStrings = new List<string>();
            string allerrString = string.Empty, allerrStopped = string.Empty;
            bool allexThrown = false, allexStopped = false;
            Parallel.ForEach(GetCollectionExclude(_lstProcessors), (pc, allstate) =>
            {
                try
                {
                    object thisLock = new object();
                    StringBuilder sb = new StringBuilder();
                    string errString = string.Empty, errStopped = string.Empty;
                    bool exThrown = false, exStopped = false;
                    Parallel.ForEach(processor.GetEqual(pc).FindRelation((IList<string>)_lstWords), (k, state) =>
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
                    lock (locker)
                        bigStrings.Append(sb.ToString());
                }
                catch (Exception ex)
                {
                    try
                    {
                        allerrString = ex.Message;
                        allexThrown = true;
                        allstate.Stop();
                    }
                    catch (Exception ex1)
                    {
                        allerrStopped = ex1.Message;
                        allexStopped = true;
                    }
                }
            });
            if (allexThrown)
                throw new Exception(allexStopped ? $@"{allerrString}{Environment.NewLine}{allerrStopped}" : allerrString);
            string result = bigStrings.ToString();
            if (IsAllMaps(result))
                return result;
            //Собрать слова и карты, затем создать новый Reflex.
            return result;
        }

        /// <summary>
        /// Проверяет, содержат ли карты в своих именах все буквы указанного слова.
        /// </summary>
        /// <param name="word">Проверяемое слово.</param>
        /// <returns>Возвращает значение true в случае, если текущий экземпляр полностью описывает указанное слово, в противном случае - false.</returns>
        bool IsAllMaps(string word)
        {
            if (string.IsNullOrEmpty(word))
                throw new ArgumentNullException(nameof(word), $"{nameof(IsAllMaps)}: Искомое слово должно быть задано.");
            for (int k = 0; k < _lstProcessors.Count; k++)
                if (!_lstProcessors[k].IsProcessorName(word, 0))
                    return false;
            return true;
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
            return !word.All(c => chars.Any(v => v == c));
        }

        /// <summary>
        /// Получает коллекцию карт методом исключения одной карты из существующей коллекции.
        /// </summary>
        /// <param name="pc">Исходная коллекция карт.</param>
        /// <returns>Возвращает коллекцию карт методом исключения одной карты из существующей коллекции.</returns>
        IEnumerable<ProcessorContainer> GetCollectionExclude(ProcessorContainer pc)
        {
            if (pc == null)
                throw new ArgumentNullException(nameof(pc), $"{nameof(GetCollectionExclude)}: Коллекция отсутствует.");
            if (pc.Count <= 0)
                throw new ArgumentException($"{nameof(GetCollectionExclude)}: Коллекция карт не может быть пустой.", nameof(pc));
            for (int k = 0, h = pc.Count - 1; k < h; k++)
            {
                Processor[] procs = new Processor[h];
                for (int j = 0, n = 0; j < pc.Count; j++, n++)
                    if (j != k)
                        procs[n] = pc[j];
                ProcessorContainer container = new ProcessorContainer(procs);
                yield return container;
            }
        }
    }
}