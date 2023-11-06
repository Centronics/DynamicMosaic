using System;
using System.Collections.Generic;
using System.Linq;
using DynamicParser;
using DynamicProcessor;
using Processor = DynamicParser.Processor;

namespace DynamicMosaic
{
    /// <summary>
    ///     Предназначен для хранения одинаковых по размерам карт, исключая совпадающие по содержимому и первой букве свойства
    ///     <see cref="DynamicParser.Processor.Tag" /> одновременно.
    /// </summary>
    /// <remarks>
    ///     Предназначен для заполнения <see cref="ProcessorContainer" />.
    /// </remarks>
    /// <seealso cref="ProcessorContainer" />
    public sealed class ProcessorHandler
    {
        /// <summary>
        ///     Основное внутреннее хранилище карт. Предназначено для быстрого поиска уже добавленных в хранилище карт, с помощью хешей.
        /// </summary>
        readonly Dictionary<int, List<Processor>> _dicProcsWithTag = new Dictionary<int, List<Processor>>();

        /// <summary>
        ///     Хранит номера добавленных карт, для того, чтобы сгенерировать новое название добавляемой карты.
        /// </summary>
        readonly Dictionary<char, ulong> _procNumbers = new Dictionary<char, ulong>();

        /// <summary>
        ///     Получает карты, содержащиеся в коллекции.
        /// </summary>
        /// <remarks>
        ///     Карты будут с уникальными значениями свойства <see cref="Processor.Tag" />.
        ///     Свойство НЕ потокобезопасно.
        /// </remarks>
        /// <seealso cref="Processor.Tag" />
        public IEnumerable<Processor> Processors => _dicProcsWithTag.Select(p => p.Value).SelectMany(s => s);

        /// <summary>
        ///     Проверяет пригодность карты для добавления в текущую коллекцию.
        /// </summary>
        /// <param name="p">Проверяемая карта.</param>
        /// <remarks>
        ///     Карта не должна быть <see langword="null" />, иначе будет выброшено исключение <see cref="ArgumentNullException" />.
        ///     В случае, если размер карты не совпадёт с размерами уже добавленных карт, будет выброшено исключение <see cref="ArgumentException" />.
        ///     В случае, если коллекция пуста, проверка проходит успешно с любым размером проверяемой карты.
        /// </remarks>
        /// <exception cref="ArgumentNullException" />
        /// <exception cref="ArgumentException" />
        void CheckProcessorSizes(Processor p)
        {
            if (p == null)
                throw new ArgumentNullException(nameof(p), "Добавляемая карта не может быть равна null.");

            Processor t = Processors.FirstOrDefault();

            if (t == null)
                return;

            if (t.Size != p.Size)
                throw new ArgumentException(
                    $"Добавляемая карта отличается по размерам от первой карты, добавленной в коллекцию. Требуется: {t.Width}, {t.Height}. Фактически: {p.Width}, {p.Height}.");
        }

        /// <summary>
        ///     Генерирует уникальное значение свойства <see cref="Processor.Tag" /> указанной карты.
        /// </summary>
        /// <param name="p">Проверяемая карта.</param>
        /// <returns>
        ///     В случае отсутствия карты в коллекции, с указанным значением первого символа (только для заглавных букв) в свойстве
        ///     <see cref="Processor.Tag" />, возвращает эту же карту, которая была подана на вход.
        ///     В противном случае, метод генерирует новую карту с таким же значением свойства <see cref="Processor.Tag" />, к
        ///     которому добавлен её порядковый номер.
        /// </returns>
        /// <remarks>
        ///     Рекомендуется, чтобы значения свойств <see cref="Processor.Tag" /> подаваемых на вход карт, состояли из одной
        ///     заглавной буквы, т.к. остальные символы будут отброшены в любом случае.
        ///     Первая буква названия карты всегда приводится в верхний регистр.
        ///     Если проверяемая карта равна <see langword="null" />, поведение метода неопределено.
        /// </remarks>
        /// <seealso cref="Processor.Tag" />
        Processor GetProcessorWithUniqueTag(Processor p)
        {
            char c = char.ToUpper(p.Tag[0]);

            if (_procNumbers.TryGetValue(c, out ulong number))
            {
                _procNumbers[c] = checked(number + 1);

                return ChangeProcessorTag(p, $"{c}{number}");
            }

            _procNumbers.Add(c, 0);

            return ChangeProcessorTag(p, c.ToString());
        }

