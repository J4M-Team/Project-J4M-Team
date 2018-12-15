﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Model.MyClass
{
    public class CCustomer : CHuman
    {
        #region design pattern singleton

        private static CCustomer _ins;
        public static CCustomer Ins
        {
            get
            {
                if (_ins == null)
                    _ins = new CCustomer();
                return _ins;
            }
            set
            {
                _ins = value;
            }

        }

        #endregion

        #region private properties

        private CCustomer_Types _Type;
        private int _NumberBook;//Tổng sách đã mua từ của hàng
        private float _SumMoney;//Tổng tiền đã trả cho cửa hàng

        #endregion

        #region public properties

        public CCustomer_Types Type { get { return _Type; } set { _Type = value; } }
        public int NumberBook { get { return _NumberBook; } set { _NumberBook = value; } }
        public float SumMoney { get { return _SumMoney; } set { _SumMoney = value; } }

        #endregion

        #region constructor

        public CCustomer()
        {

        }

        public CCustomer(string Name, string Address, string Phone, string Email) : base(Name, Address, Phone, Email)
        {

        }
        #endregion

        #region method

        /// <summary>
        /// Hàm trả về danh sách tất cả khách hàng
        /// </summary>
        /// <returns></returns>
        public List<CCustomer> Load()
        {
            var List = new List<CCustomer>();
            try
            {
                var sql = DataProvider.Ins.DB.Customers;
                foreach (var item in sql)
                {
                    CCustomer Customer;
                    Customer = new CCustomer
                    {
                        Id = item.Customer_Id,
                        Name = item.Customer_Name,
                        Address = item.Customer_Address == null ? "Không có" : item.Customer_Address,
                        Email = item.Customer_Email == null ? "Không có" : item.Customer_Email,
                        Phone = item.Customer_Phone,
                    };
                    List.Add(Customer);
                }
            }
            catch
            {

            }
            return List;
        }

        /// <summary>
        /// Hàm thêm khách hàng, thêm thành công sẽ trả về id của khách hàng đó, nếu khách hàng đã tồn tại thì trả về id khách hàng
        /// </summary>
        /// <param name="Customer"></param>
        /// <returns>0 nếu thất bại, thành công trả về id khách hàng</returns>
        public int AddCustomer(CCustomer Customer)
        {
            try
            {
                //Kiểm tra thông tin tên và số điện thoại có trống hay không
                if (string.IsNullOrEmpty(Customer.Name) || string.IsNullOrEmpty(Customer.Phone))
                {
                    return 0;
                }

                //Kiểm tra xem khách hàng có trong cơ sở dữ liệu hay chưa, nếu có rồi thì trả về id của khách hàng
                int Customer_Id = DataProvider.Ins.DB.Customers.Where(x => x.Customer_Name.ToLower() == Customer.Name.ToLower() &&
                x.Customer_Phone.ToLower() == Customer.Phone.ToLower()).Select(x => x.Customer_Id).FirstOrDefault();

                if (Customer_Id > 0)
                {
                    return Customer_Id;
                }
               
                //Nếu chưa có thì tạo mới khách hàng

                //Tạo mới customer

                Customer data = new Customer
                {
                    Customer_Name = Customer.Name,
                    Customer_Phone = Customer.Phone,
                    Customer_Email = Customer.Email,
                    Customer_Address = Customer.Address
                };

                //Thêm mới
                DataProvider.Ins.DB.Customers.Add(data);

                //Lưu thay đổi
                DataProvider.Ins.DB.SaveChanges();

                //Lấy ra id của khách hàng vừa tạo
                //Kiểm tra xem khách hàng có trong cơ sở dữ liệu hay chưa, nếu có rồi thì trả về id của khách hàng
                int Id = DataProvider.Ins.DB.Customers.Where(x => x.Customer_Name.ToLower() == Customer.Name.ToLower() &&
                x.Customer_Phone.ToLower() == Customer.Phone.ToLower()).Select(x => x.Customer_Id).FirstOrDefault();

                if (Id > 0)
                {
                    return Id;
                }
            }
            catch
            {

            }

            return 0;
        }

        /// <summary>
        /// Hàm trả về danh sách khách hàng bao gồm,tổng số sách đã mua, tổng số tiền đã trả cho cửa hàng
        /// </summary>
        /// <returns></returns>
        public List<CCustomer> TransactionHistory()
        {
            List<CCustomer> List = new List<CCustomer>();

            try
            {
                var ListCustomer = DataProvider.Ins.DB.Customers;

                foreach (var item in ListCustomer)
                {
                    //Lấy ra tổng sách mà khách mua trong bảng Bill_info
                    int Count = DataProvider.Ins.DB.Bill_Info.Where(x => x.Bill.Customer_Id == item.Customer_Id).Select(x => x.Book_Count).DefaultIfEmpty(0).Sum();

                    //Lấy ra tổng số tiền mà khách trả cho khách hàng trong bảng Bill_Info
                    float Sum = DataProvider.Ins.DB.Bill_Info.Where(x => x.Bill.Customer_Id == item.Customer_Id).Select(x => new { Sum = (float)x.Price * x.Book_Count }).Select(x => x.Sum).DefaultIfEmpty(0).Sum();

                    //Tạo mới
                    CCustomer Customer = new CCustomer
                    {
                        Id = item.Customer_Id,
                        Name = item.Customer_Name,
                        Phone = item.Customer_Phone,
                        Email = item.Customer_Email == null ? "Không có" : item.Customer_Email,
                        NumberBook = Count,
                        SumMoney = Sum
                    };

                    //Thêm vào List
                    List.Add(Customer);
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
