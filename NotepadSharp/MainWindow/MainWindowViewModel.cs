using System.Windows;
using WPFUtility;

namespace NotepadSharp {
    public class MainWindowViewModel : ViewModelBase {
        public MainWindowViewModel() {
            ActiveViewModel = new MainViewModel();
        }

        public NotifyingSetting<double> Height { get; } = new NotifyingSetting<double>(ArgsAndSettings.Height);
        public NotifyingSetting<double> Width { get; } = new NotifyingSetting<double>(ArgsAndSettings.Width);
        public NotifyingSetting<double> Top { get; } = new NotifyingSetting<double>(ArgsAndSettings.Top);
        public NotifyingSetting<double> Left { get; } = new NotifyingSetting<double>(ArgsAndSettings.Left);
        public NotifyingSetting<WindowState> WindowState { get; } = new NotifyingSetting<WindowState>(ArgsAndSettings.WindowState);
        public ViewModelBase ActiveViewModel { get; private set; }

        public override void Dispose() {
            base.Dispose();
            ActiveViewModel.Dispose();
        }
    }
}