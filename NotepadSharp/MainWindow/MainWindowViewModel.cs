using System.Windows;
using WPFUtility;

namespace NotepadSharp {
    public class MainWindowViewModel : ViewModelBase {
        public MainWindowViewModel() {
            Initialize();
        }

        public NotifyingSetting<double> Height { get; set; }
        public NotifyingSetting<double> Width { get; set; }
        public NotifyingSetting<double> Top { get; set; }
        public NotifyingSetting<double> Left { get; set; }
        public NotifyingSetting<WindowState> WindowState { get; set; }
        public ViewModelBase ActiveViewModel { get; private set; }

        private void Initialize() {
            Height = new NotifyingSetting<double>(ArgsAndSettings.Height);
            Width = new NotifyingSetting<double>(ArgsAndSettings.Width);
            Top = new NotifyingSetting<double>(ArgsAndSettings.Top);
            Left = new NotifyingSetting<double>(ArgsAndSettings.Left);
            WindowState = new NotifyingSetting<WindowState>(ArgsAndSettings.WindowState);
            ActiveViewModel = new MainViewModel();
        }
    }
}