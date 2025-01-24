using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;

namespace MusicPlayer.Converters
{
    public class IndexToColorConverter : MarkupExtension, IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length < 2) return Brushes.White;
            try
            {
                int id = (int)values[0];
                int index = (int)values[1];
                return id == index ? App.Current.Resources["PrimaryColor"] : App.Current.Resources["NeutralForeground"];
            }
            catch (Exception ex)
            {
                Console.WriteLine($"IndexToColorConverter 发生异常，异常原因:{ex.Message}");
                return Binding.DoNothing;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return (object[])Binding.DoNothing;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
