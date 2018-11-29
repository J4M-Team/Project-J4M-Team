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

namespace BookStore.ViewModel
{

    public class EditBookPageVM:BaseViewModel
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

        private string _Name;
        public string Name
        {
            get { return _Name; }
            set
            {
                _Name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
       
        private string _Author;
        public string Author
        {
            get { return _Author; }
            set
            {
                _Author = value;
                OnPropertyChanged(nameof(Author));
            }
        }

        private string _Theme;
        public string Theme
        {
            get { return _Theme; }
            set
            {
                _Theme = value;
                OnPropertyChanged(nameof(Theme));
            }
        }

        private string _Type;
        public string Type
        {
            get { return _Type; }
            set
            {
                _Type = value;
                OnPropertyChanged(nameof(Type));
            }
        }

        private string _Count;
        public string Count
        {
            get { return _Count; }
            set
            {
                _Count = value;
                OnPropertyChanged(nameof(Count));
            }
        }

        #endregion

        #region command binding

        public ICommand loadCommand { get; set; }
        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }

        #endregion

        public EditBookPageVM()
        {
            loadCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                ListBook = new ObservableCollection<CBook>(CBook.Ins.Load());
                ListTheme = new ObservableCollection<string>(CBook.Ins.ListTheme());
                ListType = new ObservableCollection<string>(CBook.Ins.ListType());
            }
               );

            AddCommand = new RelayCommand<object>((p) => {
                if (string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(Author) || string.IsNullOrEmpty(Theme) || string.IsNullOrEmpty(Type) || string.IsNullOrEmpty(Count))
                    return false;
                return true;
            }, (p) =>
            {
                
                var NewBook = new CBook { Name = Name, Author = Author, Theme = Theme, Type = Type, Count = int.Parse(Count) };
                CBook.Ins.Add(NewBook);
                ListBook = new ObservableCollection<CBook>(CBook.Ins.Load());
                ListTheme = new ObservableCollection<string>(CBook.Ins.ListTheme());
                ListType = new ObservableCollection<string>(CBook.Ins.ListType());

            }
               );

            EditCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                
                if(string.IsNullOrEmpty(SelectedItem.Name)|| string.IsNullOrEmpty(SelectedItem.Author)|| string.IsNullOrEmpty(SelectedItem.Theme)|| string.IsNullOrEmpty(SelectedItem.Type))
                {
                    return;
                }
                else
                {
                    
                    CBook.Ins.Update(SelectedItem);

                    //load lại dữ liệu
                    ListBook = new ObservableCollection<CBook>(CBook.Ins.Load());
                    ListTheme = new ObservableCollection<string>(CBook.Ins.ListTheme());
                    ListType = new ObservableCollection<string>(CBook.Ins.ListType());
                }
                
            }
               );
        }
    }
}
