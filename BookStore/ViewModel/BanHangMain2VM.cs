using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BookStore.ViewModel
{
    public class BanHangMain2VM:BaseViewModel
    {
        #region data binding

        private Page _FramePage;
        public Page FramePage
        {
            get
            {
                return _FramePage;
            }
            set
            {
                if (_FramePage == value)
                    return;
                _FramePage = value;
                OnPropertyChanged(nameof(FramePage));//Thêm nameof vào trước để khi rename thì sẽ thay đổi luôn cả biến không phải đổi tay khi để kiểu string "FramePage"
            }
        }


        #endregion

        #region properties binding


        private Thickness _GridCursorMargin;
        public Thickness GridCursorMargin
        {
            get
            {
                return _GridCursorMargin;
            }
            set
            {
                _GridCursorMargin = value;
                OnPropertyChanged(nameof(GridCursorMargin));
            }
        }

        #endregion

        #region command binding

        public ICommand DeleteCommand { get; set; }
        public ICommand PrintCommand { get; set; }
        public ICommand ExchangeCommand { get; set; }
        public ICommand AccountCommand { get; set; }
        public ICommand ExitCommand { get; set; }

        #endregion

        public BanHangMain2VM()
        {
            DeleteCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                GridCursorMargin = new Thickness(0, 97 + 52 * 1, 0, 0);
                FramePage = new DeleteBill();
            }
               );

            PrintCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                GridCursorMargin = new Thickness(0, 97, 0, 0);
                FramePage = new InHoaDonPage();
            }
               );

            ExchangeCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                GridCursorMargin = new Thickness(0, 97 + 52 * 2, 0, 0);
            }
               );

            AccountCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                GridCursorMargin = new Thickness(0, 97 + 52 * 3, 0, 0);
            }
               );

            ExitCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                GridCursorMargin = new Thickness(0, 97 + 52 * 4, 0, 0);
            }
               );
        }
    }
}
