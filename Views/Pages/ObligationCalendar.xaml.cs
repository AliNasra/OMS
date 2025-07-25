using System;
using System.Collections.Generic;
using System.Globalization;
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
using WpfApp1.ViewModels;

namespace WpfApp1.Views.Pages
{
    /// <summary>
    /// Interaction logic for ObligationCalendar.xaml
    /// </summary>
    public partial class ObligationCalendar : Page
    {
        public ObligationCalendarViewModel ObligationCalendarVM;
        public ObligationCalendar(DateTime calendarInitalDate)
        {
            InitializeComponent();
            ObligationCalendarVM = new ObligationCalendarViewModel(calendarInitalDate);
            DataContext = ObligationCalendarVM;            
        }
        public void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this)?.Close();
        }
        public void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (e.NewSize.Width > 0 && e.NewSize.Height > 0)
            {
                ObligationCalendarVM.FrameWidth = e.NewSize.Width;
                ObligationCalendarVM.FrameHeight = e.NewSize.Height;
            }
        }

    }
}
