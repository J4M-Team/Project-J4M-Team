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
    public class TopBookPageVM:BaseViewModel
    {
        #region data binding

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

        private ObservableCollection<CBook> _ListBookAll;
        public ObservableCollection<CBook> ListBookAll
        {
            get { return _ListBookAll; }
            set
            {
                _ListBookAll = value;
                OnPropertyChanged(nameof(ListBookAll));
            }
        }

        private ObservableCollection<string> _ListMonth;
        public ObservableCollection<string> ListMonth
        {
            get { return _ListMonth; }
            set
            {
                _ListMonth = value;
                OnPropertyChanged(nameof(ListMonth));
            }
        }

        private ObservableCollection<string> _ListYear;
        public ObservableCollection<string> ListYear
        {
            get { return _ListYear; }
            set
            {
                _ListYear = value;
                OnPropertyChanged(nameof(ListYear));
            }
        }

        private string _SelectedItemMonth;
        public string SelectedItemMonth
        {
            get { return _SelectedItemMonth; }
            set { _SelectedItemMonth = value; OnPropertyChanged(nameof(SelectedItemMonth)); }
        }

        private string _SelectedItemYear;
        public string SelectedItemYear
        {
            get { return _SelectedItemYear; }
            set { _SelectedItemYear = value; OnPropertyChanged(nameof(SelectedItemYear)); }
        }

        private string _TextMonth;
        public string TextMonth
        {
            get { return _TextMonth; }
            set { _TextMonth = value; OnPropertyChanged(nameof(TextMonth)); }
        }

        private string _TextYear;
        public string TextYear
        {
            get { return _TextYear; }
            set { _TextYear = value; OnPropertyChanged(nameof(TextYear)); }
        }

        #endregion

        #region command binding

        public ICommand loadCommand { get; set; }
        public ICommand SelectionChangedMonth { get; set; }
        public ICommand SelectionChangedYear { get; set; }

        #endregion

        #region method

        public TopBookPageVM()
        {
            loadCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                ListMonth = new ObservableCollection<string> { "Tháng 1", "Tháng 2", "Tháng 3", "Tháng 4", "Tháng 5",
                "Tháng 6","Tháng 7","Tháng 8","Tháng 9","Tháng 10","Tháng 11","Tháng 12"};

                ListYear = new ObservableCollection<string> { "Năm 2015", "Năm 2016", "Năm 2017", "Năm 2018", "Năm 2019", "Năm 2020", "Năm 2021", "Năm 2022" };

                TextYear = "Năm " + DateTime.Now.Year.ToString();
                TextMonth = "Tháng " + DateTime.Now.Month.ToString();

                ListBook = new ObservableCollection<CBook>(CBook.Ins.ListTopBook(Help.StringMonthToInt(TextYear), Help.StringMonthToInt(TextMonth), 10));

                ListBookAll = new ObservableCollection<CBook>(CBook.Ins.ListTopBookAll(10));
            }
              );

            SelectionChangedMonth = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                if (SelectedItemMonth != null)
                {
                    ListBook = new ObservableCollection<CBook>(CBook.Ins.ListTopBook(Help.StringMonthToInt(TextYear), Help.StringMonthToInt(SelectedItemMonth), 10));
                }              
            }
              );

            SelectionChangedYear = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                if (SelectedItemMonth != null)
                {
                    ListBook = new ObservableCollection<CBook>(CBook.Ins.ListTopBook(Help.StringMonthToInt(SelectedItemYear), Help.StringMonthToInt(TextMonth), 10));
                }
            }
             );
        }

        #endregion
    }
}
