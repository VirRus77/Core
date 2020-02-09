using System;

namespace Core.Tools
{
    public class EventArgs<T>:EventArgs
    {
        public EventArgs(T value)
        {
            Value = value;
        }

        public T Value { get; }
    }
}
