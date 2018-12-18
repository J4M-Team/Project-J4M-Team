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

        private ObservableCollection<COutput_History> _History;
        public ObservableCollection<COutput_History> History
        {
            get { return _History; }
            set
            {
                _History = value;
                OnPropertyChanged(nameof(History));
            }
        }

        private ObservableCollection<COutput_History> _DataHistory;
        public ObservableCollection<COutput_History> DataHistory
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

                DataHistory = new ObservableCollection<COutput_History>(CWarehouse.Ins_Warehouse.OutPutHistory(FilterString, MinDate, MaxDate).OrderByDescending(x => x.Date));
                History = DataHistory;

            }
               );

            SelectedDateMinChanged = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                if (MinDate > MaxDate)
                {
                    MinDate = CWarehouse.Ins_Warehouse.MinDateOutput();
                    MaxDate = CWarehouse.Ins_Warehouse.MaxDateOutput();

                    DataHistory = new ObservableCollection<COutput_History>(CWarehouse.Ins_Warehouse.OutPutHistory(FilterString, MinDate, MaxDate).OrderByDescending(x => x.Date));
                    History = DataHistory;
                }

                DataHistory = new ObservableCollection<COutput_History>(CWarehouse.Ins_Warehouse.OutPutHistory(FilterString, MinDate, MaxDate).OrderByDescending(x => x.Date));
                History = DataHistory;

            }
               );

            SelectedDateMaxChanged = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                if (MinDate > MaxDate)
                {
                    MinDate = CWarehouse.Ins_Warehouse.MinDateOutput();
                    MaxDate = CWarehouse.Ins_Warehouse.MaxDateOutput();

                    DataHistory = new ObservableCollection<COutput_History>(CWarehouse.Ins_Warehouse.OutPutHistory(FilterString, MinDate, MaxDate).OrderByDescending(x => x.Date));
                    History = DataHistory;
                }

                DataHistory = new ObservableCollection<COutput_History>(CWarehouse.Ins_Warehouse.OutPutHistory(FilterString, MinDate, MaxDate).OrderByDescending(x => x.Date));
                History = DataHistory;

            }
               );

            SearchTextChangeCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                Search();
            }
               );
        }

        /// <summary>
        /// Hàm tìm kiếm sách theo mã hoặc theo tên
        /// </summary>
        private void Search()
        {
            if (DataHistory.Count > 0)
            {

                if (string.IsNullOrEmpty(FilterString))
                {
                    History = DataHistory;
                }
                else
                {
                    int Id;
                    //Tìm theo ID
                    if (int.TryParse(FilterString, out Id) == true)
                    {
                        var data = DataHistory.Where(x => x.WareHouse.Id == Id).Select(x => x);
                        if (data.Count() > 0)
                        {
                            //Tạo list với kết quả trả về là Id
                            History = new ObservableCollection<COutput_History>(data);
                        }
                        else
                        {
                            //Tạo list rỗng
                            History = new ObservableCollection<COutput_History>();
                        }
                    }
                    //Tìm theo tên sách
                    else
                    {
                        var data = DataHistory.Where(x => x.WareHouse.Name.ToLower().Contains(FilterString.ToLower()) == true).Select(x => x);
                        if (data.Count() > 0)
                        {
                            //Tạo list với kết quả trả về là tên sách
                            History = new ObservableCollection<COutput_History>(data);
                        }
                        else
                        {
                            //Tạo list rỗng
                            History = new ObservableCollection<COutput_History>();
                        }
                    }
                }
            }
            else
            {
                History = new ObservableCollection<COutput_History>();
            }

        }

        #endregion
    }
}
