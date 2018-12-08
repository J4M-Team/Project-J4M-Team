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
    public class BookViewModel : BaseViewModel
    {
        #region data binding

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
       
        public ICommand AccountCommand { get; set; }//Hiển thị màn hình thông tin tài khoản
        public ICommand EditBookCommand { get; set; }//Hiển thị màn hình thêm sách
        public ICommand SearchBookCommand { get; set; }//Hiển thị màn hình tìm kiếm sách
        public ICommand AccepBillCommand { get; set; }//Hiển thị màn hình duyệt hóa đơn
        public ICommand ChartCommand { get; set; } //Hiển thị màn hình biểu đồ
        public ICommand ExitCommand { get; set; }//Nút thoát

        public ICommand loadCommand { get; set; }

        public BookViewModel()
        {
          
            loadCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                if (DataTransfer.Employee_Id > 0)
                {
                    EmployeeName = CEmployee.Ins.EmployeeInfo(DataTransfer.Employee_Id).Name;
                }         
            }
               );

            ExitCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                GridCursorMargin = new Thickness(0, 97 + 52 * 5, 0, 0);
                          
            }
               );

            AccountCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                GridCursorMargin = new Thickness(0, 97 + 52 * 4, 0, 0);
                FramePage = new AccountPage();
            }
               );

            ChartCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                GridCursorMargin = new Thickness(0, 97 + 52 * 2, 0, 0);               
            }
               );

            EditBookCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                GridCursorMargin = new Thickness(0, 97 + 52 * 1, 0, 0);
                FramePage = new EditBookPage();
            }
               );

            SearchBookCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                GridCursorMargin = new Thickness(0, 97, 0, 0);
                FramePage = new SearchPage();
            }
              );

            AccepBillCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                GridCursorMargin = new Thickness(0, 97 + 52 * 3, 0, 0);
                FramePage = new AccepBillPage();
            }
               );
        }
    }
}
