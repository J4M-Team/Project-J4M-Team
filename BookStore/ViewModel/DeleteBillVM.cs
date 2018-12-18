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
    public class DeleteBillVM:BaseViewModel
    {
        #region data binding

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

        #endregion

        public ICommand loadCommand { get; set; }
        public ICommand SelectedDateMinChanged { get; set; }
        public ICommand SelectedDateMaxChanged { get; set; }

        #region command binding

        #endregion

        #region method

        public DeleteBillVM()
        {
            loadCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                MinDate = CBill.Ins.MinDate(DataTransfer.Employee_Id);
                MaxDate = CBill.Ins.MaxDate(DataTransfer.Employee_Id);

                ListBill = new ObservableCollection<CBill>(CBill.Ins.ListAllBill(DataTransfer.Employee_Id, MinDate, MaxDate).OrderByDescending(x => x.Date));

            }
               );

            SelectedDateMinChanged = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                if (MinDate > MaxDate)
                {
                    MinDate = CBill.Ins.MinDate(DataTransfer.Employee_Id);
                    MaxDate = CBill.Ins.MaxDate(DataTransfer.Employee_Id);

                    ListBill = new ObservableCollection<CBill>(CBill.Ins.ListAllBill(DataTransfer.Employee_Id, MinDate, MaxDate).OrderByDescending(x => x.Date));
                }

                ListBill = new ObservableCollection<CBill>(CBill.Ins.ListAllBill(DataTransfer.Employee_Id, MinDate, MaxDate).OrderByDescending(x => x.Date));

            }
               );

            SelectedDateMaxChanged = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                if (MinDate > MaxDate)
                {
                    MinDate = CBill.Ins.MinDate(DataTransfer.Employee_Id);
                    MaxDate = CBill.Ins.MaxDate(DataTransfer.Employee_Id);

                    ListBill = new ObservableCollection<CBill>(CBill.Ins.ListAllBill(DataTransfer.Employee_Id, MinDate, MaxDate).OrderByDescending(x => x.Date));
                }

                ListBill = new ObservableCollection<CBill>(CBill.Ins.ListAllBill(DataTransfer.Employee_Id, MinDate, MaxDate).OrderByDescending(x => x.Date));

            }
               );
        }

        #endregion
    }
}
