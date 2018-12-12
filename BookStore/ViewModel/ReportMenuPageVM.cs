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
    public class ReportMenuPageVM:BaseViewModel
    {
        #region data binding

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

        #region properties binding

        private Thickness _GridCursorMargin;
        public Thickness GridCursorMargin
        {
            get
            {
                return _GridCursorMargin;
            }
            set
            {
                _GridCursorMargin = value;
                OnPropertyChanged(nameof(GridCursorMargin));
            }
        }

        #endregion

        #region command binding

        public ICommand DateReportCommand { get; set; }
        public ICommand loadCommand { get; set; }

        #endregion

        #region method

        public ReportMenuPageVM()
        {
            DateReportCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                GridCursorMargin = new Thickness(10, 0, 0, 0);
                //Hiện màn hình thống kê theo ngày
                FramePage = new ProfitReportPage();
            }
              );

            loadCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                GridCursorMargin = new Thickness(10, 0, 0, 0);
                //Hiện màn hình thống kê theo ngày
                FramePage = new ProfitReportPage();
            }
              );
        }

        #endregion
    }
}
