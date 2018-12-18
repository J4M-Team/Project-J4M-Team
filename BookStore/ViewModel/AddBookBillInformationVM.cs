using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using BookStore.Model.MyClass;
using System.Windows;
namespace BookStore.ViewModel
{
    public class AddBookBillInformationVM : BaseViewModel
    {
        #region data binding
        private string _Name;
        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                _Name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        private string _Price;
        public string Price
        {
            get
            {
                return _Price;
            }
            set
            {
                _Price = value;
                OnPropertyChanged(nameof(Price));
            }
        }

        private string _NumberOfBook;
        public string NumberOfBook
        {
            get { return _NumberOfBook; }
            set
            {
                if (_NumberOfBook == value)
                {
                    return;
                }

                _NumberOfBook = value;
                OnPropertyChanged("NumberPagesToPrint");
            }
        }

        private CBook _SelectedItem;
        public CBook SelectedItem
        {
            get
            {
                return _SelectedItem;
            }
            set
            {
                _SelectedItem = value;
                OnPropertyChanged(nameof(SelectedItem));
            }
        }
        //List để hiển thị lên giao diện
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

        private ObservableCollection<CBook> _DataListBook;
        public ObservableCollection<CBook> DataListBook
        {
            get { return _DataListBook; }
            set
            {
                _DataListBook = value;
                OnPropertyChanged(nameof(DataListBook));
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
        public ICommand SelectBookCommand { get; set; }
        public ICommand NumberOfBookChangeCommand { get; set; }
        public ICommand TransferBookBill { get; set; }
       


        #endregion
        public AddBookBillInformationVM()
        {
           

            loadCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                //lấy data từ cơ sở dữ liệu
                DataListBook = new ObservableCollection<CBook>(CBook.Ins.ListPromotionBook());
                ListBook = DataListBook;
            }
              );
            SearchTextChangeCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                Search();
                
            }
               );
            SelectBookCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                if (SelectedItem != null)
                {
                    Name = SelectedItem.Name;
                    Price = SelectedItem.PricePromotion.ToString();
                    
                }
                
            }
              );
            NumberOfBookChangeCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
               if(CheckNumberOfBook()==false)
                {
                    MessageBox.Show("KHONG DUOC NHAP CHU");

                }
            }
              );

            TransferBookBill = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                if(NumberOfBook=="" || NumberOfBook==null)
                {
                    MessageBox.Show("Khong duoc de trong so luong");
                }
                else
                {
                    if (SelectedItem != null)
                    {
                        CBook Book = new CBook
                        {
                            Id = SelectedItem.Id,
                            Name = SelectedItem.Name,
                            Count = int.Parse(_NumberOfBook),
                            PricePromotion = SelectedItem.PricePromotion,
                            TotalPrice = SelectedItem.PricePromotion * int.Parse(_NumberOfBook)
                        };
                        
                        DataTransfer.ListBooks.Add(Book);
                    }                                                        
                }
            }
              );

        }
        /// <summary>
        /// Hàm trả về true nếu nhập vô là số
        /// </summary>
        /// <returns></returns>
        private bool CheckNumberOfBook()
        {
            if(NumberOfBook=="")
            {
                return true;
            }
            var isNumeric = int.TryParse(NumberOfBook, out int n);
            return isNumeric;
        }
        void Search()
        {
            if (DataListBook.Count > 0)
            {

                if (string.IsNullOrEmpty(FilterString))
                {
                    ListBook = DataListBook;
                }
                else
                {
                    int Id;
                    //Tìm theo ID
                    if (int.TryParse(FilterString, out Id) == true)
                    {
                        var data = DataListBook.Where(x => x.Id == Id).Select(x => x);
                        if (data.Count() > 0)
                        {
                            //Tạo list với kết quả trả về là Id
                            ListBook = new ObservableCollection<CBook>(data);
                        }
                        else
                        {
                            //Tạo list rỗng
                            ListBook = new ObservableCollection<CBook>();
                        }
                    }
                    //Tìm theo tên sách
                    else
                    {
                        var data = DataListBook.Where(x => x.Name.ToLower().Contains(FilterString.ToLower()) == true).Select(x => x);
                        if (data.Count() > 0)
                        {
                            //Tạo list với kết quả trả về là tên khách hàng
                            ListBook = new ObservableCollection<CBook>(data);
                        }
                        else
                        {
                            //Tạo list rỗng
                            ListBook = new ObservableCollection<CBook>();
                        }
                    }
                }
            }
            else
            {
                ListBook = new ObservableCollection<CBook>();
            }
        }
    }
}
