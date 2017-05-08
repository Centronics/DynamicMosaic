using System;
using System.Collections.Generic;
using System.Linq;

namespace DynamicMosaic
{
    /// <summary>
    /// Представляет собой информацию о распознанной карте.
    /// </summary>
    public sealed class Variant
    {
        /// <summary>
        /// Хранит содержимое текущего экземпляра.
        /// </summary>
        readonly List<string> _lstWords = new List<string>();

        /// <summary>
        /// Получает слово по заданному индексу.
        /// </summary>
        /// <param name="index">Индекс получаемого слова.</param>
        /// <returns>Возвращает слово по заданному индексу.</returns>
        public string this[int index] => _lstWords[index];

        /// <summary>
        /// Получает количество слов в текущем экземпляре.
        /// </summary>
        public int Count => _lstWords.Count;

        /// <summary>
        /// Инициализирует текущий экземпляр коллекцией слов.
        /// Проверяет коллекцию на отсутствие пустых слов и дубликатов.
        /// Содержит коллекцию слов с отсутствующими пустыми или дублирующимися словами.
        /// </summary>
        /// <param name="words">Загружаемые слова.</param>
        public Variant(IList<string> words)
        {
            if (words == null)
                throw new ArgumentNullException(nameof(words), $@"{nameof(Variant)}: Загружаемые слова отсутствуют.");
            if (words.Count <= 0)
                throw new ArgumentException($@"{nameof(Variant)}: Загружаемые слова должны быть указаны.", nameof(words));
            foreach (string word in words.Where(word => !string.IsNullOrEmpty(word)).Where(word =>
                _lstWords.All(s => string.Compare(s, word, StringComparison.OrdinalIgnoreCase) != 0)))
                _lstWords.Add(word);
            if (_lstWords.Count <= 0)
                throw new ArgumentException($@"{nameof(Variant)}: Слова отсутствуют.", nameof(words));
        }

        /// <summary>
        /// Проверяет, содержится указанное слово в коллекции или нет.
        /// </summary>
        /// <param name="word">Проверяемое слово.</param>
        /// <returns>В случае нахождения слова возвращает значение <see langword="true"/>, в противном случае - <see langword="false"/>.</returns>
        public bool Contains(string word) => !string.IsNullOrEmpty(word) && _lstWords.Any(s => string.Compare(s, word, StringComparison.OrdinalIgnoreCase) == 0);
    }
}