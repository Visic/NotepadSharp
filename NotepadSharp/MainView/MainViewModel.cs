using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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

            AddDocumentTab("First.txt");
            AddDocumentTab("Second.txt");
            AddTopPanelButton("Bindings", new KeyBindingsViewModel());
            AddLeftPanelToggleButton("Files", new FileExplorerViewModel(Environment.GetFolderPath(Environment.SpecialFolder.Desktop)));
            TopTabs.First().Command.Execute(null);
        }

        public SingleSelectionCollection<SelectableButtonViewModel> TopTabs { get; } = new SingleSelectionCollection<SelectableButtonViewModel>();
        public SingleSelectionCollection<ToggleButtonViewModel> LeftTabs { get; } = new SingleSelectionCollection<ToggleButtonViewModel>();
        public NotifyingProperty<ViewModelBase> TopPanelContent { get; } = new NotifyingProperty<ViewModelBase>();
        public NotifyingProperty<ViewModelBase> LeftPanelContent { get; } = new NotifyingProperty<ViewModelBase>();
        public NotifyingProperty<string> MessageAreaText { get; } = new NotifyingProperty<string>();
        public NotifyingProperty<string> MessageAreaTextColor { get; } = new NotifyingProperty<string>();

        private void AddLeftPanelToggleButton(string text, ViewModelBase vm) {
            ToggleButtonViewModel toggleVm = null;
            var cmd = new RelayCommand(x => LeftPanelContent.Value = toggleVm.IsSelected.Value ? vm : null);
            toggleVm = new ToggleButtonViewModel(text, cmd);
            LeftTabs.Add(toggleVm);
        }

        private void AddTopPanelButton(string text, ViewModelBase vm) {
            var cmd = new RelayCommand(x => TopPanelContent.Value = vm);
            TopTabs.Add(new SelectableButtonViewModel(text, cmd));
        }

        private void AddDocumentTab(string fileName) {
            AddTopPanelButton(Path.GetFileName(fileName), new DocumentViewModel(fileName));
        }
    }
}
