using System;
using System.Globalization;
using System.Windows.Data;
using System.Drawing;

namespace Prism.Converters
{
    class ToRGB : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Color col = (Color)value;

            switch (parameter.ToString().ToLower())
            {
                case "r":
                    return col.R.ToString();

                case "g":
                    return col.G.ToString();

                case "b":
                    return col.B.ToString();

                default:
                    return "0";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return 0;
        }
    }
}
