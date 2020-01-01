using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Core.Tools.Extensions
{
    public static class StreamExtensions
    {
        public static T RawDeserialize<T>(this Stream stream)
            where T : struct
        {
            var rawSize = Marshal.SizeOf(typeof(T));
            var readBufferSize = 1024 * 1024;
            var buffer = new byte[rawSize];
            var readPosition = 0;
            var readed = stream.Read(buffer, readPosition, Math.Min(readBufferSize, rawSize));
            while (readed != 0)
            {
                readPosition += readed;
                if (readPosition < rawSize)
                {
                    readed = stream.Read(buffer, readPosition, Math.Min(rawSize - readPosition, readBufferSize));
                    continue;
                }
                else
                {
                    break;
                }
            }

            if (readPosition != rawSize)
            {
                throw new Exception();
            }
            return StructExtensions.RawDeserialize<T>(buffer);
        }

        public static void RawSerialize<T>(this Stream stream, T value)
            where T : struct
        {
            var buffer = value.RawSerialize();
            stream.Write(buffer, 0, buffer.Length);
        }
    }
}
