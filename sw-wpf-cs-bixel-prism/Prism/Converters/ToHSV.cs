using System;
using System.Globalization;
using System.Windows.Data;
using System.Drawing;

namespace Prism.Converters
{
    class ToHSV : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Color col = (Color)value;

            switch (parameter.ToString().ToLower())
            {
                case "h":
                    return col.GetHue().ToString();

                case "s":
                    return col.GetSaturation().ToString();

                case "v":
                    return (col.GetBrightness() * 2).ToString();

                default:
                    return 0;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return 0;
        }
    }
}
