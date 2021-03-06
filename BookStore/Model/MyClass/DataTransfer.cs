﻿using System;
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

        
        //Biến của Lê Tường Qui
        //Biến truyền id của nhân viên giữa các màn hình
        public static int Employee_Id;


        //Biến của Hứa Thanh Sơn
        //Biến truyền truyền id nhân viên qua màn hình add account
        public static int IDEmployee;

        //Biến của Phạm Ngọc Sơn
        //Biến truyền sách 
        //private static List<CBook> bookBill;
        //public static int NumberOfBook;
        public static ObservableCollection<CBook> ListBooks;
        //public static List<CBook> BookBill { get => bookBill; set => bookBill = value; }

       

        //Hàm khởi tạo truyền dữ liệu
        static DataTransfer()
        {
            //bookBill = new List<CBook>();
            ListBooks = new ObservableCollection<CBook>();
        }
    }
}
