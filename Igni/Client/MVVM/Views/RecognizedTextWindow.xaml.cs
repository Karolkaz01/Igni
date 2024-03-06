using Client.MVVM.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Client
{
    /// <summary>
    /// Logika interakcji dla klasy CommunicationWindow.xaml
    /// </summary>
    public partial class RecognizedTextWindow : Window
    {
        public RecognizedTextWindow()
        {
            InitializeComponent();
            SetWindowPosition();
        }

        public void SetWindowPosition()
        {
            double screenWidth = SystemParameters.PrimaryScreenWidth;
            double screenHeight = SystemParameters.PrimaryScreenHeight;

            // Ustaw pozycję okna na prawym dolnym rogu ekranu
            Left = screenWidth - Width - 30;
            Top = screenHeight - Height - 70;
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer scv = (ScrollViewer)sender;
            scv.ScrollToVerticalOffset(scv.VerticalOffset - e.Delta);
            e.Handled = true;
        }
    }
}
