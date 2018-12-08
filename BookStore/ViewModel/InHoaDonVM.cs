using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using System.Collections.ObjectModel;
using BookStore.Model.MyClass;

namespace BookStore.ViewModel
{
    public class InHoaDonVM : BaseViewModel

    {
        #region data binding

        //List để hiển thị lên giao diện
        private ObservableCollection<CBook> _ListSelectedBooks;
        public ObservableCollection<CBook> ListSelectedBooks
        {
            get { return _ListSelectedBooks; }
            set
            {
                _ListSelectedBooks = value;
                OnPropertyChanged(nameof(ListSelectedBooks));
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
        public ICommand AddBookCommand { get; set; }
        public ICommand ShowListBookCommand { get; set; }
        public InHoaDonVM()
        {
            ListSelectedBooks = DataTransfer.ListBooks;

            AddBookCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {

                AddBookBillInformation wd = new AddBookBillInformation();
                wd.ShowDialog();
            }
              );

            ShowListBookCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                ListSelectedBooks = new ObservableCollection<CBook>(DataTransfer.BookBill);
            }
              );
        }
    }
}
