using System;
using System.Linq;
using System.Collections;
using System.Globalization;
using System.Windows.Data;

namespace NotepadSharp {
    public class NullOrEmptyToNullConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            return (((IEnumerable)value)?.Cast<object>()?.Count() ?? 0) == 0 ? null : value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
