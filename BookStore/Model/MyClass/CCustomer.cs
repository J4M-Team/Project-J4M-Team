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
                        Address = item.Customer_Address,
                        Email = item.Customer_Email,
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


        #endregion


    }
}
