using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace MusicPlayer.Helpers
{
    public class ThemeHelper
    {
        public static void SwitchTheme(bool isDarkTheme)
        {
            ModifyTheme(theme =>
            {
                if (isDarkTheme)
                {
                    theme.SetBaseTheme(BaseTheme.Dark);
                    // 更新按钮前景色资源
                    App.Current.Resources["WhiteForeground"] = new SolidColorBrush(Colors.White);
                }
                else
                {
                    theme.SetBaseTheme(BaseTheme.Light);
                    // 更新按钮前景色资源
                    App.Current.Resources["WhiteForeground"] = new SolidColorBrush(Colors.Black);
                }
            });
        }

        private static void ModifyTheme(Action<Theme> modificationAction)
        {
            var paletteHelper = new PaletteHelper();
            Theme theme = paletteHelper.GetTheme();
            modificationAction?.Invoke(theme);
            paletteHelper.SetTheme(theme);
        }
    }
}
