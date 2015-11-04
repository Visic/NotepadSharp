using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace NotepadSharp {
    public static class DoubleClickBehavior {
        public static DependencyProperty DoubleClickCommandProperty = DependencyProperty.RegisterAttached(
            "DoubleClickCommand", 
            typeof(ICommand), 
            typeof(DoubleClickBehavior),
            new PropertyMetadata(DoubleClick_PropertyChanged)
        );

        public static void SetDoubleClickCommand(UIElement element, ICommand value) {
            element.SetValue(DoubleClickCommandProperty, value);
        }

        public static ICommand GetDoubleClickCommand(UIElement element) {
            return (ICommand)element.GetValue(DoubleClickCommandProperty);
        }

        private static void DoubleClick_PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {

            var ele = (UIElement)d;
            if(e.NewValue != null) {
                ele.AddHandler(Control.MouseDoubleClickEvent, new RoutedEventHandler(MouseDoubleClick));
            } else {
                ele.RemoveHandler(Control.MouseDoubleClickEvent, new RoutedEventHandler(MouseDoubleClick));
            }
        }

        private static void MouseDoubleClick(object sender, RoutedEventArgs e) {
            var ele = (UIElement)sender;
            var cmd = GetDoubleClickCommand(ele);
            if(cmd.CanExecute(null)) cmd.Execute(null);
        }
    }
}
