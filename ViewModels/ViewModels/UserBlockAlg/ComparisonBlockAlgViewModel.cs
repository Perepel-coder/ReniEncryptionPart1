using MediatR;
using QueryCQRS.Queries;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls.DataVisualization.Charting;
using System.Windows.Data;
using System.Windows.Input;
using ViewModels.Models;

namespace ViewModels.ViewModels.UserBlockAlg
{
    public class ComparisonBlockAlgViewModel
    {
        private readonly IMediator mediatR;
        public ComparisonBlockAlgControls Controls { get; set; }

        public ComparisonBlockAlgViewModel(IMediator mediatR)
        {
            this.mediatR = mediatR;
            Controls = new();
            Controls.CountOfData = 500;
            Controls.CountOfCycles = 50;
            Controls.CountOfKey = 10;
            Controls.OrientationSTR = "Зашифровать";
            CreatListOfAlgAndModeOfAlg();
        }
        private async void CreatListOfAlgAndModeOfAlg()
        {
            var data = (await mediatR.Send(
                new ReqForCriptoAlgDB_REQUEST(ReqForCriptoAlgDB_REQUEST.Mode.GET_LIST_OF_ALG)
                )).data.ToList();
            if (data.Count >= 2) { data[0][0] = true; data[1][0] = true; }
            WindowControlsMain.CreatRowTwoMtrx(Controls.ListOfAlg, data);
            data = (await mediatR.Send(
                          new ReqForCriptoAlgDB_REQUEST(
                              ReqForCriptoAlgDB_REQUEST.Mode.GET_MODE_OF_ALG,
                              Controls.ListOfAlg.Select(el => el.Value))
                          )).data.ToList();
            if (data.Count != 0) { data[0][0] = true; }
            WindowControlsMain.CreatRowTwoMtrx(Controls.ListOfAlgMode, data);
        }

        private RelayCommand? comparisonBlockAlg;
        public ICommand ComparisonBlockAlg
        {
            get
            {
                return comparisonBlockAlg ??= new RelayCommand(obj =>
                {
                    Controls.VisibilityProgressBar = Visibility.Visible;
                    Controls.IsEnabledButtonTransform = false;
                    List<IEnumerable<object[]>> data = new();
                    Thread thread = new(async () =>
                    {
                        #region Формирование входных данных для анализа быстродействия
                        List<Alg> algs = new();
                        var choiceAlgs = Controls.ListOfAlg.Where(el => (bool)el.Value[0] == true).Select(el => new string[2]{ (string)el.Value[2], (string)el.Value[3]}).ToList();
                        foreach (string[] el in choiceAlgs)
                        {
                            Alg alg = (Alg)Enum.Parse(typeof(Alg), el[1]);
                            algs.Add(alg);
                        }
                        string choiceMode = (string)Controls.ListOfAlgMode.Where(el => (bool)el.Value[0]).Select(el => el.Value[3]).Single();
                        Mode mode = (Mode)Enum.Parse(typeof(Mode), choiceMode);
                        Orientation orientation;
                        if (Controls.OrientationSTR == "Зашифровать")
                        {
                            orientation = Orientation.ENCODE;
                        }
                        else
                        {
                           orientation = Orientation.DECODE;
                        }
                        data = (await mediatR.Send(
                            new BenchmarkREQUEST(
                                algs, 
                                mode, 
                                orientation, 
                                Controls.CountOfData, 
                                Controls.CountOfCycles, 
                                Controls.CountOfKey))).Data.ToList();
                        #endregion

                        #region Формирование графиков
                        var chart = (Chart)obj;
                        ObservableCollection<IEnumerable<WindowControlsMain.RowTwoMtrx>> rows = new();
                        for (int i = 0; i < data.Count; i++)
                        {
                            ObservableCollection<WindowControlsMain.RowTwoMtrx> row = new();
                            WindowControlsMain.CreatRowTwoMtrx(row, data[i]);
                            rows.Add(row);
                        }
                        WindowControlsMain.CreatRowThreeMtrx(Controls.ChartSeries, rows);
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            chart.Series.Clear();
                            for (int i = 0; i < Controls.ChartSeries.Count; i++)
                            {
                                LineSeries lineSeries = new();
                                lineSeries.Title = $"{choiceAlgs[i][0]}";
                                lineSeries.ItemsSource = Controls.ChartSeries[i].Value;
                                lineSeries.IndependentValueBinding = new Binding
                                {
                                    Path = new PropertyPath($"Value[{0}]"),
                                    Mode = BindingMode.OneWay,
                                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                                };
                                lineSeries.DependentValueBinding = new Binding
                                {
                                    Path = new PropertyPath($"Value[{1}]"),
                                    Mode = BindingMode.OneWay,
                                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                                };
                                chart.Series.Add(lineSeries);
                            }
                        });
                        #endregion

                        Controls.VisibilityProgressBar = Visibility.Collapsed;
                        Controls.IsEnabledButtonTransform = true;
                    });
                    thread.Start();
                });
            }
        }
    }
}
