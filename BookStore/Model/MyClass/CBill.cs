using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Model.MyClass
{
    public class CBill
    {
        #region design pattern singleton

        private static CBill _ins;
        public static CBill Ins
        {
            get
            {
                if (_ins == null)
                    _ins = new CBill();
                return _ins;
            }
            set
            {
                _ins = value;
            }
        }

        #endregion

        #region private properties

        private int _Id;
        private CSalesman _Salesman;
        private CCustomer _Customer;
        private float _TotalMoney;
        private int _Type;
        private float _Promotion;
        private DateTime _Date;
        private List<CBook> _ListBook;

        #endregion

        #region public properties

        public int Id { get { return _Id; } set { _Id = value; } }
        public CSalesman Salesman { get { return _Salesman; } set { _Salesman = value; } }
        public CCustomer Customer { get { return _Customer; } set { _Customer = value; } }
        public float TotalMoney { get { return _TotalMoney; } set { _TotalMoney = value; } }
        public int Type { get { return _Type; } set { _Type = value; } }
        public float Promotion { get { return _Promotion; } set { _Promotion = value; } }
        public DateTime Date { get { return _Date; } set { _Date = value; } }
        public List<CBook> ListBook { get { return _ListBook; } set { _ListBook = value; } }

        #endregion

        #region method

        /// <summary>
        /// Hàm trả về danh sách hóa đơn vừa thanh toán
        /// </summary>
        /// <returns>danh sách mã hóa đơn, tên khách hàng, mã sách, tên sách, số lượng sách</returns>
        public List<CBill> ListNewBill()
        {
            List<CBill> List = new List<CBill>();

            try
            {

                var data = from item1 in DataProvider.Ins.DB.Output_Info
                           join item2 in DataProvider.Ins.DB.Bill_Info on item1.Bill_Id equals item2.Bill_Id
                           select new
                           {
                               item2.Bill_Id,
                               item2.Bill.Customer.Customer_Name,
                               item2.Book_Id,
                               item2.Book.Book_Name,
                               item2.Book_Count
                           };

                foreach (var item in data)
                {
                    CBill Bill;
                    if (List.Count == 0)
                    {
                        CCustomer Customer = new CCustomer { Name = item.Customer_Name };
                        CBook Book = new CBook { Id = item.Book_Id, Name = item.Book_Name, Count = item.Book_Count };
                        List<CBook> ListBook = new List<CBook>() { Book };
                        Bill = new CBill { Id = item.Bill_Id, Customer = Customer, ListBook = ListBook };
                        List.Add(Bill);
                    }
                    else
                    {

                        if (List.Where(x => x.Id == item.Bill_Id).Count() > 0)
                        {
                            CBook Book = new CBook { Id = item.Book_Id, Name = item.Book_Name, Count = item.Book_Count };
                            //Thêm sách vào List
                            (List.Where(x => x.Id == item.Bill_Id).ToList<CBill>()).ForEach(p => p.ListBook.Add(Book));
                        }
                        else
                        {
                            CCustomer Customer = new CCustomer { Name = item.Customer_Name };
                            CBook Book = new CBook { Id = item.Book_Id, Name = item.Book_Name, Count = item.Book_Count };
                            List<CBook> ListBook = new List<CBook>() { Book };
                            Bill = new CBill { Id = item.Bill_Id, Customer = Customer, ListBook = ListBook };
                            List.Add(Bill);
                        }
                    }
                }
            }
            catch
            {

            }
            return List;
        }

        #endregion
    }
}
