using DynamicParser;

namespace DynamicMosaic
{
    /// <summary>
    /// Содержит сведения о следующем этапе обработки данных.
    /// </summary>
    public struct RegionInfo
    {
        /// <summary>
        /// Карта, которую необходимо обработать.
        /// </summary>
        public Processor CurrentProcessor;

        /// <summary>
        /// Искомое слово.
        /// </summary>
        public string Word;

        /// <summary>
        /// Индекс, начиная с которого будет сформирована строка названия карты.
        /// </summary>
        public int StartIndex;

        /// <summary>
        /// Количество символов для выборки из названия карты, оно должно быть кратно длине искомого слова.
        /// </summary>
        public int CharCount;
    }

    /// <summary>
    /// Функция, обрабатывающая состояние перехода от одного этапа обработки данных к следующему.
    /// </summary>
    /// <param name="word">Текущее искомое слово.</param>
    /// <param name="currentProcessor">Обработанная карта.</param>
    /// <param name="startIndex">Индекс, начиная с которого была сформирована строка названия карты.</param>
    /// <param name="charCount">Количество символов для выборки из названия карты, оно должно быть кратно длине искомого слова.</param>
    /// <param name="result">Результат обработки.</param>
    /// <returns>Возвращает сведения о следующем этапе обработки. Значение null означает остановку дальнейшей обработки данных.
    /// Значение false параметра result означает остановку дальнейшей обработки данных.</returns>
    public delegate RegionInfo? Interrupter(string word, Processor currentProcessor, int startIndex, int charCount, bool result);
}