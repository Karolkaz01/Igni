using System.Configuration;
using System.Data;
using System.Windows;
using System.Windows.Forms;
using System.Drawing;
using Core.Models.Notifications;
using MediatR;
using Client.MVVM.ViewModels;
using System.Threading;

namespace Client
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        private const string UniqueIdentifier = "{32627d1a-91c3-4f12-b2d9-7e6376d6f264}";
        private static Mutex _mutex;

        protected override void OnStartup(StartupEventArgs e)
        {
            try
            {
                _mutex = new Mutex(true, UniqueIdentifier, out bool createdNew);
                if (!createdNew)
                {
                    // Jeśli instancja już istnieje, zamykamy aplikację
                    Current.Shutdown();
                    return;
                }

                IocConfiguration.LoadDependenciesAsync();

                var recogWindow = IocConfiguration.Get<RecognizedTextWindow>();
                var recogViewModel = IocConfiguration.Get<RecognizedTextViewModel>();
                recogWindow.DataContext = recogViewModel;
                //var comViewModel = IocConfiguration.Get<CommunicationViewModel>();
                //comWindow.DataContext = comViewModel;
                //comWindow.Show();
                //comViewModel.RecognizedText = "Tekst test 1asdadsa";


                NotifyIcon notifyIcon = new NotifyIcon();
                notifyIcon.Icon = new Icon(@"Images\IgniIcon.ico");
                notifyIcon.Visible = true;
                notifyIcon.MouseClick += (s, e) =>
                {
                    this.MainWindow.Show();
                };



                var mainWindow = IocConfiguration.Get<MainWindow>();
                var mainViewModel = IocConfiguration.Get<MainWindowViewModel>();
                mainWindow.DataContext = mainViewModel;
                this.MainWindow = mainWindow;
                mainWindow.Show();

                base.OnStartup(e);
            } catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                System.Windows.MessageBox.Show(ex.ToString(), "Error");
                _mutex?.ReleaseMutex();
                Current.Shutdown();
            }
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _mutex?.ReleaseMutex();
            base.OnExit(e);
        }
    }

}
