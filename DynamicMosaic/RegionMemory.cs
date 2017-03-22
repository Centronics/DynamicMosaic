using System;
using System.Collections.Generic;
using System.Linq;
using DynamicParser;

namespace DynamicMosaic
{
    /// <summary>
    ///     Предназначен для связывания ассоциаций.
    /// </summary>
    public sealed class RegionMemory
    {
        /// <summary>
        ///     Текущие связываемые карты.
        /// </summary>
        readonly ProcessorContainer _currentProcessors;

        /// <summary>
        ///     Список классов для сопоставления.
        /// </summary>
        readonly List<RegionMemory> _nextLinkedRegion = new List<RegionMemory>();

        /// <summary>
        ///     Инициализирует текущий экземпляр указанными картами.
        /// </summary>
        /// <param name="pc">Карты, которые необходимо ассоциировать.</param>
        public RegionMemory(ProcessorContainer pc)
        {
            if (pc == null)
                throw new ArgumentNullException(nameof(pc),
                    $"{nameof(RegionMemory)}: Сопоставляемые карты должны быть указаны.");
            if (pc.Count <= 0)
                throw new ArgumentException(
                    $"{nameof(RegionMemory)}: Количество сопоставляемых карт должно быть больше ноля.", nameof(pc));
            _currentProcessors = new ProcessorContainer(pc[0]);
            for (int k = 1; k < pc.Count; k++)
                _currentProcessors.Add(pc[k]);
            Width = pc.Width;
            Height = pc.Height;
        }

        /// <summary>
        ///     Размер связываемых карт по ширине.
        /// </summary>
        public int Width { get; }

        /// <summary>
        ///     Размер связываемых карт по высоте.
        /// </summary>
        public int Height { get; }

        /// <summary>
        /// Получает длину (<see cref="Width"/> * <see cref="Height"/>) связываемых карт.
        /// </summary>
        public int Length => Width * Height;

        /// <summary>
        /// Хранит значение, показывающее, требуется ли перед началом работы выполнить сортировку или нет.
        /// </summary>
        bool _sorted;

        /// <summary>
        ///     Добавляет новые карты, которые необходимо ассоциировать с текущим объектом.
        ///     В случае необходимости создать слой для добавления карты, слой создаётся автоматически.
        /// </summary>
        /// <param name="pc">Карты, которые необходимо ассоциировать.</param>
        public void AddAssociation(ProcessorContainer pc)
        {
            if (pc == null)
                throw new ArgumentNullException(nameof(pc),
                    $"{nameof(AddAssociation)}: Ассоциируемые карты должны быть указаны.");
            if (pc.Count <= 0)
                return;
            _sorted = false;
            if (pc.Width == Width && pc.Height == Height)
            {
                for (int k = 0; k < pc.Count; k++)
                    _currentProcessors.Add(pc[k]);
                return;
            }
            RegionMemory rm = FindRegion(pc.Width, pc.Height);
            if (rm == null)
            {
                _nextLinkedRegion.Add(new RegionMemory(pc));
                return;
            }
            rm.AddAssociation(pc);
        }

        /// <summary>
        /// Предназначен для анализа указанной карты с применением карт указанного уровня.
        /// </summary>
        /// <param name="processor">Исследуемая карта.</param>
        /// <param name="word">Искомое слово.</param>
        /// <param name="startIndex">Индекс, начиная с которого будет сформирована строка названия карты.</param>
        /// <returns>Возвращает значение true для в случае нахождения искомого слова, в противном случае - false.</returns>
        public bool Recognize(Processor processor, string word, int startIndex)
        {
            if (processor == null)
                throw new ArgumentNullException(nameof(processor), $"{nameof(Recognize)}: Анализируемая карта отсутствует.");
            SortLayers();
        }

        /// <summary>
        /// Сортирует все слои по возрастанию.
        /// </summary>
        void SortLayers()
        {
            if (_sorted)
                return;
            for (int k = 0; k < _nextLinkedRegion.Count; k++)
            {
                for (int j = k + 1; j < _nextLinkedRegion.Count; j++)
                {
                    if (_nextLinkedRegion[j].Length >= _nextLinkedRegion[k].Length) continue;
                    RegionMemory regionMemory = _nextLinkedRegion[j];
                    _nextLinkedRegion[j] = _nextLinkedRegion[k];
                    _nextLinkedRegion[k] = regionMemory;
                }
            }
            _sorted = true;
        }

        /// <summary>
        ///     Выполняет поиск и возвращает <see cref="RegionMemory" /> или null в зависимости от того, содержится указанный
        ///     уровень или нет.
        /// </summary>
        /// <param name="width">Искомая ширина.</param>
        /// <param name="height">Искомая высота.</param>
        /// <returns>Возвращает <see cref="RegionMemory" /> или null в зависимости от того, содержится указанный уровень или нет.</returns>
        RegionMemory FindRegion(int width, int height)
        {
            if (width <= 0)
                throw new ArgumentException($"{nameof(FindRegion)}: Ширина меньше или равна нолю ({width}).",
                    nameof(width));
            if (height <= 0)
                throw new ArgumentException($"{nameof(FindRegion)}: Высота меньше или равна нолю ({height}).",
                    nameof(height));
            if (Width == width && Height == height)
                return this;
            return _nextLinkedRegion.FirstOrDefault(rm => rm.Width == width && rm.Height == height);
        }

        /// <summary>
        ///     Выполняет поиск и возвращает значение, показывающее, содержится ли такой уровень или нет.
        /// </summary>
        /// <param name="width">Искомая ширина.</param>
        /// <param name="height">Искомая высота.</param>
        /// <returns>Возвращает значение true в случае нахождения указанного уровня, в противном случае - false.</returns>
        public bool Contains(int width, int height) => FindRegion(width, height) != null;
    }
}