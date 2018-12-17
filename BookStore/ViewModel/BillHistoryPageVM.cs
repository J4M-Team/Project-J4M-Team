using BookStore.Model.MyClass;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BookStore.ViewModel
{
    public class BillHistoryPageVM:BaseViewModel
    {
        #region data binding
        /// <summary>
        /// Lưu List hiển thị lên List
        /// </summary>
        private ObservableCollection<CBill> _ListBill;
        public ObservableCollection<CBill> ListBill
        {
            get { return _ListBill; }
            set
            {
                _ListBill = value;
                OnPropertyChanged(nameof(ListBill));
            }
        }

        #endregion

        #region command binding

        public ICommand loadCommand { get; set; }

        #endregion

        #region method

        public BillHistoryPageVM()
        {
            loadCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                ListBill = new ObservableCollection<CBill>(CBill.Ins.ListAllBill());
                
            }
               );
        }

        #endregion
    }
}
