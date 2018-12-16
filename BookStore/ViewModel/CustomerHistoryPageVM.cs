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
    public class CustomerHistoryPageVM : BaseViewModel
    {
        #region data binding

        private ObservableCollection<CCustomer> _ListCustomer;
        public ObservableCollection<CCustomer> ListCustomer
        {
            get { return _ListCustomer; }
            set
            {
                _ListCustomer = value;
                OnPropertyChanged(nameof(ListCustomer));
            }
        }

        private ObservableCollection<CCustomer> _DataListCustomer;
        public ObservableCollection<CCustomer> DataListCustomer
        {
            get { return _DataListCustomer; }
            set
            {
                _DataListCustomer = value;
                OnPropertyChanged(nameof(DataListCustomer));
            }
        }

        private ObservableCollection<CReport> _CustomerHistory;
        public ObservableCollection<CReport> CustomerHistory
        {
            get { return _CustomerHistory; }
            set
            {
                _CustomerHistory = value;
                OnPropertyChanged(nameof(CustomerHistory));
            }
        }

        private string _FilterString;
        public string FilterString
        {
            get { return _FilterString; }
            set
            {
                _FilterString = value;
                OnPropertyChanged(nameof(FilterString));
            }
        }

        private CCustomer _SelectedItem;
        public CCustomer SelectedItem
        {
            get { return _SelectedItem; }
            set
            {
                _SelectedItem = value;
                OnPropertyChanged(nameof(SelectedItem));
            }
        }


        private CReport _HistorySelectedItem;
        public CReport HistorySelectedItem
        {
            get { return _HistorySelectedItem; }
            set
            {
                _HistorySelectedItem = value;
                OnPropertyChanged(nameof(HistorySelectedItem));
            }
        }

        #endregion

        #region command binding

        public ICommand loadCommand { get; set; }
        public ICommand SearchTextChangeCommand { get; set; }
        public ICommand SelectionChangedCommand { get; set; }
        public ICommand HistorySelectionChangedCommand { get; set; }

        #endregion

        #region method

        public CustomerHistoryPageVM()
        {
            loadCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {

                DataListCustomer = new ObservableCollection<CCustomer>(CCustomer.Ins.TransactionHistory().OrderByDescending(x => x.NumberBook));

                ListCustomer = DataListCustomer;
            }
               );

            SearchTextChangeCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                Search();
            }
               );

            SelectionChangedCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                if (SelectedItem != null)
                {
                    CustomerHistory = new ObservableCollection<CReport>(CCustomer.Ins.CustomerHistory(SelectedItem.Id));
                }
                
            }
               );

            HistorySelectionChangedCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                if (HistorySelectedItem != null)
                {
                    //CustomerHistory = new ObservableCollection<CReport>(CCustomer.Ins.CustomerHistory(SelectedItem.Id));
                    System.Windows.MessageBox.Show(HistorySelectedItem.Date.ToLongDateString());
                }

            }
               );
        }

        /// <summary>
        /// Hàm tìm kiếm sách theo mã hoặc theo tên
        /// </summary>
        private void Search()
        {
            if (DataListCustomer.Count > 0)
            {

                if (string.IsNullOrEmpty(FilterString))
                {
                    ListCustomer = DataListCustomer;
                }
                else
                {
                    int Id;
                    //Tìm theo ID
                    if (int.TryParse(FilterString, out Id) == true)
                    {
                        var data = DataListCustomer.Where(x => x.Id == Id).Select(x => x);
                        if (data.Count() > 0)
                        {
                            //Tạo list với kết quả trả về là Id
                            ListCustomer = new ObservableCollection<CCustomer>(data);
                        }
                        else
                        {
                            //Tạo list rỗng
                            ListCustomer = new ObservableCollection<CCustomer>();
                        }
                    }
                    //Tìm theo tên sách
                    else
                    {
                        var data = DataListCustomer.Where(x => x.Name.ToLower().Contains(FilterString.ToLower()) == true).Select(x => x);
                        if (data.Count() > 0)
                        {
                            //Tạo list với kết quả trả về là tên sách
                            ListCustomer = new ObservableCollection<CCustomer>(data);
                        }
                        else
                        {
                            //Tạo list rỗng
                            ListCustomer = new ObservableCollection<CCustomer>();
                        }
                    }
                }
            }
            else
            {
                ListCustomer = new ObservableCollection<CCustomer>();
            }

        }

        #endregion
    }
}
