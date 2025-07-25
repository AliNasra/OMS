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
using WpfApp1.Services;
using WpfApp1.Utility;
using WpfApp1.ViewModels;
using static System.Net.Mime.MediaTypeNames;
using MessageBox = System.Windows.MessageBox;

namespace WpfApp1.Views.Pages
{
    /// <summary>
    /// Interaction logic for AddObligation.xaml
    /// </summary>
    public partial class AddObligation : Page
    {
        public AddObligationViewModel AddObligationVM { get; set; }
        private IObligationService    _obligationService;
        private IDialogService        _dialogService;
        private DailyScheduler        _dailyScheduler;
        public AddObligation(DailyScheduler dailyScheduler)
        {
            _dialogService     = new DialogService();
            _obligationService = new ObligationService();
            _dailyScheduler    = dailyScheduler;
            InitializeComponent();
            AddObligationVM    = new AddObligationViewModel();
            DataContext        = AddObligationVM;
            
        }
        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (e.NewSize.Width > 0 && e.NewSize.Height > 0)
            {
                AddObligationVM.FrameWidth = e.NewSize.Width;
                AddObligationVM.FrameHeight = e.NewSize.Height;
            }
        }
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            string result = _obligationService.canAddObligation(faxNo.Text, AddObligationVM.FaxDate,AddObligationVM.ObligationStartDate,AddObligationVM.ObligationEndDate, ObligationContent.Text,Attendants.Text, ResponsibleUnits.Text,FaxPathFile.Text);
            if (result.Length == 0)
            {
                MessageBox.Show($"تم إضافة الإلتزام بنجاح", "نجاح العملية", MessageBoxButton.OK);
                NavigationService?.Navigate(new HomePage(_dailyScheduler));
            }
            else
            {
                MessageBox.Show($"{result}", "فشل العملية", MessageBoxButton.OK);

            }          
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new HomePage(_dailyScheduler));
        }
        private void PathButton_Click(object sender, RoutedEventArgs e)
        {
            var path = _dialogService.selectFaxFileDialog();
            FaxPathFile.Text = path;

        }
    }
}
