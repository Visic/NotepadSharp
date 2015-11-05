using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WPFUtility;

namespace NotepadSharp {
    public class KeyBindingsViewModel : ViewModelBase {
        public KeyBindingsViewModel() {
            KeyBindings = new NotifyingProperty<KeyBindingCollection>(ArgsAndSettings.KeyBindings);
            SelectedKeyBinding = new NotifyingProperty<KeyBinding>();
            EditBindingCommand = new RelayCommand(EditBinding);
        }

        public string Title { get; } = "Key Bindings";
        public NotifyingProperty<KeyBindingCollection> KeyBindings { get; private set; }
        public NotifyingProperty<KeyBinding> SelectedKeyBinding { get; private set; }
        public ICommand EditBindingCommand { get; }

        private void EditBinding(object obj) {
            var currentBinding = SelectedKeyBinding.Value;
            var setVM = new SetKeyBindingViewModel(currentBinding);

            WindowGenerator.CreateAndShow(
                x => new HostWithDialogButtonsViewModel(
                    setVM,
                    new ButtonViewModel(
                        "Ok", 
                        new RelayCommand(
                            y => {
                                UpdateKeyBinding(currentBinding, setVM.GetBinding());
                                x();
                            }
                        ), 
                        isDefault:true
                    ),
                    new ButtonViewModel(
                        "Cancel", 
                        new RelayCommand(
                            y => x()
                        ), 
                        true
                    )
                ),
                false
            );
        }

        private void UpdateKeyBinding(KeyBinding oldBinding, KeyBinding newBinding) {
            KeyBindings.Value.ClearBinding(oldBinding);
            KeyBindings.Value.SetBinding(newBinding);

            KeyBindings.Value = null;
            KeyBindings.Value = ArgsAndSettings.KeyBindings;
            SelectedKeyBinding.Value = newBinding;
        }
    }
}
