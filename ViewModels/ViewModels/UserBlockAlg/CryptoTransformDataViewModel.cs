using MediatR;
using QueryCQRS.Queries;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using ViewModels.Models;

namespace ViewModels.ViewModels.UserBlockAlg
{
    public class CryptoTransformDataViewModel
    {
        private readonly IMediator mediatR;
        private readonly DialogService dialogService;
        public CryptoTransformDataControls Controls { get; set; }

        public CryptoTransformDataViewModel(IMediator mediatR)
        {
            this.mediatR = mediatR;
            this.dialogService = new();
            this.Controls = new();
            this.Controls.OrientationSTR = "Зашифровать";
            this.Controls.ComplementarySymbol = "~";
            this.Controls.StartKeyValue = "I Love My Dog";
            this.Controls.InitVec = 1234;
            this.CreatListOfAlgAndModeOfAlg();
        }
        private async void CreatListOfAlgAndModeOfAlg()
        {
            var data = (await mediatR.Send(
                new ReqForCriptoAlgDB_REQUEST(ReqForCriptoAlgDB_REQUEST.Mode.GET_LIST_OF_ALG)
                )).data.ToList();
            WindowControlsMain.CreatRowTwoMtrx(Controls.ListOfAlg, data);
            if (Controls.ListOfAlg.Count != 0) { Controls.AlgSelectedItem = Controls.ListOfAlg[0]; }
            data = (await mediatR.Send(
                          new ReqForCriptoAlgDB_REQUEST(
                              ReqForCriptoAlgDB_REQUEST.Mode.GET_MODE_OF_ALG,
                              Controls.ListOfAlg.Select(el => el.Value))
                          )).data.ToList();
            WindowControlsMain.CreatRowTwoMtrx(Controls.ListOfAlgMode, data);
            if (Controls.ListOfAlgMode.Count != 0) { Controls.AlgModeSelectedItem = Controls.ListOfAlgMode[0]; }
        }

        private RelayCommand? openFile;
        private RelayCommand? transformData;
        private RelayCommand? openSettings;
        private RelayCommand? saveSettings;

