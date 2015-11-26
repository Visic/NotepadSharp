using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFUtility;

namespace NotepadSharp {
    public class FileViewModel : ViewModelBase {
        public FileViewModel(string path) {
            Name = Path.GetFileName(path);
        }

        public string Name { get; }
    }
}
