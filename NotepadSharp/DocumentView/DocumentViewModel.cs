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
            string uid, cachedFilePath;
            if (!ArgsAndSettings.CachedFiles.TryGetT1(filePath, out uid)) {
                uid = Guid.NewGuid().ToString();
                cachedFilePath = Path.Combine(Constants.FileCachePath, uid);
                File.Copy(filePath, cachedFilePath);
                ArgsAndSettings.CachedFiles.Add(uid, filePath);
            } else {
                cachedFilePath = Path.Combine(Constants.FileCachePath, uid);
            }

            Title = Path.GetFileName(filePath);
            DocumentContent = new RichTextViewModel(File.ReadAllText(cachedFilePath));
        }

        public string Title { get; private set; }
        public ViewModelBase DocumentContent { get; private set; }
    }
}
