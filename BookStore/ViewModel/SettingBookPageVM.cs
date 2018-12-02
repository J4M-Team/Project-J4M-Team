using BookStore.Model.MyClass;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;

namespace BookStore.ViewModel
{
    public class SettingBookPageVM:BaseViewModel
    {
        #region data binding

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

        //List để lưu data load lên từ cơ sở dữ liệu
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

        //Lưu List lịch sử cài đặt giá nhập
        private ObservableCollection<CBook> _InputHistory;
        public ObservableCollection<CBook> InputHistory
        {
            get { return _InputHistory; }
            set
            {
                _InputHistory = value;
                OnPropertyChanged(nameof(InputHistory));
            }
        }

        private ObservableCollection<CBook> _OutputHistory;
        public ObservableCollection<CBook> OutputHistory
        {
            get { return _OutputHistory; }
            set
            {
                _OutputHistory = value;
                OnPropertyChanged(nameof(OutputHistory));
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

        #endregion

        #region command binding

        public ICommand loadCommand { get; set; }
        public ICommand SearchTextChangeCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand SelectionChangedCommand { get; set; }

        #endregion

        public SettingBookPageVM()
        {
           
            loadCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                //lấy data từ cơ sở dữ liệu
                DataListBook = new ObservableCollection<CBook>(CBook.Ins.ListPriceBook());
                ListBook = DataListBook;
            }
               );
            //Lỗi chưa fixx
            EditCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                
                if (SelectedItem != null)
                {
                    //Thay đổi
                    //Kiểm tra có thay đổi so với dữ liệu ban đầu hay không
                    //Lấy ra giá ban đầu trong cơ sở dữ liệu
                    float OldInputPrice = DataListBook.Where(x => x.Id == SelectedItem.Id).Select(x => x.Price.InputPrice).First();
                    float OldOutputPrice = DataListBook.Where(x => x.Id == SelectedItem.Id).Select(x => x.Price.OutputPrice).First();

                    //Kiểm tra
                    if(SelectedItem.Price.InputPrice != OldInputPrice)
                    {
                        CBook.Ins.ChangeInputPrice(SelectedItem.Id, SelectedItem.Price.InputPrice);
                    }

                    if(SelectedItem.Price.OutputPrice != OldOutputPrice)
                    {
                        CBook.Ins.ChangeOutputPrice(SelectedItem.Id, SelectedItem.Price.OutputPrice);
                    }
                                      
                    //Cập nhật lại data
                    DataListBook = new ObservableCollection<CBook>(CBook.Ins.ListPriceBook());
                    //Load lại
                    Search();
                }
            }
               );

            SearchTextChangeCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                Search();
            }
               );

            SelectionChangedCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                if (SelectedItem != null)
                {
                   OutputHistory = new ObservableCollection<CBook>(CBook.Ins.ListHistoryOutputPrice(SelectedItem.Id));
                   InputHistory = new ObservableCollection<CBook>(CBook.Ins.ListHistoryInputPrice(SelectedItem.Id));
                }              
            }
               );
        }

        /// <summary>
        /// Hàm tìm kiếm sách theo mã hoặc theo tên
        /// </summary>
        private void Search()
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
                    //Tìm theo tên khách hàng
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
