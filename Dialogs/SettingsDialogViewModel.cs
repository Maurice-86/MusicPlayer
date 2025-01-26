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
                ThemeContent = value ? "深色" : "浅色";
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
                LanguageContent = value == (int)Enum.LanguageMode.Chinese ? "中文" : "英文";
                OnPropertyChanged(nameof(LanguageModeIndex));
            }
        }

        [ObservableProperty]
        private string? languageContent;

        [RelayCommand]
        private void Save()
        {
            if (IsDarkTheme != 
                (SettingsService.Instance.Model.ThemeMode == Enum.ThemeMode.Dark))
            {
                SettingsService.Instance.Model.ThemeMode = IsDarkTheme ? Enum.ThemeMode.Dark : Enum.ThemeMode.Light;
                ThemeHelper.SwitchTheme(IsDarkTheme);
            }
            if (LanguageModeIndex != (int)SettingsService.Instance.Model.LanguageMode)
            {
                SettingsService.Instance.Model.LanguageMode = (Enum.LanguageMode)LanguageModeIndex;
                // TODO
            }
        }
    }
}
