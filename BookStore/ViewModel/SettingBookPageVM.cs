﻿using BookStore.Model.MyClass;
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

        #region properties binding

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

        private bool _InPriceIsReadOnly;
        public bool InPriceIsReadOnly
        {
            get { return _InPriceIsReadOnly; }
            set
            {
                _InPriceIsReadOnly = value;
                OnPropertyChanged(nameof(InPriceIsReadOnly));
            }
        }

        private bool _OutPriceIsReadOnly;
        public bool OutPriceIsReadOnly
        {
            get { return _OutPriceIsReadOnly; }
            set
            {
                _OutPriceIsReadOnly = value;
                OnPropertyChanged(nameof(OutPriceIsReadOnly));
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

        #endregion

        #region command binding

        public ICommand loadCommand { get; set; }
        public ICommand SearchTextChangeCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand SelectionChangedCommand { get; set; }
        public ICommand CheckCommand { get; set; }

        #endregion

        public SettingBookPageVM()
        {
           
            loadCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                //Ẩn cột button
                ButtonColumnVisibility = Visibility.Hidden;
                InPriceIsReadOnly = true;
                OutPriceIsReadOnly = true;

                //lấy data từ cơ sở dữ liệu
                DataListBook = new ObservableCollection<CBook>(CBook.Ins.ListPriceBook());
                ListBook = DataListBook;
            }
               );
           
            EditCommand = new RelayCommand<object>((p) => 
            {
                return true;
            }, 
            (p) =>
            {
                if (SelectedItem != null)
                {
                    //Kiểm tra thông tin nhập
                    

                    //Giá nhập và giá bán <0
                    if(SelectedItem.Price.InputPrice<=0)
                    {
                        MessageBox.Show("Giá nhập không hợp lệ", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);

                        //load lại dữ liệu
                        DataListBook = new ObservableCollection<CBook>(CBook.Ins.ListPriceBook());
                        ListBook = DataListBook;
                        return;
                    }

                    if (SelectedItem.Price.OutputPrice <= 0)
                    {
                        MessageBox.Show("Giá bán không hợp lệ", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);

                        //load lại dữ liệu
                        DataListBook = new ObservableCollection<CBook>(CBook.Ins.ListPriceBook());
                        ListBook = DataListBook;
                        return;
                    }


                    if( CBook.Ins.ChangeInputPrice(SelectedItem.Id, SelectedItem.Price.InputPrice) == false && CBook.Ins.ChangeOutputPrice(SelectedItem.Id, SelectedItem.Price.OutputPrice) == false)
                    {
                        MessageBox.Show("Cập nhật thất bại", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                        //Load lại thông tin
                        DataListBook = new ObservableCollection<CBook>(CBook.Ins.ListPriceBook());
                        ListBook = DataListBook;

                        return;
                    }

                    //Thông báo cập nhật thành công
                    MessageBox.Show("Cập nhật thành công", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                    //Không cần phải load lại data vì dữ liệu trên giao diện đã thay đổi rồi
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

            CheckCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {

                if (IsChecked == true)
                {
                    //Hiện cột button
                    ButtonColumnVisibility = Visibility.Visible;                  
                    InPriceIsReadOnly = false;
                    OutPriceIsReadOnly = false;
                }
                else
                {
                    //Ẩn cột button
                    ButtonColumnVisibility = Visibility.Hidden;
                    InPriceIsReadOnly = true;
                    OutPriceIsReadOnly = true;
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
