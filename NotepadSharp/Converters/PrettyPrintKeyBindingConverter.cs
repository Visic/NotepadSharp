using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace NotepadSharp
{
    public class PrettyPrintKeyBindingConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            var keys = (IEnumerable<Key>)value;
            return keys.Count() == 0 ? "{ Unbound }" : string.Join(" + ", keys.ToArray());
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
