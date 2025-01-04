using System.Globalization;
using System.Windows;

namespace MusicPlayer.Converters
{
    public class BoolToVisbleConverter : BaseConverter
    {
        public bool Invert { get; set; } = false;

        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool boolValue = (bool)value;
            return boolValue ^ Invert ? Visibility.Visible : Visibility.Collapsed;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Visibility visibility = (Visibility)value;
            return visibility == Visibility.Visible ^ Invert;
        }
    }
}
