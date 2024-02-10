using System.Configuration;
using System.Data;
using System.Windows;
using System.Windows.Forms;
using System.Drawing;
using Core.Models.Notifications;
using MediatR;
using Client.MVVM.ViewModels;

namespace Client
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            IocConfiguration.LoadDependenciesAsync();

            //var comWindow = IocConfiguration.Get<CommunicationWindow>();
            //var comViewModel = IocConfiguration.Get<CommunicationViewModel>();
            //comWindow.DataContext = comViewModel;
            //comWindow.Show();
            //comViewModel.RecognizedText = "LALALALALAAL";

            //NotifyIcon notifyIcon = new NotifyIcon();
            //notifyIcon.Icon = new Icon(@"Images\IgniIcon.ico");
            //notifyIcon.Visible = true;

            //notifyIcon.MouseClick += (s, e) =>
            //{
            //    MainWindow.Show();
            //};



            var mainWindow = IocConfiguration.Get<MainWindow>();
            var mainViewModel = IocConfiguration.Get<MainWindowViewModel>();
            mainWindow.DataContext = mainViewModel;
            //this.MainWindow = mainWindow;
            mainWindow.Show();

            base.OnStartup(e);
        }
    }

}
