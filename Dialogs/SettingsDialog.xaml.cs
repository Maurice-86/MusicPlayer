using MusicPlayer.Resources;
using MusicPlayer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MusicPlayer.Dialogs
{
    /// <summary>
    /// SettingsDialog.xaml 的交互逻辑
    /// </summary>
    public partial class SettingsDialog : UserControl
    {
        private readonly string _dialogIdentifier;
        private readonly SettingsDialogViewModel _viewModel;
        public SettingsDialog(string dialogIdentifier = "RootDialog")
        {
            InitializeComponent();
            _dialogIdentifier = dialogIdentifier;
            _viewModel = new();
            this.DataContext = _viewModel;
        }

        /// <summary>
        /// 关闭对话框
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseDialog_Click(object sender, RoutedEventArgs e)
        {
            MaterialDesignThemes.Wpf.DialogHost.Close(_dialogIdentifier, null);
            var tag = (sender as Button)?.Tag.ToString();
            if (tag == "Save")
            {
                _viewModel.Save();
                MessageService.Instance.ShowMessage(Lang.Snackbar_SettingsSaveMessage);
            }
        }
    }
}
