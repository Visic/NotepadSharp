using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace NotepadSharp {
    public static class CommandBindingBehavior {
        public static DependencyProperty DoubleClickCommandProperty = DependencyProperty.RegisterAttached(
            "DoubleClickCommand", 
            typeof(ICommand), 
            typeof(CommandBindingBehavior),
            new PropertyMetadata(DoubleClick_PropertyChanged)
        );

        public static void SetDoubleClickCommand(UIElement element, ICommand value) {
            element.SetValue(DoubleClickCommandProperty, value);
        }

        public static ICommand GetDoubleClickCommand(UIElement element) {
            return (ICommand)element.GetValue(DoubleClickCommandProperty);
        }

        public static DependencyProperty LeftClickCommandProperty = DependencyProperty.RegisterAttached(
            "LeftClickCommand", 
            typeof(ICommand), 
            typeof(CommandBindingBehavior),
            new PropertyMetadata(LeftClick_PropertyChanged)
        );

        public static void SetLeftClickCommand(UIElement element, ICommand value) {
            element.SetValue(LeftClickCommandProperty, value);
        }

        public static ICommand GetLeftClickCommand(UIElement element) {
            return (ICommand)element.GetValue(LeftClickCommandProperty);
        }

        public static DependencyProperty GotFocusCommandProperty = DependencyProperty.RegisterAttached(
            "GotFocusCommand", 
            typeof(ICommand), 
            typeof(CommandBindingBehavior),
            new PropertyMetadata(GotFocus_PropertyChanged)
        );

        public static void SetGotFocusCommand(UIElement element, ICommand value) {
            element.SetValue(GotFocusCommandProperty, value);
        }

        public static ICommand GetGotFocusCommand(UIElement element) {
            return (ICommand)element.GetValue(GotFocusCommandProperty);
        }

        public static DependencyProperty LostFocusCommandProperty = DependencyProperty.RegisterAttached(
            "LostFocusCommand", 
            typeof(ICommand), 
            typeof(CommandBindingBehavior),
            new PropertyMetadata(LostFocus_PropertyChanged)
        );

        public static void SetLostFocusCommand(UIElement element, ICommand value) {
            element.SetValue(LostFocusCommandProperty, value);
        }

        public static ICommand GetLostFocusCommand(UIElement element) {
            return (ICommand)element.GetValue(LostFocusCommandProperty);
        }

        public static DependencyProperty KeyDownCommandProperty = DependencyProperty.RegisterAttached(
            "KeyDownCommand", 
            typeof(ICommand), 
            typeof(CommandBindingBehavior),
            new PropertyMetadata(KeyDown_PropertyChanged)
        );

        public static void SetKeyDownCommand(UIElement element, ICommand value) {
            element.SetValue(KeyDownCommandProperty, value);
        }

        public static ICommand GetKeyDownCommand(UIElement element) {
            return (ICommand)element.GetValue(KeyDownCommandProperty);
        }

        public static DependencyProperty KeyUpCommandProperty = DependencyProperty.RegisterAttached(
            "KeyUpCommand", 
            typeof(ICommand), 
            typeof(CommandBindingBehavior),
            new PropertyMetadata(KeyUp_PropertyChanged)
        );

        public static void SetKeyUpCommand(UIElement element, ICommand value) {
            element.SetValue(KeyUpCommandProperty, value);
        }

        public static ICommand GetKeyUpCommand(UIElement element) {
            return (ICommand)element.GetValue(KeyUpCommandProperty);
        }

        public static DependencyProperty PreviewKeyDownCommandProperty = DependencyProperty.RegisterAttached(
            "PreviewKeyDownCommand", 
            typeof(ICommand), 
            typeof(CommandBindingBehavior),
            new PropertyMetadata(PreviewKeyDown_PropertyChanged)
        );

        public static void SetPreviewKeyDownCommand(UIElement element, ICommand value) {
            element.SetValue(PreviewKeyDownCommandProperty, value);
        }

        public static ICommand GetPreviewKeyDownCommand(UIElement element) {
            return (ICommand)element.GetValue(PreviewKeyDownCommandProperty);
        }

        public static DependencyProperty PreviewKeyUpCommandProperty = DependencyProperty.RegisterAttached(
            "PreviewKeyUpCommand", 
            typeof(ICommand), 
            typeof(CommandBindingBehavior),
            new PropertyMetadata(PreviewKeyUp_PropertyChanged)
        );

        public static void SetPreviewKeyUpCommand(UIElement element, ICommand value) {
            element.SetValue(PreviewKeyUpCommandProperty, value);
        }

        public static ICommand GetPreviewKeyUpCommand(UIElement element) {
            return (ICommand)element.GetValue(PreviewKeyUpCommandProperty);
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
            if(cmd.CanExecute(e)) cmd.Execute(e);
        }

        private static void LeftClick_PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            var ele = (UIElement)d;
            if(e.NewValue != null) {
                ele.AddHandler(UIElement.MouseLeftButtonUpEvent, new RoutedEventHandler(MouseLeftClick));
            } else {
                ele.RemoveHandler(UIElement.MouseLeftButtonUpEvent, new RoutedEventHandler(MouseLeftClick));
            }
        }

        private static void MouseLeftClick(object sender, RoutedEventArgs e) {
            var ele = (UIElement)sender;
            var cmd = GetLeftClickCommand(ele);
            if(cmd.CanExecute(e)) cmd.Execute(e);
        }

        private static void GotFocus_PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            var ele = (UIElement)d;
            if(e.NewValue != null) {
                ele.AddHandler(UIElement.GotFocusEvent, new RoutedEventHandler(GotFocus));
            } else {
                ele.RemoveHandler(UIElement.GotFocusEvent, new RoutedEventHandler(GotFocus));
            }
        }

        private static void GotFocus(object sender, RoutedEventArgs e) {
            var ele = (UIElement)sender;
            var cmd = GetGotFocusCommand(ele);
            if(cmd.CanExecute(e)) cmd.Execute(e);
        }

        private static void LostFocus_PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            var ele = (UIElement)d;
            if(e.NewValue != null) {
                ele.AddHandler(UIElement.LostFocusEvent, new RoutedEventHandler(LostFocus));
            } else {
                ele.RemoveHandler(UIElement.LostFocusEvent, new RoutedEventHandler(LostFocus));
            }
        }

        private static void LostFocus(object sender, RoutedEventArgs e) {
            var ele = (UIElement)sender;
            var cmd = GetLostFocusCommand(ele);
            if(cmd.CanExecute(e)) cmd.Execute(e);
        }

        private static void KeyDown_PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            var ele = (UIElement)d;
            if(e.NewValue != null) {
                ele.AddHandler(UIElement.KeyDownEvent, new RoutedEventHandler(KeyDown));
            } else {
                ele.RemoveHandler(UIElement.KeyDownEvent, new RoutedEventHandler(KeyDown));
            }
        }

        private static void KeyDown(object sender, RoutedEventArgs e) {
            var ele = (UIElement)sender;
            var cmd = GetKeyDownCommand(ele);
            if(cmd.CanExecute(e)) cmd.Execute(e);
        }

        private static void KeyUp_PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            var ele = (UIElement)d;
            if(e.NewValue != null) {
                ele.AddHandler(UIElement.KeyUpEvent, new RoutedEventHandler(KeyUp));
            } else {
                ele.RemoveHandler(UIElement.KeyUpEvent, new RoutedEventHandler(KeyUp));
            }
        }

        private static void KeyUp(object sender, RoutedEventArgs e) {
            var ele = (UIElement)sender;
            var cmd = GetKeyUpCommand(ele);
            if(cmd.CanExecute(e)) cmd.Execute(e);
        }

        private static void PreviewKeyDown_PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            var ele = (UIElement)d;
            if(e.NewValue != null) {
                ele.AddHandler(UIElement.PreviewKeyDownEvent, new RoutedEventHandler(PreviewKeyDown));
            } else {
                ele.RemoveHandler(UIElement.PreviewKeyDownEvent, new RoutedEventHandler(PreviewKeyDown));
            }
        }

        private static void PreviewKeyDown(object sender, RoutedEventArgs e) {
            var ele = (UIElement)sender;
            var cmd = GetPreviewKeyDownCommand(ele);
            if(cmd.CanExecute(e)) cmd.Execute(e);
        }

        private static void PreviewKeyUp_PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            var ele = (UIElement)d;
            if(e.NewValue != null) {
                ele.AddHandler(UIElement.PreviewKeyUpEvent, new RoutedEventHandler(PreviewKeyUp));
            } else {
                ele.RemoveHandler(UIElement.PreviewKeyUpEvent, new RoutedEventHandler(PreviewKeyUp));
            }
        }

        private static void PreviewKeyUp(object sender, RoutedEventArgs e) {
            var ele = (UIElement)sender;
            var cmd = GetPreviewKeyUpCommand(ele);
            if(cmd.CanExecute(e)) cmd.Execute(e);
        }
    }
}
