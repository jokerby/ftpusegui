using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace FTPUse_gui.Helpers
{
    public sealed class EnumBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var parameterString = parameter as string;
            if (parameterString == null)
                return DependencyProperty.UnsetValue;
            if (Enum.IsDefined(value.GetType(), value) == false)
                return DependencyProperty.UnsetValue;
            var paramvalue = Enum.Parse(value.GetType(), parameterString);
            return paramvalue.Equals(value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var parameterString = parameter as string;
            var valueAsBool = (bool)value;

            if (parameterString != null && valueAsBool)
                return Enum.Parse(targetType, parameterString);
            try
            {
                return Enum.Parse(targetType, "0");
            }
            catch (Exception)
            {
                return DependencyProperty.UnsetValue;
            }
        }
    }
}