using System;
using DynamicParser;

namespace DynamicMosaic
{
    /// <summary>
    /// Представляет собой ассоциацию какого-либо объекта с какими-либо данными.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Определяет, содержат ли указанные экземпляры класса <see cref="SearchResults"/> одни и те же карты.
        /// </summary>
        /// <param name="source">Текущий экземпляр.</param>
        /// <param name="srCompare">Экземпляр, с которым произвоится сравнение.</param>
        /// <param name="startIndex">Индекс, начиная с которого будет сформирована строка названия карты.</param>
        /// <param name="count">Количество символов в строке названия карты.</param>
        /// <param name="startIndexCompare">Индекс, начиная с которого будет сформирована строка названия сопоставляемой карты.</param>
        /// <param name="countCompare">Количество символов в строке названия сопоставляемой карты.</param>
        /// <returns>Возвращает значение true в случае, если в обоих экземплярах содержатся одни и те же карты, в противном случае - false.</returns>
        public static bool IsEqual(this SearchResults source, SearchResults srCompare, int startIndex, int count, int startIndexCompare, int countCompare)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source), $"{nameof(IsEqual)}: Исходный экземпляр {nameof(SearchResults)} не указан.");
            if (srCompare == null)
                throw new ArgumentNullException(nameof(srCompare), $"{nameof(IsEqual)}: Сопоставляемый экземпляр {nameof(SearchResults)} не указан.");
            return IsEqualMaps(source, srCompare, startIndex, count, startIndexCompare, countCompare) &&
                IsEqualMaps(srCompare, source, startIndex, count, startIndexCompare, countCompare);
        }

        /// <summary>
        /// Проверяет, присутствуют ли карты поля <paramref name="srCompare"/> в поле <paramref name="source"/>.
        /// </summary>
        /// <param name="source">Проверяемое поле, в котором будет производиться поиск.</param>
        /// <param name="srCompare">Поле, поиск элементов которого должен быть произведён.</param>
        /// <param name="startIndex">Индекс, начиная с которого будет сформирована строка названия карты.</param>
        /// <param name="count">Количество символов в строке названия карты.</param>
        /// <param name="startIndexCompare">Индекс, начиная с которого будет сформирована строка названия сопоставляемой карты.</param>
        /// <param name="countCompare">Количество символов в строке названия сопоставляемой карты.</param>
        /// <returns>Возвращает значение true в случае, если элементы поля <paramref name="srCompare"/> находятся в поле <paramref name="source"/>.</returns>
        static bool IsEqualMaps(SearchResults source, SearchResults srCompare, int startIndex, int count, int startIndexCompare, int countCompare)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source), $"{nameof(IsEqualMaps)}: Исходный экземпляр {nameof(SearchResults)} не указан.");
            if (srCompare == null)
                throw new ArgumentNullException(nameof(srCompare), $"{nameof(IsEqualMaps)}: Сопоставляемый экземпляр {nameof(SearchResults)} не указан.");
            for (int y = 0; y < source.Height; y++)
                for (int x = 0; x < source.Width; x++)
                {
                    Processor[] mas = source[x, y].Procs;
                    if (mas == null || mas.Length <= 0)
                        continue;
                    foreach (Processor map in mas)
                    {
                        if (map == null)
                            throw new ArgumentNullException(nameof(source), $"{nameof(IsEqual)}: Проверяемая карта равна null.");
                        string mapName = map.GetProcessorName(startIndex, count);
                        if (string.IsNullOrEmpty(mapName))
                            return false;
                        if (!srCompare.FindRelation(mapName, startIndexCompare, countCompare))
                            return false;
                    }
                }
            return true;
        }
    }
}