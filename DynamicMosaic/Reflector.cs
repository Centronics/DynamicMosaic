using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DynamicParser;

namespace DynamicMosaic
{
    /// <summary>
    /// Пара "искомое значение - поле для поиска".
    /// </summary>
    struct PairWordValue
    {
        /// <summary>
        /// Искомая строка.
        /// </summary>
        public string FindString { get; }

        /// <summary>
        /// Поле для поиска указанной строки.
        /// </summary>
        public Processor Field { get; }

        /// <summary>
        /// Статус заполнения текущего экземпляра допустимыми значениями.
        /// </summary>
        public bool IsEmpty => string.IsNullOrEmpty(FindString) || Field == null || Field.Length <= 0;

        /// <summary>
        /// Инициализарует пару.
        /// </summary>
        /// <param name="findString">Искомая строка.</param>
        /// <param name="field">Поле для поиска.</param>
        public PairWordValue(string findString, Processor field)
        {
            FindString = findString;
            Field = field;
        }
    }

    /// <summary>
    /// Предназначен для совмещения нескольких потоков данных.
    /// </summary>
    public sealed class Reflector
    {
        /// <summary>
        /// Сохранённые запросы в виде пар "искомое значение - поле для поиска".
        /// </summary>
        readonly List<PairWordValue> _pairs = new List<PairWordValue>();

        /// <summary>
        /// Исходное значение <see cref="Reflex"/>.
        /// </summary>
        readonly Reflex _reflex;

        /// <summary>
        /// Исходное значение <see cref="Reflex"/>.
        /// Используется метод <see cref="Reflex.Clone"/>.
        /// </summary>
        public Reflex SourceReflex => (Reflex)_reflex.Clone();

        /// <summary>
        /// Анализирует входные данные.
        /// </summary>
        readonly ReflexCollection _reflexCollection;

        /// <summary>
        /// Инициализарует текущий экземпляр объектом <see cref="Reflex"/>.
        /// </summary>
        /// <param name="reflex">Начальное значение <see cref="Reflex"/>.</param>
        public Reflector(Reflex reflex)
        {
            if (reflex == null)
                throw new ArgumentNullException(nameof(reflex), $@"{nameof(Reflector)}: Начальное значение {nameof(Reflex)} должно быть указано.");
            _reflex = reflex;
            _reflexCollection = new ReflexCollection(SourceReflex);
        }

        /// <summary>
        /// Совмещает все имеющиеся пары так, чтобы все запросы все пар проходили успешно.
        /// </summary>
        /// <param name="word">Искомое слово.</param>
        /// <param name="processor">Карта, на которой необходимо выполнить поиск.</param>
        /// <returns>В случае нахождения этой позиции возвращается значение <see langword="true"/>, в противном случае - <see langword="false"/>.</returns>
        public bool Combine(string word, Processor processor)
        {

        }
    }
}
