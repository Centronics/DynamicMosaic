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
    public sealed class Reflex
    {
        /// <summary>
        /// Слова, поиск которых производится при каждом запросе.
        /// </summary>
        readonly List<string> _lstWords = new List<string>();

        /// <summary>
        /// Карты, с помощью которых производится поиск запрашиваемых данных.
        /// </summary>
        readonly List<ProcessorContainer> _lstProcessors = new List<ProcessorContainer>();

        /// <summary>
        /// Список карт, задействованных в запросе поиска слов, которые сохраняются при задании каждого запроса.
        /// </summary>
        readonly List<List<Reflex>> _lstReflex = new List<List<Reflex>>();

        /// <summary>
        /// Статус сортировки.
        /// </summary>
        bool _sorted;

        /// <summary>
        /// Получает карту, поиск которой производится при каждом запросе поиска слова.
        /// </summary>
        /// <param name="index">Индекс карты.</param>
        /// <returns>Возвращает карту, поиск которой производится при каждом запросе поиска слова.</returns>
        public ProcessorContainer GetProcessorContainerAt(int index) => _lstProcessors[index];

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
        /// Добавляет содержимое указанного контейнера в общий контейнер карт равного размера.
        /// </summary>
        /// <param name="pc">Добавляемый контейнер.</param>
        void AddToGroup(ProcessorContainer pc)
        {
            
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
            _sorted = false;
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
            if (processor == null || processor.Length <= 0 || string.IsNullOrEmpty(word))
                return null;
            SortLayers();
            WordSearcher ws = new WordSearcher((from c in word select new string(c, 1)).ToArray());
            ConcurrentBag<string> strings = new ConcurrentBag<string>();
            string errString = string.Empty, errStopped = string.Empty;
            bool exThrown = false, exStopped = false;
            Parallel.ForEach(_lstProcessors, (proc, state) =>
            {
                try
                {
                    if (proc == null)
                        throw new ArgumentNullException(
                            $@"{nameof(FindWord)}: {nameof(ProcessorContainer)} не может быть равен null.");
                    if (proc.Count <= 0)
                        throw new ArgumentException(
                            $@"{nameof(FindWord)}: {nameof(ProcessorContainer)} не может быть пустым.");
                    string str = FindRelation(processor);
                    if (string.IsNullOrEmpty(str))
                        return;
                    if (ws.IsEqual(str))
                        strings.Add(str);
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
            return strings;
        }

        /// <summary>
        /// Получает коллекцию карт методом исключения одной карты из существующей коллекции.
        /// </summary>
        /// <param name="pc">Исходная коллекция карт.</param>
        /// <returns>Возвращает коллекцию карт методом исключения одной карты из существующей коллекции.</returns>
        IEnumerable<ProcessorContainer> GetCollectionExclude(ProcessorContainer pc)
        {
            
        }

        /// <summary>
        /// Позволяет выяснить, содержит карта заданное слово или нет.
        /// Для идентификации слова используется первый символ поля <see cref="Processor.Tag"/> каждой карты.
        /// </summary>
        /// <param name="processor">Анализируемая карта.</param>
        /// <returns>Возвращает экземпляр класса <see cref="WordSearcher"/>, позволяющий выяснить, содержит карта заданное слово или нет.</returns>
        string FindRelation(Processor processor)
        {
            if (_lstWords == null || _lstWords.Count <= 0 || processor == null || processor.Length <= 0 || _lstProcessors == null || _lstProcessors.Count <= 0)
                return null;
            SearchResults[] lstResults = processor.GetEqual(_lstProcessors);
            ConcurrentBag<string> bag = new ConcurrentBag<string>();
            string errString = string.Empty, errStopped = string.Empty;
            bool exThrown = false, exStopped = false;
            Parallel.ForEach(_lstWords, (word, state) =>
            {
                try
                {
                    if (IsEqualWord(word, lstResults))//необходимо добавлять не слова поиска, а присутствующие карты, задействованные в поиске слова, а так же все имеющиеся слова.
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
            StringBuilder sb = new StringBuilder();
            foreach (string s in bag)
                sb.Append(s);
            return sb.ToString();
        }

        /// <summary>
        /// Анализирует содержимое заданных карт на предмет содержания в них заданного слова.
        /// </summary>
        /// <param name="word">Искомое слово.</param>
        /// <param name="sr">Карты, в которых необходимо осуществить поиск.</param>
        /// <returns>Возвращает значение true в случае нахождения слова в какой-либо из указанных карт, в противном случае возвращает значение false.</returns>
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
        void SortLayers()
        {
            if (_sorted)
                return;
            for (int k = 0; k < _lstProcessors.Count; k++)
            {
                for (int j = k + 1; j < _lstProcessors.Count; j++)
                {
                    if (_lstProcessors[j].Count >= _lstProcessors[k].Count) continue;
                    ProcessorContainer procContainer = _lstProcessors[j];
                    _lstProcessors[j] = _lstProcessors[k];
                    _lstProcessors[k] = procContainer;
                }
            }
            _sorted = true;
        }
    }
}