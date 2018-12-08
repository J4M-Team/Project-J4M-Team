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

        /// <summary>
        /// Hàm thêm vào lịch sử nhập kho khi nhân viên nhập thêm sách mới, hoặc thêm sách vào kho
        /// </summary>
        /// <param name="Employee_Id"></param>
        /// <param name="BookData"></param>
        /// <returns></returns>
        public bool AddHistoryInput(int Employee_Id,CBook BookData)
        {
            try
            {
                //Kiểm tra data của sách, id nhân viên truyền vào
                if (BookData.Id <= 0 || BookData.Count <= 0 || Employee_Id <= 0)
                {
                    return false;
                }

                //Tìm giá nhập của sách theo id,Lấy giá nhập mới nhất trong bảng input_Price (giá nhập mới nhất có ngày cài đặt là ngày gần nhất)
                float Input_Price = (float)DataProvider.Ins.DB.Book_Input_Price.Where(x => x.Book_Id == BookData.Id).OrderByDescending(x => x.Date_Set).Select(x => x.Input_Price).FirstOrDefault();

                //Kiểm tra nếu giá nhập không tồn tại 
                if(Input_Price<=0)
                {
                    return false;
                }

                //Tạo mới
                Book_Input data = new Book_Input
                {
                    Book_Id = BookData.Id,
                    Book_Count = BookData.Count,
                    Employee_Id = Employee_Id,
                    Input_Price = Input_Price,
                    Input_Date = DateTime.Now
                };

                //Thêm vào
                DataProvider.Ins.DB.Book_Input.Add(data);

                //Lưu thay đổi
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
