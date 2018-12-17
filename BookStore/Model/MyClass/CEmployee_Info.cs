using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Model.MyClass
{
    public class CEmployee_Info
    {
        #region design pattern singleton

        private static CEmployee_Info _ins;
        public static CEmployee_Info Ins
        {
            get
            {
                if (_ins == null)
                    _ins = new CEmployee_Info();
                return _ins;
            }
            set
            {
                _ins = value;
            }
        }

        #endregion
        #region private properties

        private int _IdInfo;
        private int _IdEmployee;
        private DateTime _DateStart;
        private int _SumDay;
        private int _DateWork;
        //Tính lương trong tháng hiện tại
        private double _Salary;


        #endregion

        #region public properties
        
        public int IdInfo { get { return _IdInfo; } set { _IdInfo = value; } }
        public int IdEmployee { get { return _IdEmployee; } set { _IdEmployee = value; } }
        public DateTime DateStart { get { return _DateStart; } set { _DateStart = value; } }
        public int SumDay{ get { return _SumDay; } set { _SumDay = value; } }
        public int DateWork { get { return _DateWork; } set { _DateWork = value; } }
        //Tính lương trong tháng hiện tại
        public double Salary { get { return _Salary; } set { _Salary = value; } }
        #endregion

        #region method
        /// <summary>
        /// Hàm trả về bảng lương nhân viên
        /// </summary>
        /// <returns></returns>
        public List<CEmployee_Info> ListSalary()
        {
            List<CEmployee_Info> List = new List<CEmployee_Info>();
            try
            {
                //Lấy ra danh sách dữ liệu
                var data = DataProvider.Ins.DB.Employee_Info;
               
              
                if (data.Count() > 0)
                {
                    foreach (var item in data)
                    {
                        var Role_Id = DataProvider.Ins.DB.Employees.Where(x => x.Employee_Id == item.Employee_Id).Select(x => x.Role_Id).FirstOrDefault();
                        var Role_Salary = DataProvider.Ins.DB.Employee_Role.Where(x => x.Role_Id == Role_Id).Select(x => x.Role_Salary).FirstOrDefault();
                        //tạo mới
                        CEmployee_Info EmployeeInfo = new CEmployee_Info
                        {
                            IdInfo = item.Emplouee_Info_Id,
                            IdEmployee = item.Employee_Id,
                            DateStart = item.Date_Start,
                            SumDay = (int)item.Sum_Date,
                            DateWork = (int)item.Date_In_Month,
                            Salary = Role_Salary * (double)item.Date_In_Month / 30,

                        };

                        //Thêm vào danh sách
                        List.Add(EmployeeInfo);
                    }
                }
            }
            catch
            {

            }



            return List;
        }

 
        /// <summary>
        /// Hàm thanh toán
        /// </summary>
        /// <returns></returns>   
        public bool Payment(CEmployee_Info employeeInfo)
        {
            try
            {
                var find = DataProvider.Ins.DB.Employee_Info.Find(employeeInfo.IdInfo);
                if (find != null)
                {
                    //Thanh toán xong reset lại số ngày làm việc bằng 0
                    find.Date_In_Month = 0;
                }
                else
                {
                    return false;
                }

                //Lấy full employee từ id employee trong info
                var employee = DataProvider.Ins.DB.Employees.Find(employeeInfo.IdEmployee);
                //Đưa thông tin thanh toán vào lịch sử thanh toán 
                PayWage_History paywate = new PayWage_History
                {

                    Employee_Id = employeeInfo.IdInfo,
                    PayWage_Date = DateTime.Now,
                    Employee_Salary = employeeInfo.Salary,
                    Employee = employee,

                };


                //Thêm vào lịch sử thanh toán lương
                DataProvider.Ins.DB.PayWage_History.Add(paywate);

                //Lưu lại
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
