using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DynamicParser;
using DynamicProcessor;
using Processor = DynamicParser.Processor;

namespace DynamicMosaic
{
    /// <summary>
    /// Предназначен для хранения одинаковых по размерам карт, исключая одинаковые как по содержимому, так и по свойству <see cref="Processor.Tag"/>.
    /// </summary>
    public sealed class ProcessorHandler
    {
        /// <summary>
        /// Основное внутреннее хранилище карт. Предназначено для быстрого поиска уже добавленных в хранилище карт, с помощью хешей.
        /// </summary>
        readonly Dictionary<int, List<Processor>> _dicProcsWithTag = new Dictionary<int, List<Processor>>();

        /// <summary>
        /// Хранит названия добавленных карт, для того, чтобы сгенерировать новое название для добавляемой карты.
        /// </summary>
        readonly HashSet<string> _hashProcs = new HashSet<string>();

        /// <summary>
        /// Хранит карты в том же порядке, в котором они были добавлены.
        /// </summary>
        readonly List<Processor> _processors = new List<Processor>();

        /// <summary>
        /// Получает добавленные карты в том же порядке, в котором они были добавлены.
        /// </summary>
        public IEnumerable<Processor> Processors => _processors;

        /// <summary>
        /// Проверяет пригодность карты вхождения в текущую коллекцию. В том числе, карта не должна быть <see langword="null" />, иначе выдаётся исключение <see cref="ArgumentNullException"/>.
        /// В случае, если размер карты не совпадёт с размерами уже добавленных карт, выдаётся исключение <see cref="ArgumentException"/>.
        /// В случае, если коллекция пуста, проверка проходит с любым размером проверяемой карты.
        /// </summary>
        /// <param name="p">Проверяемая карта.</param>
        void CheckProcessorSizes(Processor p)
        {
            if (p == null)
                throw new ArgumentNullException(nameof(p), @"Добавляемая карта не может быть равна null.");
            Processor t = _processors.FirstOrDefault();
            if (t == null)
                return;
            if (t.Size != p.Size)
                throw new ArgumentException($"Добавляемая карта отличается по размерам от первой карты, добавленной в коллекцию. Требуется: {t.Width}, {t.Height}. Фактически: {p.Width}, {p.Height}.");
        }

        /// <summary>
        /// В случае, если карта с таким же <see cref="Processor.Tag"/> уже существует в коллекции, генерирует новую карту с таким же значением поля <see cref="Processor.Tag"/>, к которому добавлен символ '0'.
        /// В противном случае, возвращается та же карта, которая была подана на вход. Регистр символов сохраняется.
        /// Проверка ведётся без учёта регистра.
        /// </summary>
        /// <param name="p">Проверяемая карта.</param>
        /// <returns>
        /// В случае отсутствия карты с таким же значением свойства <see cref="Processor.Tag"/>, возвращает ту же карту, которая была подана на вход.
        /// В противном случае, генерирует новую карту с таким же значением поля <see cref="Processor.Tag"/>, к которому добавлен символ '0'.
        /// </returns>
        Processor GetProcessorWithUniqueTag(Processor p)
        {
            string tTag = p.Tag.ToUpper();

            if (_hashProcs.Add(tTag))
                return p;

            StringBuilder sb = new StringBuilder(p.Tag, 1024);

            do
            {
                tTag += '0';
                sb.Append('0');
            } while (!_hashProcs.Add(tTag));

            return ChangeProcessorTag(p, sb.ToString());
        }

        /// <summary>
        /// Добавляет карту в коллекцию, проверяя её на соответствие по размерам, содержимому и свойству <see cref="Processor.Tag"/>.
        /// Если карта с таким же содержимым и первой буквой (без учёта регистра) в свойстве <see cref="Processor.Tag"/>, уже присутствует в коллекции, вызов будет игнорирован.
        /// </summary>
        /// <param name="p">Добавляемая карта.</param>
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
                _processors.Add(up);
                return;
            }
            Processor np = GetProcessorWithUniqueTag(p);
            _dicProcsWithTag.Add(hash, new List<Processor> { np });
            _processors.Add(np);
        }

        /// <summary>
        /// Сравнивает указанные карты.
        /// Сравнение производится как по содержимому, так и по первым буквам значения свойста <see cref="Processor.Tag"/>, без учёта регистра.
        /// Если карты различаются только по первой букве значения свойства <see cref="Processor.Tag"/>, они считаются разными.
        /// </summary>
        /// <param name="p1">Первая карта.</param>
        /// <param name="p2">Вторая карта.</param>
        /// <returns>
        /// В случае равенства карт по всем признакам, возвращается значение <see langword="true" />, в противном случае - <see langword="false" />.
        /// </returns>
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
        /// Генерирует карту с указанным новым значением свойства <see cref="Processor.Tag"/>.
        /// В случае, если новое и старое значения свойства <see cref="Processor.Tag"/> совпадают (с учётом регистра), то возвращается та же карта, которая была подана на вход.
        /// </summary>
        /// <param name="processor">Карта, значение свойства <see cref="Processor.Tag"/> которой требуется изменить.</param>
        /// <param name="newTag">Новое значение свойства <see cref="Processor.Tag"/>. Не может быть пустым.</param>
        /// <returns>Карта с новым значением свойства <see cref="Processor.Tag"/>.</returns>
        public static Processor ChangeProcessorTag(Processor processor, string newTag)
        {
            if (processor == null)
                throw new ArgumentNullException(nameof(processor), $@"{nameof(ChangeProcessorTag)}: карта равна null.");
            if (processor.Tag == newTag)
                return processor;
            if (string.IsNullOrWhiteSpace(newTag))
                throw new ArgumentException($"{nameof(ChangeProcessorTag)}: \"{nameof(newTag)}\" не может быть пустым или содержать только пробел.", nameof(newTag));

            SignValue[,] sv = new SignValue[processor.Width, processor.Height];
            for (int i = 0; i < processor.Width; i++)
                for (int j = 0; j < processor.Height; j++)
                    sv[i, j] = processor[i, j];
            return new Processor(sv, newTag);
        }

        /// <summary>
        /// Генерирует строковое представление текущего экземпляра.
        /// Строковое представление текущего экземпляра получается путём преобразования первых букв значений свойств <see cref="Processor.Tag"/> карт в верхний регистр.
        /// Порядок букв соответствует порядку добавления карт в коллекцию.
        /// </summary>
        /// <returns>Возвращает строковое представление текущего экземпляра.</returns>
        public override string ToString() => new string(_processors.Select(p => char.ToUpper(p.Tag[0])).ToArray());
    }
}