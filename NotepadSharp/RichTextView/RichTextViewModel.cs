﻿using System.IO;
using System.Windows.Input;
using WPFUtility;

namespace NotepadSharp {
    public class RichTextViewModel : ViewModelBase {
        public RichTextViewModel(string filepath) {
            Content = new NotifyingProperty<string>(File.ReadAllText(filepath));

            KeyBindingHandler = new KeyBindingExecution(ex => {
                ApplicationState.SetMessageAreaText(ex.Message);
                ApplicationState.SetMessageAreaTextColor("DarkRed");
            });
            
            LostFocusCommand = new RelayCommand(x => KeyBindingHandler.ClearPressedKeys());
            KeyBindingHandler.SetScriptArg("textbox", ApiProvider);
            KeyBindingHandler.SetScriptArgs(new DefaultProviderRegistry());
        }

        public RichTextBoxApiProvider ApiProvider { get; } = new RichTextBoxApiProvider();
        public NotifyingProperty<string> Content { get; }
        public KeyBindingExecution KeyBindingHandler { get; }
        public ICommand LostFocusCommand { get; }
    }
}
