using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WPFUtility;

namespace NotepadSharp {
    public class MainViewModel : ViewModelBase {
        public MainViewModel() {
            SelectedTabIndex = new NotifyingProperty<int>();
            Tabs = new ObservableCollection<ViewModelBase>();
            Tabs.Add(new DocumentViewModel("Placeholder"));
            Tabs.Add(new KeyBindingsViewModel());

            ArgsAndSettings.KeyBindings.SetBinding(new KeyBinding(() => { }, "Testing 1", Key.LeftCtrl, Key.A));
            ArgsAndSettings.KeyBindings.SetBinding(new KeyBinding(() => { }, "Testing 2", Key.LeftCtrl, Key.B));
        }

        public ObservableCollection<ViewModelBase> Tabs { get; private set; }
        public NotifyingProperty<int> SelectedTabIndex { get; private set; }
    }
}
