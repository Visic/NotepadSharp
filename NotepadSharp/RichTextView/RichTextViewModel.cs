using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WPFUtility;

namespace NotepadSharp {
    public class RichTextViewModel : ViewModelBase {
        public RichTextViewModel(string content) {
            Content = new NotifyingProperty<string>(content);
            KeyDownCommand = new RelayCommand(x => KeyDown((KeyEventArgs)x));
            KeyUpCommand = new RelayCommand(x => KeyUp((KeyEventArgs)x));
            ArgsAndSettings.KeyBindings.SetBinding(new KeyBinding(() => Content.Value = Content.Value + "!", "Testing 1", Key.LeftCtrl, Key.E));
        }
        
        public NotifyingProperty<string> Content { get; set; }
        public ICommand KeyDownCommand { get; }
        public ICommand KeyUpCommand { get; }
        
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
