using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFUtility;

namespace NotepadSharp {
    public class DocumentViewModel : ViewModelBase {
        public DocumentViewModel(string filePath) {
            Title = Path.GetFileName(filePath);
            DocumentContent = new RichTextViewModel(File.ReadAllText(filePath));
        }

        public string Title { get; private set; }
        public ViewModelBase DocumentContent { get; private set; }
    }
}
