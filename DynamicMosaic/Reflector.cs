using System;
using System.Collections.Generic;
using System.Linq;
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
        /// Получает исходную <see cref="ReflexCollection"/>, используя метод <see cref="ReflexCollection.Clone"/>.
        /// </summary>
        public ReflexCollection SourceReflexCollection => (ReflexCollection)_reflexCollection.Clone();

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
        /// Находит связь между заданным словом и данными в контексте.
        /// В случае нахождения связи возвращает значение <see langword="true"/>, в противном случае - <see langword="false"/>.
        /// </summary>
        /// <param name="word">Проверяемое слово.</param>
        /// <param name="startIndex">Индекс, с которого необходимо начать поиск в названии карт.</param>
        /// <param name="count">Количество символов, которое необходимо взять из названия карты для определения соответствия карт указанному слову.</param>
        /// <returns>В случае нахождения связи возвращает значение <see langword="true"/>, в противном случае - <see langword="false"/>.</returns>
        public bool FindRelation(string word, int startIndex = 0, int count = 1)
        {
            if (word == null)
                throw new ArgumentNullException(nameof(word), $"{nameof(FindRelation)}: Искомое слово равно null.");
            if (word == string.Empty)
                throw new ArgumentException($"{nameof(FindRelation)}: Искомое слово не указано.", nameof(word));
            if (startIndex < 0)
                throw new ArgumentException($"{nameof(FindRelation)}: Индекс начала поиска имеет некорректное значение: {startIndex}.");
            if (count <= 0)
                throw new ArgumentException($"{nameof(FindRelation)}: Количество символов поиска задано неверно: {count}.");
            if (!Contains(word))
                return false;
            List<PairWordValue> lstPairWordValues = new List<PairWordValue>();
            List<int> counting = new List<int>(_pairs.Count);
            for (int z = 1; z < _pairs.Count; z++)
            {
                for (int k = 0; k < counting.Count; k++)
                    counting[k] = 0;
                counting.Add(0);
                for (int k = 1; k < _pairs.Count; k++)
                    while (ChangeCount(counting, k) != -1)
                    {
                        ReflexCollection reflexCollection = SourceReflexCollection;
                        GetWord(counting, lstPairWordValues);
                        foreach (PairWordValue p in lstPairWordValues)
                        {
                            if (p.IsEmpty)
                                throw new Exception($@"{nameof(FindRelation)}: {nameof(PairWordValue)} пустая.");
                            if (reflexCollection.FindRelation(p.Field, p.FindString, startIndex, count))
                                return true;
                        }
                    }
            }
            return false;
        }

        /// <summary>
        /// Получает значение, показывающее, содержится ли указанное слово в коллекции.
        /// </summary>
        /// <param name="word">Проверяемое слово.</param>
        /// <returns>Возвращает значение, показывающее, содержится ли указанное слово в коллекции.</returns>
        public bool Contains(string word) => !string.IsNullOrEmpty(word) && _pairs.Any(p => string.Compare(word, p.FindString, StringComparison.OrdinalIgnoreCase) == 0);

        /// <summary>
        ///     Увеличивает значение старших разрядов счётчика букв, если это возможно.
        ///     Если увеличение было произведено, возвращается номер позиции, на которой произошло изменение, в противном случае
        ///     -1.
        /// </summary>
        /// <param name="count">Массив-счётчик.</param>
        /// <param name="maxCount">Максимальное значение, которого может достигать счётчик в одном разряде.</param>
        /// <returns>Возвращается номер позиции, на которой произошло изменение, в противном случае -1.</returns>
        int ChangeCount(IList<int> count, int maxCount)
        {
            if (count == null || count.Count <= 0)
                throw new ArgumentException($"{nameof(ChangeCount)}: Массив-счётчик не указан или его длина некорректна ({count?.Count}).", nameof(count));
            if (maxCount <= 0)
                throw new ArgumentException($@"{nameof(ChangeCount)}: Значение {nameof(maxCount)} должно быть больше ноля ({maxCount}).", nameof(maxCount));
            for (int k = count.Count - 1; k >= 0; k--)
            {
                if (count[k] >= maxCount) continue;
                count[k]++;
                for (int x = k + 1; x < count.Count; x++)
                    count[x] = 0;
                return k;
            }
            return -1;
        }

        /// <summary>
        ///     Генерирует слово из частей, содержащихся в коллекции, основываясь на данных счётчиков.
        /// </summary>
        /// <param name="count">Данные счётчиков по каждому слову.</param>
        /// <param name="lstPairs">Коллекция, в которую будут добавлены элементы, при этом будут удалены существующие.</param>
        void GetWord(IList<int> count, List<PairWordValue> lstPairs)
        {
            if (count == null)
                throw new ArgumentNullException(nameof(count), $"{nameof(GetWord)}: Массив данных равен null.");
            if (count.Count <= 0)
                throw new ArgumentException($"{nameof(GetWord)}: Длина массива данных должна совпадать с количеством хранимых слов.", nameof(count));
            if (lstPairs == null)
                throw new ArgumentNullException(nameof(lstPairs), $@"{nameof(GetWord)}: Массив коллекций равен null.");
            lstPairs.Clear();
            lstPairs.AddRange(count.Select(c => _pairs[c]));
        }
    }
}
