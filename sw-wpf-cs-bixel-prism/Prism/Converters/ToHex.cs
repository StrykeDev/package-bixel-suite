using System;
using System.Globalization;
using System.Windows.Data;
using System.Drawing;

namespace Prism.Converters
{
    class ToHex : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Color col = (Color)value;
            return col.Name;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Color col = Color.FromName((string)value);
            return col;
        }
    }
}
