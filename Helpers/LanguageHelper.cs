using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFLocalizeExtension.Engine;


namespace MusicPlayer.Helpers
{
    public static class LanguageHelper
    {
        /// <summary>
        /// 切换语言
        /// </summary>
        /// <param name="languageMode"></param>
        public static void SwitchLanguage(Enum.LanguageMode languageMode)
        {
            string culture = "";    // 文化
            switch (languageMode)
            {
                case Enum.LanguageMode.Chinese:
                    culture = "zh-CN";
                    break;
                case Enum.LanguageMode.English:
                    culture = "en";
                    break;
            }
            var cultureInfo = new CultureInfo(culture);
            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;
            LocalizeDictionary.Instance.Culture = cultureInfo;
        }
    }
}
