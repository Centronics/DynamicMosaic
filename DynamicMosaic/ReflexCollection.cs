using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynamicMosaic
{
    /// <summary>
    /// Представляет концепцию всеобъёмлющего анализа входных данных с помощью класса <see cref="Reflex"/>.
    /// </summary>
    public sealed class ReflexCollection
    {
        /// <summary>
        /// Внутреннее хранилище ранее загруженных данных.
        /// </summary>
        readonly List<Reflex> _reflexs = new List<Reflex>();

        /// <summary>
        /// Находит связь между заданным словом и текущими картами объекта <see cref="ReflexCollection"/>.
        /// </summary>
        /// <param name="word">Искомое слово.</param>
        /// <returns>В случае нахождения связи возвращает значение <see langword="true"/>, в противном случае - <see langword="false"/>.</returns>
        public bool FindRelation(string word)
        {

        }
    }
}
