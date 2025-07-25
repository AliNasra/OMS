using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Models;
using System.Windows;
using WpfApp1.Services;

namespace WpfApp1.ViewModels
{
    public class ObligationRevisionViewModel : INotifyPropertyChanged
    {
        public IObligationService _obligationService;
        public ObligationRevisionViewModel(Obligation obligation)
        {
            _obligationService     = new ObligationService();
;           Obligation             = obligation;
            FaxNo                  = obligation.faxNo.ToString();
            FaxDate                = obligation.faxDate;
            ObligationText         = obligation.obligationText;
            ObligationStartDate    = obligation.startDate;
            ObligationEndDate      = obligation.endDate;
            Attendants             = obligation.attendants;
            ResponsibleUnit        = obligation.responsibleUnits;
            FaxFilePath            = obligation.faxPath;
            
        }
        private Obligation _obligation;
        public Obligation Obligation
        {
            get => _obligation;
            set
            {
                _obligation = value;
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
                OnPropertyChanged(nameof(LabelWidth));
                OnPropertyChanged(nameof(TextBoxWidth));
                OnPropertyChanged(nameof(PathButtonWidth));
                OnPropertyChanged(nameof(PathTextBoxWidth));

                OnPropertyChanged(nameof(ItemMargin));
                OnPropertyChanged(nameof(SaveButtonMargin));
                OnPropertyChanged(nameof(ReturnButtonMargin));
                OnPropertyChanged(nameof(ScrollViewMargin));

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
                OnPropertyChanged(nameof(LabelHeight));

                OnPropertyChanged(nameof(ItemMargin));
                OnPropertyChanged(nameof(SaveButtonMargin));
                OnPropertyChanged(nameof(ReturnButtonMargin));
                OnPropertyChanged(nameof(ScrollViewMargin));
            }
        }
        private DateTime _faxDate;
        public DateTime FaxDate
        {
            get => _faxDate;
            set
            {
                _faxDate = value;
                OnPropertyChanged();
            }
        }

        private string _faxNo;
        public string FaxNo
        {
            get => _faxNo;
            set
            {
                _faxNo = value;
                OnPropertyChanged();
            }
        }

        private string _obligationText;
        public string ObligationText
        {
            get => _obligationText;
            set
            {
                _obligationText = value;
                OnPropertyChanged();
            }
        }

        

        private string _attendants;
        public string Attendants
        {
            get => _attendants;
            set
            {
                _attendants = value;
                OnPropertyChanged();
            }
        }

        private DateTime _obligationStartDate;

        public DateTime ObligationStartDate
        {
            get => _obligationStartDate;
            set
            {
                _obligationStartDate = value;
                OnPropertyChanged();
            }
        }

        private DateTime _obligationEndDate;

        public DateTime ObligationEndDate
        {
            get => _obligationEndDate;
            set
            {
                _obligationEndDate = value;
                OnPropertyChanged();
            }
        }

        private string _responsibleUnits;
        public string ResponsibleUnit
        {
            get => _responsibleUnits;
            set
            {
                _responsibleUnits = value;
                OnPropertyChanged();
            }
        }

        private string _faxFilePath;
        public string FaxFilePath
        {
            get => _faxFilePath;
            set
            {
                _faxFilePath = value;
                OnPropertyChanged();
            }
        }


        public double ButtonWidth => FrameWidth * 0.2;
        public double LabelWidth => FrameWidth * 0.10;
        public double ButtonHeight => FrameHeight * 0.15;
        public double TextBoxWidth => FrameWidth * 0.45;
        public double TextBoxHeight => FrameHeight * 0.15;
        public double PathTextBoxWidth => FrameWidth * 0.35;
        public double PathButtonWidth => FrameWidth * 0.1;
        public double LabelHeight => FrameHeight * 0.10;

        public Thickness ItemMargin => new Thickness(
            FrameWidth * 0.01,      // Left
            FrameHeight * 0.01,     // Top
            FrameWidth * 0.01,       // Right
            FrameHeight * 0.01      // Bottom
        );

        public Thickness ScrollViewMargin => new Thickness(
            FrameWidth * 0.01,      // Left
            FrameHeight * 0.05,     // Top
            FrameWidth * 0.01,       // Right
            FrameHeight * 0.05      // Bottom
        );

        public Thickness SaveButtonMargin => new Thickness(
            0,      // Left
            FrameHeight * 0.01,     // Top
            FrameWidth * 0.1,       // Right
            FrameHeight * 0.01      // Bottom
        );

        public Thickness ReturnButtonMargin => new Thickness(
            FrameWidth * 0.1,      // Left
            FrameHeight * 0.01,     // Top
            0,       // Right
            FrameHeight * 0.01      // Bottom
        );

        public string ModifyObligation()
        {
            string prompt = _obligationService.modifyObligation(FaxNo,FaxDate,ObligationStartDate,ObligationEndDate,ObligationText,Attendants,ResponsibleUnit,FaxFilePath,Obligation);
            return prompt;
        }
        public string DeleteObligation()
        {
            string prompt = _obligationService.deleteObligation(Obligation);
            return prompt;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
