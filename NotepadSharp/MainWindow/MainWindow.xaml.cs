using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using WPFUtility;

namespace NotepadSharp {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public static readonly DependencyProperty RestoreHeightProperty = DependencyProperty.Register(
            "RestoreHeight",
            typeof(double),
            typeof(MainWindow),
            new PropertyMetadata(RestoreHeightPropertyChanged)
        );

        public double RestoreHeight {
            get { return (double)GetValue(RestoreHeightProperty); }
            set { SetValue(RestoreHeightProperty, value); }
        }

        private static void RestoreHeightPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            var src = (MainWindow)d;
            src.Height = src.RestoreHeight;
        }

        public static readonly DependencyProperty RestoreWidthProperty = DependencyProperty.Register(
            "RestoreWidth",
            typeof(double),
            typeof(MainWindow),
            new PropertyMetadata(RestoreWidthPropertyChanged)
        );

        public double RestoreWidth {
            get { return (double)GetValue(RestoreWidthProperty); }
            set { SetValue(RestoreWidthProperty, value); }
        }

        private static void RestoreWidthPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            var src = (MainWindow)d;
            src.Width = src.RestoreWidth;
        }

        public static readonly DependencyProperty RestoreLeftProperty = DependencyProperty.Register(
            "RestoreLeft",
            typeof(double),
            typeof(MainWindow),
            new PropertyMetadata(RestoreLeftPropertyChanged)
        );

        public double RestoreLeft {
            get { return (double)GetValue(RestoreLeftProperty); }
            set { SetValue(RestoreLeftProperty, value); }
        }

        private static void RestoreLeftPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            var src = (MainWindow)d;
            src.Left = src.RestoreLeft;
        }

        public static readonly DependencyProperty RestoreTopProperty = DependencyProperty.Register(
            "RestoreTop",
            typeof(double),
            typeof(MainWindow),
            new PropertyMetadata(RestoreTopPropertyChanged)
        );

        public double RestoreTop {
            get { return (double)GetValue(RestoreTopProperty); }
            set { SetValue(RestoreTopProperty, value); }
        }

        private static void RestoreTopPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            var src = (MainWindow)d;
            src.Top = src.RestoreTop;
        }

        public static readonly DependencyProperty IsDockedOnSideProperty = DependencyProperty.Register(
            "IsDockedOnSide",
            typeof(bool),
            typeof(MainWindow),
            new PropertyMetadata(IsDockedOnSidePropertyChanged)
        );

        public bool IsDockedOnSide {
            get { return (bool)GetValue(IsDockedOnSideProperty); }
            set { SetValue(IsDockedOnSideProperty, value); }
        }

        private static void IsDockedOnSidePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            var src = (MainWindow)d;
            if(!src.IsDockedOnSide) {
                src.Top = src.RestoreBounds.Top;
                src.Left = src.RestoreBounds.Left;
                src.Height = src.RestoreBounds.Height;
                src.Width = src.RestoreBounds.Width;
            }
        }

        public MainWindow() {
            DataContext = new MainWindowViewModel();
            InitializeComponent();
        }

        bool _handlingMovement;
        protected override void OnLocationChanged(EventArgs e) {
            if (_handlingMovement) return;
            _handlingMovement = true;
            MouseButtonEventHandler handler = null;
            handler = (s, a) => {
                MouseUp -= handler;
                _handlingMovement = false;
                RestoreTop = RestoreBounds.Top;
                RestoreLeft = RestoreBounds.Left;
            };
            PreviewMouseUp += handler;
            base.OnLocationChanged(e);
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo) {
            if(sizeInfo.NewSize.Height != RestoreBounds.Height && sizeInfo.NewSize.Width != RestoreBounds.Width && WindowState != WindowState.Maximized) {
                IsDockedOnSide = true;
            } else if(IsDockedOnSide) {
                IsDockedOnSide = false;
            } else {
                RestoreHeight = RestoreBounds.Height;
                RestoreWidth = RestoreBounds.Width;
            }

            base.OnRenderSizeChanged(sizeInfo);
        }

        protected override void OnClosing(CancelEventArgs e) {
            ((IViewModelBase)DataContext).Dispose();
            base.OnClosing(e);
        }
    }
}
