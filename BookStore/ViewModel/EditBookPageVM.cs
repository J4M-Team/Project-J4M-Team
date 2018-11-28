using BookStore.Model;
using BookStore.Model.MyClass;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

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

        #endregion

        public EditBookPageVM()
        {
            loadCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                ListBook = new ObservableCollection<CBook>(CBook.Ins.Load());
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
            }
               );
        }
    }
}
