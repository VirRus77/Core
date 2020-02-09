using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Data;

namespace Core.Converters
{
    /// <summary>
    /// Конвертер для вызова нескольких конвертеров подряд
    /// <see cref="http://stackoverflow.com/questions/2607490/is-there-a-way-to-chain-multiple-value-converters-in-xaml"/>
    /// <see cref="https://www.codeproject.com/articles/15061/piping-value-converters-in-wpf"/>
    /// </summary>
    public class ValueConverterGroup : List<IValueConverter>, IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return this.Aggregate(value, (current, converter) => converter.Convert(current, targetType, parameter, culture));
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
