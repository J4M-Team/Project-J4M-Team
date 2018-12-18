using BookStore.Model.MyClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BookStore.ViewModel
{
    public class SettingInfoPageVM:BaseViewModel
    {
        #region data binding

        private CEmployee _Employee;
        public CEmployee Employee
        {
            get { return _Employee; }
            set
            {
                _Employee = value;
                OnPropertyChanged(nameof(Employee));
            }
        }

        #endregion

        #region command binding

        public ICommand loadCommand { get; set; }

        #endregion

        #region method

        public SettingInfoPageVM()
        {
            loadCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                if (DataTransfer.Employee_Id > 0)
                {
                    Employee = CEmployee.Ins.EmployeeInfo(DataTransfer.Employee_Id);
                }
            }
               );
        }

        #endregion
    }
}
