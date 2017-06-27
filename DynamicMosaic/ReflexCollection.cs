using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace DynamicMosaic
{
    /// <summary>
    /// Представляет концепцию всеобъёмлющего анализа входных данных с помощью класса <see cref="Reflex"/>.
    /// </summary>
    public sealed class ReflexCollection : ICloneable
    {
        /// <summary>
        /// Внутреннее хранилище ранее загруженных данных.
        /// </summary>
        ConcurrentBag<Reflex> _reflexs = new ConcurrentBag<Reflex>();

        /// <summary>
        /// Содержит первоначальный экземпляр <see cref="Reflex"/>.
        /// </summary>
        readonly Reflex _startReflex;

        /// <summary>
        /// Получает коллекцию <see cref="Reflex"/> из текущего экземпляра.
        /// </summary>
        public IEnumerable<Reflex> Reflexs => _reflexs.Select(r => (Reflex)r.Clone());

        /// <summary>
        /// Получает количество объектов <see cref="Reflex"/> в текущем экземпляре.
        /// </summary>
        public int CountReflexs => _reflexs.Count;

        /// <summary>
        /// Получает клон <see cref="Reflex"/>, который изначально был загружен в текущий экземпляр <see cref="ReflexCollection"/>.
        /// Использует метод <see cref="Clone"/>.
        /// </summary>
        public Reflex StartReflex => (Reflex)_startReflex.Clone();

        /// <summary>
        /// Инициализирует начальным значением типа <see cref="Reflex"/>.
        /// Необходимо, чтобы в <see cref="Reflex"/> присутствовало хотя бы две карты.
        /// </summary>
        /// <param name="reflex">Изначальное значение <see cref="Reflex"/>.</param>
        public ReflexCollection(Reflex reflex)
        {
            if (reflex == null)
                throw new ArgumentNullException(nameof(reflex), $@"{nameof(ReflexCollection)}: Начальное значение {nameof(Reflex)} должно быть указано.");
            if (reflex.CountProcessors < 2)
                throw new ArgumentException($@"{nameof(ReflexCollection)}: В изначальном значении {nameof(Reflex)} должно быть хотя бы две карты.", nameof(reflex));
            _startReflex = (Reflex)reflex.Clone();
        }

        /// <summary>
        /// Удаляет все объекты <see cref="Reflex"/> из коллекции текущего экземпляра.
        /// </summary>
        public void Clear()
        {
            _reflexs = new ConcurrentBag<Reflex>();
        }

        /// <summary>
        /// Добавляет <see cref="Reflex"/> в коллекцию текущего экземпляра <see cref="ReflexCollection"/>.
        /// В случае успешной инициализации возвращает значение <see langword="true"></see>, в противном случае - <see langword="false"></see>.
        /// </summary>
        /// <param name="pairs">Поисковые запросы для инициализации текущего экземпляра <see cref="ReflexCollection"/>.</param>
        /// <returns>В случае успешной инициализации возвращает значение <see langword="true"></see>, в противном случае - <see langword="false"></see>.</returns>
        public bool AddPair(IList<PairWordValue> pairs)
        {
            if (pairs == null)
                throw new ArgumentNullException(nameof(pairs), $"{nameof(AddPair)}: Запросы для выполнения должны быть указаны.");
            if (pairs.Count <= 0)
                throw new ArgumentException($"{nameof(AddPair)}: Для выполнения запросов поиска, должен присутствовать хотя бы один запрос.", nameof(pairs));
            Reflex r = StartReflex;
            bool res = true;
            foreach (PairWordValue p in pairs)
            {
                if (p.IsEmpty)
                    throw new ArgumentException($"{nameof(AddPair)}: Для выполнения запроса поиска все его аргументы должны быть указаны.", nameof(pairs));
                if (r.FindRelation(p.Field, p.FindString))
                    continue;
                res = false;
                break;
            }
            if (res && !Contains(r))
                _reflexs.Add(r);
            return res;
        }

        /// <summary>
        /// Проверяет, содержит ли текущий экземпляр заданный объект <see cref="Reflex"/>.
        /// В случае нахождения указанного <see cref="Reflex"/> в текущем экземпляре <see cref="ReflexCollection"/>
        /// возвращается значение <see langword="true"></see>, в противном случае - <see langword="false"></see>.
        /// </summary>
        /// <param name="reflex">Проверяемый <see cref="Reflex"/>.</param>
        /// <returns>В случае нахождения указанного <see cref="Reflex"/> в текущем экземпляре <see cref="ReflexCollection"/>
        /// возвращается значение <see langword="true"></see>, в противном случае - <see langword="false"></see>.</returns>
        bool Contains(Reflex reflex) => reflex != null && _reflexs.Any(r => r == reflex);

        /// <summary>
        /// Создаёт неполную копию текущего экземпляра <see cref="ReflexCollection"/>.
        /// Копируется только изначальное значение текущего экземпляра <see cref="ReflexCollection"/>.
        /// </summary>
        /// <returns>Возвращает неполную копию текущего экземпляра <see cref="ReflexCollection"/>.</returns>
        public object Clone() => new ReflexCollection(StartReflex);
    }
}
