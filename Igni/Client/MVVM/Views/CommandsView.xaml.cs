using Core.Consts;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Client.MVVM.Views
{
    /// <summary>
    /// Logika interakcji dla klasy CommandsView.xaml
    /// </summary>
    public partial class CommandsView : UserControl
    {
        public CommandsView()
        {
            InitializeComponent();
        }

        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer scv = (ScrollViewer)sender;
            scv.ScrollToVerticalOffset(scv.VerticalOffset - e.Delta);
            e.Handled = true;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string directoryPath = System.IO.Path.Combine(Directory.GetCurrentDirectory() , Paths.SCRIPTS);

            // Check if the directory exists
            if (System.IO.Directory.Exists(directoryPath))
            {
                // Open File Explorer to the specified directory
                Process.Start("explorer.exe", directoryPath);
            }
        }
    }
}
