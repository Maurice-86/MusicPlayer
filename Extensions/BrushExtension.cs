using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace MusicPlayer.Extensions
{
    public static class BrushExtension
    {
        /// <summary>
        /// SolidColorBrush 画刷颜色取反
        /// </summary>
        /// <param name="originalBrush"></param>
        /// <returns></returns>
        public static SolidColorBrush Invert(this SolidColorBrush originalBrush)
        {
            Color originalColor = originalBrush.Color;
            Color invertedColor = Color.FromArgb(
                originalColor.A,
                (byte)(255 - originalColor.R),
                (byte)(255 - originalColor.G),
                (byte)(255 - originalColor.B)
            );
            return new SolidColorBrush(invertedColor);
        }
    }
}
