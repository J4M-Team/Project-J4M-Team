using BookStore.Model.MyClass;
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

        //Lưu tên của nhân viên
        private string _EmployeeName;
        public string EmployeeName
        {
            get { return _EmployeeName; }
            set
            {
                _EmployeeName = value;
                OnPropertyChanged(nameof(EmployeeName));
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
        public ICommand AccountCommand { get; set; }
        public ICommand ExitCommand { get; set; }
        public ICommand loadCommand { get; set; }
        public ICommand SearchBookCommand { get; set; }//Hiển thị màn hình tìm kiếm sách

        #endregion

        public BanHangMain2VM()
        {
            loadCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                if (DataTransfer.Employee_Id > 0)
                {
                    EmployeeName = CEmployee.Ins.EmployeeInfo(DataTransfer.Employee_Id).Name;
                }
            }
               );

            SearchBookCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                GridCursorMargin = new Thickness(0, 97, 0, 0);
                FramePage = new SearchPage();
            }
              );

            DeleteCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                GridCursorMargin = new Thickness(0, 97 + 52 * 2, 0, 0);
                FramePage = new DeleteBill();
            }
               );

            PrintCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                GridCursorMargin = new Thickness(0, 97 + 52 * 1, 0, 0);
                FramePage = new InHoaDonPage();
            }
               );
            
            AccountCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                GridCursorMargin = new Thickness(0, 97 + 52 * 3, 0, 0);
                FramePage = new SettingInfoPage();
            }
               );

            ExitCommand = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                GridCursorMargin = new Thickness(0, 97 + 52 * 4, 0, 0);
                p.Close();
            }
               );
        }
    }
}
