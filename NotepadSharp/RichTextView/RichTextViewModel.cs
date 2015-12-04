using System;
using System.IO;
using System.Windows.Input;
using Utility;
using WPFUtility;

namespace NotepadSharp {
    public class RichTextViewModel : ViewModelBase {
        SerializableFileInfo _fileInfo;
        
        public RichTextViewModel(SerializableFileInfo fileInfo) {
            _fileInfo = fileInfo;
            IsDirty = new  NotifyingPropertyWithChangedAction<bool>(x => { _fileInfo.IsDirty = x; SaveState(); }, _fileInfo.IsDirty);

            ApiProvider = new NotifyingProperty<RichTextBoxApiProvider>(new RichTextBoxApiProvider());
            ApiProvider.Value.MarkDirty = () => IsDirty.Value = true;
            ApiProvider.Value.MarkClean = () => IsDirty.Value = false;

            Content = new NotifyingPropertyWithChangedAction<string>(x => UpdateFile(), File.ReadAllText(fileInfo.CachedFilePath));
            KeyBindingHandler = new KeyBindingExecution(ex => {
                ApplicationState.SetMessageAreaText(ex.Message);
                ApplicationState.SetMessageAreaTextColor("DarkRed");
            });
            
            LostFocusCommand = new RelayCommand(x => KeyBindingHandler.ClearPressedKeys());
            KeyBindingHandler.SetScriptArg("textbox", ApiProvider.Value);
            KeyBindingHandler.SetScriptArgs(new DefaultProviderRegistry());

            UpdateIsDirty();
        }

        public NotifyingProperty<RichTextBoxApiProvider> ApiProvider { get; }
        public NotifyingProperty<string> Content { get; }
        public NotifyingProperty<bool> IsDirty { get; }
        public KeyBindingExecution KeyBindingHandler { get; }
        public ICommand LostFocusCommand { get; }

        private void UpdateIsDirty() {
            bool state = false;
            if(!File.Exists(_fileInfo.OriginalFilePath)) {
                state = true;
            } else if (Methods.HashFile(_fileInfo.OriginalFilePath) != _fileInfo.Hash) {
                state = true;
            }
            IsDirty.Value = state;
        }

        private void UpdateFile() {
            File.WriteAllText(_fileInfo.CachedFilePath, Content.Value);
            _fileInfo.Hash = Methods.HashFile(_fileInfo.CachedFilePath);
            UpdateIsDirty();
        }

        private void SaveState() {
            ArgsAndSettings.CachedFiles.AddOrReplace(_fileInfo);
        }
    }
}
