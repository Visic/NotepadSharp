﻿using System.Windows.Input;
using WPFUtility;

namespace NotepadSharp {
    public class RichTextViewModel : ViewModelBase {
        public RichTextViewModel(string content) {
            Content = new NotifyingProperty<string>(content);
            KeyDownCommand = new RelayCommand(x => KeyDown((KeyEventArgs)x));
            KeyUpCommand = new RelayCommand(x => KeyUp((KeyEventArgs)x));
            LostFocusCommand = new RelayCommand(x => LostFocus());
        }
        
        public NotifyingProperty<string> Content { get; set; }
        public ICommand KeyDownCommand { get; }
        public ICommand KeyUpCommand { get; }
        public ICommand LostFocusCommand { get; }

        private void LostFocus() {
            ArgsAndSettings.KeyBindings.ClearPressedKeys();
        }
        
        private void KeyDown(KeyEventArgs e) {
            e.Handled = ArgsAndSettings.KeyBindings.KeyPressed(GetRelevantKey(e), true);
        }

        private void KeyUp(KeyEventArgs e) {
            ArgsAndSettings.KeyBindings.KeyReleased(GetRelevantKey(e));
        }

        private Key GetRelevantKey(KeyEventArgs e) {
            return e.Key == Key.System ? e.SystemKey : e.Key;
        }
    }
}