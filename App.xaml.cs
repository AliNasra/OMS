using System.Configuration;
using System.Data;
using System.Windows;
using System.IO;
using Newtonsoft.Json;
using WpfApp1.Utility;
using WpfApp1.Services;
using WpfApp1.Models;
using System.ComponentModel;
using MessageBox = System.Windows.MessageBox;
using Application = System.Windows.Application;
using Microsoft.Win32;
using CommunityToolkit.WinUI.Notifications;
using System.Windows.Controls;
using System.Windows.Navigation;
using WpfApp1.Views.Pages;
using WpfApp1.Properties;
using System.Runtime.InteropServices;
using System.IO.Pipes;
using CsvHelper;


namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        DailyScheduler         _dailyScheduler = new DailyScheduler();
        private static Mutex   _startupMutex;
        private const string   PipeName        = "OMS_PIPE";


        [DllImport("shell32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        static extern int SetCurrentProcessExplicitAppUserModelID(string AppID);
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            bool isNewInstance;
            _startupMutex = new Mutex(true, "Global\\MyUniqueOMSAppNameMutex", out isNewInstance);
            ToastNotificationManagerCompat.OnActivated += ToastActivated;
            if (!isNewInstance)
            {
                bool launchedFromToast = false;

                foreach (var arg in e.Args)
                {
                    if (arg.Contains("action=openCalendar"))
                    {
                        launchedFromToast = true;
                        break;
                    }
                }
                using (NamedPipeClientStream pipe = new NamedPipeClientStream(".", PipeName, PipeDirection.Out))
                {
                    try
                    {
                        pipe.Connect(100);
                        using (StreamWriter writer = new StreamWriter(pipe))
                        {
                            if (launchedFromToast)
                            {
                                writer.WriteLine("SHOW_CALENDAR");
                            }
                            else
                            {
                                writer.WriteLine("OPEN_WINDOW");                        
                            }
                            writer.Flush();
                        }
                        
                    }
                    catch
                    {
                        MessageBox.Show("Failed to communicate with the running instance."); 
                    }
                }
                Shutdown();

                return;

            }
            StartPipeServer();
            SetCurrentProcessExplicitAppUserModelID("com.Al.OMS");
            AddAppToStartup();
            IObligationService _obligationService = new ObligationService();
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string JSONPath = Path.Combine(basePath, "appsettings.json");
            var defaultSettings = new
            {
                ReminderHour = "",
                SettingsStorageFolder = "",
                LastNotificationTime = DateTime.MinValue
            };
            string defaultJSON = JsonConvert.SerializeObject(defaultSettings, Formatting.Indented);
            if (!File.Exists(JSONPath))
            {
                File.WriteAllText(JSONPath, defaultJSON);
            }
            else
            {
                try
                {
                    using (var fs = File.OpenRead(JSONPath))
                    {
                    }
                }
                catch (IOException)
                {
                    File.Delete(JSONPath);
                    File.WriteAllText(JSONPath, defaultJSON);
                }
            }
            var json = File.ReadAllText(JSONPath);
            var settings = JsonConvert.DeserializeObject<AppSettings>(json);
            if (string.Equals(settings.ReminderHour, ""))
            {
                settings.ReminderHour = "09:00";
                AppSettings.getSettings().ReminderHour = "09:00";          
            }
            else
            {
                AppSettings.getSettings().ReminderHour = settings.ReminderHour;
            }
            
            if (string.Equals(settings.SettingsStorageFolder, ""))
            {
                settings.SettingsStorageFolder                  = basePath;
                AppSettings.getSettings().SettingsStorageFolder = basePath;
                var databasePath = Path.Combine(basePath, "database.csv");
                using (StreamWriter writer = new StreamWriter(File.Create(databasePath)))
                {
                    writer.WriteLine("faxNo,faxDate,startDate,endDate,obligationText,attendants,responsibleUnits,faxPath");
                }
            }
            else
            {
                var databasePath = Path.Combine(settings.SettingsStorageFolder, "database.csv");
                if (Directory.Exists(settings.SettingsStorageFolder))
                {
                    try
                    {
                        using (FileStream fs = File.OpenRead(databasePath))
                        {

                        }
                        AppSettings.getSettings().SettingsStorageFolder = settings.SettingsStorageFolder;
                    }
                    catch (Exception ex)
                    {                        
                        using (StreamWriter writer = new StreamWriter(File.Create(databasePath)))
                        {
                            writer.WriteLine("faxNo,faxDate,startDate,endDate,obligationText,attendants,responsibleUnits,faxPath");
                        }
                    }
                }
                else
                {
                    databasePath = Path.Combine(basePath, "database.csv");
                    settings.SettingsStorageFolder = basePath;
                    AppSettings.getSettings().SettingsStorageFolder = basePath;
                    using (StreamWriter writer = new StreamWriter(File.Create(databasePath)))
                    {
                        writer.WriteLine("faxNo,faxDate,startDate,endDate,obligationText,attendants,responsibleUnits,faxPath");
                    }
                }            
            }

            if (settings.LastNotificationTime.Date < DateTime.Now.Date)
            {
                int reminderHour = int.Parse(settings.ReminderHour.Split(":").First().Trim());
                TimeSpan currentTime = DateTime.Now.TimeOfDay;
                TimeSpan reminderTime = new TimeSpan(reminderHour, 0, 0);
                if (currentTime >= reminderTime)
                {
                    settings.LastNotificationTime = DateTime.Now;
                    _dailyScheduler.PerformScheduledTask();
                    _dailyScheduler.Schedule(int.Parse(settings.ReminderHour.Split(":")[0]), 0);
                }
                AppSettings.getSettings().LastNotificationTime = settings.LastNotificationTime;
            }
            else
            {
                AppSettings.getSettings().LastNotificationTime = settings.LastNotificationTime;
            }
            

            string updatedJson = JsonConvert.SerializeObject(settings, Formatting.Indented);          
            File.WriteAllText(JSONPath, updatedJson);
            MainWindow mainWindow = new MainWindow(_dailyScheduler);
            mainWindow.Show();
        }
        private void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true; // Cancel the close
            Application.Current.MainWindow.ShowInTaskbar = false;
            Application.Current.MainWindow.Hide();
        }
        private void AddAppToStartup()
        {
            string appName = "OMS";
            string appPath = Path.Combine(AppContext.BaseDirectory, "WpfApp1.exe");
            RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true);
            key.SetValue(appName, $"\"{appPath}\"");
        }
        private void ToastActivated(ToastNotificationActivatedEventArgsCompat e)
        {
            using (NamedPipeClientStream pipe = new NamedPipeClientStream(".", PipeName, PipeDirection.Out))
            {
                try
                {
                    pipe.Connect(100);
                    using (StreamWriter writer = new StreamWriter(pipe))
                    {
                            writer.WriteLine("SHOW_CALENDAR");
                            writer.Flush();
                    }
                }
                catch
                {
                }
                Shutdown();
            }
        }
        protected override void OnExit(ExitEventArgs e)
        {
            _startupMutex?.ReleaseMutex();
            _startupMutex?.Dispose();
            base.OnExit(e);
        }

        private void openObligationCalendar()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var window = new Window();
                var frame = new Frame();
                frame.NavigationUIVisibility = NavigationUIVisibility.Hidden;
                frame.Navigate(new ObligationCalendar(DateTime.Today.AddDays(1)));
                window.Content = frame;
                window.Title = "Obligation Calendar";
                window.Width = 600;
                window.Height = 400;
                window.Show();
            });
        }

        private void StartPipeServer()
        {
            Task.Run(() =>
            {
                while (true)
                {
                    using (NamedPipeServerStream pipeServer = new NamedPipeServerStream(PipeName, PipeDirection.In))
                    {
                        pipeServer.WaitForConnection();
                        using (StreamReader reader = new StreamReader(pipeServer))
                        {
                            string command = reader.ReadLine();
                            if (command == "SHOW_CALENDAR")
                            {
                                openObligationCalendar();
                            }
                            else if (command == "OPEN_WINDOW")
                            {
                                Application.Current.Dispatcher.Invoke(() =>
                                {
                                    Application.Current.MainWindow.ShowInTaskbar = true;
                                    Application.Current.MainWindow.Show();
                                });                  
                            }
                            else
                            {

                            }
                        }
                    }
                }
            });
        }
    }

}
