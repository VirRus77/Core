using System;

namespace Core.Tools.Extensions
{
    /// <summary>
    /// Класс расширения для <see cref="IComparable"/>
    /// </summary>
    public static class IComparableExtensions
    {
        /// <summary>
        /// Возвращает максимальное значение учитывающее <see cref="IComparable"/> сравниваемых значений.
        /// </summary>
        /// <typeparam name="T">Тип сравниваемого класса</typeparam>
        /// <param name="value1">Значение от которого сравниваем</param>
        /// <param name="value2">Значение с которым сравниваем</param>
        /// <returns>Возвращает максимальное значение, иначе текущее</returns>
        public static T Max<T>(this T value1, T value2)
            where T: IComparable
        {
            var comparable = value1 as IComparable;
            switch (comparable.CompareTo(value2))
            {
                case 1:
                    return value1;
                case 0:
                    return value1;
                case -1:
                    return value2;
            }
            throw new Exception();
        }

        /// <summary>
        /// Возвращает минимальное значение учитывающее <see cref="IComparable"/> сравниваемых значений.
        /// </summary>
        /// <typeparam name="T">Тип сравниваемого класса</typeparam>
        /// <param name="value1">Значение от которого сравниваем</param>
        /// <param name="value2">Значение с которым сравниваем</param>
        /// <returns>Возвращает максимальное значение, иначе текущее</returns>
        public static T Min<T>(this T value1, T value2)
            where T: IComparable
        {
            var comparable = value1 as IComparable;
            switch (comparable.CompareTo(value2))
            {
                case 1:
                    return value2;
                case 0:
                    return value1;
                case -1:
                    return value1;
            }
            throw new Exception();
        }
    }
}
