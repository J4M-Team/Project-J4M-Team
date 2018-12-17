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
    class TableSalaryEmployee : BaseViewModel
    {
        #region data binding
        private ObservableCollection<CEmployee> _ListSalary;
        public ObservableCollection<CEmployee> ListSalary
        {
            get { return _ListSalary; }
            set
            {
                _ListSalary = value;
                OnPropertyChanged(nameof(ListSalary));
            }
        }
        #endregion

        #region properties binding

        #endregion

        #region command binding
        public ICommand loadCommand { get; set; }
        #endregion

        public TableSalaryEmployee()
        {
            loadCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                //lấy data từ cơ sở dữ liệu
               

            }
                );
        }
    }
}
