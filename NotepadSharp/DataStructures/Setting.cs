using System;
using WPFUtility;

namespace NotepadSharp
{
    public class Setting<T> : Property<T>
    {
        public Setting(T initialValue, Action<T> setter) 
            : base(x => { setter(x); ArgsAndSettings.SaveSettings(); }, initialValue)
        {
        }
    }
}
