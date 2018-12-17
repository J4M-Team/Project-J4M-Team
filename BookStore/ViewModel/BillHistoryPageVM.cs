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

        private ObservableCollection<CBill> _DataListBill;
        public ObservableCollection<CBill> DataListBill
        {
            get { return _DataListBill; }
            set
            {
                _DataListBill = value;
                OnPropertyChanged(nameof(DataListBill));
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

        public BillHistoryPageVM()
        {
            loadCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                MinDate = CBill.Ins.MinDate();
                MaxDate = CBill.Ins.MaxDate();

                DataListBill = new ObservableCollection<CBill>(CBill.Ins.ListAllBill().OrderByDescending(x => x.Date));
                ListBill = DataListBill;
                
            }
               );

            SelectedDateMinChanged = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                if (MinDate > MaxDate)
                {
                    MinDate = CBill.Ins.MinDate();
                    MaxDate = CBill.Ins.MaxDate();

                    DataListBill = new ObservableCollection<CBill>(CBill.Ins.ListAllBill().OrderByDescending(x => x.Date));
                    ListBill = DataListBill;
                }

                DataListBill = new ObservableCollection<CBill>(CBill.Ins.ListAllBill(FilterString, MinDate, MaxDate).OrderByDescending(x => x.Date));
                ListBill = DataListBill;

            }
               );

            SelectedDateMaxChanged = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                if (MinDate > MaxDate)
                {
                    MinDate = CBill.Ins.MinDate();
                    MaxDate = CBill.Ins.MaxDate();

                    DataListBill = new ObservableCollection<CBill>(CBill.Ins.ListAllBill().OrderByDescending(x => x.Date));
                    ListBill = DataListBill;
                }

                DataListBill = new ObservableCollection<CBill>(CBill.Ins.ListAllBill(FilterString, MinDate, MaxDate).OrderByDescending(x => x.Date));
                ListBill = DataListBill;

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
            if (DataListBill.Count > 0)
            {

                if (string.IsNullOrEmpty(FilterString))
                {
                    ListBill = DataListBill;
                }
                else
                {
                    int Id;
                    //Tìm theo ID
                    if (int.TryParse(FilterString, out Id) == true)
                    {
                        var data = DataListBill.Where(x => x.Salesman.Id == Id).Select(x => x);
                        if (data.Count() > 0)
                        {
                            //Tạo list với kết quả trả về là Id
                            ListBill = new ObservableCollection<CBill>(data);
                        }
                        else
                        {
                            //Tạo list rỗng
                            ListBill = new ObservableCollection<CBill>();
                        }
                    }
                    //Tìm theo tên sách
                    else
                    {
                        var data = DataListBill.Where(x => x.Salesman.Name.ToLower().Contains(FilterString.ToLower()) == true).Select(x => x);
                        if (data.Count() > 0)
                        {
                            //Tạo list với kết quả trả về là tên sách
                            ListBill = new ObservableCollection<CBill>(data);
                        }
                        else
                        {
                            //Tạo list rỗng
                            ListBill = new ObservableCollection<CBill>();
                        }
                    }
                }
            }
            else
            {
                ListBill = new ObservableCollection<CBill>();
            }

        }

        #endregion
    }
}
