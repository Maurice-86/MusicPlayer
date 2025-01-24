using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MaterialDesignThemes.Wpf;
using Microsoft.Extensions.DependencyInjection;
using MusicPlayer.Services;
using MusicPlayer.ViewModels;

namespace MusicPlayer.Dialogs
{
    public partial class SettingsDialogViewModel : ViewModelBase
    {
        private readonly SettingsService _settingsService;
        public SettingsDialogViewModel()
        {
            _settingsService = App.Current.Services.GetRequiredService<SettingsService>();
            ThemeNumber = _settingsService.Model.ThemeNumber;
            LanguageNumber = _settingsService.Model.LanguageNumber;
        }

        [ObservableProperty]
        private int themeNumber = -1;

        [ObservableProperty]
        private string? themeContent;

        partial void OnThemeNumberChanged(int value)
        {
            ThemeContent = value == 0 ? "深色" : "浅色";
        }

        [ObservableProperty]
        private int languageNumber = -1;

        [ObservableProperty]
        private string? languageContent;

        partial void OnLanguageNumberChanged(int value)
        {
            LanguageContent = value == 0 ? "中文" : "英文";
        }

        [RelayCommand]
        private void Save()
        {
            if (_settingsService.Model.ThemeNumber != ThemeNumber)
            {
                _settingsService.Model.ThemeNumber = ThemeNumber;
                App.Current.SwitchTheme(ThemeNumber == 0);
            }
            
            _settingsService.Model.LanguageNumber = LanguageNumber;
        }
    }
}
