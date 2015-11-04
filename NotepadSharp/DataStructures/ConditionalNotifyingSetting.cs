using System;
using WPFUtility;

namespace NotepadSharp
{
    public class ConditionalNotifyingSetting<T> : ConditionalNotifyingProperty<T>
    {
        Setting<T> _setting;

        public ConditionalNotifyingSetting(Func<T,T,bool> condition, Setting<T> setting) : base(condition, setting.Value) {
            _setting = setting;
        }

        protected override void OnPropertyChanged() {
            _setting.Value = Value;
            base.OnPropertyChanged();
        }
    }
}
