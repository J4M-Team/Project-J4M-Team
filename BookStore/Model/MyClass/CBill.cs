using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
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
        //Id hoa don
        public int Id { get { return _Id; } set { _Id = value; } }
        //Thông tin người bán
        public CSalesman Salesman { get { return _Salesman; } set { _Salesman = value; } }
        //Thông tin khác hàng
        public CCustomer Customer { get { return _Customer; } set { _Customer = value; } }
        //Thông tin tổng số tiền
        public float TotalMoney { get { return _TotalMoney; } set { _TotalMoney = value; } }
        public int Type { get { return _Type; } set { _Type = value; } }
        public float Promotion { get { return _Promotion; } set { _Promotion = value; } }
        //Ngày thanh toán
        public DateTime Date { get { return _Date; } set { _Date = value; } }
        //Dánh Sách Sách
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
                               item2.Book_Count,
                               item1.Bill.Bill_Date,
                               item1.Bill.Employee.Employee_Name,
                               item1.Bill.Employee_Id
                           };

                foreach (var item in data)
                {
                    CBill Bill;
                    if (List.Count == 0)
                    {
                        CCustomer Customer = new CCustomer { Name = item.Customer_Name };
                        CBook Book = new CBook { Id = item.Book_Id, Name = item.Book_Name, Count = item.Book_Count };
                        List<CBook> ListBook = new List<CBook>() { Book };
                        Bill = new CBill
                        {
                            Id = item.Bill_Id,
                            Customer = Customer,
                            ListBook = ListBook,
                            Date = (DateTime)item.Bill_Date,
                            Salesman = new CSalesman
                            {
                                Id = item.Employee_Id,
                                Name = item.Employee_Name
                            }
                        };
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
                            Bill = new CBill
                            {
                                Id = item.Bill_Id,
                                Customer = Customer,
                                ListBook = ListBook,
                                Date = (DateTime)item.Bill_Date,
                                Salesman = new CSalesman
                                {
                                    Id = item.Employee_Id,
                                    Name = item.Employee_Name
                                }
                            };
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

        public bool RemoveBillInOutputinfo(int Bill_Id)
        {
            try
            {
                //Tìm
                Output_Info BilltoDelete = DataProvider.Ins.DB.Output_Info.Find(Bill_Id);
                if (BilltoDelete != null)
                {
                    //Xóa
                    DataProvider.Ins.DB.Output_Info.Remove(BilltoDelete);
                    //Lưu thay đổi
                    DataProvider.Ins.DB.SaveChanges();

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                
            }
            return false;
        }

        /// <summary>
        /// Hàm trả về danh sách hóa đơn, kèm tên khách hàng, tên người thanh toán và chi tiết hóa đơn
        /// </summary>
        /// <returns></returns>
        public List<CBill> ListAllBill()
        {
            List<CBill> List = new List<CBill>();

            try
            {
                var data = DataProvider.Ins.DB.Bill_Info;

                foreach (var item in data)
                {
                    CBill Bill;
                    if (List.Count == 0)
                    {
                        CCustomer Customer = new CCustomer { Name = item.Bill.Customer.Customer_Name };

                        CBook Book = new CBook
                        {
                            Id = item.Book_Id,
                            Name = item.Book.Book_Name,
                            Count = item.Book_Count,
                            PricePromotion = (float)item.Price,
                            TotalPrice = (float)item.Price * item.Book_Count
                        };

                        CSalesman Salesman = new CSalesman
                        {
                            Id = item.Bill.Employee_Id,
                            Name = item.Bill.Employee.Employee_Name
                        };

                        List<CBook> ListBook = new List<CBook>() { Book };

                        Bill = new CBill
                        {
                            Id = item.Bill_Id,
                            Customer = Customer,
                            Salesman = Salesman,
                            ListBook = ListBook,
                            Date = (DateTime)item.Bill.Bill_Date
                        };

                        List.Add(Bill);
                    }
                    else
                    {

                        if (List.Where(x => x.Id == item.Bill_Id).Count() > 0)
                        {
                            CBook Book = new CBook
                            {
                                Id = item.Book_Id,
                                Name = item.Book.Book_Name,
                                Count = item.Book_Count,
                                PricePromotion = (float)item.Price,
                                TotalPrice = (float)item.Price * item.Book_Count
                            };

                            //Thêm sách vào List
                            (List.Where(x => x.Id == item.Bill_Id).ToList<CBill>()).ForEach(p => p.ListBook.Add(Book));
                        }
                        else
                        {
                            CCustomer Customer = new CCustomer { Name = item.Bill.Customer.Customer_Name };

                            CBook Book = new CBook
                            {
                                Id = item.Book_Id,
                                Name = item.Book.Book_Name,
                                Count = item.Book_Count,
                                PricePromotion = (float)item.Price,
                                TotalPrice = (float)item.Price * item.Book_Count
                            };

                            CSalesman Salesman = new CSalesman
                            {
                                Id = item.Bill.Employee_Id,
                                Name = item.Bill.Employee.Employee_Name
                            };

                            List<CBook> ListBook = new List<CBook>() { Book };

                            Bill = new CBill
                            {
                                Id = item.Bill_Id,
                                Customer = Customer,
                                Salesman = Salesman,
                                ListBook = ListBook,
                                Date = (DateTime)item.Bill.Bill_Date
                            };

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

        /// <summary>
        /// Hàm trả về danh sách hóa đơn từ ngày đến ngày........
        /// </summary>
        /// <param name="Customer_Id"></param>
        /// <param name="MinDate"></param>
        /// <param name="MaxDate"></param>
        /// <returns></returns>
        public List<CBill> ListAllBill(string Customer_Name,DateTime MinDate,DateTime MaxDate)
        {
            List<CBill> List = new List<CBill>();

            try
            {
                IQueryable<Bill_Info> data = DataProvider.Ins.DB.Bill_Info.Select(x => x);

                if (MinDate > MaxDate)
                {
                    return List;
                }
                else
                {
                    if (!string.IsNullOrEmpty(Customer_Name))
                    {
                        data = DataProvider.Ins.DB.Bill_Info.Where(x => x.Bill.Employee.Employee_Name.ToLower().Contains(Customer_Name.ToLower())
                        && EntityFunctions.TruncateTime(x.Bill.Bill_Date) >= EntityFunctions.TruncateTime(MinDate) &&
                        EntityFunctions.TruncateTime(x.Bill.Bill_Date) <= EntityFunctions.TruncateTime(MaxDate));
                    }
                    else
                    {
                        data = DataProvider.Ins.DB.Bill_Info.Where(x => EntityFunctions.TruncateTime(x.Bill.Bill_Date) >= EntityFunctions.TruncateTime(MinDate) &&
                        EntityFunctions.TruncateTime(x.Bill.Bill_Date) <= EntityFunctions.TruncateTime(MaxDate));
                    }
                }

                if (data.Count() > 0)
                {
                    foreach (var item in data)
                    {
                        CBill Bill;
                        if (List.Count == 0)
                        {
                            CCustomer Customer = new CCustomer { Name = item.Bill.Customer.Customer_Name };

                            CBook Book = new CBook
                            {
                                Id = item.Book_Id,
                                Name = item.Book.Book_Name,
                                Count = item.Book_Count,
                                PricePromotion = (float)item.Price,
                                TotalPrice = (float)item.Price * item.Book_Count
                            };

                            CSalesman Salesman = new CSalesman
                            {
                                Id = item.Bill.Employee_Id,
                                Name = item.Bill.Employee.Employee_Name
                            };

                            List<CBook> ListBook = new List<CBook>() { Book };

                            Bill = new CBill
                            {
                                Id = item.Bill_Id,
                                Customer = Customer,
                                Salesman = Salesman,
                                ListBook = ListBook,
                                Date = (DateTime)item.Bill.Bill_Date
                            };

                            List.Add(Bill);
                        }
                        else
                        {

                            if (List.Where(x => x.Id == item.Bill_Id).Count() > 0)
                            {
                                CBook Book = new CBook
                                {
                                    Id = item.Book_Id,
                                    Name = item.Book.Book_Name,
                                    Count = item.Book_Count,
                                    PricePromotion = (float)item.Price,
                                    TotalPrice = (float)item.Price * item.Book_Count
                                };

                                //Thêm sách vào List
                                (List.Where(x => x.Id == item.Bill_Id).ToList<CBill>()).ForEach(p => p.ListBook.Add(Book));
                            }
                            else
                            {
                                CCustomer Customer = new CCustomer { Name = item.Bill.Customer.Customer_Name };

                                CBook Book = new CBook
                                {
                                    Id = item.Book_Id,
                                    Name = item.Book.Book_Name,
                                    Count = item.Book_Count,
                                    PricePromotion = (float)item.Price,
                                    TotalPrice = (float)item.Price * item.Book_Count
                                };

                                CSalesman Salesman = new CSalesman
                                {
                                    Id = item.Bill.Employee_Id,
                                    Name = item.Bill.Employee.Employee_Name
                                };

                                List<CBook> ListBook = new List<CBook>() { Book };

                                Bill = new CBill
                                {
                                    Id = item.Bill_Id,
                                    Customer = Customer,
                                    Salesman = Salesman,
                                    ListBook = ListBook,
                                    Date = (DateTime)item.Bill.Bill_Date
                                };

                                List.Add(Bill);
                            }
                        }
                    }
                }                                             
            }
            catch
            {

            }
            return List;
        }

        /// <summary>
        /// Hàm trả về danh sách hóa đơn từ ngày đến ngày của nhân viên có Id
        /// </summary>
        /// <param name="Employee_Id">Id nhân viên</param>
        /// <param name="MinDate">Ngày bắt đầu</param>
        /// <param name="MaxDate">Ngày kết thúc</param>
        /// <returns></returns>
        public List<CBill> ListAllBill(int Employee_Id, DateTime MinDate, DateTime MaxDate)
        {
            List<CBill> List = new List<CBill>();

            try
            {
                if (Employee_Id <= 0)
                {
                    return List;
                }

                IQueryable<Bill_Info> data = DataProvider.Ins.DB.Bill_Info.Where(x => x.Bill.Employee_Id == Employee_Id).Select(x => x);

                if (MinDate > MaxDate)
                {
                    return List;
                }
                else
                {
                    data = DataProvider.Ins.DB.Bill_Info.Where(x => x.Bill.Employee_Id == Employee_Id
                    && EntityFunctions.TruncateTime(x.Bill.Bill_Date) >= EntityFunctions.TruncateTime(MinDate) &&
                    EntityFunctions.TruncateTime(x.Bill.Bill_Date) <= EntityFunctions.TruncateTime(MaxDate));

                }

                if (data.Count() > 0)
                {
                    foreach (var item in data)
                    {
                        CBill Bill;
                        if (List.Count == 0)
                        {
                            CCustomer Customer = new CCustomer { Name = item.Bill.Customer.Customer_Name };

                            CBook Book = new CBook
                            {
                                Id = item.Book_Id,
                                Name = item.Book.Book_Name,
                                Count = item.Book_Count,
                                PricePromotion = (float)item.Price,
                                TotalPrice = (float)item.Price * item.Book_Count
                            };

                            CSalesman Salesman = new CSalesman
                            {
                                Id = item.Bill.Employee_Id,
                                Name = item.Bill.Employee.Employee_Name
                            };

                            List<CBook> ListBook = new List<CBook>() { Book };

                            Bill = new CBill
                            {
                                Id = item.Bill_Id,
                                Customer = Customer,
                                Salesman = Salesman,
                                ListBook = ListBook,
                                Date = (DateTime)item.Bill.Bill_Date
                            };

                            List.Add(Bill);
                        }
                        else
                        {

                            if (List.Where(x => x.Id == item.Bill_Id).Count() > 0)
                            {
                                CBook Book = new CBook
                                {
                                    Id = item.Book_Id,
                                    Name = item.Book.Book_Name,
                                    Count = item.Book_Count,
                                    PricePromotion = (float)item.Price,
                                    TotalPrice = (float)item.Price * item.Book_Count
                                };

                                //Thêm sách vào List
                                (List.Where(x => x.Id == item.Bill_Id).ToList<CBill>()).ForEach(p => p.ListBook.Add(Book));
                            }
                            else
                            {
                                CCustomer Customer = new CCustomer { Name = item.Bill.Customer.Customer_Name };

                                CBook Book = new CBook
                                {
                                    Id = item.Book_Id,
                                    Name = item.Book.Book_Name,
                                    Count = item.Book_Count,
                                    PricePromotion = (float)item.Price,
                                    TotalPrice = (float)item.Price * item.Book_Count
                                };

                                CSalesman Salesman = new CSalesman
                                {
                                    Id = item.Bill.Employee_Id,
                                    Name = item.Bill.Employee.Employee_Name
                                };

                                List<CBook> ListBook = new List<CBook>() { Book };

                                Bill = new CBill
                                {
                                    Id = item.Bill_Id,
                                    Customer = Customer,
                                    Salesman = Salesman,
                                    ListBook = ListBook,
                                    Date = (DateTime)item.Bill.Bill_Date
                                };

                                List.Add(Bill);
                            }
                        }
                    }
                }
            }
            catch
            {

            }
            return List;
        }

        /// <summary>
        /// Hàm lấy ra ngày nhỏ nhất trong lịch sử thanh toán hóa đơn
        /// </summary>
        /// <returns></returns>
        public DateTime MinDate()
        {
            DateTime MinDate = new DateTime();

            try
            {
                var data = DataProvider.Ins.DB.Bills.OrderBy(x => x.Bill_Date).Select(x => x.Bill_Date).FirstOrDefault();

                MinDate = (DateTime)data;
            }
            catch
            {

            }

            return MinDate;
        }

        /// <summary>
        /// Hàm trả về ngày nhỏ nhất trong lịch sử thanh toán hóa đơn của nhân viên
        /// </summary>
        /// <param name="Employee_Id"></param>
        /// <returns></returns>
        public DateTime MinDate(int Employee_Id)
        {
            DateTime MinDate = DateTime.Now;

            try
            {
                var data = DataProvider.Ins.DB.Bills.Where(x => x.Employee_Id == Employee_Id).OrderBy(x => x.Bill_Date).Select(x => x.Bill_Date).FirstOrDefault();

                MinDate = (DateTime)data;
            }
            catch
            {

            }

            return MinDate;
        }

        /// <summary>
        /// Hàm trả về ngày lớn nhất trong lịch sử thanh toán hóa đơn
        /// </summary>
        /// <returns></returns>
        public DateTime MaxDate()
        {
            DateTime MaxDate = new DateTime();

            try
            {
                var data = DataProvider.Ins.DB.Bills.OrderByDescending(x => x.Bill_Date).Select(x => x.Bill_Date).FirstOrDefault();

                MaxDate = (DateTime)data;
            }
            catch
            {

            }

            return MaxDate;
        }

        public DateTime MaxDate(int Employee_Id)
        {
            DateTime MaxDate = DateTime.Now;

            try
            {
                var data = DataProvider.Ins.DB.Bills.Where(x => x.Employee_Id == Employee_Id).OrderByDescending(x => x.Bill_Date).Select(x => x.Bill_Date).FirstOrDefault();

                MaxDate = (DateTime)data;
            }
            catch
            {

            }

            return MaxDate;
        }

        #endregion
    }
}
