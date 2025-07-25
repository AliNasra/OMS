using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using WpfApp1.Models;
using WpfApp1.Services;

namespace WpfApp1.ViewModels
{
    public class FetchObligationViewModel : INotifyPropertyChanged
    {
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
        public IObligationService obligationService;

        public FetchObligationViewModel()
        {
            obligationService = new ObligationService();
            fetchData();
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
                OnPropertyChanged(nameof(LabelWidth));
                OnPropertyChanged(nameof(TextBoxWidth));
                OnPropertyChanged(nameof(ItemMargin));
                OnPropertyChanged(nameof(ReturnButtonMargin));
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
                OnPropertyChanged(nameof(TextBoxHeight));
                OnPropertyChanged(nameof(ItemMargin));
                OnPropertyChanged(nameof(ReturnButtonMargin));
            }
        }
        private string _faxNo = "";
        public string  FaxNo
        {
            get => _faxNo;
            set
            {
                _faxNo = value;
                OnPropertyChanged();
                fetchData();
            }
        }

        private DateTime _obligationDate = DateTime.Today.Date;

        public DateTime ObligationDate
        {
            get => _obligationDate;
            set
            {
                _obligationDate = value;
                OnPropertyChanged();
                fetchData();
            }
        }

        private string _obligationText = "";

        public string ObligationText
        {
            get => _obligationText;
            set
            {
                _obligationText = value;
                OnPropertyChanged();
                fetchData();
            }
        }

        private bool _isFaxNoChecked = true;

        public bool IsFaxNoChecked
        {
            get => _isFaxNoChecked;
            set
            {
                _isFaxNoChecked = value;
                OnPropertyChanged();
                fetchData();
            }
        }

        private bool _isObligationTextChecked = true;

        public bool IsObligationTextChecked
        {
            get => _isObligationTextChecked;
            set
            {
                _isObligationTextChecked = value;
                OnPropertyChanged();
                fetchData();
            }
        }

        private bool _isObligationDateChecked = false;

        public bool IsObligationDateChecked
        {
            get => _isObligationDateChecked;
            set
            {
                _isObligationDateChecked = value;
                OnPropertyChanged();
                fetchData();
            }
        }

        public double ButtonWidth => FrameWidth * 0.20;
        public double LabelWidth => FrameWidth * 0.10;
        public double ButtonHeight => FrameHeight * 0.15;
        public double TextBoxWidth => FrameWidth * 0.25;
        public double TextBoxHeight => FrameHeight * 0.15;
        //
        public Thickness ItemMargin => new Thickness(
            FrameWidth * 0.01,      // Left
            FrameHeight * 0.01,     // Top
            FrameWidth * 0.01,       // Right
            FrameHeight * 0.01      // Bottom
        );

        public Thickness ReturnButtonMargin => new Thickness(
            FrameWidth * 0.1,      // Left
            FrameHeight * 0.01,     // Top
            0,       // Right
            FrameHeight * 0.01      // Bottom
        );

        public void fetchData()
        {
            string? _faxNoTemp          = FaxNo;
            string? _obligationTextTemp = ObligationText;
            DateTime? _obligationDateTemp = ObligationDate;
            if (IsFaxNoChecked == false) {
                _faxNoTemp = null;
            }
            if (IsObligationTextChecked == false)
            {
                _obligationTextTemp = null;
            }
            if (IsObligationDateChecked == false)
            {
                _obligationDateTemp = null;
            }
            Obligations = new ObservableCollection<Obligation>(obligationService.fetchObligation(_faxNoTemp, _obligationDateTemp, _obligationTextTemp));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

