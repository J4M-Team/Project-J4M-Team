using BookStore.Model.MyClass;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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
     
        private ObservableCollection<CBook> _ListBook;
        public ObservableCollection<CBook> ListBook
        {
            get { return _ListBook; }
            set
            {
                _ListBook = value;
                OnPropertyChanged(nameof(ListBook));
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



        private DateTime _Date;
        public DateTime Date
        {
            get { return _Date; }
            set
            {
                _Date = value;
                OnPropertyChanged(nameof(Date));
            }
        }

        private string _CustomerName;
        public string CustomerName
        {
            get { return _CustomerName; }
            set
            {
                _CustomerName = value;
                OnPropertyChanged(nameof(CustomerName));
            }
        }

        #endregion

        #region command binding

        public ICommand loadCommand { get; set; }
        public ICommand SearchTextChangeCommand { get; set; }
        public ICommand SelectionChangedCommand { get; set; }
        public ICommand HistorySelectionChangedCommand { get; set; }
       

        #endregion

        #region properties command

        private Visibility _CustomerInfoVisibility;
        public Visibility CustomerInfoVisibility
        {
            get { return _CustomerInfoVisibility; }
            set
            {
                _CustomerInfoVisibility = value;
                OnPropertyChanged(nameof(CustomerInfoVisibility));
            }
        }

        private Visibility _BillInfoVisibility;
        public Visibility BillInfoVisibility
        {
            get { return _BillInfoVisibility; }
            set
            {
                _BillInfoVisibility = value;
                OnPropertyChanged(nameof(BillInfoVisibility));
            }
        }

        private Visibility _CardVisibility;
        public Visibility CardVisibility
        {
            get { return _CardVisibility; }
            set
            {
                _CardVisibility = value;
                OnPropertyChanged(nameof(CardVisibility));
            }
        }

        #endregion

        #region method

        public CustomerHistoryPageVM()
        {
            loadCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                CustomerInfoVisibility = Visibility.Collapsed;
                BillInfoVisibility = Visibility.Visible;
                CardVisibility = Visibility.Visible;

                DataListCustomer = new ObservableCollection<CCustomer>(CCustomer.Ins.TransactionHistory().OrderByDescending(x => x.NumberBook));

                ListCustomer = DataListCustomer;
            }
               );

            SearchTextChangeCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                CustomerInfoVisibility = Visibility.Collapsed;
                BillInfoVisibility = Visibility.Visible;
                CardVisibility = Visibility.Visible;

                Search();
            }
               );

            SelectionChangedCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                if (SelectedItem != null)
                {
                    CustomerInfoVisibility = Visibility.Collapsed;
                    BillInfoVisibility = Visibility.Visible;
                    CardVisibility = Visibility.Visible;

                    CustomerHistory = new ObservableCollection<CReport>(CCustomer.Ins.CustomerHistory(SelectedItem.Id).OrderByDescending(x => x.Date));

                    ListBook = new ObservableCollection<CBook>();
                }
                
            }
               );

            HistorySelectionChangedCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                if (HistorySelectedItem != null && SelectedItem != null)
                {                   
                    Date = HistorySelectedItem.Date;
                    CustomerName = SelectedItem.Name;

                    CustomerInfoVisibility = Visibility.Visible;
                    BillInfoVisibility = Visibility.Visible;
                    CardVisibility = Visibility.Visible;

                    ListBook = new ObservableCollection<CBook>(CCustomer.Ins.CustomerDateHistory(HistorySelectedItem.Date, SelectedItem.Id));                 
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
