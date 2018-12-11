using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;
using BookStore.Model.MyClass;

namespace BookStore.ViewModel
{
    public class ManagementWindowVM : BaseViewModel
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

        public ICommand EmployeeCommand { get; set; }
        public ICommand SettingCommand { get; set; }
        public ICommand CustomerCommand { get; set; }
        public ICommand ReportCommand { get; set; }
        public ICommand AccountCommand { get; set; }
        public ICommand ExitCommand { get; set; }
        public ICommand loadCommand { get; set; }
        #endregion


        public ManagementWindowVM()
        {
            loadCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                if (DataTransfer.Employee_Id > 0)
                {
                    EmployeeName = CEmployee.Ins.EmployeeInfo(DataTransfer.Employee_Id).Name;
                }
            }
               );

            EmployeeCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                GridCursorMargin = new Thickness(0, 97, 0, 0);
                FramePage = new ManagementEmployee();
            });

            CustomerCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                GridCursorMargin = new Thickness(0, 97 + 52 * 1, 0, 0);

            });

            ReportCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                GridCursorMargin = new Thickness(0, 97 + 52 * 2, 0, 0);

            });

            AccountCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                GridCursorMargin = new Thickness(0, 97 + 52 * 4, 0, 0);

            });

            ExitCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                GridCursorMargin = new Thickness(0, 97 + 52 * 5, 0, 0);

            });

            SettingCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                GridCursorMargin = new Thickness(0, 97 + 52 * 3, 0, 0);
                FramePage = new SettingPage();
            });
        } 
    }
}
