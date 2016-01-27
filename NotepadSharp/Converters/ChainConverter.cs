using System;
using System.Windows.Data;

namespace NotepadSharp {
    public class ChainConverter : IValueConverter {
        public IValueConverter In { get; set; }
        public IValueConverter Out { get; set; }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            return Out.Convert(In.Convert(value, targetType, parameter, culture), targetType, parameter, culture);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            return In.Convert(Out.Convert(value, targetType, parameter, culture), targetType, parameter, culture);
        }
    }
}
