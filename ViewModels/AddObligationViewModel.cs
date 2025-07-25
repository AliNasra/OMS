using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApp1.ViewModels
{
    public class AddObligationViewModel : INotifyPropertyChanged
    {
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
                OnPropertyChanged(nameof(ScrollViewMargin));
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
                OnPropertyChanged(nameof(LabelHeight));

                OnPropertyChanged(nameof(ItemMargin));
                OnPropertyChanged(nameof(ScrollViewMargin)); 
                OnPropertyChanged(nameof(SaveButtonMargin));
                OnPropertyChanged(nameof(ReturnButtonMargin));
            }
        }
        private DateTime _faxDate = DateTime.Today;
        public DateTime FaxDate
        {
            get => _faxDate;
            set
            {
                _faxDate = value;
                OnPropertyChanged();
            }
        }

        private DateTime _obligationStartDate = DateTime.Today;

        public DateTime ObligationStartDate
        {
            get => _obligationStartDate;
            set
            {
                _obligationStartDate = value;
                OnPropertyChanged();
            }
        }

        private DateTime _obligationEndDate = DateTime.Today;

        public DateTime ObligationEndDate
        {
            get => _obligationEndDate;
            set
            {
                _obligationEndDate = value;
                OnPropertyChanged();
            }
        }

        public double ButtonHeight => FrameHeight * 0.15;
        public double ButtonWidth   => FrameWidth * 0.2;
        public double LabelHeight => FrameHeight * 0.10;
        public double LabelWidth    => FrameWidth * 0.10;
        public double TextBoxWidth  => FrameWidth * 0.45;
        public double PathTextBoxWidth => FrameWidth * 0.35;
        public double PathButtonWidth => FrameWidth * 0.1;


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


        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
