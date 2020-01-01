using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Tools
{
    /// <summary>
    /// Хранилище данных. При отсутствии создает значение и сохраняет.
    /// </summary>
    /// <typeparam name="TKey">Тип ключа</typeparam>
    /// <typeparam name="TValue">Тип значения</typeparam>
    public class Storage<TKey, TValue> : Dictionary<TKey, TValue>
        where TKey : class
    {
        private readonly Func<TKey, TKey, bool> _keyEquals;
        private readonly Func<TKey, TValue> _createValue;
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="keyEquals">Функция сравнинея на равнозначность ключей</param>
        /// <param name="createValue">Функция для сознания значения по ключу</param>
        public Storage(Func<TKey, TKey, bool> keyEquals, Func<TKey, TValue> createValue)
        {
            _keyEquals = keyEquals;
            _createValue = createValue;
        }
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="createValue">Функция для сознания значения по ключу</param>
        public Storage(Func<TKey, TValue> createValue)
            : this((v1, v2) => Equals(v1, v2), createValue)
        {
        }

        /// <summary>
        /// Получить значение по ключу
        /// </summary>
        /// <param name="key">Ключ</param>
        /// <returns>Возвращает значение, если отсутствует в хранилище создаст и вернёт</returns>
        public TValue GetTryValue(TKey key)
        {
            var foundKey = Keys.FirstOrDefault(v => _keyEquals(v, key));
            var value = foundKey == null
                ? _createValue(key)
                : this[foundKey];

            if (foundKey == null)
            {
                Add(key, value);
            }

            return value;
        }
    }
}
