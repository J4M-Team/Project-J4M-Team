using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Model.MyClass
{
    public class CWarehouse : CEmployee
    {
        #region design pattern singleton

        private static CWarehouse _ins;
        public static CWarehouse Ins_Warehouse
        {
            get
            {
                if (_ins == null)
                    _ins = new CWarehouse();
                return _ins;
            }
            set
            {
                _ins = value;
            }
        }

        #endregion

        #region method

        /// <summary>
        /// Hàm thêm lịch sử xuất kho khi nhân viên quản kho xác nhận hóa đơn từ khách hàng
        /// </summary>
        /// <param name="Bill_Id"></param>
        /// <param name="Employee_Id"></param>
        /// <returns></returns>
        public bool AddHistoryOutput(int Bill_Id, int Employee_Id)
        {
            try
            {
                Book_Output data = new Book_Output { Bill_Id = Bill_Id, Employee_Id = Employee_Id, Output_Date = DateTime.Now };
                DataProvider.Ins.DB.Book_Output.Add(data);
                DataProvider.Ins.DB.SaveChanges();
                return true;
            }
            catch
            {

            }
            return false;
        }

        #endregion
    }
}
