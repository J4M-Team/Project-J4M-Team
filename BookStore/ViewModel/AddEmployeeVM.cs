﻿using BookStore.Model;
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
        private ObservableCollection<string> _ListRoleName;
        public ObservableCollection<string> ListRoleName
        {
            get { return _ListRoleName; }
            set
            {
                _ListRoleName = value;
                OnPropertyChanged(nameof(ListRoleName));
            }
        }

        private string _ComboBoxSelectedItem;
        public string ComboBoxSelectedItem
        {
            get { return _ComboBoxSelectedItem; }
            set
            {
                _ComboBoxSelectedItem = value;
                OnPropertyChanged(nameof(ComboBoxSelectedItem));
            }
        }


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

        private DateTime _EmployeeBirthday;
        public DateTime EmployeeBirthday
        { 
            get { return _EmployeeBirthday; }
            set
            {
                _EmployeeBirthday = value;
                OnPropertyChanged(nameof(EmployeeBirthday));
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

        public ICommand loadCommand { get; set; }
        public ICommand AddEmployeeCommand { get; set; }


        #endregion

        #region method

        public AddEmployeeVM()
        {
            loadCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                //Thêm data vào combobox
                ListRoleName = new ObservableCollection<string>(CRole.Ins.ListRoleName());
            }
                );
            AddEmployeeCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                if (string.IsNullOrEmpty(EmployeeName) || string.IsNullOrEmpty(EmployeeAddress) || string.IsNullOrEmpty(EmployeeEmail) || string.IsNullOrEmpty(EmployeePhone) || string.IsNullOrEmpty(EmployeeIdentity))
                {
                    return;
                }
                else
                {                 
                    //Tạo mới CRole
                    CRole role = new CRole
                    {
                        Name = ComboBoxSelectedItem
                    };
                    CEmployee Employee = new CEmployee
                   
                    { Name = EmployeeName,
                      Address = EmployeeAddress,
                      Email = EmployeeEmail,
                      Phone = EmployeePhone,
                      BirthDay = EmployeeBirthday,
                      Identity = EmployeeIdentity,
                      Role = role
                    };

                    //Thêm vào cơ sở dữ liệu
                    CEmployee.Ins.AddEmployee(Employee);

                    MessageBox.Show("Thêm vào thành công ", "Thông báo !!");
                }

            }
                );
        }

        #endregion
    }
}

