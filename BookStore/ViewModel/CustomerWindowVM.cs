using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BookStore.ViewModel
{
    public class CustomerWindowVM:BaseViewModel
    {
        #region databinding

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

        #endregion

        #region command binding

        public ICommand loadCommand { get; set; }
        public ICommand CloseCommand { get; set; }

        #endregion

        #region method

        public CustomerWindowVM()
        {
            loadCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                FramePage = new SearchPage();
            }
               );

            CloseCommand = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                //đóng cửa sổ
                p.Close();
            }
                );
        }

        #endregion
    }
}
