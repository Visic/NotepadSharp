using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NotepadSharp {
    public class ThreadingApiProvider {
        public ThreadingApiProvider() {

        }

        public Action<int> Sleep { get; } = zzz => Thread.Sleep(zzz);
    }
}
