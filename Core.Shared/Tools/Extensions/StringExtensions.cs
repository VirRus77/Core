using System;
using System.Collections.Generic;

namespace Core.Tools.Extensions
{
    public static class StringExtensions
    {
        public static bool Contains(this string baseValue, string value, StringComparison comparison)
        {
            if (string.IsNullOrEmpty(baseValue))
                return false;
            return baseValue.IndexOf(value, comparison) != -1;
        }
        /// <summary>
        /// <see cref="https://stackoverflow.com/questions/2641326/finding-all-positions-of-substring-in-a-larger-string-in-c-sharp"/>
        /// </summary>
        /// <param name="baseValue"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static IEnumerable<int> AllIndexesOf(this string baseValue, string value, StringComparison comparison = StringComparison.CurrentCulture)
        {
            if (string.IsNullOrEmpty(baseValue))
                throw new ArgumentException("the string to find may not be empty", nameof(baseValue));

            if (string.IsNullOrEmpty(value))
                throw new ArgumentException("the string to find may not be empty", nameof(value));

            for (var index = 0; ; index += value.Length)
            {
                index = baseValue.IndexOf(value, index, comparison);
                if (index == -1)
                {
                    yield break;
                }
                yield return index;
            }
        }
    }
}
