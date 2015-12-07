using System.IO;
using System.Windows.Input;
using WPFUtility;

namespace NotepadSharp {
    public class RichTextViewModel : ViewModelBase {
        public RichTextViewModel(string filepath) {
            ApiProvider = new NotifyingProperty<RichTextBoxApiProvider>(new RichTextBoxApiProvider());
            ApiProvider.Value.MarkDirty = () => IsDirty.Value = true;
            ApiProvider.Value.MarkClean = () => IsDirty.Value = false;

            Content = new NotifyingProperty<string>(File.ReadAllText(filepath));

            KeyBindingHandler = new KeyBindingExecution(ex => {
                ApplicationState.SetMessageAreaText(ex.Message);
                ApplicationState.SetMessageAreaTextColor("DarkRed");
            });
            
            LostFocusCommand = new RelayCommand(x => KeyBindingHandler.ClearPressedKeys());
            KeyBindingHandler.SetScriptArg("textbox", ApiProvider.Value);
            KeyBindingHandler.SetScriptArgs(new DefaultProviderRegistry());
        }

        public NotifyingProperty<RichTextBoxApiProvider> ApiProvider { get; }
        public NotifyingProperty<string> Content { get; }
        public NotifyingProperty<bool> IsDirty { get; } = new NotifyingProperty<bool>();
        public KeyBindingExecution KeyBindingHandler { get; }
        public ICommand LostFocusCommand { get; }
    }
}
