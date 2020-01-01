using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Wpf.Tools
{
    public class FilteredValue<T>
    {
        private readonly Func<T, IList<T>> _getChild;
        private readonly Func<T, string, bool> _filter;
        public FilteredValue(T value, Func<T, IList<T>> getChild, Func<T, string, bool> filter)
        {
            _getChild = getChild;
            _filter = filter;
            Value = value;
            AllChild = getChild(value).Select(v => new FilteredValue<T>(v, getChild, filter)).ToList();
            Filter(string.Empty);
        }

        public T Value { get; }
        public IList<FilteredValue<T>> AllChild { get; }
        public IList<FilteredValue<T>> Child { get; private set; }

        public bool Filter(string filter)
        {
            if (string.IsNullOrWhiteSpace(filter))
            {
                Child = AllChild;
                return true;
            }

            if (_filter(Value, filter))
            {
                Child = AllChild;
                return true;
            }
            var contains = AllChild.Where(v => v.Filter(filter)).ToList();
            if (contains.Count > 0)
            {
                Child = contains;
                return true;
            }
            return false;
        }
    }

    public class FilteredListValue<T> : ObservableObject
    {
        private Func<T, IList<T>> _getChild;
        private Func<T, string, bool> _filter;
        private IList<FilteredValue<T>> _values;

        public FilteredListValue(IList<T> values, Func<T, IList<T>> getChild, Func<T, string, bool> filter)
        {
            _getChild = getChild;
            _filter = filter;
            AllValues = values.Select(v => new FilteredValue<T>(v, getChild, filter)).ToList();
            Filter(string.Empty);
        }

        public IList<FilteredValue<T>> AllValues { get; }

        public IList<FilteredValue<T>> Values
        {
            get => _values;
            private set
            {
                if (Equals(value, _values)) return;
                _values = value;
                OnPropertyChanged();
            }
        }

        public bool Filter(string filter)
        {
            if (string.IsNullOrWhiteSpace(filter))
            {
                Values = AllValues;
                return true;
            }

            Values = AllValues.Where(v => v.Filter(filter)).ToList();
            return true;
        }
    }
}