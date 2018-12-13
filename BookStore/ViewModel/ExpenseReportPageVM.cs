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
    public class ExpenseReportPageVM:BaseViewModel
    {
        #region data binding

        private ObservableCollection<CReport> _ListReport;
        public ObservableCollection<CReport> ListReport
        {
            get { return _ListReport; }
            set
            {
                _ListReport = value;
                OnPropertyChanged(nameof(ListReport));
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

        private string _SelectedItemYear;
        public string SelectedItemYear
        {
            get { return _SelectedItemYear; }
            set { _SelectedItemYear = value; OnPropertyChanged(nameof(SelectedItemYear)); }
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
        public ICommand SelectionChangedYear { get; set; }

        #endregion

        #region method

        public ExpenseReportPageVM()
        {
            loadCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                ListYear = new ObservableCollection<string> { "Năm 2015", "Năm 2016", "Năm 2017", "Năm 2018", "Năm 2019", "Năm 2020", "Năm 2021", "Năm 2022" };

                TextYear = "Năm " + DateTime.Now.Year.ToString();

                ListReport = new ObservableCollection<CReport>(CReport.Ins.ListReportAllMonth(Help.StringMonthToInt(TextYear)));
            }
              );

            SelectionChangedYear = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                if (SelectedItemYear != null)
                {
                    ListReport = new ObservableCollection<CReport>(CReport.Ins.ListReportAllMonth(Help.StringMonthToInt(SelectedItemYear)));
                }               
            }
              );
        }

        #endregion
    }
}
