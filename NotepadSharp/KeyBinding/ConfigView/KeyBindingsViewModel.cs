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

        public ObservableCollection<KeyBindingViewModel> KeyBindings { get; }
        public NotifyingProperty<KeyBindingViewModel> EmptyBinding { get; }

        private void EditBinding(KeyBinding oldBinding, KeyBinding newBinding) {
            ArgsAndSettings.KeyBindings.ClearBinding(oldBinding);
            ArgsAndSettings.KeyBindings.SetBinding(newBinding);
        }

        private void DeleteBinding(KeyBindingViewModel obj) {
            KeyBindings.Remove(obj);
            ArgsAndSettings.KeyBindings.ClearBinding(obj.GetBinding());
        }

        private void NewBinding(KeyBinding oldBinding, KeyBinding newBinding) {
            KeyBindings.FirstOrDefault(x => x.Keys.Value.SetEquals(newBinding.Keys))?.ClearBinding(); //clear conflicted bindings
            ArgsAndSettings.KeyBindings.SetBinding(newBinding);

            var newBindingVm = new KeyBindingViewModel(newBinding, EditBinding, DeleteBinding);
            KeyBindings.Add(newBindingVm);

            var oldEmptyBinding = EmptyBinding.Value;
            EmptyBinding.Value = null; //have to null this first or the content template bindings don't update
            EmptyBinding.Value = MakeEmptyBinding();

            if (oldEmptyBinding.ScriptFilePathIsFocused.Value) newBindingVm.ScriptFilePathIsFocused.Value = true;
        }

        private KeyBindingViewModel MakeEmptyBinding() {
            return new KeyBindingViewModel(new LuaKeyBinding(""), NewBinding, null);
        }
    }
}
