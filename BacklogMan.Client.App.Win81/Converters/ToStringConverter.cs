using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace BacklogMan.Client.App.Win81.Converters
{
    public class ToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string myString;

            if (value is string)
            {
                myString = value as string;
            }
            else
            {
                myString = value.ToString();
            }

            if (parameter is string)
            {
                var parameterString = parameter as string;

                if (parameterString == "(")
                {
                    myString = "(" + myString + ")";
                }
                else if (parameterString == "«")
                {
                    myString = "« " + myString + " »";
                }
                else
                {
                    myString += parameterString + myString;
                }
            }

            return myString;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
