using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ViewModels.ViewModels
{
    public class MainUserMenu_ViewModel
    {
        private readonly Action[] windows;
        public MainUserMenu_ViewModel(Action[] windows)
        {
            this.windows = windows;
        }

        private RelayCommand? openWindow_ComparisonBlockAlg;
        private RelayCommand? openWindow_CryptoTransformData;

        public ICommand OpenWindow_ComparisonBlockAlg
        {
            get
            {
                return openWindow_ComparisonBlockAlg ??= new RelayCommand(obj =>
                {
                    windows[0].Invoke();
                });
            }
        }
        public ICommand OpenWindow_CryptoTransformData
        {
            get
            {
                return openWindow_CryptoTransformData ??= new RelayCommand(obj =>
                {
                    windows[1].Invoke();
                });
            }
        }
    }
}
