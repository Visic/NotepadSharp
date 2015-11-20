using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotepadSharp
{
    public static class ApplicationState
    {
        public static Action<string> SetMessageAreaText { get; set; }
        public static Action<string> SetMessageAreaTextColor { get; set; }
    }
}