        /// <summary>
        ///     Добавляет карту в коллекцию, проверяя её на соответствие по размерам, содержимому и свойству <see cref="Processor.Tag" />.
        /// </summary>
        /// <param name="p">Добавляемая карта.</param>
        /// <remarks>
        ///     Если карта с таким же содержимым и первой буквой (без учёта регистра) в свойстве <see cref="Processor.Tag" />, уже
        ///     присутствует в коллекции, вызов будет игнорирован.
        ///     Карта не должна быть <see langword="null" />, иначе будет выброшено исключение <see cref="ArgumentNullException" />.
        ///     В случае, если размер добавляемой карты не совпадёт с размерами уже добавленных карт, будет выброшено исключение <see cref="ArgumentException" />.
        ///     В случае, если коллекция пуста, проверка проходит успешно с любым размером добавляемой карты.
        /// </remarks>
        /// <exception cref="ArgumentNullException" />
        /// <exception cref="ArgumentException" />
        /// <seealso cref="Processor.Tag" />
        public void Add(Processor p)
        {
            CheckProcessorSizes(p);

            int hash = HashCreator.GetHash(p);

            if (_dicProcsWithTag.TryGetValue(hash, out List<Processor> prcs))
            {
                if (prcs.Any(prc => ProcessorCompare(prc, p)))
                    return;

                Processor up = GetProcessorWithUniqueTag(p);
                prcs.Add(up);
                return;
            }

            _dicProcsWithTag.Add(hash, new List<Processor> { GetProcessorWithUniqueTag(p) });
        }

        /// <summary>
        ///     Сравнивает указанные карты.
        /// </summary>
        /// <param name="p1">Первая карта.</param>
        /// <param name="p2">Вторая карта.</param>
        /// <returns>
        ///     В случае равенства карт по всем признакам, метод возвращает значение <see langword="true" />, в противном случае - <see langword="false" />.
        /// </returns>
        /// <remarks>
        ///     Метод производит сравнение как по содержимому, так и по первым буквам значения свойста <see cref="Processor.Tag" />, без учёта регистра.
        ///     Если карты различаются только по первой букве значения свойства <see cref="Processor.Tag" />, они считаются разными.
        ///     Если хотя бы одна карта равна <see langword="null" />, поведение метода неопределено.
        /// </remarks>
        /// <seealso cref="Processor.Tag" />
        static bool ProcessorCompare(Processor p1, Processor p2)
        {
            if (char.ToUpper(p1.Tag[0]) != char.ToUpper(p2.Tag[0]))
                return false;

            for (int i = 0; i < p1.Width; i++)
            for (int j = 0; j < p1.Height; j++)
                if (p1[i, j] != p2[i, j])
                    return false;

            return true;
        }

        /// <summary>
        ///     Генерирует карту с указанным значением свойства <see cref="Processor.Tag" />.
        /// </summary>
        /// <param name="processor">Карта, значение свойства <see cref="Processor.Tag" /> которой необходимо изменить. Не может быть <see langword="null" />.</param>
        /// <param name="newTag">
        ///     Новое значение свойства <see cref="Processor.Tag" />.
        ///     Не может быть пустым (<see langword="null" />, <see cref="string.Empty" /> или состоять из пробелов).
        /// </param>
        /// <returns>Карта с новым значением свойства <see cref="Processor.Tag" />.</returns>
        /// <remarks>
        ///     В случае, если новое и старое значения свойства <see cref="Processor.Tag" /> совпадают (с учётом регистра), метод
        ///     возвращает ту же карту, которая была подана на вход.
        /// </remarks>
        /// <exception cref="ArgumentNullException" />
        /// <exception cref="ArgumentException" />
        /// <seealso cref="Processor.Tag" />
        public static Processor ChangeProcessorTag(Processor processor, string newTag)
        {
            if (processor == null)
                throw new ArgumentNullException(nameof(processor), $"{nameof(ChangeProcessorTag)}: карта равна null.");

            if (processor.Tag == newTag)
                return processor;

            if (string.IsNullOrWhiteSpace(newTag))
                throw new ArgumentException(
                    $"{nameof(ChangeProcessorTag)}: \"{nameof(newTag)}\" не может быть пустым или содержать только пробел.",
                    nameof(newTag));

            SignValue[,] sv = new SignValue[processor.Width, processor.Height];
            for (int i = 0; i < processor.Width; i++)
            for (int j = 0; j < processor.Height; j++)
                sv[i, j] = processor[i, j];

            return new Processor(sv, newTag);
        }

        /// <summary>
        ///     Генерирует строковое представление текущего экземпляра.
        /// </summary>
        /// <returns>Возвращает строковое представление текущего экземпляра.</returns>
        /// <remarks>
        ///     Строковое представление текущего экземпляра получается путём преобразования первых букв значений свойств
        ///     <see cref="Processor.Tag" /> хранимых карт в верхний регистр.
        /// </remarks>
        /// <seealso cref="Processor.Tag" />
        public override string ToString()
        {
            return new string(Processors.Select(p => p.Tag[0]).ToArray());
        }
    }
}