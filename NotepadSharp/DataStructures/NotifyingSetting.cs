﻿using System;
using WPFUtility;

namespace NotepadSharp {
    public class NotifyingSetting<T> : NotifyingProperty<T>
    {
        Setting<T> _setting;

        public NotifyingSetting(Setting<T> setting) : base(setting.Value) {
            _setting = setting;
        }

        public NotifyingSetting(Action<T> changed, Setting<T> setting) : base(changed, setting.Value) {
            _setting = setting;
        }

        protected override void OnPropertyChanged() {
            _setting.Value = Value;
            base.OnPropertyChanged();
        }
    }
}
