using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace BacklogMan.Client.App.Win81
{
    public static class Helper
    {
        public static void Rows(this Grid that, string rowsString)
        {
            foreach (var row in rowsString.Split(",".ToCharArray()))
            {
                var def = new RowDefinition();
                if (row.ToLower() == "Auto") 
                {
                    def.Height = new Windows.UI.Xaml.GridLength(0, Windows.UI.Xaml.GridUnitType.Auto);
                }
                else if (row.EndsWith("*")) 
                {
                    def.Height = new Windows.UI.Xaml.GridLength(double.Parse(row.Replace("*","")), Windows.UI.Xaml.GridUnitType.Star);
                }
                else
                {
                    def.Height = new Windows.UI.Xaml.GridLength(double.Parse(row), Windows.UI.Xaml.GridUnitType.Pixel);
                }
                that.RowDefinitions.Add(def);
            }            
        }

        public static void Columns(this Grid that, string columnsString)
        {
            foreach (var col in columnsString.Split(",".ToCharArray()))
            {
                var def = new ColumnDefinition();
                if (col.ToLower() == "Auto")
                {
                    def.Width = new Windows.UI.Xaml.GridLength(0, Windows.UI.Xaml.GridUnitType.Auto);
                }
                else if (col.EndsWith("*"))
                {
                    def.Width = new Windows.UI.Xaml.GridLength(double.Parse(col.Replace("*", "")), Windows.UI.Xaml.GridUnitType.Star);
                }
                else
                {
                    def.Width = new Windows.UI.Xaml.GridLength(double.Parse(col), Windows.UI.Xaml.GridUnitType.Pixel);
                }
                that.ColumnDefinitions.Add(def);
            }
        }
    }
}
