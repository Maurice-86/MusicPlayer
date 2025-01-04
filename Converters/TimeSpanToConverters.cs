using System.Globalization;

namespace MusicPlayer.Converters
{
    public class TimeSpanToIntConverter : BaseConverter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is TimeSpan timeSpan)
            {
                return (int)timeSpan.TotalSeconds;
            }
            return 0;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return TimeSpan.FromSeconds((int)value);
        }
    }

    public class TimeSpanToStringConverter : BaseConverter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is TimeSpan timeSpan)
            {
                if (timeSpan.TotalMinutes >= 60)
                {
                    return timeSpan.ToString(@"hh\:mm\:ss");
                }
                else
                {
                    return timeSpan.ToString(@"mm\:ss");
                }
            }
            return "";
        }
    }
}

