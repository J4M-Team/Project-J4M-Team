using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace BookStore.ViewModel
{
    public class BookViewModel : BaseViewModel
    {
       //Biến lưu framepage
        private Page _FramePage;
        public Page FramePage
        {
            get
            {
                return _FramePage;
            }
            set
            {
                if (_FramePage == value)
                    return;
                _FramePage = value;
                OnPropertyChanged(nameof(FramePage));//Thêm nameof vào trước để khi rename thì sẽ thay đổi luôn cả biến không phải đổi tay khi để kiểu string "FramePage"
            }
        }

        public ICommand BookCommand { get; set; }
        public ICommand AccountCommand { get; set; }
        public ICommand EditBookCommand { get; set; }

        public BookViewModel()
        {
            BookCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                FramePage = new BookPage();
            }
               );

            AccountCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                FramePage = new AccountPage();
            }
               );
            EditBookCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                FramePage = new EditBookPage();
            }
               );
        }
    }
}
