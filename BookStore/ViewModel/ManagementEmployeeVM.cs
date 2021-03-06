﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using System.Windows.Controls;

namespace BookStore.ViewModel
{
    class ManagementEmployeeVM : BaseViewModel
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

        public ICommand loadCommand { get; set; }
        public ICommand InformationCommand { get; set; }
        public ICommand SalaryCommand { get; set; }
        public ICommand AccountCommand { get; set; }
        #endregion

        public ManagementEmployeeVM()
        {
            loadCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                //Thay đổi vị trí của thanh ngang
                GridCursorMargin = new Thickness(10, 0, 0, 0);

                FramePage = new ManagementEmployeePage();
            }
              );
            InformationCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                //Thay đổi vị trí của thanh ngang
              
                    GridCursorMargin = new Thickness(10, 0, 0, 0);
                    FramePage = new ManagementEmployeePage();
                

            }
              );
           SalaryCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                //Thay đổi vị trí của thanh ngang
                
                    GridCursorMargin = new Thickness(10 + 170, 0, 0, 0);
                    FramePage = new TableSalaryEmployeePage();
                

            }
             );
            AccountCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                //Thay đổi vị trí của thanh ngang

                GridCursorMargin = new Thickness(10 + 170 + 170, 0, 0, 0);
                FramePage = new ManagementAccountPage();


            }
             );

        }

    }
}

