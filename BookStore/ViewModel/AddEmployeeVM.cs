using BookStore.Model.MyClass;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BookStore.ViewModel
{
    class AddEmployeeVM : BaseViewModel
    {
        #region data binding



        #endregion

        #region properties binding


        //Thông tin nhân viên mới được thêm
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

        private string _EmployeeAddress;
        public string EmployeeAddress
        {
            get { return _EmployeeAddress; }
            set
            {
                _EmployeeAddress = value;
                OnPropertyChanged(nameof(EmployeeAddress));
            }
        }

        private string _EmployeeEmail;
        public string EmployeeEmail
        {
            get { return _EmployeeEmail; }
            set
            {
                _EmployeeEmail = value;
                OnPropertyChanged(nameof(EmployeeEmail));
            }
        }

        private string _EmployeePhone;
        public string EmployeePhone
        {
            get { return _EmployeePhone; }
            set
            {
                _EmployeePhone = value;
                OnPropertyChanged(nameof(EmployeePhone));
            }
        }


        private string _EmployeeIdentity;
        public string EmployeeIdentity
        {
            get { return _EmployeeIdentity; }
            set
            {
                _EmployeeIdentity = value;
                OnPropertyChanged(nameof(EmployeeIdentity));
            }
        }

        #endregion

        #region command binding

        public ICommand AddEmployeeCommand { get; set; }

        #endregion

        #region method

        public AddEmployeeVM()
        {
            AddEmployeeCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                if (string.IsNullOrEmpty(EmployeeName) || string.IsNullOrEmpty(EmployeeAddress) || string.IsNullOrEmpty(EmployeeEmail) || string.IsNullOrEmpty(EmployeePhone) || string.IsNullOrEmpty(EmployeeIdentity))
                {
                    return;
                }
                else
                {
                    //Tạo mới CRole
                    CEmployee Employee = new CEmployee { Name = EmployeeName, Address = EmployeeAddress, Email = EmployeeEmail, Phone = EmployeePhone, Identity = EmployeeIdentity };

                    //Thêm vào cơ sở dữ liệu
                    CEmployee.Ins.AddEmployee(Employee);



                }

            });
        }

        #endregion
    }
}

