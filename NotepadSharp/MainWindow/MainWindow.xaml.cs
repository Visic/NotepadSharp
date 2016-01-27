using System.ComponentModel;
using System.Windows;
using WPFUtility;

namespace NotepadSharp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            DataContext = new MainWindowViewModel();
            InitializeComponent();
        }

        protected override void OnClosing(CancelEventArgs e) {
            ((IViewModelBase)DataContext).Dispose();
            base.OnClosing(e);
        }
    }
}
