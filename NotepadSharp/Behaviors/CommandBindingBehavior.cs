using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace NotepadSharp {
    public static class CommandBindingBehavior {
        public static DependencyProperty TextChangedCommandProperty = DependencyProperty.RegisterAttached(
            "TextChangedCommand", 
            typeof(ICommand), 
            typeof(CommandBindingBehavior),
            new PropertyMetadata(TextChangedCommand_PropertyChanged)
        );

        public static void SetTextChangedCommand(TextBox element, ICommand value) {
            element.SetValue(TextChangedCommandProperty, value);
        }

        public static ICommand GetTextChangedCommand(TextBox element) {
            return (ICommand)element.GetValue(TextChangedCommandProperty);
        }

        public static DependencyProperty PreviewLostKeyboardFocusCommandProperty = DependencyProperty.RegisterAttached(
            "PreviewLostKeyboardFocusCommand", 
            typeof(ICommand), 
            typeof(CommandBindingBehavior),
            new PropertyMetadata(PreviewLostKeyboardFocus_PropertyChanged)
        );

        public static void SetPreviewLostKeyboardFocusCommand(UIElement element, ICommand value) {
            element.SetValue(PreviewLostKeyboardFocusCommandProperty, value);
        }

        public static ICommand GetPreviewLostKeyboardFocusCommand(UIElement element) {
            return (ICommand)element.GetValue(PreviewLostKeyboardFocusCommandProperty);
        }

        public static DependencyProperty PreviewDropCommandProperty = DependencyProperty.RegisterAttached(
            "PreviewDropCommand", 
            typeof(ICommand), 
            typeof(CommandBindingBehavior),
            new PropertyMetadata(PreviewDrop_PropertyChanged)
        );

        public static void SetPreviewDropCommand(UIElement element, ICommand value) {
            element.SetValue(PreviewDropCommandProperty, value);
        }

        public static ICommand GetPreviewDropCommand(UIElement element) {
            return (ICommand)element.GetValue(PreviewDropCommandProperty);
        }

        public static DependencyProperty PreviewDragOverCommandProperty = DependencyProperty.RegisterAttached(
            "PreviewDragOverCommand", 
            typeof(ICommand), 
            typeof(CommandBindingBehavior),
            new PropertyMetadata(PreviewDragOver_PropertyChanged)
        );

        public static void SetPreviewDragOverCommand(UIElement element, ICommand value) {
            element.SetValue(PreviewDragOverCommandProperty, value);
        }

        public static ICommand GetPreviewDragOverCommand(UIElement element) {
            return (ICommand)element.GetValue(PreviewDragOverCommandProperty);
        }

        public static DependencyProperty LostKeyboardFocusCommandProperty = DependencyProperty.RegisterAttached(
            "LostKeyboardFocusCommand", 
            typeof(ICommand), 
            typeof(CommandBindingBehavior),
            new PropertyMetadata(LostKeyboardFocus_PropertyChanged)
        );

        public static void SetLostKeyboardFocusCommand(UIElement element, ICommand value) {
            element.SetValue(LostKeyboardFocusCommandProperty, value);
        }

        public static ICommand GetLostKeyboardFocusCommand(UIElement element) {
            return (ICommand)element.GetValue(LostKeyboardFocusCommandProperty);
        }

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

        public static DependencyProperty PreviewLeftClickCommandProperty = DependencyProperty.RegisterAttached(
            "PreviewLeftClickCommand", 
            typeof(ICommand), 
            typeof(CommandBindingBehavior),
            new PropertyMetadata(PreviewLeftClick_PropertyChanged)
        );

        public static void SetPreviewLeftClickCommand(UIElement element, ICommand value) {
            element.SetValue(PreviewLeftClickCommandProperty, value);
        }

        public static ICommand GetPreviewLeftClickCommand(UIElement element) {
            return (ICommand)element.GetValue(PreviewLeftClickCommandProperty);
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

        public static DependencyProperty PreviewMouseMoveCommandProperty = DependencyProperty.RegisterAttached(
            "PreviewMouseMoveCommand", 
            typeof(ICommand), 
            typeof(CommandBindingBehavior),
            new PropertyMetadata(PreviewMouseMove_PropertyChanged)
        );

        public static void SetPreviewMouseMoveCommand(UIElement element, ICommand value) {
            element.SetValue(PreviewMouseMoveCommandProperty, value);
        }

        public static ICommand GetPreviewMouseMoveCommand(UIElement element) {
            return (ICommand)element.GetValue(PreviewMouseMoveCommandProperty);
        }

        private static void TextChangedCommand_PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            var ele = (TextBox)d;
            if(e.NewValue != null) {
                ele.AddHandler(TextBox.TextChangedEvent, new RoutedEventHandler(TextChanged));
            } else {
                ele.RemoveHandler(TextBox.TextChangedEvent, new RoutedEventHandler(TextChanged));
            }
        }

        private static void TextChanged(object sender, RoutedEventArgs e) {
            var ele = (TextBox)sender;
            var cmd = GetTextChangedCommand(ele);
            if(cmd.CanExecute(e)) cmd.Execute(e);
        }

        private static void PreviewMouseMove_PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            var ele = (UIElement)d;
            if(e.NewValue != null) {
                ele.AddHandler(UIElement.PreviewMouseMoveEvent, new RoutedEventHandler(PreviewMouseMove));
            } else {
                ele.RemoveHandler(UIElement.PreviewMouseMoveEvent, new RoutedEventHandler(PreviewMouseMove));
            }
        }

        private static void PreviewMouseMove(object sender, RoutedEventArgs e) {
            var ele = (UIElement)sender;
            var cmd = GetPreviewMouseMoveCommand(ele);
            if(cmd.CanExecute(e)) cmd.Execute(e);
        }

        private static void PreviewLostKeyboardFocus_PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            var ele = (UIElement)d;
            if(e.NewValue != null) {
                ele.AddHandler(UIElement.PreviewLostKeyboardFocusEvent, new RoutedEventHandler(PreviewLostKeyboardFocus));
            } else {
                ele.RemoveHandler(UIElement.PreviewLostKeyboardFocusEvent, new RoutedEventHandler(PreviewLostKeyboardFocus));
            }
        }

        private static void PreviewLostKeyboardFocus(object sender, RoutedEventArgs e) {
            var ele = (UIElement)sender;
            var cmd = GetPreviewLostKeyboardFocusCommand(ele);
            if(cmd.CanExecute(e)) cmd.Execute(e);
        }

        private static void PreviewDrop_PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            var ele = (UIElement)d;
            if(e.NewValue != null) {
                ele.AddHandler(UIElement.PreviewDropEvent, new RoutedEventHandler(PreviewDrop));
            } else {
                ele.RemoveHandler(UIElement.PreviewDropEvent, new RoutedEventHandler(PreviewDrop));
            }
        }

        private static void PreviewDrop(object sender, RoutedEventArgs e) {
            var ele = (UIElement)sender;
            var cmd = GetPreviewDropCommand(ele);
            if(cmd.CanExecute(e)) cmd.Execute(e);
        }

        private static void PreviewDragOver_PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            var ele = (UIElement)d;
            if(e.NewValue != null) {
                ele.AddHandler(UIElement.PreviewDragOverEvent, new RoutedEventHandler(PreviewDragOver));
            } else {
                ele.RemoveHandler(UIElement.PreviewDragOverEvent, new RoutedEventHandler(PreviewDragOver));
            }
        }

        private static void PreviewDragOver(object sender, RoutedEventArgs e) {
            var ele = (UIElement)sender;
            var cmd = GetPreviewDragOverCommand(ele);
            if(cmd.CanExecute(e)) cmd.Execute(e);
        }

        private static void LostKeyboardFocus_PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            var ele = (UIElement)d;
            if(e.NewValue != null) {
                ele.AddHandler(UIElement.LostKeyboardFocusEvent, new RoutedEventHandler(LostKeyboardFocus));
            } else {
                ele.RemoveHandler(UIElement.LostKeyboardFocusEvent, new RoutedEventHandler(LostKeyboardFocus));
            }
        }

        private static void LostKeyboardFocus(object sender, RoutedEventArgs e) {
            var ele = (UIElement)sender;
            var cmd = GetLostKeyboardFocusCommand(ele);
            if(cmd.CanExecute(e)) cmd.Execute(e);
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

        private static void PreviewLeftClick_PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            var ele = (UIElement)d;
            if(e.NewValue != null) {
                ele.AddHandler(UIElement.PreviewMouseLeftButtonDownEvent, new RoutedEventHandler(MousePreviewLeftClick));
            } else {
                ele.RemoveHandler(UIElement.PreviewMouseLeftButtonDownEvent, new RoutedEventHandler(MousePreviewLeftClick));
            }
        }

        private static void MousePreviewLeftClick(object sender, RoutedEventArgs e) {
            var ele = (UIElement)sender;
            var cmd = GetPreviewLeftClickCommand(ele);
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
