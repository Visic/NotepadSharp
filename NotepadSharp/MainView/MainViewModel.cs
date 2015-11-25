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
            AddLeftPanelToggleButton("Files", new FileExplorerViewModel());
            AddLeftPanelToggleButton("Bindings", new KeyBindingsViewModel());
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

        private void AddDocumentTab(string fileName) {
            var docVm = new DocumentViewModel(fileName);
            var cmd = new RelayCommand(x => TopPanelContent.Value = docVm);
            TopTabs.Add(new SelectableButtonViewModel(Path.GetFileName(fileName), cmd));
        }
    }
}
