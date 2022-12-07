using ReactiveUI;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;

namespace ViewModels.Models
{
    public class WindowControlsMain : ReactiveObject
    {
        public class RowTwoMtrx : ReactiveObject
        {
            private object[] value;
            public object[] Value
            {
                get => this.value;
                set => this.RaiseAndSetIfChanged(ref this.value, value);
            }
        }
        public class RowThreeMtrx : ReactiveObject
        {
            private IEnumerable<RowTwoMtrx> value;
            public IEnumerable<RowTwoMtrx> Value
            {
                get => this.value;
                set => this.RaiseAndSetIfChanged(ref this.value, value);
            }
        }
        public static void CreatRowTwoMtrx(ObservableCollection<RowTwoMtrx> rowTables, IEnumerable<object[]> data)
        {
            rowTables.Clear();
            foreach (var el in data)
            {
                rowTables.Add(new RowTwoMtrx() { Value = el });
            }
        }
        public static void CreatRowThreeMtrx(ObservableCollection<RowThreeMtrx> rowTables, IEnumerable<IEnumerable<RowTwoMtrx>> data)
        {
            rowTables.Clear();
            foreach (var el in data)
            {
                rowTables.Add(new RowThreeMtrx() { Value = el });
            }
        }
        public static DataTable CreatDataTableRowTwoMtrx(ObservableCollection<RowTwoMtrx> rowTables, int countColumn)
        {
            DataTable table = new() { TableName = "CryptoTable" };
            for(int i = 0; i < countColumn; i++)
            {
                table.Columns.Add($"Столбец {i + 1}");
            }
            foreach(var el in rowTables)
            {
                table.Rows.Add(el.Value);
            }
            return table;
        }
    }
}
