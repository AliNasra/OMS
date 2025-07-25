using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using WpfApp1.Models;
using WpfApp1.Services;
using MessageBox = System.Windows.Forms.MessageBox;

namespace WpfApp1.ViewModels
{
    public class ObligationCalendarViewModel : INotifyPropertyChanged
    {
        public IObligationService obligationService;
        private ObservableCollection<Obligation> _obligations;
        public ObservableCollection<Obligation> Obligations
        {
            get => _obligations;
            set
            {
                _obligations = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Obligation> _dayObligations;
        public ObservableCollection<Obligation> DayObligations
        {
            get => _dayObligations;
            set
            {
                _dayObligations = value;
                OnPropertyChanged();
            }
        }
        
        private double _frameWidth;
        public double FrameWidth
        {
            get => _frameWidth;
            set
            {
                _frameWidth = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(ButtonWidth));
                OnPropertyChanged(nameof(ObligationWidth));     
                OnPropertyChanged(nameof(ItemMargin));
                OnPropertyChanged(nameof(CloseButtonMargin));
                OnPropertyChanged(nameof(ObligationMargin));
            }
        }

        private double _frameHeight;
        public double FrameHeight
        {
            get => _frameHeight;
            set
            {
                _frameHeight = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(ButtonHeight));
                OnPropertyChanged(nameof(ItemMargin));
                OnPropertyChanged(nameof(CloseButtonMargin));
                OnPropertyChanged(nameof(ObligationMargin));
            }
        }

        private DateTime _selectedDate;
        public DateTime SelectedDate
        {
            get => _selectedDate;
            set
            {
                _selectedDate = value;
                OnPropertyChanged();
                filterObligations();
            }
        }

        public double ButtonWidth => FrameWidth * 0.30;
        public double ObligationWidth => FrameWidth * 0.28;
        public double ButtonHeight => FrameHeight * 0.15;
        public Thickness ItemMargin => new Thickness(
                    FrameWidth * 0.005,      // Left
                    FrameHeight * 0.005,     // Top
                    FrameWidth * 0.005,       // Right
                    FrameHeight * 0.005      // Bottom
        );

        public Thickness ObligationMargin => new Thickness(
                    FrameWidth * 0.01,      // Left
                    FrameHeight * 0.005,     // Top
                    FrameWidth * 0.005,       // Right
                    FrameHeight * 0.005      // Bottom
        );

        public Thickness CloseButtonMargin => new Thickness(
            0,      // Left
            FrameHeight * 0.01,     // Top
            FrameWidth * 0.01,       // Right
            FrameHeight * 0.01      // Bottom
        );

        public void fetchData()
        {
            Obligations = new ObservableCollection<Obligation>(obligationService.fetchObligation(null, null, null));
            OnPropertyChanged();
        }

        public void filterObligations()
        {
            DayObligations = new ObservableCollection<Obligation>(Obligations.Where(p=>p.startDate.Date == _selectedDate.Date).ToList());
            OnPropertyChanged();
        }
        public ObligationCalendarViewModel(DateTime calendarDate)
        {
            _selectedDate = calendarDate;
            obligationService = new ObligationService();
            fetchData();
            filterObligations();

        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
