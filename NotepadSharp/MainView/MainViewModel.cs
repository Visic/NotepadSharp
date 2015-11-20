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
            ApplicationState.SetMessageAreaText = msg => MessageAreaText.Value = msg;
            ApplicationState.SetMessageAreaTextColor = color => MessageAreaTextColor.Value = color;
            Tabs.Add(new DocumentViewModel("Placeholder"));
            Tabs.Add(new KeyBindingsViewModel());
        }

        public ObservableCollection<ViewModelBase> Tabs { get; } = new ObservableCollection<ViewModelBase>();
        public NotifyingProperty<int> SelectedTabIndex { get; } = new NotifyingProperty<int>();
        public NotifyingProperty<string> MessageAreaText { get; } = new NotifyingProperty<string>();
        public NotifyingProperty<string> MessageAreaTextColor { get; } = new NotifyingProperty<string>();
    }
}
