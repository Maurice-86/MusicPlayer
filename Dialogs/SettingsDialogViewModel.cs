using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MaterialDesignThemes.Wpf;
using Microsoft.Extensions.DependencyInjection;
using MusicPlayer.Helpers;
using MusicPlayer.Resources;
using MusicPlayer.Services;
using MusicPlayer.ViewModels;

namespace MusicPlayer.Dialogs
{
    public partial class SettingsDialogViewModel : ViewModelBase
    {
        public SettingsDialogViewModel()
        {
            IsDarkTheme = SettingsService.Instance.Model.ThemeMode == Enum.ThemeMode.Dark;
            LanguageModeIndex = (int)SettingsService.Instance.Model.LanguageMode;
        }

        private bool isDarkTheme;
        public bool IsDarkTheme
        {
            get => isDarkTheme;
            set
            {
                isDarkTheme = value;
                ThemeContent = value ? Lang.SettingsDialog_DarkLabel_Content : Lang.SettingsDialog_LightLabel_Content;
                OnPropertyChanged(nameof(IsDarkTheme));
            }
        }

        [ObservableProperty]
        private string? themeContent;

        private int languageModeIndex;
        public int LanguageModeIndex
        {
            get => languageModeIndex;
            set
            {
                languageModeIndex = value;
                LanguageContent = value == (int)Enum.LanguageMode.Chinese ? Lang.SettingsDialog_ChineseLanguageLabel_Content : Lang.SettingsDialog_EnglishLanguageLabel_Content;
                OnPropertyChanged(nameof(LanguageModeIndex));
            }
        }

        [ObservableProperty]
        private string? languageContent;

        public void Save()
        {
            if (IsDarkTheme != 
                (SettingsService.Instance.Model.ThemeMode == Enum.ThemeMode.Dark))
            {
                SettingsService.Instance.Model.ThemeMode = IsDarkTheme ? Enum.ThemeMode.Dark : Enum.ThemeMode.Light;
                // 切换主题
                ThemeHelper.SwitchTheme(IsDarkTheme);
            }
            if (LanguageModeIndex != (int)SettingsService.Instance.Model.LanguageMode)
            {
                SettingsService.Instance.Model.LanguageMode = (Enum.LanguageMode)LanguageModeIndex;
                // 切换语言
                LanguageHelper.SwitchLanguage(SettingsService.Instance.Model.LanguageMode);

                // 更新当前页面 ThemeContent 和 LanguageContent
                ThemeContent = IsDarkTheme ? Lang.SettingsDialog_DarkLabel_Content : Lang.SettingsDialog_LightLabel_Content;
                LanguageContent = LanguageModeIndex == (int)Enum.LanguageMode.Chinese ? Lang.SettingsDialog_ChineseLanguageLabel_Content : Lang.SettingsDialog_EnglishLanguageLabel_Content;
            }
        }
    }
}
