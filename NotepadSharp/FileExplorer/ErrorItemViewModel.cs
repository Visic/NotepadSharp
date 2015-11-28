namespace NotepadSharp {
    public class ErrorItemViewModel : FileSystemEntityViewModel {
        public ErrorItemViewModel(string path, string message) : base(path) {
            ErrorMessage.Value = message;
        }
    }
}
