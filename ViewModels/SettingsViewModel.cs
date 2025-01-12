using MusicPlayer.Enums;

namespace MusicPlayer.ViewModels
{
    public partial class SettingsViewModel : ViewModelBase
    {
        private WindowExitMode windowExitMode = App.Current.Settings.WindowExitMode;
        private PlayMode playMode = App.Current.Settings.PlayMode;
        private PlaylistAddMode playlistAddMode = App.Current.Settings.PlaylistAddMode;

        public WindowExitMode WindowExitMode
        {
            get => windowExitMode;
            set
            {
                if (SetProperty(ref windowExitMode, value))
                {
                    App.Current.Settings.WindowExitMode = value;
                };
            }
        }

        public PlayMode PlayMode
        {
            get => playMode;
            set
            {
                if (SetProperty(ref playMode, value))
                {
                    App.Current.Settings.PlayMode = value;
                };
            }
        }

        public PlaylistAddMode PlaylistAddMode
        {
            get => playlistAddMode;
            set
            {
                if (SetProperty(ref playlistAddMode, value))
                {
                    App.Current.Settings.PlaylistAddMode = value;
                };
            }
        }
    }
}
