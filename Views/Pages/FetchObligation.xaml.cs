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
using WpfApp1.Models;
using WpfApp1.Utility;
using WpfApp1.ViewModels;

namespace WpfApp1.Views.Pages
{
    /// <summary>
    /// Interaction logic for ModifyObligation.xaml
    /// </summary>
    public partial class FetchObligation : Page
    {
        public FetchObligationViewModel FetchObligationVM { get; set; }
        private DailyScheduler          _dailyScheduler;
        public FetchObligation (DailyScheduler dailyScheduler)
        {
            InitializeComponent();
            FetchObligationVM = new FetchObligationViewModel();
            _dailyScheduler   = dailyScheduler;
            DataContext       = FetchObligationVM;

        }
        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (e.NewSize.Width > 0 && e.NewSize.Height > 0)
            {
                FetchObligationVM.FrameWidth = e.NewSize.Width;
                FetchObligationVM.FrameHeight = e.NewSize.Height;
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new HomePage(_dailyScheduler));
        }

        private void ObligationGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ObligationGrid.SelectedItem is Obligation selectedItem)
            {
                // Call your function with the selected item
                NavigationService?.Navigate(new ObligationRevision(selectedItem, _dailyScheduler));
            }
        }

        
    }
}
