using System;

namespace NotepadSharp {
    //Note:: The private method/property pattern allows us to call methods like "SetMessageAreaText" through 
    //lua with the typical "Class.Method" notation, rather than "Class:Method"
    public class ApplicationApiProvider {
        public ApplicationApiProvider() {
            SetMessageAreaText = SetMessageAreaText_Impl;
            OpenDocument = ApplicationState.OpenDocument;
            NewDocument = ApplicationState.NewDocument;
        }

        public Action<string, string> SetMessageAreaText { get; }
        public Action<string> OpenDocument { get; }
        public Action NewDocument { get; }

        private void SetMessageAreaText_Impl(object message, string color = "Black") {
            ApplicationState.SetMessageAreaTextColor(color);
            ApplicationState.SetMessageAreaText(message.ToString());
        }
    }
}