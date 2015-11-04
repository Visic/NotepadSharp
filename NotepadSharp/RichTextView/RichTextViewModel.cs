using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WPFUtility;

namespace NotepadSharp {
    public class RichTextViewModel : ViewModelBase {
        public RichTextViewModel(string content) {
            Content = new NotifyingProperty<string>(content);
        }

        public KeyBindingCollection KeyBindings { get { return ArgsAndSettings.KeyBindings; } }
        public NotifyingProperty<string> Content { get; set; }
    }
}
