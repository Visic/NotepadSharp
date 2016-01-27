using System.Collections.Generic;
using System.Collections.ObjectModel;
using WPFUtility;

namespace NotepadSharp {
    public class MainMenuViewModel : ViewModelBase {
        Dictionary<string, ButtonViewModel> _lookup = new Dictionary<string, ButtonViewModel>();

        public ObservableCollection<ButtonViewModel> MenuButtons { get; } = new ObservableCollection<ButtonViewModel>();

        public void SetMenuButton(string key, ButtonViewModel button) {
            ButtonViewModel current;
            if (_lookup.TryGetValue(key, out current)) MenuButtons.Remove(current);

            _lookup[key] = button;
            MenuButtons.Add(button);
        }

        public void ClearMenuButton(string key) {
            ButtonViewModel current;
            if(_lookup.TryGetValue(key, out current)) {
                _lookup.Remove(key);
                MenuButtons.Remove(current);
            }
        }
    }
}
