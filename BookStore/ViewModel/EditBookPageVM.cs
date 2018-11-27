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

        public ICommand loadCommand { get; set; }

        public EditBookPageVM()
        {
            loadCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                ListBook = new ObservableCollection<CBook>(CBook.Ins.Load());
            }
               );
        }
    }
}
