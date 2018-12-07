using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;

namespace BookStore.ViewModel
{
    public class ManagementWindowVM : BaseViewModel
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

        #region command binding

        public ICommand EmployeeCommand { get; set; }
        public ICommand SettingCommand { get; set; }

        #endregion


        public ManagementWindowVM()
        {
            EmployeeCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                FramePage = new ManagementEmployee();
            });
            SettingCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                FramePage = new SettingPage();
            });
        } 
    }
}
