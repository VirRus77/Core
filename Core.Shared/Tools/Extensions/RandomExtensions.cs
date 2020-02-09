using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Tools.Extensions
{
    public static class RandomExtensions
    {
        private const string EnLowCase = "abcdefghijklmnopqrstuvwxyz";
        private const string EnUpCase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private const string Numbers = "0123456789";
        private const string RusLowCase = "абвгдеёжзийклмнопрстуфхцчшщъыьэюя";
        private const string RusUpCase = "АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ";

        private static readonly Dictionary<UsingStringChars, string> StringCharsToString =
            new Dictionary<UsingStringChars, string>()
            {
                {UsingStringChars.EnLowCase, EnLowCase},
                {UsingStringChars.EnUpCase, EnUpCase},
                {UsingStringChars.Numbers, Numbers},
                {UsingStringChars.RusLowCase, RusLowCase},
                {UsingStringChars.RusUpCase, RusUpCase},
            };

        [Flags]
        public enum UsingStringChars
        {
            EnLowCase = 0x1,
            EnUpCase = 0x2,
            Numbers = 0x4,
            RusLowCase = 0x8,
            RusUpCase = 0x10,
            All = EnLowCase | EnUpCase | Numbers | RusLowCase | RusUpCase,
        }

        public static string GetString(UsingStringChars usingChars)
        {
            var chars = string.Join("",
                StringCharsToString
                    .Where(v => usingChars.HasFlag(v.Key))
                    .Select(v => v.Value));
            return chars;
        }

        public static string RandomString(
            this Random random,
            int length,
            UsingStringChars usingChars = UsingStringChars.All
        )
        {
            return random.RandomString(length, GetString(usingChars));
        }

        public static string RandomString(this Random random, int length, string chars)
        {
            return new string(
                Enumerable
                    .Repeat(chars, length)
                    .Select(s => s[random.Next(s.Length)])
                    .ToArray());
        }

        public static bool RandomBool(this Random random)
        {
            return random.Next(2) == 1;
        }

        public static T RandomEnum<T>(this Random random)
        {
            var values = Enum.GetValues(typeof(T)).OfType<T>().ToList();
            return random.Random(values);
        }

        public static T RandomEnumFlags<T>(this Random random, int minValues = 1, int maxValues = 3)
        {
            var values = Enum.GetValues(typeof(T)).OfType<T>().ToList();
            minValues = Math.Min(values.Count, minValues);
            maxValues = Math.Min(values.Count, maxValues);

            var randValues = new List<T>();
            var value = random.Random(values);
            var cicle = 0;
            while (true)
            {
                cicle += 1;
                if (randValues.Contains(value))
                {
                    value = random.Random(values);
                    continue;
                }

                if (randValues.Count >= maxValues)
                    break;
                if (randValues.Count >= minValues && cicle >= maxValues)
                    break;
                randValues.Add(value);
                value = value = random.Random(values);
            }

            return (T) randValues.Aggregate(default(T), (seed, nextValue) => seed.SetFlag(nextValue));
        }

        public static T Random<T>(this Random random, ICollection<T> collection)
        {
            return collection.ElementAt(random.Next(collection.Count));
        }
    }
}