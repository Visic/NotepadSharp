using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFUtility;

namespace NotepadSharp {
    public class DirectoryViewModel : ViewModelBase {
        public DirectoryViewModel(string path) {
            Name = Path.GetFileName(path);
            var directoryVms = Directory.GetDirectories(path).Select(x => new DirectoryViewModel(x));
            var fileVms = Directory.GetFiles(path).Select(x => new FileViewModel(x));
            Items = new ObservableCollection<ViewModelBase>(directoryVms.Cast<ViewModelBase>().Concat(fileVms));
        }

        public string Name { get; }
        public ObservableCollection<ViewModelBase> Items { get; }
    }
}
