using System.IO;
using System.Windows;
using System.Windows.Threading;
using Utility;

namespace NotepadSharp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void App_Startup(object sender, StartupEventArgs e)
        {
            Logger.ApplicationLoggerPath = ArgsAndSettings.LogPath;
            if (!Directory.Exists(Constants.FileCachePath)) Directory.CreateDirectory(Constants.FileCachePath);
            CreateAndShowMainWindow();
        }

        private void CreateAndShowMainWindow()
        {
            MainWindow = new MainWindow();
            MainWindow.Show();
        }

        private void OnUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            Logger.LogException(e.Exception);
        }
    }
}
