using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
using WpfApp1.Services;
using WpfApp1.Utility;
using Windows.UI.Notifications;

namespace WpfApp1.ViewModels
{
    public class SettingsViewModel : INotifyPropertyChanged
    {
        // Private Fields
        private double _frameHeight;
        private double _frameWidth;
        private string _folderPath;
        private string _selectedHour;

        // Constructor
        public SettingsViewModel()
        {
            if (AppSettings.getSettings().SettingsStorageFolder != null) {
                _folderPath = AppSettings.getSettings().SettingsStorageFolder;
            }
        }
        // Source Variables
        
        public double FrameWidth
        {
            get => _frameWidth;
            set
            {
                _frameWidth = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(ButtonWidth));
                OnPropertyChanged(nameof(TextBoxWidth));
                OnPropertyChanged(nameof(ItemMargin));
            }
        }
        public double FrameHeight
        {
            get => _frameHeight;
            set
            {
                _frameHeight = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(ButtonHeight));
                OnPropertyChanged(nameof(TextBoxHeight));
                OnPropertyChanged(nameof(ItemMargin));
            }
        }

        public string FolderPath
        {
            get => _folderPath;
            set
            {
                _folderPath = value;
                OnPropertyChanged();
            }
        }

        public string SelectedHour
        {
            get => _selectedHour;
            set
            {
                _selectedHour = value;
                OnPropertyChanged();
            }
        }

        // Target Variables
        public double ButtonWidth => FrameWidth * 0.10;
        public double ButtonHeight => FrameHeight * 0.10;
        public double TextBoxWidth => FrameWidth * 0.3;
        public double TextBoxHeight => FrameHeight * 0.10;
        public Thickness ItemMargin => new Thickness(
            FrameWidth * 0.01,      // Left
            FrameHeight * 0.01,     // Top
            FrameWidth * 0.01,      // Right
            FrameHeight * 0.01      // Bottom
        );
        public void UpdateSettings(DailyScheduler dailyScheduler)
        {
            string oldPath                                  = Path.Combine(AppSettings.getSettings().SettingsStorageFolder, "database.csv");
            string newPath                                  =  Path.Combine(FolderPath, "database.csv");
            AppSettings.getSettings().ReminderHour          = SelectedHour;
            AppSettings.getSettings().SettingsStorageFolder = FolderPath;
            string basePath                                 = AppDomain.CurrentDomain.BaseDirectory;
            string JSONPath                                 = Path.Combine(basePath, "appsettings.json");
            var json                                        = File.ReadAllText(JSONPath);
            var settings                                    = JsonConvert.DeserializeObject<AppSettings>(json);
            settings.ReminderHour                           = AppSettings.getSettings().ReminderHour;
            settings.SettingsStorageFolder                  = AppSettings.getSettings().SettingsStorageFolder;
            string updatedJson                              = JsonConvert.SerializeObject(settings, Formatting.Indented);
            var notifier = ToastNotificationManager.CreateToastNotifier("com.Al.OMS");
            var scheduledToasts = notifier.GetScheduledToastNotifications();
            foreach (var toast in scheduledToasts)
            {
                
                 notifier.RemoveFromSchedule(toast);
            }
            dailyScheduler.Schedule(int.Parse(SelectedHour.Split(":")[0]), 0);
            File.WriteAllText(JSONPath, updatedJson);
            File.Move(oldPath, newPath);
        }

        // Standard fields and methods for INotifyPropertyChanged interface 
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
