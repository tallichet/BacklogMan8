using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace BacklogMan.Client.App.Win81.Controls
{
    public sealed partial class PointsPicker : UserControl
    {
        public PointsPicker()
        {
            this.InitializeComponent();
        }

        private void point_click(object sender, TappedRoutedEventArgs e)
        {
            if (this.PointsChoosed != null)
            {
                PointsChoosed(this, new PointsChoosedEventArgs(int.Parse((sender as Grid).Tag.ToString())));
            }
        }

        public event EventHandler<PointsChoosedEventArgs> PointsChoosed;
    }

    public class PointsChoosedEventArgs : EventArgs
    {
        public PointsChoosedEventArgs(int points)
        {
            this.Points = points;
        }

        public int Points { get; private set; }
    }
}
