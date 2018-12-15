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
    public class CustomerInfoPageVM:BaseViewModel
    {
        #region data binding

        private ObservableCollection<CCustomer> _ListCustomer;
        public ObservableCollection<CCustomer> ListCustomer
        {
            get { return _ListCustomer; }
            set
            {
                _ListCustomer = value;
                OnPropertyChanged(nameof(ListCustomer));
            }
        }

        private ObservableCollection<CCustomer> _DataListCustomer;
        public ObservableCollection<CCustomer> DataListCustomer
        {
            get { return _DataListCustomer; }
            set
            {
                _DataListCustomer = value;
                OnPropertyChanged(nameof(DataListCustomer));
            }
        }

        private CCustomer _SelectedItem;
        public CCustomer SelectedItem
        {
            get { return _SelectedItem; }
            set
            {
                _SelectedItem = value;
                OnPropertyChanged(nameof(SelectedItem));
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

        #region properties binding

        //Thuộc tính isreadonly cột tên sách
        private bool _NameisReadOnly;
        public bool NameisReadOnly
        {
            get { return _NameisReadOnly; }
            set
            {
                _NameisReadOnly = value;
                OnPropertyChanged(nameof(NameisReadOnly));
            }
        }

        //Thuộc tính isreadonly cột địa chỉ
        private bool _AddressisReadOnly;
        public bool AddressisReadOnly
        {
            get { return _AddressisReadOnly; }
            set
            {
                _AddressisReadOnly = value;
                OnPropertyChanged(nameof(AddressisReadOnly));
            }
        }

        //Thuộc tính isreadonly cột Email
        private bool _EmailisReadOnly;
        public bool EmailisReadOnly
        {
            get { return _EmailisReadOnly; }
            set
            {
                _EmailisReadOnly = value;
                OnPropertyChanged(nameof(EmailisReadOnly));
            }
        }

        //Thuộc tính isreadonly cột số điện thoại
        private bool _PhoneisReadOnly;
        public bool PhoneisReadOnly
        {
            get { return _PhoneisReadOnly; }
            set
            {
                _PhoneisReadOnly = value;
                OnPropertyChanged(nameof(PhoneisReadOnly));
            }
        }

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

        //Thuộc tính ẩn của cột button
        private Visibility _EditColumnVisibility;
        public Visibility EditColumnVisibility
        {
            get { return _EditColumnVisibility; }
            set
            {
                _EditColumnVisibility = value;
                OnPropertyChanged(nameof(EditColumnVisibility));
            }
        }

        #endregion

        #region command binding

        public ICommand CheckCommand { get; set; }
        public ICommand loadCommand { get; set; }
        public ICommand SearchTextChangeCommand { get; set; }

        #endregion

        #region method

        public CustomerInfoPageVM()
        {
            loadCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                NameisReadOnly = true;
                PhoneisReadOnly = true;
                EmailisReadOnly = true;
                AddressisReadOnly = true;
                IsChecked = false;

                EditColumnVisibility = Visibility.Hidden;

                DataListCustomer = new ObservableCollection<CCustomer>(CCustomer.Ins.Load());

                ListCustomer = DataListCustomer;
            }
               );

            SearchTextChangeCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                Search();
            }
               );

            CheckCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {

                if (IsChecked == true)
                {
                    NameisReadOnly = false;
                    PhoneisReadOnly = false;
                    EmailisReadOnly = false;
                    AddressisReadOnly = false;

                    EditColumnVisibility = Visibility.Visible;
                   
                }
                else
                {
                    NameisReadOnly = true;
                    PhoneisReadOnly = true;
                    EmailisReadOnly = true;
                    AddressisReadOnly = true;

                    EditColumnVisibility = Visibility.Hidden;
                    
                }

            }
               );
        }

        /// <summary>
        /// Hàm tìm kiếm sách theo mã hoặc theo tên
        /// </summary>
        private void Search()
        {
            if (DataListCustomer.Count > 0)
            {

                if (string.IsNullOrEmpty(FilterString))
                {
                    ListCustomer = DataListCustomer;
                }
                else
                {
                    int Id;
                    //Tìm theo ID
                    if (int.TryParse(FilterString, out Id) == true)
                    {
                        var data = DataListCustomer.Where(x => x.Id == Id).Select(x => x);
                        if (data.Count() > 0)
                        {
                            //Tạo list với kết quả trả về là Id
                            ListCustomer = new ObservableCollection<CCustomer>(data);
                        }
                        else
                        {
                            //Tạo list rỗng
                            ListCustomer = new ObservableCollection<CCustomer>();
                        }
                    }
                    //Tìm theo tên sách
                    else
                    {
                        var data = DataListCustomer.Where(x => x.Name.ToLower().Contains(FilterString.ToLower()) == true).Select(x => x);
                        if (data.Count() > 0)
                        {
                            //Tạo list với kết quả trả về là tên sách
                            ListCustomer = new ObservableCollection<CCustomer>(data);
                        }
                        else
                        {
                            //Tạo list rỗng
                            ListCustomer = new ObservableCollection<CCustomer>();
                        }
                    }
                }
            }
            else
            {
                ListCustomer = new ObservableCollection<CCustomer>();
            }

        }

        #endregion
    }
}
