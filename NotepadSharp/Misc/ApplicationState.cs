using System;

namespace NotepadSharp {
    public static class ApplicationState
    {
        public static MainMenuViewModel MainMenu { get; } = new MainMenuViewModel();
        public static Action<string> SetMessageAreaText { get; set; }
        public static Action<string> SetMessageAreaTextColor { get; set; }
        public static Action<string> OpenDocument { get; set; }
        public static Action NewDocument { get; set; }
    }
}
