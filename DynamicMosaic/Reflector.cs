using System;
using System.Collections.Generic;
using DynamicParser;

namespace DynamicMosaic
{
    /// <summary>
    /// Представляет один "разряд", т.е. 
    /// </summary>
    struct ReflexCollector
    {

    }

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
        /// Содержит результаты предыдущих запросов.
        /// </summary>
        readonly List<ReflexCollection> _startReflexCollections = new List<ReflexCollection>();

        /// <summary>
        /// Инициализирует текущий экземпляр объектом <see cref="Reflex"/>.
        /// </summary>
        /// <param name="reflex">Начальное значение <see cref="Reflex"/>.</param>
        public Reflector(Reflex reflex)
        {
            if (reflex == null)
                throw new ArgumentNullException(nameof(reflex), $@"{nameof(Reflector)}: Начальное значение {nameof(Reflex)} должно быть указано.");
            _reflex = (Reflex)reflex.Clone();
            _reflexCollection = new ReflexCollection(SourceReflex);
        }

        /// <summary>
        /// Добавляет пару в коллекцию.
        /// </summary>
        /// <param name="word">Искомое слово.</param>
        /// <param name="processor">Карта, на которой необходимо выполнить поиск.</param>
        /// <returns>В случае нахождения этой позиции возвращается значение <see langword="true"/>, в противном случае - <see langword="false"/>.</returns>
        public void Add(string word, Processor processor)
        {
            if (processor == null)
                throw new ArgumentNullException(nameof(processor), $@"{nameof(Add)}: Карта для поиска должна быть указана.");
            if (string.IsNullOrEmpty(word))
                return;
            _pairs.Add(new PairWordValue(word, processor));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public bool Test(string word)
        {

        }

        void GetSequencePairs(ref byte[] bytes)
        {
            
        }
    }
}
