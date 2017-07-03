using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace DynamicMosaic
{
    /// <summary>
    /// Представляет концепцию всеобъёмлющего анализа входных данных с помощью класса <see cref="Reflex"/>.
    /// </summary>
    public sealed class ReflexCollection
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
        /// Получает размер загруженных карт.
        /// </summary>
        public Size MapSize => _startReflex.MapSize;

        /// <summary>
        /// Получает клон <see cref="Reflex"/>, который изначально был загружен в текущий экземпляр <see cref="ReflexCollection"/>.
        /// Использует метод <see cref="Reflex.Clone"/>.
        /// </summary>
        Reflex StartReflex => (Reflex)_startReflex.Clone();

        /// <summary>
        /// Инициализирует начальным значением типа <see cref="Reflex"/>.
        /// Необходимо, чтобы в <see cref="Reflex"/> присутствовало хотя бы две карты.
        /// </summary>
        /// <param name="reflex">Изначальное значение <see cref="Reflex"/>.</param>
        public ReflexCollection(Reflex reflex)
        {
            if (reflex == null)
                throw new ArgumentNullException(nameof(reflex), $@"{nameof(ReflexCollection)}: Начальное значение {nameof(Reflex)} должно быть указано.");
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
        /// Позволяет выяснить, содержит ли текущий контекст достаточное количество карт для составления указанного слова.
        /// </summary>
        /// <param name="word">Проверяемое слово.</param>
        /// <returns>В случае успешной проверки возвращается значение <see langword="true"/>, иначе <see langword="false"/>.</returns>
        public bool IsMapsWord(string word) => _startReflex.IsMapsWord(word);

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
                throw new ArgumentException($"{nameof(AddPair)}: Для выполнения запросов поиска должен присутствовать хотя бы один запрос.", nameof(pairs));
            foreach (PairWordValue p in pairs)
            {
                if (p.Field.Width < _startReflex.MapSize.Width)
                    throw new ArgumentException($@"{nameof(AddPair)}: Ширина подаваемой карты ({p.Field.Width
                        }) должна быть больше или равна ширине загруженных карт ({_startReflex.MapSize.Width}).", nameof(pairs));
                if (p.Field.Height < _startReflex.MapSize.Height)
                    throw new ArgumentException($@"{nameof(AddPair)}: Высота подаваемой карты ({p.Field.Height
                        }) должна быть больше или равна высоте загруженных карт ({_startReflex.MapSize.Height}).", nameof(pairs));
            }
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
    }
}
