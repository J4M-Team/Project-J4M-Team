using BookStore.Model;
using BookStore.Model.MyClass;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows;
using Microsoft.Win32;
using System.Windows.Media.Imaging;
using System.IO;

namespace BookStore.ViewModel
{
    public class SearchPageVM : BaseViewModel
    {
        #region data binding
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

        private ObservableCollection<string> _ListTheme;
        public ObservableCollection<string> ListTheme
        {
            get { return _ListTheme; }
            set
            {
                _ListTheme = value;
                OnPropertyChanged(nameof(ListTheme));
            }
        }

        private ObservableCollection<string> _ListType;
        public ObservableCollection<string> ListType
        {
            get { return _ListType; }
            set
            {
                _ListType = value;
                OnPropertyChanged(nameof(ListType));
            }
        }

        private ObservableCollection<string> _ListAuthor;
        public ObservableCollection<string> ListAuthor
        {
            get { return _ListAuthor; }
            set
            {
                _ListAuthor = value;
                OnPropertyChanged(nameof(ListAuthor));
            }
        }

        private ObservableCollection<string> _ListPrice;
        public ObservableCollection<string> ListPrice
        {
            get { return _ListPrice; }
            set
            {
                _ListPrice = value;
                OnPropertyChanged(nameof(ListPrice));
            }
        }
        
        //binding Text Combobox

        private string _TextType;
        public string TextType
        {
            get { return _TextType; }
            set
            {
                _TextType = value;
                OnPropertyChanged(nameof(TextType));
            }
        }

        private string _TextTheme;
        public string TextTheme
        {
            get { return _TextTheme; }
            set
            {
                _TextTheme = value;
                OnPropertyChanged(nameof(TextTheme));
            }
        }

        private string _TextAuthor;
        public string TextAuthor
        {
            get { return _TextAuthor; }
            set
            {
                _TextAuthor = value;
                OnPropertyChanged(nameof(TextAuthor));
            }
        }

        private string _TextPrice;
        public string TextPrice
        {
            get { return _TextPrice; }
            set
            {
                _TextPrice = value;
                OnPropertyChanged(nameof(TextPrice));
            }
        }

        //binding selecteditem combobox
        
        private string _SelectedItemAuthor;
        public string SelectedItemAuthor
        {
            get { return _SelectedItemAuthor; }
            set
            {
                _SelectedItemAuthor = value;
                OnPropertyChanged(nameof(SelectedItemAuthor));
            }
        }

        private string _SelectedItemPrice;
        public string SelectedItemPrice
        {
            get { return _SelectedItemPrice; }
            set
            {
                _SelectedItemPrice = value;
                OnPropertyChanged(nameof(SelectedItemPrice));
            }
        }
        
        private string _SelectedItemTheme;
        public string SelectedItemTheme
        {
            get { return _SelectedItemTheme; }
            set
            {
                _SelectedItemTheme = value;
                OnPropertyChanged(nameof(SelectedItemTheme));
            }
        }
        private string _SelectedItemType;
        public string SelectedItemType
        {
            get { return _SelectedItemType; }
            set
            {
                _SelectedItemType = value;
                OnPropertyChanged(nameof(SelectedItemType));
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

        #region binding Command

        public ICommand loadCommand { get; set; }
        public ICommand SelectionChangedType { get; set; }
        public ICommand SelectionChangedTheme { get; set; }
        public ICommand SelectionChangedAuthor { get; set; }
        public ICommand SelectionChangedPrice { get; set; }

        #endregion

        public SearchPageVM()
        {
            loadCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                ListBook = new ObservableCollection<CBook>(CBook.Ins.Load());


                ListTheme = new ObservableCollection<string>(CBook.Ins.ListTheme());
                ListTheme.Add("Tất cả");


                ListType = new ObservableCollection<string>(CBook.Ins.ListType());
                ListType.Add("Tất cả");

                ListAuthor = new ObservableCollection<string>(CBook.Ins.ListAuthor());
                ListAuthor.Add("Tất cả");

                ListPrice = new ObservableCollection<string> { "Tất cả", "Dưới 50000", "50000 - 100000", "100000 - 150000", "Lớn hơn 150000" };

                //Đặt text
                TextType = "Tất cả";
                TextTheme = "Tất cả";
                TextPrice = "Tất cả";
                TextAuthor = "Tất cả";
            }
              );

            SelectionChangedType = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                if (SelectedItemType != null)
                {
                    ListBook = new ObservableCollection<CBook>(CBook.Ins.FindBookType(SelectedItemType));
                }
            }
              );

            SelectionChangedTheme = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                if (SelectedItemTheme != null)
                {
                    ListBook = new ObservableCollection<CBook>(CBook.Ins.FindBookTheme(SelectedItemTheme));
                }
            }
              );

            SelectionChangedAuthor = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                if (SelectedItemAuthor != null)
                {
                    ListBook = new ObservableCollection<CBook>(CBook.Ins.FindBookAuthor(SelectedItemAuthor));
                }
            }
              );

            SelectionChangedPrice = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                if (SelectedItemPrice != null)
                {
                    ListBook = new ObservableCollection<CBook>(CBook.Ins.FindBookPrice(90000, 1000000));
                }
            }
              );
        }
    } 
}
