using System;
using System.Globalization;
using System.Windows.Data;

namespace NotepadSharp {
    public class BoolToPropValueConverter : IValueConverter {
        public object ValueForTrue { get; set; }
        public object ValueForFalse { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            return (bool)value ? ValueForTrue : ValueForFalse;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
