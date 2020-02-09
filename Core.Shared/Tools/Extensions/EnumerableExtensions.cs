using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Core.Tools.Extensions
{
    // ReSharper disable once InconsistentNaming
    public static partial class EnumerableExtensions
    {
        public static IEnumerable Concat(this IEnumerable enumerable, IEnumerable concatEnumerable)
        {
            foreach (var value in enumerable)
            {
                yield return value;
            }

            foreach (var value in concatEnumerable)
            {
                yield return value;
            }
        }

        public static int IndexOf<T>(this IEnumerable<T> enumerable, T findValue)
        {
            var index = 0;
            foreach (var value in enumerable)
            {
                if (Equals(value, findValue))
                    return index;
                index++;
            }

            return -1;
        }

        public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> addedItems)
        {
            if (collection is List<T> list)
            {
                list.AddRange(addedItems);
                return;
            }

            foreach (var addedItem in addedItems)
            {
                collection.Add(addedItem);
            }
        }

        /// <summary>
        /// Делегат описания фукции
        /// </summary>
        /// <typeparam name="T">Тип входящих значений</typeparam>
        /// <typeparam name="V">Тип исходящих значений</typeparam>
        /// <param name="parent">Владелец значения</param>
        /// <param name="selectedValue">Выбранное значение</param>
        /// <returns></returns>
        public delegate V SelectValue<T, V>(T parent, T selectedValue);

        /// <summary>
        /// Рекурсивная выборка вложенных элементов
        /// </summary>
        /// <typeparam name="T">Тип входящего перечисления</typeparam>
        /// <typeparam name="V">Тип исходящего перечисления</typeparam>
        /// <param name="enumerable">Входящее перечисление</param>
        /// <param name="getChild">Функция получения дочерних перечислений по которым будет проходить выборка</param>
        /// <returns>Выбранные значения</returns>
        public static IEnumerable<T> SelectRecursive<T>(
            this IEnumerable<T> enumerable,
            Func<T, IEnumerable<T>> getChild
        )
            where T : class
        {
            return enumerable.SelectRecursive(getChild, v => v);
        }

        /// <summary>
        /// Рекурсивная выборка вложенных элементов
        /// </summary>
        /// <typeparam name="T">Тип входящего перечисления</typeparam>
        /// <typeparam name="V">Тип исходящего перечисления</typeparam>
        /// <param name="enumerable">Входящее перечисление</param>
        /// <param name="getChild">Функция получения дочерних перечислений по которым будет проходить выборка</param>
        /// <param name="selectValue">Функция преобразования в исходящий тип</param>
        /// <param name="parent">Взладелец входящего перечисления</param>
        /// <returns>Выбранные значения</returns>
        public static IEnumerable<V> SelectRecursive<T, V>(
            this IEnumerable<T> enumerable,
            Func<T, IEnumerable<T>> getChild,
            SelectValue<T, V> selectValue,
            T parent = null
        )
            where T : class
        {
            if (enumerable == null)
                yield break;
            foreach (var value in enumerable)
            {
                yield return selectValue(parent, value);

                var childEnumerable = getChild(value);
                if (childEnumerable == null)
                    continue;

                foreach (var valueV in childEnumerable.SelectRecursive(getChild, selectValue, value))
                {
                    yield return valueV;
                }
            }
        }

        /// <summary>
        /// Рекурсивная выборка вложенных элементов
        /// </summary>
        /// <typeparam name="T">Тип входящего перечисления</typeparam>
        /// <typeparam name="V">Тип исходящего перечисления</typeparam>
        /// <param name="enumerable">Входящее перечисление</param>
        /// <param name="getChild">Функция получения дочерних перечислений по которым будет проходить выборка</param>
        /// <param name="selectValue">Функция преобразования в исходящий тип</param>
        /// <returns>Выбранные значения</returns>
        public static IEnumerable<V> SelectRecursive<T, V>(
            this IEnumerable<T> enumerable,
            Func<T, IEnumerable<T>> getChild,
            Func<T, V> selectValue
        )
        {
            if (enumerable == null)
                yield break;
            foreach (var value in enumerable)
            {
                yield return selectValue(value);

                var childEnumerable = getChild(value);
                if (childEnumerable == null)
                    continue;

                foreach (var valueV in childEnumerable.SelectRecursive(getChild, selectValue))
                {
                    yield return valueV;
                }
            }
        }

        /// <summary>
        /// Вытаскиваем все элементы, которые можем привести к указанному типу
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="iterator"></param>
        /// <returns></returns>
        public static IEnumerable<T> OfType<T>(this IEnumerator iterator)
        {
            while (iterator.MoveNext())
            {
                if (iterator.Current is T)
                    yield return (T) iterator.Current;
            }
        }

        /// <summary>
        /// Перечисление
        /// </summary>
        /// <typeparam name="T">Тип</typeparam>
        /// <param name="enumerable">Перечисление</param>
        /// <param name="action">Метод выполняемый над значениями</param>
        /// <returns>Перечисление</returns>
        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            if (enumerable == null)
                return;
            foreach (var value in enumerable)
            {
                action(value);
            }
        }

        /// <summary>
        /// Делегат для описания вызываемого метода
        /// </summary>
        /// <typeparam name="T">Тип</typeparam>
        /// <param name="value">Значение</param>
        /// <param name="index">Индекс</param>
        public delegate void ForEachAction<T>(T value, int index);

        /// <summary>
        /// Перечисление с индексом
        /// </summary>
        /// <typeparam name="T">Тип</typeparam>
        /// <param name="enumerable">Перечисление</param>
        /// <param name="action">Метод выполняемый над значениями</param>
        /// <returns>Перечисление</returns>
        public static void ForEach<T>(this IEnumerable<T> enumerable, ForEachAction<T> action)
        {
            if (enumerable == null)
                return;
            var i = 0;
            foreach (var value in enumerable)
            {
                action(value, i++);
            }
        }

        /// <summary>
        /// Проверяем является ли перечисление null или пустым/>
        /// </summary>
        /// <typeparam name="T">Тип перечисления</typeparam>
        /// <param name="source">Перечисление</param>
        /// <returns></returns>
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> source)
        {
            return source?.Any() != true;
        }

        public static IEnumerable<T> Descendants<T>(this IEnumerable<T> source, Func<T, IEnumerable<T>> descendBy)
        {
            if (source.IsNullOrEmpty())
                yield break;

            foreach (var value in source)
            {
                yield return value;

                if (descendBy(value).IsNullOrEmpty())
                    continue;

                foreach (T child in descendBy(value).Descendants<T>(descendBy))
                {
                    yield return child;
                }
            }
        }

        public static IEnumerable<T> Distinct<T, V>(this IEnumerable<T> enumerable, Func<T, V> action)
        {
            if (enumerable == null)
                yield break;

            foreach (var value in enumerable.GroupBy(v => action(v)).Select(v => v.First()))
            {
                yield return value;
            }
        }

        public static IEnumerable<T> Distinct<T>(this IEnumerable<T> enumerable, ComparerCreator.EqualityDelegate<T> comparerDelegate)
        {
            if (enumerable == null)
                yield break;

            foreach (var distinctValue in enumerable.Distinct(ComparerCreator.CreateEquality(comparerDelegate)))
            {
                yield return distinctValue;
            }
        }

        public static IEnumerable<T> Cycle<T>(this IEnumerable<T> enumerable)
        {
            if (!(enumerable is IList<T> list))
            {
                list = new List<T>(enumerable);
            }

            if (!list.Any())
            {
                yield break;
            }

            var index = 0;
            while (true)
            {
                yield return list[index++];
                index = index % list.Count;
            }
        }
    }
}
