using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace BacklogMan.Client.App.Win81.Converters
{
    public class StatusToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is Core.Model.StoryStatus)
            {
                switch ((Core.Model.StoryStatus)value)
                {
                    case Core.Model.StoryStatus.InProgress:
                        return "In Progress";
                    case Core.Model.StoryStatus.ToDo:
                        return "To Do";
                    default:
                        return ((Core.Model.StoryStatus)value).ToString();
                }
            }
            return "unknown";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
