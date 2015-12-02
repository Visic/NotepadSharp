using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using WPFUtility;

namespace NotepadSharp {
    public class MainViewModel : ViewModelBase {
        Dictionary<string, SelectableButtonViewModel> _buttonLookup = new Dictionary<string, SelectableButtonViewModel>();

        public MainViewModel() {
            ApplicationState.SetMessageAreaText = msg => MessageAreaText.Value = msg;
            ApplicationState.SetMessageAreaTextColor = color => MessageAreaTextColor.Value = color;
            ApplicationState.OpenDocument = filePath =>  AddOrSelectTopPanelButton(Path.GetFileName(filePath), () => new DocumentViewModel(filePath), filePath);

            AddLeftPanelToggleButton("Menu", ApplicationState.MainMenu);

            var fileExplorer = new FileExplorerViewModel(Environment.GetFolderPath(Environment.SpecialFolder.Desktop));
            AddLeftPanelToggleButton(
                "Files", 
                fileExplorer, 
                x => fileExplorer.IsExpanded.Value = x
            );

            ApplicationState.MainMenu.SetMenuButton(
                "Bindings", 
                new ButtonViewModel(
                    "Key Bindings", 
                    new RelayCommand(x => AddOrSelectTopPanelButton("Key Bindings", () => new KeyBindingsViewModel()))
                )
            );
            
            if(TopTabs.Count() > 0) TopTabs.First().IsSelected.Value = true;
        }
        
        public SingleSelectionCollection<SelectableButtonViewModel> TopTabs { get; } = new SingleSelectionCollection<SelectableButtonViewModel>();
        public SingleSelectionCollection<ToggleButtonViewModel> LeftTabs { get; } = new SingleSelectionCollection<ToggleButtonViewModel>();
        public NotifyingProperty<ViewModelBase> TopPanelContent { get; } = new NotifyingProperty<ViewModelBase>();
        public NotifyingProperty<ViewModelBase> LeftPanelContent { get; } = new NotifyingProperty<ViewModelBase>();
        public NotifyingProperty<string> MessageAreaText { get; } = new NotifyingProperty<string>();
        public NotifyingProperty<string> MessageAreaTextColor { get; } = new NotifyingProperty<string>("Black");

        private ToggleButtonViewModel AddLeftPanelToggleButton(string text, ViewModelBase vm, Action<bool> selectionStateChanged = null) {
            if (selectionStateChanged == null) selectionStateChanged = x => { };

            ToggleButtonViewModel toggleVm = null;
            var cmd = new RelayCommand(x => {
                LeftPanelContent.Value = toggleVm.IsSelected.Value ? vm : null;
                selectionStateChanged(toggleVm.IsSelected.Value);
            });

            toggleVm = new ToggleButtonViewModel(text, cmd);
            LeftTabs.Add(toggleVm);
            return toggleVm;
        }

        private SelectableButtonViewModel AddOrSelectTopPanelButton(string text, Func<ViewModelBase> vmGenerator, string uniqueName = null) {
            SelectableButtonViewModel button;
            if(!_buttonLookup.TryGetValue(uniqueName ?? text, out button)) {
                var cmd = new RelayCommand(x => TopPanelContent.Value = vmGenerator());
                _buttonLookup[uniqueName ?? text] = button = new SelectableButtonViewModel(text, cmd);
                TopTabs.Add(button);
            }
            button.IsSelected.Value = true;
            return button;
        }
    }
}
