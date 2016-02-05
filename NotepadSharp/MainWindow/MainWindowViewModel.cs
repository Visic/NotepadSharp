using System.Windows;
using System.Windows.Input;
using WPFUtility;

namespace NotepadSharp {
    public class MainWindowViewModel : ViewModelBase {
        public MainWindowViewModel() {
            ActiveViewModel = new MainViewModel();

            WindowState = new NotifyingSetting<WindowState>(x => OnPropertyChanged("IsMaximized"), ArgsAndSettings.WindowState);
            if (WindowState.Value == System.Windows.WindowState.Minimized) WindowState.Value = System.Windows.WindowState.Normal;

            CloseCommand = new RelayCommand(x => Application.Current.MainWindow.Close());
            MinimizeCommand = new RelayCommand(x => WindowState.Value = System.Windows.WindowState.Minimized);
            MaximizeCommand = new RelayCommand(x => WindowState.Value = System.Windows.WindowState.Maximized);
            RestoreCommand = new RelayCommand(x => {
                if(WindowState.Value == System.Windows.WindowState.Maximized) {
                    WindowState.Value = System.Windows.WindowState.Normal;
                } else if (IsDockedOnSide.Value) {
                    IsDockedOnSide.Value = false;
                }
            });

            Height = new NotifyingSetting<double>(ArgsAndSettings.Height);
            Width = new NotifyingSetting<double>(ArgsAndSettings.Width);
            IsDockedOnSide = new NotifyingProperty<bool>(x => OnPropertyChanged("IsMaximized"));
        }

        public bool IsMaximized { get { return WindowState.Value == System.Windows.WindowState.Maximized || IsDockedOnSide.Value; } }
        public NotifyingProperty<bool> IsDockedOnSide { get; }
        public NotifyingSetting<WindowState> WindowState { get; }
        public ICommand CloseCommand { get; }
        public ICommand MinimizeCommand { get; }
        public ICommand MaximizeCommand { get; }
        public ICommand RestoreCommand { get; }
        public ViewModelBase ActiveViewModel { get; private set; }
        public NotifyingSetting<double> Height { get; }
        public NotifyingSetting<double> Width { get; }
        public NotifyingSetting<double> Top { get; } = new NotifyingSetting<double>(ArgsAndSettings.Top);
        public NotifyingSetting<double> Left { get; } = new NotifyingSetting<double>(ArgsAndSettings.Left);

        public override void Dispose() {
            base.Dispose();
            ActiveViewModel.Dispose();
        }
    }
}