using System;
using System.Collections.Generic;
using DynamicParser;

namespace DynamicMosaic
{
    /// <summary>
    /// Предназначен для связывания ассоциаций.
    /// </summary>
    public sealed class LinkedRegion
    {
        /// <summary>
        /// Список ассоциаций с текущими ассоциациями.
        /// </summary>
        readonly List<LinkedRegion> _linkedRegions = new List<LinkedRegion>();

        /// <summary>
        /// Текущая связываемая карта.
        /// </summary>
        public Processor CurrentProcessor { get; set; }
    }

    /// <summary>
    /// Предназначен для запоминания образов и ассоциации их с другими образами.
    /// </summary>
    public sealed class RegionMemory
    {
        /// <summary>
        /// Карты, с которыми производятся ассоциации.
        /// </summary>
        readonly ProcessorContainer _processorContainer;

        /// <summary>
        /// Инициализирует текущий экземпляр картами, с которыми в дальнейшем будет производиться ассоциация других карт.
        /// </summary>
        /// <param name="processorContainer">Набор исходных карт.</param>
        public RegionMemory(ProcessorContainer processorContainer)
        {
            if (processorContainer == null)
                throw new ArgumentNullException(nameof(processorContainer), $@"{nameof(RegionMemory)}: Набор исходных карт равен null.");
            if (processorContainer.Count <= 0)
                throw new ArgumentException($@"{nameof(RegionMemory)}: Набор исходных карт пустой ({processorContainer.Count}).", nameof(processorContainer));
            _processorContainer = processorContainer;
        }


    }
}