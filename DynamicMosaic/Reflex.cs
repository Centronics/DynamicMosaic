using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using DynamicParser;

namespace DynamicMosaic
{
    /// <summary>
    ///     Предназначен для связывания карт.
    /// </summary>
    public sealed class Reflex
    {
        /// <summary>
        /// Слова, поиск которых производится при каждом запросе.
        /// </summary>
        readonly List<string> _lstWords = new List<string>();

        /// <summary>
        /// Карты, с помощью которых производится поиск запрашиваемых данных.
        /// </summary>
        readonly List<Processor> _lstProcessors = new List<Processor>();

        /// <summary>
        /// Список карт, задействованных в запросе поиска слов, которые сохраняются при задании каждого запроса.
        /// </summary>
        readonly List<Reflex> _lstReflex = new List<Reflex>();

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
                if (string.IsNullOrEmpty(words[k]))
                    throw new ArgumentNullException($"{nameof(VerifyWords)}: В коллекции слов найдена позиция, которая ничего не содержит.");
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
        /// Добавляет указанные карты, группируя их по размерам.
        /// Поиск по этим картам производится при каждом запросе.
        /// </summary>
        /// <param name="processors">Добавляемые карты.</param>
        public void Add(IList<Processor> processors)
        {
            if (processors == null || processors.Count <= 0)
                return;
            List<Processor> lstProcessors = new List<Processor>(processors);
            List<Processor> procs = new List<Processor>();
            while (lstProcessors.Count > 0)
            {
                Processor proc = lstProcessors[0];
                lstProcessors.RemoveAt(0);
                if (proc == null)
                    continue;
                procs.Clear();
                procs.Add(proc);
                for (int j = 0; j < lstProcessors.Count; j++)
                {
                    if (lstProcessors[j] == null)
                        continue;
                    if (lstProcessors[j].Width != procs[0].Width || lstProcessors[j].Height != procs[0].Height) continue;
                    procs.Add(lstProcessors[j]);
                    lstProcessors.RemoveAt(j--);
                }
                //AddToGroup
                //_lstProcessors.Add(new ProcessorContainer(procs));
            }
        }

        /// <summary>
        /// Добавляет указанные карты, сортируя их по размерам.
        /// Поиск по этим картам производится при каждом запросе.
        /// </summary>
        /// <param name="processors">Добавляемые карты.</param>
        public void Add(params Processor[] processors) => Add((IList<Processor>)processors);

        /// <summary>
        /// Производит поиск слова в имеющихся картах.
        /// Возвращает значение true в случае, если слово найдено, в противном случае возвращает значение false.
        /// </summary>
        /// <param name="processor">Анализируемая карта, на которой будет производиться поиск.</param>
        /// <param name="word">Искомое слово.</param>
        /// <returns>Возвращает список слов, которые так или иначе связаны с указанным словом.</returns>
        public ConcurrentBag<string> FindWord(Processor processor, string word)
        {
            if (processor == null || processor.Length <= 0 || string.IsNullOrEmpty(word) ||
                _lstWords == null || _lstWords.Count <= 0 || _lstProcessors == null || _lstProcessors.Count <= 0)
                return null;
            WordSearcher ws = new WordSearcher((from c in word select new string(c, 1)).ToArray());
            //необходимо добавлять не слова поиска, а присутствующие карты, задействованные в поиске слова, а так же все имеющиеся слова.
            ConcurrentBag<string> strings = processor.GetEqual(new ProcessorContainer(_lstProcessors)).FindRelation((IList<string>)_lstWords);

        }

        /// <summary>
        /// Позволяет узнать, состоит ли запрашиваемое слово из указанных символов.
        /// </summary>
        /// <param name="chars">Набор символов для проверки.</param>
        /// <param name="word">Проверяемое слово.</param>
        /// <returns>Возвращает значение true в случае успешной проверки, в противном случае - false.</returns>
        bool IsCharsInWord(ICollection<char> chars, string word)
        {
            if (chars == null || chars.Count <= 0)
                return false;
            char[] chs = chars.Select(char.ToUpper).ToArray();
            word = word.ToUpper();
            return !word.All(c => chs.Any(v => v == c));
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