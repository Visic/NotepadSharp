namespace NotepadSharp {
    public class ErrorItemViewModel : FileSystemEntityViewModel {
        public ErrorItemViewModel(string path, string errMsg) : base(path) {
            ErrorMessage = errMsg;
            Focusable = false;
        }

        public string ErrorMessage { get; }
    }
}
