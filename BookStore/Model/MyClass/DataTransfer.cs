using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Model.MyClass
{
    //Lớp truyền dữ liệu giữa các màn hình
    static class DataTransfer
    {

        static public event PropertyChangedEventHandler PropertyChanged;

        //Biến của Lê Tường Qui
        //Biến truyền id của nhân viên giữa các màn hình
        public static int Employee_Id;




        //Biến của Phạm Ngọc Sơn
        //Biến truyền sách 
        //private static List<CBook> bookBill;
        //public static int NumberOfBook;
        public static ObservableCollection<CBook> ListBooks;
        //public static List<CBook> BookBill { get => bookBill; set => bookBill = value; }

        private static float _TotalMoney;
        public static float TotalMoney
        {
            get { return _TotalMoney; }
            set
            {
                _TotalMoney = value;
                new PropertyChangedEventArgs(nameof(TotalMoney));
            }
        }

        //Hàm khởi tạo truyền dữ liệu
        static DataTransfer()
        {
            //bookBill = new List<CBook>();
            ListBooks = new ObservableCollection<CBook>();
        }
    }
}
