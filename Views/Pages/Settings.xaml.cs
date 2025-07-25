using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using WpfApp1.Services;
using WpfApp1.Utility;
using WpfApp1.ViewModels;
using Newtonsoft.Json;
using System.Diagnostics;
using Microsoft.Toolkit.Uwp.Notifications;


namespace WpfApp1.Views.Pages
{
    /// <summary>
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class Settings : Page
    {
        
        private IDialogService _dialogService;
        private DailyScheduler _dailyScheduler;
        public SettingsViewModel SettingsPageVM { get; set; }
        public Settings(DailyScheduler dailyScheduler)
        {
            _dialogService              = new DialogService();
            _dailyScheduler             = dailyScheduler;
            InitializeComponent();
            SettingsPageVM              = new SettingsViewModel();
            SettingsPageVM.SelectedHour = AppSettings.getSettings().ReminderHour;
            this.DataContext            = SettingsPageVM;
        }

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (e.NewSize.Width > 0 && e.NewSize.Height > 0)
            {
                SettingsPageVM.FrameWidth = e.NewSize.Width;
                SettingsPageVM.FrameHeight = e.NewSize.Height;
            }
        }

        private void PathButton_Click(object sender, RoutedEventArgs e)
        {
            var path                  = _dialogService.selectPathDialog();
            SettingsPageVM.FolderPath = path;

        }


        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            SettingsPageVM.UpdateSettings(_dailyScheduler);
            NavigationService?.Navigate(new HomePage(_dailyScheduler));
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new HomePage(_dailyScheduler));
        }
    }
}
