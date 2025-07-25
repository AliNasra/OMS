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
using WpfApp1.ViewModels;
using WpfApp1.Models;
using static System.Net.Mime.MediaTypeNames;
using WpfApp1.Utility;
using System.Diagnostics;
using MessageBox = System.Windows.MessageBox;

namespace WpfApp1.Views.Pages
{
    /// <summary>
    /// Interaction logic for AddObligation.xaml
    /// </summary>
    public partial class ObligationRevision : Page
    {
        public     ObligationRevisionViewModel ObligationRevisionVM { get; set; }
        private    IDialogService             _dialogService;
        private    DailyScheduler             _dailyScheduler;

        public ObligationRevision(Obligation obligation,DailyScheduler dailyScheduler)
        {
            InitializeComponent();
            ObligationRevisionVM = new ObligationRevisionViewModel(obligation);
            DataContext          = ObligationRevisionVM;
            _dialogService       = new DialogService();
            _dailyScheduler      = dailyScheduler;

        }
        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (e.NewSize.Width > 0 && e.NewSize.Height > 0)
            {
                ObligationRevisionVM.FrameWidth = e.NewSize.Width;
                ObligationRevisionVM.FrameHeight = e.NewSize.Height;
            }
        }
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (AppSettings.getSettings().OperationState == 0)
            {
                string prompt = ObligationRevisionVM.ModifyObligation();
                if (prompt.Length == 0)
                {
                    MessageBox.Show($"تم تعديل الإلتزام بنجاح", "نجاح العملية", MessageBoxButton.OK);
                }
                else
                {
                    MessageBox.Show($"{prompt}", "فشل العملية", MessageBoxButton.OK);

                }
            }
            else if (AppSettings.getSettings().OperationState == 1)
            {
                string prompt = ObligationRevisionVM.DeleteObligation();
                if (prompt.Length == 0)
                {
                    MessageBox.Show($"تم مسح الإلتزام بنجاح", "نجاح العملية", MessageBoxButton.OK);
                }
                else
                {
                    MessageBox.Show($"{prompt}", "فشل العملية", MessageBoxButton.OK);

                }
            }
            else
            {
            }
            NavigationService?.Navigate(new FetchObligation(_dailyScheduler));
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new FetchObligation(_dailyScheduler));
        }
        private void PathButton_Click(object sender, RoutedEventArgs e)
        {
            if (AppSettings.getSettings().OperationState == 0)
            {
                var path = _dialogService.selectFaxFileDialog();
                ObligationRevisionVM.FaxFilePath = path;
            }
            else
            {
                if (System.IO.File.Exists(ObligationRevisionVM.FaxFilePath))
                {
                    Process.Start(new ProcessStartInfo(ObligationRevisionVM.FaxFilePath)
                    {
                        UseShellExecute = true
                    });
                }
                else
                {
                    MessageBox.Show("!هذا الملف لا يمكن الوصول إليه","تنبيه",MessageBoxButton.OK);
                }
            }
            

        }
    }
}
