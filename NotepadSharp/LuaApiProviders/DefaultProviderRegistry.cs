using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotepadSharp {
    public class DefaultProviderRegistry : Dictionary<string, object> {
        public DefaultProviderRegistry() {
            Add("app", new ApplicationApiProvider());
            Add("thread", new ThreadingApiProvider());
        }
    }
}
