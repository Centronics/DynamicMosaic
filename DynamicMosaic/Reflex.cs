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
        /// Карты, с помощью которых производится поиск запрашиваемых данных.
        /// </summary>
        readonly ProcessorContainer _seaProcessors;

        /// <summary>
        /// Получает карту, поиск которой производится при каждом запросе поиска слова.
        /// </summary>
        /// <param name="index">Индекс карты.</param>
        /// <returns>Возвращает карту, поиск которой производится при каждом запросе поиска слова.</returns>
        public Processor GetProcessor(int index) => _seaProcessors[index];

        /// <summary>
        /// Получает количество карт в контексте.
        /// </summary>
        public int CountProcessor => _seaProcessors.Count;

        /// <summary>
        /// Инициализирует текущий контекст указанными картами.
        /// </summary>
        /// <param name="processors">Карты, которые необходимо добавить в контекст.</param>
        public Reflex(IList<Processor> processors)
        {
            if (processors == null)
                throw new ArgumentNullException(nameof(processors), $"{nameof(Reflex)}: Карты должны быть добавлены в контекст (null).");
            if (processors.Count <= 0)
                throw new ArgumentException($"{nameof(Reflex)}: Карты должны присутствовать в контексте (Count = 0).");
            _seaProcessors = new ProcessorContainer(processors);
        }

        /// <summary>
        /// Получает <see cref="Processor"/>, поле <see cref="Processor.Tag"/> которых начинается указанным символом.
        /// Поиск производится без учёта регистра.
        /// </summary>
        /// <param name="c">Искомый символ.</param>
        /// <returns>Возвращает <see cref="Processor"/>, поле <see cref="Processor.Tag"/> которых начинается указанным символом.</returns>
        public IEnumerable<Processor> GetMap(char c)
        {
            char ch = char.ToUpper(c);
            for (int k = 0; k < _seaProcessors.Count; k++)
                if (char.ToUpper(_seaProcessors[k].Tag[0]) == ch)
                    yield return _seaProcessors[k];
        }

        /// <summary>
        /// Производит поиск слова в имеющихся картах.
        /// Возвращает <see cref="Reflex"/>, который так или иначе связан с указанным словом или <see langword="null"/>, если связи нет.
        /// </summary>
        /// <param name="processor">Анализируемая карта, на которой будет производиться поиск.</param>
        /// <param name="words">Искомое слово.</param>
        /// <returns>Возвращает <see cref="Reflex"/>, который так или иначе связан с указанным словом или <see langword="null"/>, если связи нет.</returns>
        public Variant FindWord(Processor processor, IList<string> words)
        {
            if (processor == null || processor.Length <= 0 || words == null || words.Count <= 0)
                return null;
            if (_seaProcessors == null || _seaProcessors.Count <= 0)
                throw new ArgumentException(
                    $"{nameof(FindWord)}: Карты для поиска искомого слова должны присутствовать.");
            bool exThrown = false, exStopping = false;
            string exString = string.Empty, exStoppingString = string.Empty;
            Parallel.ForEach(words, (word, state) =>
            {
                try
                {
                    if (!IsMapsWord(word))
                        return;

                }
                catch (Exception ex)
                {
                    try
                    {
                        exThrown = true;
                        exString = ex.Message;
                        state.Stop();
                    }
                    catch (Exception ex1)
                    {
                        exStopping = true;
                        exStoppingString = ex1.Message;
                    }
                }
            });
            if (exThrown)
                throw new Exception(exStopping ? $"{exString}{Environment.NewLine}{exStoppingString}" : exString);

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
            return word.All(c =>
            {
                for (int k = 0; k < _seaProcessors.Count; k++)
                    if (char.ToUpper(_seaProcessors[k].Tag[0]) != c)
                        return true;
                return false;
            });
        }

        /// <summary>
        /// Получает карты, между которыми отсутствуют конфликты, представляющие заданное слово.
        /// Возвращаются в карты с одинаковыми названиями.
        /// </summary>
        /// <param name="word">Проверяемое слово.</param>
        /// <returns>Возвращает карты, между которыми отсутствуют конфликты, представляющие заданное слово.</returns>
        IEnumerable<Processor> GetProcessors(string word)
        {
            
        }
    }
}