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

        public ICommand BookCommand { get; set; }//Hiển thị màn hình thống kê sách
        public ICommand AccountCommand { get; set; }//Hiển thị màn hình thông tin tài khoản
        public ICommand EditBookCommand { get; set; }//Hiển thị màn hình thêm sách
        public ICommand SearchBookCommand { get; set; }//Hiển thị màn hình tìm kiếm sách
        public ICommand AccepBillCommand { get; set; }//Hiển thị màn hình duyệt hóa đơn

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

            SearchBookCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                FramePage = new SearchPage();
            }
              );

            AccepBillCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                FramePage = new AccepBillPage();
            }
               );
        }
    }
}
