using MusicPlayer.ViewModels;

namespace MusicPlayer.Services
{
    public class NavigationService
    {
        private ViewModelBase? currentViewModel;
        public ViewModelBase? CurrentViewModel
        {
            get => currentViewModel;
            set
            {
                if (currentViewModel != value)
                {
                    currentViewModel = value;
                    CurrentViewModelChanged?.Invoke();
                }
            }
        }

        public event Action? CurrentViewModelChanged;

        public void NavigateTo(Type viewModelType)
        {
            var viewModel = App.Current.Services.GetService(viewModelType);
            if (viewModel == null)
                return;

            CurrentViewModel = (ViewModelBase)viewModel;
        }
    }
}
