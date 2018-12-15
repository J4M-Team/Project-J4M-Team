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
    public class CustomerMenuPageVM:BaseViewModel
    {
        #region data binding

        //Biến lưu framepage
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

        public ICommand InfoCustomerCommand { get; set; }
        public ICommand HistoryCommand { get; set; }

        #endregion

        #region method

        public CustomerMenuPageVM()
        {
            InfoCustomerCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                GridCursorMargin = new Thickness(10, 0, 0, 0);

                FramePage = new CustomerInfoPage();
            }
              );

            HistoryCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                //Thay đổi vị trí của thanh ngang
                GridCursorMargin = new Thickness(10 + 170, 0, 0, 0);

                FramePage = new CustomerHistoryPage();
            }
              );
        }

        #endregion

    }
}
