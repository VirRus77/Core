using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using Core.Tools.Extensions;

namespace Core.Converters
{
    public class EnumFlagToBooleanConverter : DependencyObject,IValueConverter, IMultiValueConverter
    {
        public static readonly DependencyProperty EnumFlagProperty = DependencyProperty.Register(
            "EnumFlag", typeof(object), typeof(EnumFlagToBooleanConverter), new PropertyMetadata(default(object)));

        public object EnumFlag
        {
            get { return (object) GetValue(EnumFlagProperty); }
            set { SetValue(EnumFlagProperty, value); }
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is Enum enumValue) || !(parameter is Enum enumParamteterValue))
                return DependencyProperty.UnsetValue;

            return enumValue.GetFlag(enumParamteterValue);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var enumValues = values.OfType<Enum>().ToList();
            if (enumValues.Count != values.Length)
            {
                return DependencyProperty.UnsetValue;
            }

            var enumType = enumValues.First().GetType();

            var flags = enumValues.Skip(1)
                .Aggregate(Activator.CreateInstance(enumType), (aggregate, nextValue) => aggregate.SetFlag(nextValue));
            return enumValues.First().GetFlag(flags);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
