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
        public int IdEmloyee { get { return _IdEmployee; } set { _IdEmployee = value; } }
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
                            IdInfo = item.Employee_Info_Id,
                            IdEmloyee = item.Employee_Id,
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
        #endregion
    }
}
