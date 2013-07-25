using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace BacklogMan.Client.Phone8.Converter
{
    public class ColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
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

            return ConvertStringToColor(value as string, opacity);
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public static Color ConvertStringToColor(string value, double opacity)
        {
            if (value == null)
            {
                return Color.FromArgb(255, 247, 194, 17);
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
                    return Color.FromArgb(255, 247, 194, 17);
                }
            }
        }
    }
}
