using System;
using System.Windows.Input;
using WPFUtility;

namespace NotepadSharp {
    public class RichTextViewModel : ViewModelBase {
        KeyBindingExecution _keyBindingExecutor;

        public RichTextViewModel(string content) {
            ApiProvider = new NotifyingProperty<RichTextBoxApiProvider>(new RichTextBoxApiProvider());
            ApiProvider.Value.MarkDirty = () => IsDirty.Value = true;
            ApiProvider.Value.MarkClean = () => IsDirty.Value = false;

            Content = new NotifyingProperty<string>(content);
            KeyDownCommand = new RelayCommand(x => _keyBindingExecutor.KeyDown((KeyEventArgs)x));
            KeyUpCommand = new RelayCommand(x => _keyBindingExecutor.KeyUp((KeyEventArgs)x));
            LostFocusCommand = new RelayCommand(x => _keyBindingExecutor.ClearPressedKeys());

            _keyBindingExecutor = new KeyBindingExecution(ex => {
                ApplicationState.SetMessageAreaText(ex.Message);
                ApplicationState.SetMessageAreaTextColor("DarkRed");
            });

            _keyBindingExecutor.SetScriptArg("textbox", ApiProvider.Value);
            _keyBindingExecutor.SetScriptArgs(new DefaultProviderRegistry());
        }
        
        public NotifyingProperty<RichTextBoxApiProvider> ApiProvider { get; }
        public NotifyingProperty<string> Content { get; }
        public NotifyingProperty<bool> IsDirty { get; } = new NotifyingProperty<bool>();
        public ICommand KeyDownCommand { get; }
        public ICommand KeyUpCommand { get; }
        public ICommand LostFocusCommand { get; }
    }
}
