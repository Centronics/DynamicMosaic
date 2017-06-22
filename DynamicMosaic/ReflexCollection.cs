using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DynamicParser;

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
        /// <param name="startIndex">Индекс, с которого необходимо начать поиск в названии карт.</param>
        /// <param name="count">Количество символов, которое необходимо взять из названия карты для определения соответствия карт указанному слову.</param>
        /// <returns>В случае успешной инициализации возвращает значение <see langword="true"></see>, в противном случае - <see langword="false"></see>.</returns>
        public bool AddPair(IList<PairWordValue> pairs, int startIndex = 0, int count = 1)
        {
            if (pairs == null)
                throw new ArgumentNullException(nameof(pairs), $"{nameof(AddPair)}: Запросы для выполнения должны быть указаны.");
            if (pairs.Count <= 0)
                throw new ArgumentException($"{nameof(AddPair)}: Для выполнения запросов поиска, должен присутствовать хотя бы один запрос.", nameof(pairs));
            if (startIndex < 0)
                throw new ArgumentException($"{nameof(AddPair)}: Индекс начала поиска имеет некорректное значение: {startIndex}.", nameof(startIndex));
            if (count <= 0)
                throw new ArgumentException($"{nameof(AddPair)}: Количество символов поиска задано неверно: {count}.", nameof(count));
            Reflex r = StartReflex;
            bool res = true;
            foreach (PairWordValue p in pairs)
            {
                if (p.IsEmpty)
                    throw new ArgumentException($"{nameof(AddPair)}: Для выполнения запроса поиска все его аргументы должны быть указаны.", nameof(pairs));
                if (r.FindRelation(p.Field, p.FindString, startIndex, count))
                    continue;
                res = false;
                break;
            }
            if (res)
                _reflexs.Add(r);
            return res;
        }

        /// <summary>
        /// Находит связь между заданным словом и текущими картами объекта <see cref="ReflexCollection"/>.
        /// </summary>
        /// <param name="processor">Карта, на которой необходимо выполнить поиск.</param>
        /// <param name="word">Искомое слово.</param>
        /// <param name="startIndex">Индекс, с которого необходимо начать поиск в названии карт.</param>
        /// <param name="count">Количество символов, которое необходимо взять из названия карты для определения соответствия карт указанному слову.</param>
        /// <returns>В случае нахождения связи возвращает значение <see langword="true"/>, в противном случае - <see langword="false"/>.</returns>
        public bool FindRelation(Processor processor, string word, int startIndex = 0, int count = 1)
        {
            if (processor == null)
                throw new ArgumentNullException(nameof(processor), $"{nameof(FindRelation)}: Карта для поиска не указана (null).");
            if (word == null)
                throw new ArgumentNullException(nameof(word), $"{nameof(FindRelation)}: Искомое слово равно null.");
            if (word == string.Empty)
                throw new ArgumentException($"{nameof(FindRelation)}: Искомое слово не указано.", nameof(word));
            if (startIndex < 0)
                throw new ArgumentException($"{nameof(FindRelation)}: Индекс начала поиска имеет некорректное значение: {startIndex}.", nameof(startIndex));
            if (count <= 0)
                throw new ArgumentException($"{nameof(FindRelation)}: Количество символов поиска задано неверно: {count}.", nameof(count));
            if (CountReflexs <= 0)
                throw new ArgumentException($@"{nameof(FindRelation)}: Невозможно начать операцию анализа данных по причине отсутствия {nameof(Reflex)
                    }. Используйте метод {nameof(AddPair)} для добавления рефлексов ({nameof(Reflex)}).");
            string errString = string.Empty, errStopped = string.Empty;
            bool exThrown = false, exStopped = false, val = false;
            Parallel.ForEach(_reflexs, (reflex, state) =>
            {
                try
                {
                    if (reflex.FindRelation(processor, word, startIndex, count))
                        val = true;
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
            return val;
        }

        /// <summary>
        /// Создаёт неполную копию текущего экземпляра <see cref="ReflexCollection"/>.
        /// Копируется только изначальное значение текущего экземпляра <see cref="ReflexCollection"/>.
        /// </summary>
        /// <returns>Возвращает неполную копию текущего экземпляра <see cref="ReflexCollection"/>.</returns>
        public object Clone() => new ReflexCollection(StartReflex);
    }
}
