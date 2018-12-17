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
    public class InputHistoryPageVM:BaseViewModel
    {
        #region data binding

        private ObservableCollection<CInput_History> _ListInput;
        public ObservableCollection<CInput_History> ListInput
        {
            get { return _ListInput; }
            set
            {
                _ListInput = value;
                OnPropertyChanged(nameof(ListInput));
            }
        }

        private ObservableCollection<CInput_History> _DataListInput;
        public ObservableCollection<CInput_History> DataListInput
        {
            get { return _DataListInput; }
            set
            {
                _DataListInput = value;
                OnPropertyChanged(nameof(DataListInput));
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

        public InputHistoryPageVM()
        {
            
            loadCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                MinDate = CWarehouse.Ins_Warehouse.MinDateInput();
                MaxDate = CWarehouse.Ins_Warehouse.MaxDateInput();

                DataListInput = new ObservableCollection<CInput_History>(CWarehouse.Ins_Warehouse.InputHistory(FilterString, MinDate, MaxDate).OrderByDescending(x => x.Date));
                ListInput = DataListInput;

            }
               );
            SelectedDateMinChanged = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                if (MinDate > MaxDate)
                {
                    MinDate = CWarehouse.Ins_Warehouse.MinDateInput();
                    MaxDate = CWarehouse.Ins_Warehouse.MaxDateInput();

                    DataListInput = new ObservableCollection<CInput_History>(CWarehouse.Ins_Warehouse.InputHistory(FilterString, MinDate, MaxDate).OrderByDescending(x => x.Date));
                    ListInput = DataListInput;

                }

                DataListInput = new ObservableCollection<CInput_History>(CWarehouse.Ins_Warehouse.InputHistory(FilterString, MinDate, MaxDate).OrderByDescending(x => x.Date));
                ListInput = DataListInput;


            }
               );

            SelectedDateMaxChanged = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                if (MinDate > MaxDate)
                {
                    MinDate = CWarehouse.Ins_Warehouse.MinDateInput();
                    MaxDate = CWarehouse.Ins_Warehouse.MaxDateInput();

                    DataListInput = new ObservableCollection<CInput_History>(CWarehouse.Ins_Warehouse.InputHistory(FilterString, MinDate, MaxDate).OrderByDescending(x => x.Date));
                    ListInput = DataListInput;
                }

                DataListInput = new ObservableCollection<CInput_History>(CWarehouse.Ins_Warehouse.InputHistory(FilterString, MinDate, MaxDate).OrderByDescending(x => x.Date));
                ListInput = DataListInput;

            }
               );

            SearchTextChangeCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                Search();
            }
               );
        }

        /// <summary>
        /// Hàm tìm kiếm sách theo mã hoặc tên nhân viên
        /// </summary>
        private void Search()
        {
            if (DataListInput.Count > 0)
            {

                if (string.IsNullOrEmpty(FilterString))
                {
                    ListInput = DataListInput;
                }
                else
                {
                    int Id;
                    //Tìm theo ID
                    if (int.TryParse(FilterString, out Id) == true)
                    {
                        var data = DataListInput.Where(x => x.WareHouse.Id == Id).Select(x => x);
                        if (data.Count() > 0)
                        {
                            //Tạo list với kết quả trả về là Id
                            ListInput = new ObservableCollection<CInput_History>(data);
                        }
                        else
                        {
                            //Tạo list rỗng
                            ListInput = new ObservableCollection<CInput_History>();
                        }
                    }
                    //Tìm theo tên sách
                    else
                    {
                        var data = DataListInput.Where(x => x.WareHouse.Name.ToLower().Contains(FilterString.ToLower()) == true).Select(x => x);
                        if (data.Count() > 0)
                        {
                            //Tạo list với kết quả trả về là tên sách
                            ListInput = new ObservableCollection<CInput_History>(data);
                        }
                        else
                        {
                            //Tạo list rỗng
                            ListInput = new ObservableCollection<CInput_History>();
                        }
                    }
                }
            }
            else
            {
                ListInput = new ObservableCollection<CInput_History>();
            }

        }

        #endregion
    }
}
