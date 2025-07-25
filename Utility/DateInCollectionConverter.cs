using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using WpfApp1.Models;

namespace WpfApp1.Utility
{
    public class DateInCollectionConverter : IMultiValueConverter
    {
        public object Convert(object[] value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value[0] is DateTime date && value[1] is IEnumerable<Obligation> obligations)
            {
                return obligations.Any(p=>p.startDate.Date == date.Date);
            }
            return false;
        }
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        => throw new NotImplementedException();

    }
}
