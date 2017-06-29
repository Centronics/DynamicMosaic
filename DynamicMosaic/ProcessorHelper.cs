using System;
using DynamicProcessor;
using Processor = DynamicParser.Processor;

namespace DynamicMosaic
{
    /// <summary>
    /// Содержит сервисные функции.
    /// </summary>
    public static class ProcessorHelper
    {
        /// <summary>
        /// Создаёт полную копию указанного экземпляра объекта <see cref="DynamicParser.Processor"/>.
        /// </summary>
        /// <param name="processor">Клонируемый экземпляр <see cref="DynamicParser.Processor"/>.</param>
        /// <returns>Возвращает полную копию указанного экземпляра объекта <see cref="DynamicParser.Processor"/>.</returns>
        public static Processor GetMapClone(this Processor processor)
        {
            if (processor == null)
                return null;
            SignValue[,] map = new SignValue[processor.Width, processor.Height];
            for (int y = 0; y < processor.Height; y++)
                for (int x = 0; x < processor.Width; x++)
                    map[x, y] = processor[x, y];
            return new Processor(map, processor.Tag);
        }

        /// <summary>
        /// Уточняет, являются ли две указанные карты одинаковыми.
        /// В случае совпадения возвращает значение <see langword="true"></see>, в противном случае - <see langword="false"></see>.
        /// </summary>
        /// <param name="processor1">Первая сопоставляемая карта.</param>
        /// <param name="processor2">Вторая сопоставляемая карта.</param>
        /// <returns>В случае совпадения возвращает значение <see langword="true"></see>, в противном случае - <see langword="false"></see>.</returns>
        public static bool ProcessorCompare(this Processor processor1, Processor processor2)
        {
            if (processor1 == null)
                throw new ArgumentNullException(nameof(processor1), $"{nameof(ProcessorCompare)}: Сопоставляемая карта отсутствует.");
            if (processor2 == null)
                throw new ArgumentNullException(nameof(processor2), $"{nameof(ProcessorCompare)}: Сопоставляемая карта отсутствует.");
            if (processor1.Width != processor2.Width || processor1.Height != processor2.Height)
                return false;
            for (int y = 0; y < processor1.Height; y++)
                for (int x = 0; x < processor1.Width; x++)
                    if (processor1[x, y] != processor2[x, y])
                        return false;
            return true;
        }
    }
}