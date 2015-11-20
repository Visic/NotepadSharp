using System;

namespace NotepadSharp {
    //Note:: The private method/property pattern allows us to call methods like "SetMessageAreaText" through 
    //lua with the typical "Class.Method" notation, rather than "Class:Method"
    public class ApplicationApiProvider {
        public ApplicationApiProvider() {
            SetMessageAreaText = SetMessageAreaText_Impl;
        }

        public Action<string, string> SetMessageAreaText { get; }

        private void SetMessageAreaText_Impl(string text, string color = "Black") {
            ApplicationState.SetMessageAreaTextColor(color);
            ApplicationState.SetMessageAreaText(text);
        }
    }
}