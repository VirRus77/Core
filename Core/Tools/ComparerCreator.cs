using System;
using System.Collections.Generic;

namespace Core.Tools
{
    /// <summary>
    /// Создание класса сравнения по делегату
    /// </summary>
    public static class ComparerCreator
    {
        /// <summary>
        /// Делегат сравнения
        /// </summary>
        /// <typeparam name="T">Тип сравниваемых значений</typeparam>
        /// <param name="value1">Значение 1</param>
        /// <param name="value2">Значение 2</param>
        /// <returns></returns>
        public delegate int ComparerDelegate<T>(T value1, T value2);

        /// <summary>
        /// Делегат сравнения
        /// </summary>
        /// <typeparam name="T">Тип сравниваемых значений</typeparam>
        /// <param name="value1">Значение 1</param>
        /// <param name="value2">Значение 2</param>
        /// <returns>true если равны, иначе false</returns>
        public delegate bool EqualityDelegate<T>(T value1, T value2);

        /// <summary>
        /// Класс сравнения работающий по делегату
        /// </summary>
        /// <typeparam name="T">Тип сравниваемых значений</typeparam>
        private class CompareByDelegate<T> : IComparer<T>, IEqualityComparer<T>
        {
            private readonly ComparerDelegate<T> _compareDelegate;

            public CompareByDelegate(ComparerDelegate<T> comparerDelegate)
            {
                if (comparerDelegate == null)
                    throw new ArgumentNullException("comparerDelegate");
                _compareDelegate = comparerDelegate;
            }

            public int Compare(T value1, T value2)
            {
                return _compareDelegate(value1, value2);
            }

            public bool Equals(T value1, T value2)
            {
                return Compare(value1, value2) == 0;
            }

            public int GetHashCode(T obj)
            {
                return 0;
            }
        }

        /// <summary>
        /// Класс сравнения работающий по делегату
        /// </summary>
        /// <typeparam name="T">Тип сравниваемых значений</typeparam>
        private class EquatableByDelegate<T> : IEqualityComparer<T>
        {
            private readonly EqualityDelegate<T> _equalityDelegate;

            public EquatableByDelegate(EqualityDelegate<T> equalityDelegate)
            {
                if (equalityDelegate == null)
                    throw new ArgumentNullException("equalityDelegate");
                _equalityDelegate = equalityDelegate;
            }

            public bool Equals(T value1, T value2)
            {
                return _equalityDelegate(value1, value2);
            }

            public int GetHashCode(T obj)
            {
                return 0;
            }
        }

        ///// <summary>
        ///// Статический метод создающий класса сравнения по делегату
        ///// </summary>
        ///// <typeparam name="T">Тип сравниваемых значений</typeparam>
        ///// <param name="compareDelegate">Делегат сравнения</param>
        ///// <returns>Класс сравнения работающий по делегату</returns>
        //public static IComparer<T> Create<T>(ComparerDelegate<T> compareDelegate)
        //{
        //    return new CompareByDelegate<T>(compareDelegate);
        //}

        /// <summary>
        /// Статический метод создающий класса сравнения по делегату
        /// </summary>
        /// <typeparam name="T">Тип сравниваемых значений</typeparam>
        /// <param name="compareDelegate">Делегат сравнения</param>
        /// <returns>Класс сравнения работающий по делегату</returns>
        public static IComparer<T> Create<T>(Func<T, T, int> compareDelegate)
        {
            return new CompareByDelegate<T>((value1, value2) => compareDelegate(value1, value2));
        }

        ///// <summary>
        ///// Статический метод создающий класса сравнения по делегату
        ///// </summary>
        ///// <typeparam name="T">Тип сравниваемых значений</typeparam>
        ///// <param name="compareDelegate">Делегат сравнения</param>
        ///// <returns>Класс сравнения работающий по делегату</returns>
        //public static IEqualityComparer<T> CreateEquality<T>(ComparerDelegate<T> compareDelegate)
        //{
        //    return new CompareByDelegate<T>(compareDelegate);
        //}

        /// <summary>
        /// Статический метод создающий класса сравнения по делегату
        /// </summary>
        /// <typeparam name="T">Тип сравниваемых значений</typeparam>
        /// <param name="compareDelegate">Делегат сравнения</param>
        /// <returns>Класс сравнения работающий по делегату</returns>
        public static IEqualityComparer<T> CreateEquality<T>(EqualityDelegate<T> compareDelegate)
        {
            return new EquatableByDelegate<T>(compareDelegate);
        }

        /// <summary>
        /// Статический метод создающий класса сравнения по делегату
        /// </summary>
        /// <typeparam name="T">Тип сравниваемых значений</typeparam>
        /// <param name="compareDelegate">Делегат сравнения</param>
        /// <returns>Класс сравнения работающий по делегату</returns>
        public static IEqualityComparer<T> CreateEquality<T>(Func<T, T, bool> compareDelegate)
        {
            return new EquatableByDelegate<T>((value1, value2) => compareDelegate(value1, value2));
        }
    }
}
