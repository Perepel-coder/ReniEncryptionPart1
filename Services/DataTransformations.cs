using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Media.Imaging;

namespace Services
{
    public class DataTransformations
    {
        public FileStream? Stream { get; set; }

        public void SaveFileCTF(IEnumerable<byte>? data)
        {
            try
            {
                if (this.Stream == null) { throw new Exception("Поток данных не существует."); }
                if (data == null) { throw new Exception("Массив данных пуст"); }
                byte[] array = data.Select(el => (byte)(el ^ 0xad)).ToArray();
                Stream.Write(array, 0, array.Length);
            }
            catch{}
        }
        public void SaveFile(IEnumerable<byte>? data)
        {
            try
            {
                if (this.Stream == null) { throw new Exception("Поток данных не существует."); }
                if (data == null) { throw new Exception("Массив данных пуст"); }
                byte[] array = data.ToArray();
                Stream.Write(array, 0, array.Length);
            }
            catch { }
        }
        public void SaveExcelFile(DataTable table)
        {
            using var excelEngine = new ExcelEngine();
            IApplication application = excelEngine.Excel;
            application.DefaultVersion = ExcelVersion.Excel2013;
            IWorkbook workbook = application.Workbooks.Create(1);
            IWorksheet worksheet = workbook.Worksheets[0];
            worksheet.ImportDataTable(table, true, 1, 1);
            workbook.SaveAs(this.Stream);
        }

        public static IEnumerable<byte> GetToByte(DataTable table)
        {
            using MemoryStream ms = new();
            table.WriteXml(ms);
            return ms.ToArray();
        }
        public static IEnumerable<byte> GetToByte(string data)
        {
            List<byte> result = new();
            foreach (char el in data) { result.Add((byte)el); }
            return result;
        }

        public DataTable OpenDataTableFile()
        {
            using ExcelEngine excelEngine = new();
            var myTable = new List<object[]>();
            IApplication application = excelEngine.Excel;
            application.DefaultVersion = ExcelVersion.Excel2013;
            IWorkbook workbook = application.Workbooks.Open(this.Stream, ExcelOpenType.Automatic);
            IWorksheet worksheet = workbook.Worksheets[0];
            DataTable table = worksheet.ExportDataTable(worksheet.UsedRange, ExcelExportDataTableOptions.ColumnNames);
            for (int i = 0; i < table.Columns.Count; i++)
            {
                while (table.Columns[i].ColumnName.IndexOf("/") != -1)
                {
                    table.Columns[i].ColumnName = Convert.ToString(table.Columns[i].ColumnName.Replace("/", "÷"));
                }
            }
            return table;
        }
        public static DataTable? GetDataTable(IEnumerable<byte> data)
        {
            try
            {
                DataSet ds = new DataSet();
                using MemoryStream ms = new(data.ToArray());
                ds.ReadXml(ms);
                return ds.Tables[0];
            }
            catch
            {
                throw new Exception(
                    "Файл формата .ctf поврежден. " +
                    "Чтение данных не возможно.\n" +
                    "Попробуйте поменять теущие настройки преобразования.");
            }
        }  

        public BitmapImage OpenImageFile()
        {
            if (Stream == null) { throw new Exception("Поток данных не существует."); }
            byte[] data = new byte[Stream.Length];

            BitmapImage image = new();
            using (var mem = new MemoryStream(data))
            {
                mem.Position = 0;
                image.BeginInit();
                image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.UriSource = null;
                image.StreamSource = mem;
                image.EndInit();
            }
            image.Freeze();
            return image;
        }
        public static BitmapImage GetImage(IEnumerable<byte> data)
        {
            BitmapImage image = new();
            using (var mem = new MemoryStream(data.ToArray()))
            {
                mem.Position = 0;
                image.BeginInit();
                image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.UriSource = null;
                image.StreamSource = mem;
                image.EndInit();
            }
            image.Freeze();
            return image;
        }

        public string OpenStringFile()
        {
            if (Stream == null) { throw new Exception("Поток данных не существует."); }
            byte[] data = new byte[Stream.Length];
            Stream.Read(data, 0, data.Length);
            string result = string.Empty;
            if (Path.GetExtension(Stream.Name) == ".ctf")
            {
                foreach (var el in data)
                {
                    result += (char)(el ^ 0xad);
                }
            }
            else
            { 
                foreach (var el in data)
                {
                    result += (char)el;
                }
            }
            return result;
        }
        public static string GetString(IEnumerable<byte> data)
        {
            string result = string.Empty;
            foreach (var el in data)
            {
                result += (char)el;
            }
            return result;
        }

        public static void GetSound()
        {

        }
    }
}
