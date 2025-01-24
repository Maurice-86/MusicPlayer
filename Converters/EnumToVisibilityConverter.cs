using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace MusicPlayer.Converters
{
    public class EnumToVisibilityConverter : BooleanToVisibilityConverter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var boolValue = value.ToString() == parameter.ToString();
            return boolValue ^ IsReversed ? Visibility.Visible : (UseHidden ? Visibility.Hidden : Visibility.Collapsed);
        }
    }
}
