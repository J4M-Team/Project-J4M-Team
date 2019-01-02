using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using System.Collections.ObjectModel;
using BookStore.Model.MyClass;

namespace BookStore.ViewModel
{
    public class InHoaDonVM : BaseViewModel

    {
        #region data binding

        //List để hiển thị lên giao diện

        private float _TotalOfPrice;
        public  float TotalOfPrice
        {
            get { return _TotalOfPrice; }
            set
            {
                _TotalOfPrice = value;
                OnPropertyChanged(nameof(TotalOfPrice));
            }
        }



        private ObservableCollection<CBook> _ListSelectedBooks;
        public ObservableCollection<CBook> ListSelectedBooks
        {
            get { return _ListSelectedBooks; }
            set
            {
                _ListSelectedBooks = value;
                OnPropertyChanged(nameof(ListSelectedBooks));
            }
        }


        private CBook _SelectedItem;
        public CBook SelectedItem
        {
            get { return _SelectedItem; }
            set
            {
                _SelectedItem = value;
                OnPropertyChanged(nameof(SelectedItem));
            }
        }

        private int _SelectedIndex;
        public int SelectedIndex
        {
            get { return _SelectedIndex; }
            set
            {
                _SelectedIndex = value;
                OnPropertyChanged(nameof(SelectedIndex));
            }
        }

        private string _CustomerEmail;
        public string CustomerEmail
        {
            get { return _CustomerEmail; }
            set
            {
                _CustomerEmail = value;
                OnPropertyChanged(nameof(CustomerEmail));
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

        private string _CustomerPhone;
        public string CustomerPhone
        {
            get { return _CustomerPhone; }
            set
            {
                _CustomerPhone = value;
                OnPropertyChanged(nameof(CustomerPhone));
            }
        }

        private string _CustomerAddress;
        public string CustomerAddress
        {
            get { return _CustomerAddress; }
            set
            {
                _CustomerAddress = value;
                OnPropertyChanged(nameof(CustomerAddress));
            }
        }
        #endregion
        public ICommand AddBookCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand ShowListBookCommand { get; set; }
        public ICommand TotalOfPriceCommand { get; set; }
        public ICommand PayCommand { get; set; }


        public InHoaDonVM()
        {
            
            ListSelectedBooks = DataTransfer.ListBooks;
            DeleteCommand = new RelayCommand<object>((p) =>
            {
                return true;
            },
          (p) =>
          {
              if (SelectedItem != null)
              {
                  ListSelectedBooks.RemoveAt(SelectedIndex);
              }

          }
             );


            AddBookCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {

                AddBookBillInformation wd = new AddBookBillInformation();
                wd.ShowDialog();

                TotalOfPrice = ListSelectedBooks.Sum(x => x.TotalPrice);
                                          
            }
              );

            ShowListBookCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                // ListSelectedBooks = new ObservableCollection<CBook>(DataTransfer.BookBill);
            }
              );

            PayCommand = new RelayCommand<object>((p) => {
                //Kiểm tra List không có sách
                if (ListSelectedBooks.Count() == 0)
                {
                    return false;
                }
                //Kiểm tra thông tin tên và số điện thoại của khách hàng
                if (string.IsNullOrEmpty(CustomerName) || string.IsNullOrEmpty(CustomerPhone))
                {
                    return false;
                }

                //Kiểm tra số điện thoại hợp lệ
                if (Help.isInt(CustomerPhone) == false)
                    return false;

                //Kiểm tra thông tin mail không hợp lệ
                if (!string.IsNullOrEmpty(CustomerEmail))
                {
                    if (Help.isEmail(CustomerEmail) == false)
                        return false;
                }
                return true;
            }, 
            (p) =>
            {
                //Thêm danh khách hàng vào trong danh sách khách hàng nếu chưa có
                //Lấy id của khách hàng
                int CustomerId = CCustomer.Ins.AddCustomer(new CCustomer
                {
                    Name = Help.StandardizeName(CustomerName),
                    Phone = CustomerPhone,
                    Address = CustomerAddress,
                    Email = CustomerEmail
                });

                //Thêm mới Bill
                CBill Bill = new CBill
                {
                    Customer = new CCustomer { Id = CustomerId },
                    Salesman = new CSalesman { Id = DataTransfer.Employee_Id },
                    Type = 1,
                    TotalMoney = TotalOfPrice,
                    Date = DateTime.Now,
                    ListBook = ListSelectedBooks.ToList()
                };

                //Thêm vào lịch sử thanh toán, thông tin hóa đơn
                int Check = CSalesman.Ins_Salesman.AddBill(Bill);

                if (Check > 0)
                {
                    MessageBox.Show("Thanh toán thành công", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Asterisk);

                    //Xóa bảng
                    CustomerAddress = "";
                    CustomerEmail = "";
                    CustomerName = "";
                    CustomerPhone = "";

                    //Xóa List book
                    DataTransfer.ListBooks = new ObservableCollection<CBook>();
                    ListSelectedBooks = new ObservableCollection<CBook>();
                    //Trả tổng giá về 0
                    TotalOfPrice = 0;
                }
                else
                {
                    MessageBox.Show("Thanh toán thất bại", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
              );
        }
    }
}
