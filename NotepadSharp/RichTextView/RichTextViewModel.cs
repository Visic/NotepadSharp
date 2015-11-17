using System.Windows.Input;
using WPFUtility;

namespace NotepadSharp {
    public class RichTextViewModel : ViewModelBase {
        KeyBindingExecution _keyBindingExecutor = new KeyBindingExecution();

        public RichTextViewModel(string content) {
            Content = new NotifyingProperty<string>(content);
            KeyDownCommand = new RelayCommand(x => _keyBindingExecutor.KeyDown((KeyEventArgs)x));
            KeyUpCommand = new RelayCommand(x => _keyBindingExecutor.KeyUp((KeyEventArgs)x));
            LostFocusCommand = new RelayCommand(x => _keyBindingExecutor.ClearPressedKeys());
        }
        
        public NotifyingProperty<string> Content { get; set; }
        public ICommand KeyDownCommand { get; }
        public ICommand KeyUpCommand { get; }
        public ICommand LostFocusCommand { get; }
    }
}
