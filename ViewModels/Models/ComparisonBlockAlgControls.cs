using ReactiveUI;
using System.Collections.ObjectModel;
using System.Windows;

namespace ViewModels.Models
{
    public class ComparisonBlockAlgControls: WindowControlsMain
    {
        public ObservableCollection<RowTwoMtrx> ListOfAlg { get; set; } = new();
        public ObservableCollection<RowTwoMtrx> ListOfAlgMode { get; set; } = new();
        public ObservableCollection<RowThreeMtrx> ChartSeries { get; set; } = new();

        private Visibility visibilityProgressBar = Visibility.Collapsed;
        public Visibility VisibilityProgressBar
        {
            get => visibilityProgressBar;
            set => this.RaiseAndSetIfChanged(ref visibilityProgressBar, value);
        }

        private int countOfData;
        public int CountOfData
        {
            get => countOfData;
            set
            {
                this.RaiseAndSetIfChanged(ref countOfData, value);
                if (value < this.CountOfCycles)
                {
                    this.CountOfCycles = value;
                }
            }
        }
        private int countOfKey;
        public int CountOfKey
        {
            get => countOfKey;
            set => this.RaiseAndSetIfChanged(ref countOfKey, value);
        }
        private int countOfCycles;
        public int CountOfCycles
        {
            get => countOfCycles;
            set
            {
                if (value <= this.CountOfData)
                {
                    this.RaiseAndSetIfChanged(ref countOfCycles, value);
                }
                else 
                {
                    this.RaiseAndSetIfChanged(ref countOfCycles, this.CountOfData);
                }
            }
        }

        private string orientationSTR = string.Empty;
        public string OrientationSTR
        {
            get => orientationSTR;
            set => this.RaiseAndSetIfChanged(ref orientationSTR, value);
        }

        private bool isEnabledButtonTransform = true;
        public bool IsEnabledButtonTransform
        {
            get => isEnabledButtonTransform;
            set => this.RaiseAndSetIfChanged(ref isEnabledButtonTransform, value);
        }
    }
}
