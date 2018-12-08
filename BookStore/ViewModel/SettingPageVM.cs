using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using System.Windows.Controls;

namespace BookStore.ViewModel
{
    public class SettingPageVM:BaseViewModel
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

        private Thickness _GridCursorMargin;
        public Thickness GridCursorMargin
        {
            get { return _GridCursorMargin; }
            set
            {
                _GridCursorMargin = value;
                OnPropertyChanged(nameof(GridCursorMargin));
            }
        }

        #endregion

        #region command binding

        public ICommand PriceCommand { set; get; }
        public ICommand EmployeeCommand { set; get; }
        public ICommand loadCommand { get; set; }
        public ICommand PromotionCommand { get; set; }

        #endregion

        public SettingPageVM()
        {
            PriceCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {   
                //Thay đổi vị trí của thanh ngang
                if(GridCursorMargin != new Thickness(10, 0, 0, 0))
                {
                    GridCursorMargin = new Thickness(10, 0, 0, 0);
                    FramePage = new SettingBookPage();
                }
                     
            }
               );

            EmployeeCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                //Thay đổi vị trí của thanh ngang
                GridCursorMargin = new Thickness(10+170, 0, 0, 0);
                FramePage = new SettingEmployeePage();
            }
               );

            PromotionCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                //Thay đổi vị trí của thanh ngang
                GridCursorMargin = new Thickness(10 + 170 + 170, 0, 0, 0);
                FramePage = new SettingPromotionBookpage();
            }
               );

            loadCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                //Thay đổi vị trí của thanh ngang
                GridCursorMargin = new Thickness(10, 0, 0, 0);
                FramePage = new SettingBookPage();
            }
               );
        }
    }
}
