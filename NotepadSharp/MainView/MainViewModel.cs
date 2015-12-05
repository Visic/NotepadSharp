using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using WPFUtility;

namespace NotepadSharp {
    public class MainViewModel : ViewModelBase {
        Dictionary<string, ISelectableButtonViewModel> _buttonLookup = new Dictionary<string, ISelectableButtonViewModel>();

        public MainViewModel() {
            ApplicationState.SetMessageAreaText = msg => MessageAreaText.Value = msg;
            ApplicationState.SetMessageAreaTextColor = color => MessageAreaTextColor.Value = color;
            ApplicationState.OpenDocument = AddOrSelectDocumentButton;

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

            //load previously open files
            foreach(var path in ArgsAndSettings.CachedFiles.Select(x => x.OriginalFilePath).ToArray()) {
                AddOrSelectDocumentButton(path);
            }
            
            if(TopTabs.Count() > 0) TopTabs.First().IsSelected.Value = true;
        }
        
        public SingleSelectionCollection<ISelectableButtonViewModel> TopTabs { get; } = new SingleSelectionCollection<ISelectableButtonViewModel>();
        public SingleSelectionCollection<ISelectableButtonViewModel> LeftTabs { get; } = new SingleSelectionCollection<ISelectableButtonViewModel>();
        public NotifyingProperty<ViewModelBase> TopPanelContent { get; } = new NotifyingProperty<ViewModelBase>();
        public NotifyingProperty<ViewModelBase> LeftPanelContent { get; } = new NotifyingProperty<ViewModelBase>();
        public NotifyingProperty<string> MessageAreaText { get; } = new NotifyingProperty<string>();
        public NotifyingProperty<string> MessageAreaTextColor { get; } = new NotifyingProperty<string>("Black");

        private void AddLeftPanelToggleButton(string text, ViewModelBase vm, Action<bool> selectionStateChanged = null) {
            if (selectionStateChanged == null) selectionStateChanged = x => { };

            ISelectableButtonViewModel toggleVm = null;
            var cmd = new RelayCommand(x => {
                LeftPanelContent.Value = toggleVm.IsSelected.Value ? vm : null;
                selectionStateChanged(toggleVm.IsSelected.Value);
            });

            toggleVm = new ToggleButtonViewModel(text, cmd);
            LeftTabs.Add(toggleVm);
        }

        private void AddOrSelectTopPanelButton(string text, Func<ViewModelBase> vmGenerator) {
            ISelectableButtonViewModel button;
            if(!_buttonLookup.TryGetValue(text, out button)) {
                var vm = new Lazy<ViewModelBase>(vmGenerator);
                var cmd = new RelayCommand(x => TopPanelContent.Value = vm.Value);
                var closeCmd = new RelayCommand(x => CloseTopTab(text, vm.Value));
                _buttonLookup[text] = button = new SelectableCloseableButtonViewModel(text, cmd, closeCmd);
                TopTabs.Add(button);
            }
            button.IsSelected.Value = true;
        }

        private void AddOrSelectDocumentButton(string filePath) {
            ISelectableButtonViewModel button;
            if(!_buttonLookup.TryGetValue(filePath, out button)) {
                var vm = new Lazy<DocumentViewModel>(() => new DocumentViewModel(filePath));

                var cmd = new RelayCommand(x => {
                    ((MaybeDirtySelectableClosableButtonViewModel)button).IsDirty = vm.Value.IsDirty;
                    TopPanelContent.Value = vm.Value;
                });

                var closeCmd = new RelayCommand(x => CloseTopTab(filePath, vm.Value, vm.Value.Close));

                var fileName = Path.GetFileName(filePath);
                _buttonLookup[filePath] = button = new MaybeDirtySelectableClosableButtonViewModel(fileName, cmd, closeCmd);
                TopTabs.Add(button);
            }
            button.IsSelected.Value = true;
        }

        private void CloseTopTab(string tabId, ViewModelBase vm, Action closeCallback = null) {
            var button = _buttonLookup[tabId];
            _buttonLookup.Remove(tabId);

            var tabIndex = TopTabs.IndexOf(button);
            TopTabs.Remove(button);
            if(TopPanelContent.Value == vm) {
                //select the next tab and update the top panel content
                var nextTab = TopTabs.ElementAtOrDefault(tabIndex) ?? TopTabs.ElementAtOrDefault(tabIndex - 1);
                if (nextTab == null) TopPanelContent.Value = null;
                else nextTab.Command.Execute(null);
            }
            closeCallback?.Invoke();
        }
    }
}