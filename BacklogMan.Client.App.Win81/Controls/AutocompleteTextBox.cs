using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace BacklogMan.Client.App.Win81.Controls
{
    public class AutocompleteTextBox : TextBox
    {
        PopupMenu popup = null;

        protected override void OnKeyUp(Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            var start = this.Text.LastIndexOf(this.Separator) + 1;
            var text = this.Text.Substring(start).Trim().ToLower();
            if (string.IsNullOrWhiteSpace(text) == false)
            {
                if (AllAutoCompleteValues != null)
                {
                    var values = from v in AllAutoCompleteValues
                                 where v.ToLower().StartsWith(text)
                                 orderby v
                                 select v;

                    updatePopup(values);
                }
            }
        }

        private bool isOpen = false;
        private async void updatePopup(IEnumerable<string> values)
        {
            if (popup == null) popup = new PopupMenu();
            popup.Commands.Clear();
            foreach (var v in values)
            {
                popup.Commands.Add(new UICommand(v, setAutocompletedWord, v));
            }

            var rect = this.GetRectFromCharacterIndex(Text.LastIndexOf(Separator) + 1, true);

            var targetRect = TransformToVisual(Window.Current.Content).TransformBounds(rect);
            if (!isOpen)
            {
                await popup.ShowForSelectionAsync(targetRect, Placement.Below);
                isOpen = true;
            }
        }

        private void setAutocompletedWord(IUICommand command)
        {
            isOpen = false;
            this.Text = this.Text.Substring(0, this.Text.LastIndexOf(Separator)) + command.Label + Separator;
        }




        public string Separator
        {
            get { return (string)GetValue(SeparatorProperty); }
            set { SetValue(SeparatorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Separator.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SeparatorProperty =
            DependencyProperty.Register("Separator", typeof(string), typeof(AutocompleteTextBox), new PropertyMetadata(" "));


        public List<string> AllAutoCompleteValues
        {
            get { return (List<string>)GetValue(AllAutoCompleteValuesProperty); }
            set { SetValue(AllAutoCompleteValuesProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AllAutoCompleteValues.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AllAutoCompleteValuesProperty =
            DependencyProperty.Register("AllAutoCompleteValues", typeof(List<string>), typeof(AutocompleteTextBox), new PropertyMetadata(null));

        
    }
}
