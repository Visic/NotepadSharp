﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Utility;
using WPFUtility;

namespace NotepadSharp {
    public class MainViewModel : ViewModelBase {
        Dictionary<string, ISelectableButtonViewModel> _buttonLookup = new Dictionary<string, ISelectableButtonViewModel>();
        List<IViewModelBase> _currentViewModels = new List<IViewModelBase>();

        public MainViewModel() {
            ApplicationState.SetMessageAreaText = msg => MessageAreaText.Value = msg;
            ApplicationState.SetMessageAreaTextColor = color => MessageAreaTextColor.Value = color;
            ApplicationState.OpenDocument = x => {
                if(!File.Exists(x)) throw new FileNotFoundException($"File [{x}] not found.");
                AddOrSelectDocumentButton(x);
            };

            AddLeftPanelToggleButton("Menu", ApplicationState.MainMenu);

            var fileExplorer = NewVm(new FileExplorerViewModel(Environment.GetFolderPath(Environment.SpecialFolder.Desktop)));
            AddLeftPanelToggleButton(
                "Files",
                fileExplorer,
                x => fileExplorer.IsExpanded.Value = x
            );

            var newDocumentCommand = new RelayCommand(y => NewDocument());
            ApplicationState.MainMenu.SetMenuButton(
                "New File",
                new ButtonViewModel(
                    "New File",
                    newDocumentCommand
                )
            );
            ApplicationState.NewDocument = () => newDocumentCommand.Execute(null);

            ApplicationState.MainMenu.SetMenuButton(
                "Bindings",
                new ButtonViewModel(
                    "Key Bindings",
                    new RelayCommand(x => AddOrSelectTopPanelButton("Key Bindings", () => new KeyBindingsViewModel()))
                )
            );

            if(ArgsAndSettings.CachedFiles.Count() == 0 && string.IsNullOrEmpty(ArgsAndSettings.OpenOnStartup)) { //nothing to load
                newDocumentCommand.Execute(null);
            } else {
                if (!string.IsNullOrEmpty(ArgsAndSettings.OpenOnStartup)) AddOrSelectDocumentButton(ArgsAndSettings.OpenOnStartup);

                //load previously open files
                foreach(var fileInfo in ArgsAndSettings.CachedFiles.ToArray()) {
                    AddOrSelectDocumentButton(fileInfo.OriginalFilePath ?? fileInfo.CachedFilePath);
                }
            }

            TopTabs.First().IsSelected.Value = true;
        }

        public SingleSelectionCollection<ISelectableButtonViewModel> TopTabs { get; } = new SingleSelectionCollection<ISelectableButtonViewModel>();
        public SingleSelectionCollection<ISelectableButtonViewModel> LeftTabs { get; } = new SingleSelectionCollection<ISelectableButtonViewModel>();
        public NotifyingProperty<ViewModelBase> TopPanelContent { get; } = new NotifyingProperty<ViewModelBase>();
        public NotifyingProperty<ViewModelBase> LeftPanelContent { get; } = new NotifyingProperty<ViewModelBase>();
        public NotifyingProperty<string> MessageAreaText { get; } = new NotifyingProperty<string>();
        public NotifyingProperty<string> MessageAreaTextColor { get; } = new NotifyingProperty<string>("Black");

        private void AddLeftPanelToggleButton(string text, ViewModelBase vm, Action<bool> selectionStateChanged = null) {
            if(selectionStateChanged == null) selectionStateChanged = x => { };

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
                var vm = new Lazy<ViewModelBase>(() => NewVm(vmGenerator()));
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
                var vm = new Lazy<DocumentViewModel>(() => NewVm(
                    new DocumentViewModel(
                        filePath,
                        (oldPath, newPath) => {
                            _buttonLookup.Remove(oldPath);
                            _buttonLookup[newPath] = button;
                            filePath = newPath;
                        }
                    )
                ));

                var cmd = new RelayCommand(x => TopPanelContent.Value = vm.Value);

                var closeCmd = new RelayCommand(x => {
                    if (_currentViewModels.Count(y => y is DocumentViewModel) == 1) NewDocument();
                    CloseTopTab(filePath, vm.Value, vm.Value.Close);
                });

                _buttonLookup[filePath] = button = new FileTabButtonViewModel(cmd, closeCmd, vm.Value);
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
                if(nextTab == null) TopPanelContent.Value = null;
                else nextTab.Command.Execute(null);
            }
            closeCallback?.Invoke();

            _currentViewModels.Remove(vm);
            vm.Dispose();
        }

        private T NewVm<T>(T vm) where T : IViewModelBase {
            _currentViewModels.Add(vm);
            return vm;
        }

        private void NewDocument() {
            AddOrSelectDocumentButton(UniqueNameGenerator.FirstAvailableNumbered("Untitled ", TopTabs.Select(x => x.Text.Value)));
        }

        public override void Dispose() {
            foreach(var ele in _buttonLookup.Values) {
                ele.Dispose();
            }

            foreach(var ele in _currentViewModels) {
                ele.Dispose();
            }

            base.Dispose();
        }
    }
}