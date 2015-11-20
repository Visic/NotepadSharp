using System;
using System.Windows.Input;
using WPFUtility;

namespace NotepadSharp {
    public class RichTextViewModel : ViewModelBase {
        KeyBindingExecution _keyBindingExecutor;

        public RichTextViewModel(string content) {
            ApiProvider = new NotifyingProperty<RichTextBox_LuaApiProvider>(new RichTextBox_LuaApiProvider());
            Content = new NotifyingProperty<string>(content);
            KeyDownCommand = new RelayCommand(x => _keyBindingExecutor.KeyDown((KeyEventArgs)x));
            KeyUpCommand = new RelayCommand(x => _keyBindingExecutor.KeyUp((KeyEventArgs)x));
            LostFocusCommand = new RelayCommand(x => _keyBindingExecutor.ClearPressedKeys());
            _keyBindingExecutor = new KeyBindingExecution(ex => {
                ApplicationState.SetMessageAreaTextColor("DarkRed");
                ApplicationState.SetMessageAreaText(ex.Message);
            });

            ApiProvider.Value.SetMessageAreaText = ApplicationState.SetMessageAreaText;
            ApiProvider.Value.SetMessageAreaTextColor = ApplicationState.SetMessageAreaTextColor;
            _keyBindingExecutor.SetScriptArg("textbox", ApiProvider.Value);
        }
        
        public NotifyingProperty<RichTextBox_LuaApiProvider> ApiProvider { get; }
        public NotifyingProperty<string> Content { get; }
        public ICommand KeyDownCommand { get; }
        public ICommand KeyUpCommand { get; }
        public ICommand LostFocusCommand { get; }
    }
}
