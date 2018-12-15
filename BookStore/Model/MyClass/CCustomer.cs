using System;
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

        #endregion

        #region public properties

        public CCustomer_Types Type { get { return _Type; } set { _Type = value; } }

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

        


        #endregion


    }
}
