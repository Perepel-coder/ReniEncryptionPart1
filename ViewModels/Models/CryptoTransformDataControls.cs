using ReactiveUI;
using System.Collections.ObjectModel;
using System.Windows;

namespace ViewModels.Models
{
    public class CryptoTransformDataControls : WindowControlsMain
    {
        public ObservableCollection<RowTwoMtrx> ListOfAlg { get; set; } = new();
        public ObservableCollection<RowTwoMtrx> ListOfAlgMode { get; set; } = new();
        private object? inputData;
        public object? InputData
        {
            get => inputData;
            set => this.RaiseAndSetIfChanged(ref inputData, value);
        }

        private RowTwoMtrx algSelectedItem = new();
        private RowTwoMtrx algModeSelectedItem = new();
        public RowTwoMtrx AlgSelectedItem
        {
            get => algSelectedItem;
            set => this.RaiseAndSetIfChanged(ref algSelectedItem, value);
        }
        public RowTwoMtrx AlgModeSelectedItem
        {
            get => algModeSelectedItem;
            set => this.RaiseAndSetIfChanged(ref algModeSelectedItem, value);
        }

        private string statusBar = string.Empty;
        public string StatusBar
        {
            get => statusBar;
            set => this.RaiseAndSetIfChanged(ref statusBar, value);
        }

        private Visibility visibilityDataGrid = Visibility.Collapsed;
        private Visibility visibilityImage = Visibility.Collapsed;
        private Visibility visibilityTextBlock = Visibility.Collapsed;

        public Visibility VisibilityDataGrid
        {
            get => visibilityDataGrid;
            set => this.RaiseAndSetIfChanged(ref visibilityDataGrid, value);
        }
        public Visibility VisibilityImage
        {
            get => visibilityImage;
            set => this.RaiseAndSetIfChanged(ref visibilityImage, value);
        }
        public Visibility VisibilityTextBlock
        {
            get => visibilityTextBlock;
            set => this.RaiseAndSetIfChanged(ref visibilityTextBlock, value);
        }

        private string complementarySymbol = string.Empty;
        private string orientationSTR = string.Empty;
        private string startKeyValue = string.Empty;
        private int initVec;
        public string ComplementarySymbol
        {
            get => complementarySymbol;
            set => this.RaiseAndSetIfChanged(ref complementarySymbol, value);
        }
        public string StartKeyValue
        {
            get => startKeyValue;
            set => this.RaiseAndSetIfChanged(ref startKeyValue, value);
        }
        public int InitVec
        {
            get => initVec;
            set => this.RaiseAndSetIfChanged(ref initVec, value);
        }
        public string OrientationSTR
        {
            get => orientationSTR;
            set => this.RaiseAndSetIfChanged(ref orientationSTR, value);
        }
    }
}
