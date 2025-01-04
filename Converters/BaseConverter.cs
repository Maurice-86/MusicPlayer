using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace MusicPlayer.Converters
{
    public class BaseConverter : MarkupExtension, IValueConverter
    {
        public virtual object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

        public virtual object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
