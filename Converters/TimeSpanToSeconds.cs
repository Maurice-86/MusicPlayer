﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MusicPlayer.Converters
{
    public class TimeSpanToSeconds : BaseValueConverter<TimeSpanToSeconds>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is TimeSpan timeSpan)
            {
                return timeSpan.TotalSeconds;
            }
            return Binding.DoNothing;
        }
    }
}
