using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using WpfApp1.Utility;
using WpfApp1.ViewModels;
using Application = System.Windows.Application;


namespace WpfApp1.Views.Pages
{

    public partial class HomePage : Page
    {
        public HomePageViewModel HomePageVM   { get; set; }
        private DailyScheduler  _dailyScheduler;
        public HomePage(DailyScheduler dailyScheduler)
        {
            InitializeComponent();
            HomePageVM       = new HomePageViewModel();
            _dailyScheduler  = dailyScheduler;
            DataContext      = HomePageVM;
        }

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (e.NewSize.Width > 0 && e.NewSize.Height > 0)
            {
                HomePageVM.FrameWidth = e.NewSize.Width;
                HomePageVM.FrameHeight = e.NewSize.Height;
            }
        }



        private void AddObligation_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new AddObligation(_dailyScheduler));
        }

        private void ModifyObligation_Click(object sender, RoutedEventArgs e)
        {
            AppSettings.getSettings().OperationState = 0;
            NavigationService?.Navigate(new FetchObligation(_dailyScheduler));
        }

        private void DeleteObligation_Click(object sender, RoutedEventArgs e)
        {
            AppSettings.getSettings().OperationState = 1;
            NavigationService?.Navigate(new FetchObligation(_dailyScheduler));
        }

        private void FetchObligation_Click(object sender, RoutedEventArgs e)
        {
            AppSettings.getSettings().OperationState = 3;
            NavigationService?.Navigate(new FetchObligation(_dailyScheduler));
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new Settings(_dailyScheduler));
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.ShowInTaskbar = false;
            Application.Current.MainWindow.Hide();
        }
        private void Icon_Click(object sender, RoutedEventArgs e)
        {
            var window = new Window();
            var frame = new Frame();
            frame.NavigationUIVisibility = NavigationUIVisibility.Hidden;
            frame.Navigate(new ObligationCalendar(DateTime.Today));
            window.Content = frame;
            window.Title = "Obligation Calendar";
            window.Width = 600;
            window.Height = 400;
            window.Show();
        }

    }
}
