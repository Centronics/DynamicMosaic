using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DynamicParser;

namespace DynamicMosaic
{
    /// <summary>
    /// Предоставляет функции управления процессом анализа данных.
    /// </summary>
    public sealed class Reflector
    {
        /// <summary>
        /// Содержит коллекцию информацию о предыдущих вызовах, предназначенных для сопоставления входных данных с ней.
        /// </summary>
        readonly List<ReflexCollection> _reflexCollections = new List<ReflexCollection>();

        /// <summary>
        /// Инициализирует текущий экземпляр <see cref="Reflector"/> коллекцией <see cref="ReflexCollection"/>, которую предполагается взять под контроль.
        /// </summary>
        /// <param name="reflexCollection">Текущая управляемая коллекция <see cref="ReflexCollection"/>.</param>
        /// <param name="word">Слово, для которого необходимо создать рефлексы в требуемой <see cref="ReflexCollection"/>.</param>
        /// <param name="processors">Коллекция карт для обучения текущего контекста <see cref="Reflector"/>.</param>
        public Reflector(ReflexCollection reflexCollection, string word, IList<Processor> processors)
        {
            if (reflexCollection == null)
                throw new ArgumentNullException(nameof(reflexCollection), $@"{nameof(Reflector)}: Необходимо указать управляемую коллекцию.");
            if (word == null)
                throw new ArgumentNullException(nameof(word), $@"{nameof(Reflector)}: Коллекцию слов необходимо указать.");
            if (word == string.Empty)
                throw new ArgumentException($@"{nameof(Reflector)}: В коллекции слов должно быть хотя бы одно слово.", nameof(word));
            if (processors == null)
                throw new ArgumentNullException(nameof(processors), $@"{nameof(Reflector)}: Карты для обучения должны быть указаны.");
            if (processors.Count <= 0)
                throw new ArgumentException($@"{nameof(Reflector)}: Для обучения системы должна присутствовать хотя бы одна карта.", nameof(processors));

        }
    }
}
