using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MusicPlayer.Converters
{
    public class SecondsToTimeSpan : BaseValueConverter<SecondsToTimeSpan>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double dv)
            {
                return string.Format("{0:D2}:{1:D2}", (int)TimeSpan.FromSeconds(dv).TotalMinutes, TimeSpan.FromSeconds(dv).Seconds);
            }
            return Binding.DoNothing;
        }
    }
}
