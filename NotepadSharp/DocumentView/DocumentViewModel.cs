using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFUtility;

namespace NotepadSharp {
    public class DocumentViewModel : ViewModelBase {
        public DocumentViewModel(string filename) {
            Title = Path.GetFileName(filename);
            DocumentContent = new RichTextViewModel(Constants.LoremIpsum);
            //DocumentContent = new RichTextViewModel(File.ReadAllText(filename));
        }

        public string Title { get; private set; }
        public ViewModelBase DocumentContent { get; private set; }
    }
}
