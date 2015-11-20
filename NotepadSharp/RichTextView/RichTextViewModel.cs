﻿using System;
using System.Windows.Input;
using WPFUtility;

namespace NotepadSharp {
    public class RichTextViewModel : ViewModelBase {
        KeyBindingExecution _keyBindingExecutor;

        public RichTextViewModel(string content) {
            ApiProvider = new NotifyingProperty<RichTextBoxApiProvider>(new RichTextBoxApiProvider());
            Content = new NotifyingProperty<string>(content);
            KeyDownCommand = new RelayCommand(x => _keyBindingExecutor.KeyDown((KeyEventArgs)x));
            KeyUpCommand = new RelayCommand(x => _keyBindingExecutor.KeyUp((KeyEventArgs)x));
            LostFocusCommand = new RelayCommand(x => _keyBindingExecutor.ClearPressedKeys());
            _keyBindingExecutor = new KeyBindingExecution(ex => {
                ApplicationState.SetMessageAreaTextColor("DarkRed");
                ApplicationState.SetMessageAreaText(ex.Message);
            });
            
            _keyBindingExecutor.SetScriptArg("textbox", ApiProvider.Value);
            _keyBindingExecutor.SetScriptArg("app", new ApplicationApiProvider());
        }
        
        public NotifyingProperty<RichTextBoxApiProvider> ApiProvider { get; }
        public NotifyingProperty<string> Content { get; }
        public ICommand KeyDownCommand { get; }
        public ICommand KeyUpCommand { get; }
        public ICommand LostFocusCommand { get; }
    }
}
