using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MusicPlayer.Models;
using MusicPlayer.Services;
using System.Collections.ObjectModel;

namespace MusicPlayer.ViewModels
{
    public partial class MainViewModel : ViewModelBase
    {
        private readonly NavigationService navigationService;

        public MainViewModel(NavigationService navigationService, PlayerViewModel playerViewModel)
        {
            MenuItems =
            [
                new MenuItem { Icon = "Home", Name = "Home"},
                new MenuItem { Icon = "Home", Name = "Home"},
                new MenuItem { Icon = "Home", Name = "Home"},
                new MenuItem { Icon = "FolderOutline", Name = "本地音乐", ViewModelType = typeof(LocalMusicViewModel)},
                new MenuItem { Icon = "AccountArrowUp", Name = "插件管理"},
                new MenuItem { Icon = "History", Name = "历史播放"}
            ];

            this.navigationService = navigationService;
            this.navigationService.CurrentViewModelChanged += () =>
            {
                CurrentViewModel = this.navigationService.CurrentViewModel;
            };

            PlayerViewModel = playerViewModel;
        }

        [ObservableProperty]
        private ViewModelBase? currentViewModel;

        [ObservableProperty]
        private ViewModelBase? playerViewModel;

        [ObservableProperty]
        private ObservableCollection<MenuItem>? menuItems;

        [RelayCommand]
        private void NavigateToView(MenuItem? menuItem)
        {
            if (menuItem != null && menuItem.ViewModelType != null)
            {
                navigationService.NavigateTo(menuItem.ViewModelType);
            }
        }

        [RelayCommand]
        private void GoSettings()
        {
            navigationService.NavigateTo(typeof(SettingsViewModel));
        }
    }
}
