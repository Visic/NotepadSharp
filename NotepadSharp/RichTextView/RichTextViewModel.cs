using System;
using System.Windows.Input;
using WPFUtility;

namespace NotepadSharp {
    public class RichTextViewModel : ViewModelBase {
        KeyBindingExecution _keyBindingExecutor = new KeyBindingExecution();

        public RichTextViewModel(string content) {
            Api = new NotifyingProperty<RichTextBox_LuaApiProvider>(new RichTextBox_LuaApiProvider());
            Content = new NotifyingProperty<string>(content);
            KeyDownCommand = new RelayCommand(x => _keyBindingExecutor.KeyDown((KeyEventArgs)x));
            KeyUpCommand = new RelayCommand(x => _keyBindingExecutor.KeyUp((KeyEventArgs)x));
            LostFocusCommand = new RelayCommand(x => _keyBindingExecutor.ClearPressedKeys());

            _keyBindingExecutor.SetScriptArg("api", Api.Value);
        }
        
        public NotifyingProperty<RichTextBox_LuaApiProvider> Api { get; }
        public NotifyingProperty<string> Content { get; set; }
        public ICommand KeyDownCommand { get; }
        public ICommand KeyUpCommand { get; }
        public ICommand LostFocusCommand { get; }
    }
}
