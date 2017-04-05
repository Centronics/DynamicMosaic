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
            List<Processor> procs = new List<Processor>(processors.Count);
            for (int k = 0; k < processors.Count; k++)
            {
                Processor proc = processors[k];
                if (proc == null)
                    continue;
                procs.Add(proc);
                procs.AddRange(processors.Where((t, j) => t != null && k != j).Where(t => proc.Width == t.Width && proc.Height == t.Height));
                _lstProcessors.Add(new ProcessorContainer(procs));
            }
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
        IEnumerable<string> FindRelation(string word) => word.Select((t, j) => word.Substring(j, 1));

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
        void SortLayers()
        {
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
        }
    }
}