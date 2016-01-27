using System;
using System.Collections;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NotepadSharp {
    /// <summary>
    /// Interaction logic for ButtonBarView.xaml
    /// </summary>
    public partial class ButtonBarView : UserControl {
        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.RegisterAttached(
            "ItemsSource",
            typeof(IEnumerable),
            typeof(ButtonBarView),
            new PropertyMetadata(ItemsSourceProperty_PropertyChanged)
        );

        public IEnumerable ItemsSource {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        FlowDirection _flowDirection;
        public new FlowDirection FlowDirection {
            get { return _flowDirection; }
            set {
                _flowDirection = value;
                ItemsControl.FlowDirection = value;
            }
        }

        public ButtonBarView() {
            InitializeComponent();
        }

        private static void ItemsSourceProperty_PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            var src = (ButtonBarView)d;
            src.ItemsControl.ItemsSource = (IEnumerable)e.NewValue;
        }
    }
}
