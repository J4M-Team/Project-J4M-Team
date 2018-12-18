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
    public class OutputHistoryPageVM:BaseViewModel
    {
        #region databinding

        private ObservableCollection<CBill> _History;
        public ObservableCollection<CBill> History
        {
            get { return _History; }
            set
            {
                _History = value;
                OnPropertyChanged(nameof(History));
            }
        }

        private ObservableCollection<CBill> _DataHistory;
        public ObservableCollection<CBill> DataHistory
        {
            get { return _DataHistory; }
            set
            {
                _DataHistory = value;
                OnPropertyChanged(nameof(DataHistory));
            }
        }

        private DateTime _MinDate;
        public DateTime MinDate
        {
            get { return _MinDate; }
            set
            {
                _MinDate = value;
                OnPropertyChanged(nameof(MinDate));
            }
        }

        private DateTime _MaxDate;
        public DateTime MaxDate
        {
            get { return _MaxDate; }
            set
            {
                _MaxDate = value;
                OnPropertyChanged(nameof(MaxDate));
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

        #endregion

        #region command binding

        public ICommand loadCommand { get; set; }
        public ICommand SearchTextChangeCommand { get; set; }
        public ICommand SelectedDateMinChanged { get; set; }
        public ICommand SelectedDateMaxChanged { get; set; }


        #endregion

        #region method

        public OutputHistoryPageVM()
        {
            loadCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                MinDate = CWarehouse.Ins_Warehouse.MinDateOutput();
                MaxDate = CWarehouse.Ins_Warehouse.MaxDateOutput();

                DataHistory = new ObservableCollection<CBill>(CWarehouse.Ins_Warehouse.OutPutHistory(FilterString, MinDate, MaxDate));
                History = DataHistory;

            }
               );
        }

        #endregion
    }
}
