using System;
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
    /// Interaction logic for RichTextView.xaml
    /// </summary>
    public partial class RichTextView : UserControl {
        public static readonly DependencyProperty KeyBindingsProperty =
            DependencyProperty.Register("KeyBindings", typeof(KeyBindingCollection), typeof(RichTextView));

        public KeyBindingCollection KeyBindings {
            get { return (KeyBindingCollection)GetValue(KeyBindingsProperty); }
            set { SetValue(KeyBindingsProperty, value); }
        }

        public RichTextView() {
            InitializeComponent();
        }

        private void UserControl_PreviewKeyDown(object sender, KeyEventArgs e) {
            e.Handled = KeyBindings?.KeyPressed(GetRelevantKey(e)) ?? false;
        }

        private void UserControl_PreviewKeyUp(object sender, KeyEventArgs e) {
            KeyBindings?.KeyReleased(GetRelevantKey(e));
        }

        private Key GetRelevantKey(KeyEventArgs e) {
            return e.Key == Key.System ? e.SystemKey : e.Key;
        }
    }
}