        public ICommand OpenFile
        {
            get
            {
                return openFile ??= new RelayCommand(obj =>
                {
                    if (dialogService.OpenFileDialog() == true)
                    {
                        Thread thread = new(async () => 
                        {
                            FileStream stream = new(dialogService.FilePath, FileMode.Open);
                            if (dialogService.fileType == DialogService.FileType.TEXT)
                            {
                                Controls.InputData = (await mediatR.Send(new GetDataFromFileREQUEST<string>(stream))).data;
                                Controls.VisibilityTextBlock = Visibility.Visible;
                                Controls.VisibilityImage = Visibility.Collapsed;
                                Controls.VisibilityDataGrid = Visibility.Collapsed;
                            }
                            if (dialogService.fileType == DialogService.FileType.CTF)
                            {
                                Controls.InputData = (await mediatR.Send(new GetDataFromFileREQUEST<string>(stream))).data;
                                Controls.VisibilityTextBlock = Visibility.Visible;
                                Controls.VisibilityImage = Visibility.Collapsed;
                                Controls.VisibilityDataGrid = Visibility.Collapsed;
                            }
                            if (dialogService.fileType == DialogService.FileType.EXCEL)
                            {
                                Controls.InputData = (await mediatR.Send(new GetDataFromFileREQUEST<DataTable>(stream))).data;
                                Controls.VisibilityDataGrid = Visibility.Visible;
                                Controls.VisibilityTextBlock = Visibility.Collapsed;
                                Controls.VisibilityImage = Visibility.Collapsed;
                            }
                            Controls.VisibilityProjectTable = Visibility.Collapsed;
                            Controls.VisibilityProjectText = Visibility.Collapsed;
                            stream.Close();
                        });
                        try 
                        { 
                            thread.Start();
                            Controls.StatusBar = $"Файл {dialogService.FileName} успешно открыт.";
                        }
                        catch (Exception ex)
                        {
                            Controls.StatusBar = $"Ошибка при открытии файла {dialogService.FileName}. Файл не был открыт";
                            MessageBox.Show(ex.Message, "Ошибка при открытии файла", MessageBoxButton.OK, MessageBoxImage.Error); 
                        }
                        
                    }
                });
            }
        }
        public ICommand TransformData
        {
            get
            {
                return transformData ??= new RelayCommand(obj =>
                {
                    Thread thread = new(async () =>
                    {
                        try
                        {
                            if (Controls.InputData == null) { return; }

                            #region Определить параметры процесса
                            var mode = Enum.Parse(typeof(Mode), (string)Controls.AlgModeSelectedItem.Value[3]);
                            var alg = Enum.Parse(typeof(Alg), (string)Controls.AlgSelectedItem.Value[3]);
                            Orientation orientation;
                            if (Controls.OrientationSTR == "Зашифровать")
                            {
                                orientation = Orientation.ENCODE;

                            }
                            else if (Controls.OrientationSTR == "Расшифровать")
                            {
                                orientation = Orientation.DECODE;
                            }
                            else { throw new Exception("Ошибка при определении направления {CryptoTransformREQUEST<string>.Orientation}"); }
                            #endregion

                            if (dialogService.fileType == DialogService.FileType.TEXT)
                            {
                                Controls.InputData = (await mediatR.Send(new CryptoTransformREQUEST<string, string>(
                                    (string)Controls.InputData,
                                    Convert.ToChar(Controls.ComplementarySymbol),
                                    Controls.StartKeyValue,
                                    (Mode)mode,
                                    (Alg)alg,
                                    orientation,
                                    Controls.InitVec))).CurrentDataState;
                                return;
                            }
                            if (dialogService.fileType == DialogService.FileType.EXCEL)
                            {
                                if (dialogService.SaveFileDialog("CryptoTable files|*.ctf") == true)
                                {
                                    var table = (await mediatR.Send(new CryptoTransformREQUEST<DataTable, string>(
                                        (DataTable)Controls.InputData,
                                        Convert.ToChar(Controls.ComplementarySymbol),
                                        Controls.StartKeyValue,
                                        (Mode)mode,
                                        (Alg)alg,
                                        orientation,
                                        Controls.InitVec)));
                                    FileStream stream = new FileStream(dialogService.FilePath, FileMode.Create, FileAccess.Write);
                                    await mediatR.Send(new SaveDataInFileREQUEST<DataTable, string>(table.CurrentDataState, stream));
                                    Controls.StatusBar = $"Файл {dialogService.FileName} успешно сохранен.";
                                    stream.Close();
                                }
                                return;
                            }
                            if (dialogService.fileType == DialogService.FileType.CTF)
                            {
                                Controls.InputData = (await mediatR.Send(new CryptoTransformREQUEST<string, DataTable>(
                                    (string)Controls.InputData,
                                    Convert.ToChar(Controls.ComplementarySymbol),
                                    Controls.StartKeyValue,
                                    (Mode)mode,
                                    (Alg)alg,
                                    orientation,
                                    Controls.InitVec))).CurrentDataState;
                                Controls.VisibilityDataGrid = Visibility.Visible;
                                Controls.VisibilityTextBlock = Visibility.Collapsed;
                                Controls.VisibilityImage = Visibility.Collapsed;
                                Controls.VisibilityProjectTable = Visibility.Collapsed;
                                Controls.VisibilityProjectText = Visibility.Collapsed;
                                return;
                            }
                            if (dialogService.fileType == DialogService.FileType.NAN)
                            {
                                if(Controls.InputData.GetType() == typeof(string))
                                {
                                    Controls.InputData = (await mediatR.Send(new CryptoTransformREQUEST<string, string>(
                                        (string)Controls.InputData,
                                        Convert.ToChar(Controls.ComplementarySymbol),
                                        Controls.StartKeyValue,
                                        (Mode)mode,
                                        (Alg)alg,
                                        orientation,
                                        Controls.InitVec))).CurrentDataState;
                                    return;
                                }
                                if(Controls.InputData.GetType() == typeof(ObservableCollection<WindowControlsMain.RowTwoMtrx>))
                                {
                                    if (dialogService.SaveFileDialog("CryptoTable files|*.ctf") == true)
                                    {
                                        var inputData = WindowControlsMain.CreatDataTableRowTwoMtrx(
                                            (ObservableCollection<WindowControlsMain.RowTwoMtrx>)Controls.InputData, 20);
                                        var table = (await mediatR.Send(new CryptoTransformREQUEST<DataTable, string>(
                                            inputData,
                                            Convert.ToChar(Controls.ComplementarySymbol),
                                            Controls.StartKeyValue,
                                            (Mode)mode,
                                            (Alg)alg,
                                            orientation,
                                            Controls.InitVec)));
                                        FileStream stream = new FileStream(dialogService.FilePath, FileMode.Create, FileAccess.Write);
                                        await mediatR.Send(new SaveDataInFileREQUEST<DataTable, string>(table.CurrentDataState, stream));
                                        Controls.StatusBar = $"Файл {dialogService.FileName} успешно сохранен.";
                                        stream.Close();
                                    }
                                    return;
                                }
                            }
                            throw new Exception("Тип данных для преобразования не определен.");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "Ошибка преобразования данных", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    });
                    thread.Start();
                });
            }
        }
        public ICommand OpenSettings
        {
            get
            {
                return openSettings ??= new RelayCommand(async obj =>
                {
                    if (dialogService.OpenFileDialog("CryptoTable files|*.xml") == true)
                    {
                        FileStream stream = new(dialogService.FilePath, FileMode.Open, FileAccess.Read);
                        try
                        {
                            var data = (await mediatR.Send(new OpenXmlREQUEST(stream, "settings"))).Pairs;

                            foreach(var el in data)
                            {
                                switch (el.Key)
                                {
                                    case nameof(Controls.ComplementarySymbol):
                                        Controls.ComplementarySymbol = (string)el.Value;
                                        break;
                                    case nameof(Controls.StartKeyValue):
                                        Controls.StartKeyValue = (string)el.Value;
                                        break;
                                    case nameof(Controls.InitVec):
                                        Controls.InitVec = int.Parse((string)el.Value);
                                        break;
                                    case nameof(Controls.AlgModeSelectedItem):
                                        Controls.AlgModeSelectedItem = Controls.ListOfAlgMode
                                        .Where(elm => (string)elm.Value[3] == (string)el.Value)
                                        .Select(elm => elm)
                                        .Single();
                                        break;
                                    case nameof(Controls.AlgSelectedItem):
                                        Controls.AlgSelectedItem = Controls.ListOfAlg
                                        .Where(elm => (string)elm.Value[3] == (string)el.Value)
                                        .Select(elm => elm)
                                        .Single();
                                        break;
                                    case nameof(Controls.OrientationSTR):
                                        if ((string)el.Value == "ENCODE") { Controls.OrientationSTR = "Зашифровать"; }
                                        if ((string)el.Value == "DECODE") { Controls.OrientationSTR = "Расшифровать"; }
                                        break;
                                }
                            }
                            Controls.StatusBar = $"Файл {dialogService.FileName} успешно открыт.";
                            stream.Close();
                        }
                        catch(Exception ex)
                        {
                            stream.Close();
                            Controls.StatusBar = $"Ошибка при открытии файла {dialogService.FileName}. Файл не был открыт";
                            MessageBox.Show(ex.Message, "Ошибка загрузки файла настроек", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                });
            }
        }
        public ICommand SaveSettings
        {
            get
            {
                return saveSettings ??= new RelayCommand(async obj =>
                {
                    if (dialogService.SaveFileDialog("CryptoTable files|*.xml") == true)
                    {
                        FileStream stream = new FileStream(dialogService.FilePath, FileMode.Create, FileAccess.Write);
                        try
                        {
                            #region Определить параметры процесса
                            var mode = Enum.Parse(typeof(Mode), (string)Controls.AlgModeSelectedItem.Value[3]);
                            var alg = Enum.Parse(typeof(Alg), (string)Controls.AlgSelectedItem.Value[3]);
                            Orientation orientation;
                            if (Controls.OrientationSTR == "Зашифровать")
                            {
                                orientation = Orientation.ENCODE;

                            }
                            else if (Controls.OrientationSTR == "Расшифровать")
                            {
                                orientation = Orientation.DECODE;
                            }
                            else { throw new Exception("Ошибка при определении направления {CryptoTransformREQUEST<string>.Orientation}"); }
                            #endregion

                            await mediatR.Send(new SaveXmlSettingFileREQUEST(
                                        Controls.ComplementarySymbol,
                                        Controls.StartKeyValue,
                                        Controls.InitVec,
                                        (Mode)mode,
                                        (Alg)alg,
                                        orientation,
                                        stream));
                            Controls.StatusBar = $"Файл {dialogService.FileName} успешно сохранен.";
                            stream.Close();
                        }
                        catch (Exception ex)
                        {
                            stream.Close();
                            Controls.StatusBar = $"Ошибка при сохранении файла {dialogService.FileName}! Файл не был сохранен.";
                            MessageBox.Show(ex.Message, "Ошибка при сохранении файла настроек", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                });
            }
        }

        private RelayCommand? creatProjectTable;
        private RelayCommand? creatProjectText;
        public ICommand CreatProjectTable
        {
            get
            {
                return creatProjectTable ??= new RelayCommand(obj =>
                {
                    this.dialogService.SetFileType(DialogService.FileType.NAN);
                    ObservableCollection<WindowControlsMain.RowTwoMtrx> rows = new();
                    var colums = (ICollection<System.Windows.Controls.DataGridColumn>)obj;
                    for(int i = 0; i < 20; i++)
                    {
                        rows.Add(new() { Value = new object[20] });

                        var column = new System.Windows.Controls.DataGridTextColumn()
                        {
                            IsReadOnly = false,
                            Header = $"Столбец {i + 1}",
                            Binding = new Binding()
                            {
                                Path = new PropertyPath($"Value[{i}]"),
                                Mode = BindingMode.TwoWay,
                                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                            }
                        };
                        colums.Add(column);
                    }
                    Controls.InputData = rows;
                    Controls.VisibilityDataGrid = Visibility.Visible;
                    Controls.VisibilityTextBlock = Visibility.Collapsed;
                    Controls.VisibilityImage = Visibility.Collapsed;
                    Controls.VisibilityProjectTable = Visibility.Visible;
                    Controls.VisibilityProjectText = Visibility.Collapsed;
                });
            }
        }
        public ICommand CreatProjectText
        {
            get
            {
                return creatProjectText ??= new RelayCommand(obj =>
                {
                    this.dialogService.SetFileType(DialogService.FileType.NAN);
                    Controls.InputData = string.Empty;
                    Controls.VisibilityProjectText = Visibility.Visible;
                    Controls.VisibilityProjectTable = Visibility.Collapsed;
                    Controls.VisibilityTextBlock = Visibility.Collapsed;
                    Controls.VisibilityDataGrid = Visibility.Collapsed;
                    Controls.VisibilityImage = Visibility.Collapsed;
                });
            }
        }
    }
}