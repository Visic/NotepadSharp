namespace NotepadSharp {
    public class ErrorItemViewModel : FileSystemEntityViewModel {
        public ErrorItemViewModel(string path, string message) {
            ErrorMessage.Value = message;
            SetPath(path);
        }
    }
}
