using System;
using System.Runtime.InteropServices;

namespace Core.Tools.Extensions
{
    public static class StructExtensions
    {
        /// <summary>
        /// converts byte[] to struct
        /// </summary>
        public static T RawDeserialize<T>(byte[] rawData, int position = 0)
            where T : struct
        {
            var rawSize = Marshal.SizeOf(typeof(T));
            if (rawSize > rawData.Length - position)
                throw new ArgumentException("Not enough data to fill struct. Array length from position: " + (rawData.Length - position) + ", Struct length: " + rawSize);
            var buffer = Marshal.AllocHGlobal(rawSize);
            Marshal.Copy(rawData, position, buffer, rawSize);
            var deserialize = (T)Marshal.PtrToStructure(buffer, typeof(T));
            Marshal.FreeHGlobal(buffer);
            return deserialize;
        }

        /// <summary>
        /// converts a struct to byte[]
        /// </summary>
        public static byte[] RawSerialize<T>(this T anything)
            where T : struct
        {
            var rawSize = Marshal.SizeOf(anything);
            var buffer = Marshal.AllocHGlobal(rawSize);
            Marshal.StructureToPtr(anything, buffer, false);
            var rawData = new byte[rawSize];
            Marshal.Copy(buffer, rawData, 0, rawSize);
            Marshal.FreeHGlobal(buffer);
            return rawData;
        }
    }
}
