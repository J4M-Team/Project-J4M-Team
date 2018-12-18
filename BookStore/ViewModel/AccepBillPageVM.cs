using BookStore.Model.MyClass;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using System.Windows.Data;
using System.ComponentModel;

namespace BookStore.ViewModel
{
    public class AccepBillPageVM:BaseViewModel
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
      
        /// <summary>
        /// Lưu List load từ cơ sở dữ liệu
        /// </summary>
        private ObservableCollection<CBill> _ListBillData;
        public ObservableCollection<CBill> ListBillData
        {
            get { return _ListBillData; }
            set
            {
                _ListBillData = value;
                OnPropertyChanged(nameof(ListBillData));
            }
        }

        private CBill _SelectedItem;
        public CBill SelectedItem
        {
            get { return _SelectedItem; }
            set
            {
                _SelectedItem = value;
                OnPropertyChanged(nameof(SelectedItem));
            }
        }

        #endregion

        #region command binding

        public ICommand loadCommand { get; set; }
        public ICommand AccepCommand { get; set; }
        public ICommand SearchCommand { get; set; }
        public ICommand SearchTextChangeCommand { get; set; }

        #endregion
      
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
       

        public AccepBillPageVM()
        {
                    
            loadCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {               
                ListBillData = new ObservableCollection<CBill>(CBill.Ins.ListNewBill());
                ListBill = ListBillData;
            }
               );

            AccepCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                if (SelectedItem != null)
                {
                    //Thêm vào lịch sử
                    CWarehouse.Ins_Warehouse.AddHistoryOutput(SelectedItem.Id, DataTransfer.Employee_Id);

                    //Xóa hóa đơn khỏi dach sách cần xử lí
                    CBill.Ins.RemoveBillInOutputinfo(SelectedItem.Id);

                    //load lại bảng
                   ListBillData = new ObservableCollection<CBill>(CBill.Ins.ListNewBill());
                   Search();
                }
                else
                {
                   //2 sự kiện bấm button và selectionchange bị gọi chồng vào nhau
                }
               
            }
               );

            SearchCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                
            }
               );

            SearchTextChangeCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                Search();             
            }
               );
        }

        /// <summary>
        /// Hàm tìm kiếm theo tên khách hàng hoặc theo id Bill
        /// </summary>
        private void Search()
        {
            if (ListBillData.Count>0)
            {

                if (string.IsNullOrEmpty(FilterString))
                {
                    ListBill = ListBillData;
                }
                else
                {
                    int Id;
                    //Tìm theo ID
                    if (int.TryParse(FilterString, out Id) == true)
                    {
                        var data = ListBillData.Where(x => x.Id == Id).Select(x => x);
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
                    //Tìm theo tên khách hàng
                    else
                    {
                        var data = ListBillData.Where(x => x.Customer.Name.ToLower().Contains(FilterString.ToLower()) == true).Select(x => x);
                        if (data.Count() > 0)
                        {
                            //Tạo list với kết quả trả về là tên khách hàng
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

        

    }
}
