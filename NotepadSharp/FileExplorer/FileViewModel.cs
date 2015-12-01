using NotepadSharp.Properties;
using System.Drawing;
using System.Threading.Tasks;
using WPFUtility;

namespace NotepadSharp {
    public class FileViewModel : FileSystemEntityViewModel {
        public FileViewModel(string path) {
            InteractCommand = new RelayCommand(x => ApplicationState.OpenDocument(path));
            SetPath(path);
            IconImage.Value = Resources.Document_Generic.ToBitmapImage();
        }

        public async Task UpdateIcon_Async() {
            using(var icon = await Task.Run(() => Icon.ExtractAssociatedIcon(EntityPath.Value))) {
                IconImage.Value = icon.ToBitmapImage();
            }
        }
    }
}
