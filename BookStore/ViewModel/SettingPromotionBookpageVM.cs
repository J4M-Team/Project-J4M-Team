using BookStore.Model.MyClass;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace BookStore.ViewModel
{
    public class SettingPromotionBookpageVM:BaseViewModel
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

        private bool _IsChecked;
        public bool IsChecked
        {
            get { return _IsChecked; }
            set
            {
                _IsChecked = value;
                OnPropertyChanged(nameof(IsChecked));
            }
        }

        private bool _PromotionIsReadOnly;
        public bool PromotionIsReadOnly
        {
            get { return _PromotionIsReadOnly; }
            set
            {
                _PromotionIsReadOnly = value;
                OnPropertyChanged(nameof(PromotionIsReadOnly));
            }
        }

        private Visibility _ButtonColumnVisibility;
        public Visibility ButtonColumnVisibility
        {
            get { return _ButtonColumnVisibility; }
            set
            {
                _ButtonColumnVisibility = value;
                OnPropertyChanged(nameof(ButtonColumnVisibility));
            }
        }


        #region properties binding


        #endregion

        #region command binding

        public ICommand loadCommand { get; set; }
        public ICommand CheckCommand { get; set; }
        public ICommand SearchTextChangeCommand { get; set; }
        public ICommand EditCommand { get; set; }

        #endregion

        public SettingPromotionBookpageVM()
        {
            loadCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                //Ẩn cột button
                ButtonColumnVisibility = Visibility.Hidden;
                PromotionIsReadOnly = true;
                IsChecked = false;

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

            EditCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                if (SelectedItem != null)
                {
                    //Kiểm tra điều kiện nhập
                    if (SelectedItem.Promotion <= 0)
                    {
                        return;
                    }
                    else
                    {
                        //Gọi hàm cập nhật
                        CBook.Ins.ChangePromotion(SelectedItem.Id, SelectedItem.Promotion);

                        //Load lại
                        DataListBook = new ObservableCollection<CBook>(CBook.Ins.ListPromotionBook());
                        ListBook = DataListBook;
                    }                  
                }
                
            }
               );

            CheckCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {

                if (IsChecked == true)
                {
                    //Hiện cột button
                    ButtonColumnVisibility = Visibility.Visible;
                    PromotionIsReadOnly = false;
                }
                else
                {
                    //Ẩn cột button
                    ButtonColumnVisibility = Visibility.Hidden;
                    PromotionIsReadOnly = true;
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
