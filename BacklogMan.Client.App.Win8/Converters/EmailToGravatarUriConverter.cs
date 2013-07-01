using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace BacklogMan.Client.App.Win8.Converters
{
    public class EmailToGravatarUriConverter :IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
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

        public object ConvertBack(object value, Type targetType, object parameter, string language)
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
            // Create a string that contains the name of the hashing algorithm to use.
            String strAlgName = HashAlgorithmNames.Md5;

            // Create a HashAlgorithmProvider object.
            HashAlgorithmProvider objAlgProv = HashAlgorithmProvider.OpenAlgorithm(strAlgName);

            // Create a CryptographicHash object. This object can be reused to continually
            // hash new messages.
            CryptographicHash objHash = objAlgProv.CreateHash();

            // Hash message 1.
            IBuffer buffMsg1 = CryptographicBuffer.ConvertStringToBinary(source, BinaryStringEncoding.Utf8);
            objHash.Append(buffMsg1);
            IBuffer buffHash1 = objHash.GetValueAndReset();

            // Convert the hashes to string values (for display);
            return CryptographicBuffer.EncodeToHexString(buffHash1);
            //return CryptographicBuffer.ConvertBinaryToString(BinaryStringEncoding.Utf8, buffHash1);

        }
    }
}
