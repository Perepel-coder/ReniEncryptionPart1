using Microsoft.Win32;
using System.IO;
using System.Windows;

namespace ViewModels.ViewModels
{
    internal class DialogService
    {
        public enum FileType { TEXT, EXCEL, CTF};
        public string FilePath { get; set; } = string.Empty;
        public string FileName{ get; set; } = string.Empty;
        public FileType fileType { get; private set; }
        public bool OpenFileDialog(string filter = "Txt files|*.txt|Excel Files|*.xls;*.xlsx;*.xlsm|CryptoTable files|*.ctf")
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = filter;
            if (openFileDialog.ShowDialog() == true)
            {
                string ext = Path.GetExtension(openFileDialog.FileName);
                if(ext == ".txt") { fileType = FileType.TEXT; }
                if(ext == ".ctf") { fileType = FileType.CTF; }
                if (ext == ".xls" || ext == ".xlsx" || ext == ".xlsm") { fileType = FileType.EXCEL; }
                this.FilePath = openFileDialog.FileName;
                this.FileName = Path.GetFileName(this.FilePath);
                return true;
            }
            return false;
        }
        public bool SaveFileDialog(string filter = "Txt files|*.txt|Excel Files|*.xls;*.xlsx;*.xlsm")
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = filter;
            if (saveFileDialog.ShowDialog() == true)
            {
                this.FilePath = saveFileDialog.FileName;
                this.FileName = Path.GetFileName(this.FilePath);
                return true;
            }
            return false;
        }
        public void ShowMessage(string message)
        {
            MessageBox.Show(message);
        }
    }
}
