using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace BacklogMan.Client.Phone8.Converter
{
    public class EmailToGravatarUriConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var email = value as string;

            if (targetType == typeof(Uri))
            {
                return getGravatarUri(email);
            }
            else if (targetType == typeof(ImageSource))
            {
                return new BitmapImage(getGravatarUri(email));
            }
            else
            {
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
        
        private static Uri getGravatarUri(string email)
        {

            var hash = Md5(email.Trim().ToLower()).ToLower();

            return new Uri("http://www.gravatar.com/avatar/" + hash);
        }

        private static string Md5(string source)
        {
            return System.Security.Cryptography.MD5Core.GetHashString(source);
        }
    }
}
