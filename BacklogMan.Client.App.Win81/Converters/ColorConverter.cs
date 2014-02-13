using System;
using System.Net;
using System.Windows;
using System.Globalization;
using Windows.UI.Xaml.Data;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace BacklogMan.Client.App.Win81.Converters
{
    public class ColorConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var opacity = 1.0;
            if (parameter is string)
            {
                opacity = double.Parse((string)parameter, CultureInfo.InvariantCulture.NumberFormat);
            }
            else if (parameter is double)
            {
                opacity = (double)opacity;
            }

            var color = ConvertStringToColor(value as string, opacity);
            if (targetType == typeof(Color))
            {
                return color;
            }
            else if (targetType == typeof(Brush))
            {
                return new SolidColorBrush(color);
            }
            else
            {
                return color.ToString();
            }
        }

        public static Color ConvertStringToColor(string value, double opacity)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return Color.FromArgb(255, 0, 0x6d, 0xcc); //006dcc
            }
            else
            {
                if (opacity > 1.0)
                    opacity = 1.0;

                if (opacity < 0.0)
                    opacity = 0.0;

                // get the hex value of the alpha chanel (opacity):
                byte a = (byte)(System.Convert.ToInt32(255 * opacity));

                // --cmt:5afff4b1-daca-4daf-ab56-64864c80235e--
                // deal with the color
                var svalue = value as string;
                try
                {
                    byte r = (byte)(System.Convert.ToUInt32(svalue.Substring(1, 2), 16));
                    byte g = (byte)(System.Convert.ToUInt32(svalue.Substring(3, 2), 16));
                    byte b = (byte)(System.Convert.ToUInt32(svalue.Substring(5, 2), 16));

                    return Color.FromArgb(a, r, g, b);
                }
                catch
                {
                    // pick a fugly color, but don't cause the system to barf.
                    return Color.FromArgb(255, 0, 0x6d, 0xcc);
                }
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
