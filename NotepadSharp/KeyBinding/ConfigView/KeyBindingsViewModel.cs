using System.Collections.ObjectModel;
using System.Linq;
using WPFUtility;

namespace NotepadSharp {
    public class KeyBindingsViewModel : ViewModelBase {
        public KeyBindingsViewModel() {
            KeyBindings = new ObservableCollection<KeyBindingViewModel>(
                ArgsAndSettings.KeyBindings.Select(x => new KeyBindingViewModel(x, EditBinding, DeleteBinding))
            );
            EmptyBinding = new NotifyingProperty<KeyBindingViewModel>(MakeEmptyBinding());
        }

        public string Title { get; } = "Key Bindings";
        public ObservableCollection<KeyBindingViewModel> KeyBindings { get; }
        public NotifyingProperty<KeyBindingViewModel> EmptyBinding { get; }

        private void EditBinding(KeyBinding oldBinding, KeyBinding newBinding) {
            if (!BindingsAreDifferent(oldBinding, newBinding)) return;
            ArgsAndSettings.KeyBindings.ClearBinding(oldBinding);
            ArgsAndSettings.KeyBindings.SetBinding(newBinding);
        }

        private void DeleteBinding(KeyBindingViewModel obj) {
            KeyBindings.Remove(obj);
            ArgsAndSettings.KeyBindings.ClearBinding(obj.GetBinding());
        }

        private void NewBinding(KeyBinding oldBinding, KeyBinding newBinding) {
            if (!BindingsAreDifferent(oldBinding, newBinding)) return;
            ArgsAndSettings.KeyBindings.SetBinding(newBinding);
            KeyBindings.Add(new KeyBindingViewModel(newBinding, EditBinding, DeleteBinding));

            EmptyBinding.Value = null; //have to null this first or the content template bindings don't update
            EmptyBinding.Value = MakeEmptyBinding();
        }

        private bool BindingsAreDifferent(KeyBinding oldBinding, KeyBinding newBinding) {
            return newBinding != oldBinding || 
                   newBinding.Label != oldBinding.Label || 
                   (oldBinding as LuaKeyBinding)?.ScriptPath != (newBinding as LuaKeyBinding)?.ScriptPath;
        }

        private KeyBindingViewModel MakeEmptyBinding() {
            return new KeyBindingViewModel(new LuaKeyBinding(""), NewBinding, null);
        }
    }
}
