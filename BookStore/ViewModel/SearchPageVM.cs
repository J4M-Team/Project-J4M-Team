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
    public class SearchPageVM: BaseViewModel
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

        #endregion

        #region binding Command

        public ICommand loadCommand { get; set; }

        #endregion

        public SearchPageVM()
        {
            loadCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                ListBook = new ObservableCollection<CBook>(CBook.Ins.Load());
                ListTheme = new ObservableCollection<string>(CBook.Ins.ListTheme());
                ListType = new ObservableCollection<string>(CBook.Ins.ListType());
                ListAuthor = new ObservableCollection<string>(CBook.Ins.ListAuthor());
            }
              );
        }
    }
}
