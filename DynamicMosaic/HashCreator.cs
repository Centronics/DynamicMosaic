using System;
using System.Collections.Generic;
using System.Linq;
using DynamicParser;

namespace DynamicMosaic
{
    /// <summary>
    ///     Генерирует хеш-код указанной карты <see cref="Processor" />.
    /// </summary>
    public static class HashCreator
    {
        /// <summary>
        ///     Таблица значений, необходимых для генерации хеш-кода.
        /// </summary>
        static readonly int[] Table = new int[256];

        /// <summary>
        ///     Инициализирует таблицу значений, необходимых для генерации хеш-кода.
        /// </summary>
        static HashCreator()
        {
            for (int k = 0; k < Table.Length; k++)
            {
                int num = k;

                for (int i = 0; i < 8; i++)
                    if ((uint)(num & 128) > 0U)
                        num = (num << 1) ^ 49;
                    else
                        num <<= 1;

                Table[k] = num;
            }
        }

        /// <summary>
        ///     Получает хеш-код указанной карты, без учёта значения свойства <see cref="Processor.Tag" />.
        /// </summary>
        /// <param name="p">Карта, для которой необходимо вычислить хеш.</param>
        /// <returns>Возвращает хеш-код указанной карты.</returns>
        /// <remarks>
        ///     Карта не может быть равна <see langword="null" />, иначе будет выброшено исключение <see cref="ArgumentNullException" />.
        /// </remarks>
        /// <exception cref="ArgumentNullException" />
        public static int GetHash(Processor p)
        {
            return GetProcessorBytes(p).Aggregate(255,
                (currentValue, currentByte) => Table[unchecked((byte)(currentValue ^ currentByte))]);
        }

        /// <summary>
        ///     Представляет содержимое указанной карты в виде последовательности байт.
        /// </summary>
        /// <param name="p">Карта, содержимое которой необходимо получить.</param>
        /// <returns>Возвращает содержимое карты в виде последовательности байт.</returns>
        /// <remarks>
        ///     Поле <see cref="Processor.Tag" /> не учитывается.
        ///     Перечисление строк карты происходит последовательно: от меньшего индекса к большему.
        /// </remarks>
        /// <exception cref="ArgumentNullException" />
        static IEnumerable<byte> GetProcessorBytes(Processor p)
        {
            if (p == null)
                throw new ArgumentNullException(nameof(p), $"{nameof(GetProcessorBytes)}: Карта равна значению null.");

            for (int y = 0; y < p.Height; y++)
            for (int x = 0; x < p.Width; x++)
                foreach (byte r in BitConverter.GetBytes(p[x, y].Value))
                    yield return r;
        }
    }
}