using System;

namespace Core.Tools.Extensions
{
    /// <summary>
    /// Расширение для работы с флаговыми <see cref="Enum"/>
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// Выставляю или сбрасываю бит в маске флагового перечисления
        /// </summary>
        /// <typeparam name="T">Тип перечисления</typeparam>
        /// <param name="enumValue">Основная маска</param>
        /// <param name="flag">Флаг, который хотим выстваить</param>
        /// <param name="isAdd">Добавляем ли бит</param>
        public static T SetFlag<T>(this T enumValue, T flag, bool isAdd = true)
        {
            // т.к. флаги могут быть отрицательными, было решено взять максимальное
            // целое число со знаком
            var num = isAdd
                ? (Convert.ToInt64(enumValue) | Convert.ToInt64(flag))
                : (Convert.ToInt64(enumValue) & ~Convert.ToInt64(flag));

            return (T)Enum.ToObject(enumValue.GetType(), num);
        }

        public static bool GetFlag<T>(this T enumValue, T flag)
        {
            return (Convert.ToInt64(enumValue) & Convert.ToInt64(flag)) == Convert.ToInt64(flag);
        }
    }
}
