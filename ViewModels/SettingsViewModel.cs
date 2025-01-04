using MusicPlayer.Enum;

namespace MusicPlayer.ViewModels
{
    public partial class SettingsViewModel : ViewModelBase
    {
        private ExitModeEnum exitMode = App.Current.Settings.ExitMode;
        private PlayModeEnum playMode = App.Current.Settings.PlayMode;
        private RowDoubleClickedModeEnum rowDoubleClickedMode = App.Current.Settings.RowDoubleClickedMode;

        public ExitModeEnum ExitMode
        {
            get => exitMode;
            set
            {
                if (exitMode == value) return;

                SetProperty(ref exitMode, value);
                App.Current.Settings.ExitMode = value;
            }
        }

        public PlayModeEnum PlayMode
        {
            get => playMode;
            set
            {
                if (playMode == value) return;

                SetProperty(ref playMode, value);
                App.Current.Settings.PlayMode = value;
            }
        }

        public RowDoubleClickedModeEnum RowDoubleClickedMode
        {
            get => rowDoubleClickedMode;
            set
            {
                if (rowDoubleClickedMode == value) return;

                SetProperty(ref rowDoubleClickedMode, value);
                App.Current.Settings.RowDoubleClickedMode = value;
            }
        }
    }
}
