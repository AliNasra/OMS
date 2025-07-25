using CsvHelper;
using Newtonsoft.Json;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using static System.Formats.Asn1.AsnWriter;
using static System.Windows.Forms.AxHost;
using Color = System.Windows.Media.Color;
using ColorConverter = System.Windows.Media.ColorConverter;


namespace WpfApp1.Utility
{
    [JsonObject(MemberSerialization.OptIn)]
    public class AppSettings : INotifyPropertyChanged
    {
        private static AppSettings _settings;

        ////////////////////////////////////////
        private string _reminderHour;
        [JsonProperty]
        public string ReminderHour
        {
            get { return _reminderHour; }
            set
            {
                if (_reminderHour != null)
                {
                    _reminderHour = value;
                    OnPropertyChanged(nameof(ReminderHour));
                };

            }
        }
        //////////////////////////////////
        private string _settingsStorageFolder;
        [JsonProperty]
        public string SettingsStorageFolder
        {
            get { return _settingsStorageFolder; }
            set
            {
                if (_settingsStorageFolder != null)
                {
                    _settingsStorageFolder = value;
                    OnPropertyChanged(nameof(SettingsStorageFolder));

                }

            }
        }
        //////////////////
        private DateTime _lastNotificationTime;
        [JsonProperty]
        public DateTime LastNotificationTime
        {
            get { return _lastNotificationTime; }
            set
            {
                if (_lastNotificationTime != null)
                {
                    _lastNotificationTime = value;
                    OnPropertyChanged(nameof(LastNotificationTime));

                }

            }
        }
        //////////////////
        public ObservableCollection<string> HourOptions { get; set; }
        private AppSettings()
        {
            _settingsStorageFolder = "";
            _reminderHour          = "00:00";
            HourOptions            = new ObservableCollection<string>(
            Enumerable.Range(0, 24).Select(h => h.ToString("D2") + ":00")
        );
            BackgroundBrush = new LinearGradientBrush
            {
                StartPoint = new System.Windows.Point(0, 0),
                EndPoint = new System.Windows.Point(1, 0),
                GradientStops = new GradientStopCollection
            {
                new GradientStop((Color)ColorConverter.ConvertFromString("#FF1A1A2E"), 0.0),
                new GradientStop((Color)ColorConverter.ConvertFromString("#FF1C1C1C"), 1.0)
            }
            };
            TitleColor = new SolidColorBrush(((Color)ColorConverter.ConvertFromString("#FF42DA67")))  ;
            LabelColor = new SolidColorBrush(((Color)ColorConverter.ConvertFromString("#FFFF00")));
            ButtonColor = new SolidColorBrush(((Color)ColorConverter.ConvertFromString("#FFA500")));          
        }
        //////////////////
        private LinearGradientBrush _backgroundBrush;

        public LinearGradientBrush BackgroundBrush
        {
            get => _backgroundBrush;
            set
            {
                _backgroundBrush = value;
                OnPropertyChanged(nameof(BackgroundBrush));
            }
        }
        //////////////////
        private SolidColorBrush _titleColor;

        public SolidColorBrush TitleColor
        {
            get => _titleColor;
            set
            {
                _titleColor = value;
                OnPropertyChanged(nameof(TitleColor));
            }
        }
        //////////////////
        private SolidColorBrush _labelColor;

        public SolidColorBrush LabelColor
        {
            get => _labelColor;
            set
            {
                _labelColor = value;
                OnPropertyChanged(nameof(LabelColor));
            }
        }

        //////////////////
        private SolidColorBrush _buttonColor;

        public SolidColorBrush ButtonColor
        {
            get => _buttonColor;
            set
            {
                _buttonColor = value;
                OnPropertyChanged(nameof(ButtonColor));
            }
        }
        //////////////////
        private int _operationState;
        public int OperationState
        {
            get => _operationState;
            set
            {
                _operationState = value;
                if (_operationState == 0)
                {
                    IsReadOnly     = false;
                    IsEnabled      = true;
                    ButtonText     = "عدل";
                    TitleText      = "تعديل إلتزامات";
                    IsVisible      = true;
                    PathButtonText = "بحث";
                }
                else if (_operationState == 1)
                {
                    IsReadOnly     = true;
                    IsEnabled      = false;
                    ButtonText     = "مسح";
                    TitleText      = "مسح إلتزامات";
                    IsVisible      = true;
                    PathButtonText = "فتح";
                }
                else
                {
                    TitleText      = "البحث عن إلتزامات";
                    IsReadOnly     = true;
                    IsEnabled      = false;
                    IsVisible      = false;
                    PathButtonText = "فتح";
                }
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsReadOnly));
                OnPropertyChanged(nameof(IsVisible));
                OnPropertyChanged(nameof(ButtonText));
                OnPropertyChanged(nameof(TitleText));
                OnPropertyChanged(nameof(IsEnabled));
                OnPropertyChanged(nameof(PathButtonText));
            }
        }
        //////////////////
        private string _titleText;
        public string TitleText
        {
            get => _titleText;
            set
            {
                _titleText = value;
                OnPropertyChanged(nameof(TitleText));
            }
        }
        //////////////////
        private string _buttonText;
        public string ButtonText
        {
            get => _buttonText;
            set
            {
                _buttonText = value;
                OnPropertyChanged(nameof(ButtonText));
            }
        }
        //////////////////
        private bool _isReadOnly;
        public bool IsReadOnly
        {
            get => _isReadOnly;
            set
            {
                _isReadOnly = value;
                OnPropertyChanged(nameof(IsReadOnly));
            }
        }
        //////////////////
        private bool _isEnabled;
        public bool IsEnabled
        {
            get => _isEnabled;
            set
            {
                _isEnabled = value;
                OnPropertyChanged(nameof(IsEnabled));
            }
        }
        //////////////////
        private bool _isVisible;
        public bool IsVisible
        {
            get => _isVisible;
            set
            {
                _isVisible = value;
                OnPropertyChanged(nameof(IsVisible));
            }
        }
        //////////////////

        private string _pathButtonText;
        public string PathButtonText
        {
            get => _pathButtonText;
            set
            {
                _pathButtonText = value;
                OnPropertyChanged();
            }
        }
        //////////////////
        public static AppSettings getSettings()
        {
            if (_settings == null)
            {
                _settings = new AppSettings();
                return _settings;
            }
            else
            {
                return _settings;
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
