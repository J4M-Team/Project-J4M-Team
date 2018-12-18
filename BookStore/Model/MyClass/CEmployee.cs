using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Model.MyClass
{
    public class CEmployee : CHuman
    {
        #region design pattern singleton

        private static CEmployee _ins;
        public static CEmployee Ins
        {
            get
            {
                if (_ins == null)
                    _ins = new CEmployee();
                return _ins;
            }
            set
            {
                _ins = value;
            }
        }

        #endregion

        #region private properties

        private DateTime _BirthDay;
        private string _Identity; //Lưu số chứng minh nhân dân
        private CRole _Role;//Lưu chức vụ của nhân viên
        private CEmployee_Info _Info;//Lưu thông tin của nhân viên

        #endregion

        #region public properties

        public DateTime BirthDay { get { return _BirthDay; } set { _BirthDay = value; } }
        public string Identity { get { return _Identity; } set { _Identity = value; } }
        public CRole Role { get { return _Role; } set { _Role = value; } }
        public CEmployee_Info Info { get { return _Info; } set { _Info = value; } }

        #endregion

        #region constructor



        #endregion

        #region method

        /// <summary>
        /// Hàm trả về danh sách nhân viên
        /// </summary>
        /// <returns></returns>
        public List<CEmployee> ListEmployee()
        {
            List<CEmployee> List = new List<CEmployee>();
            try
            {
                //Lấy ra danh sách dữ liệu
                var data = DataProvider.Ins.DB.Employees;
                if (data.Count() > 0)
                {
                    foreach (var item in data)
                    {
                        CRole role = new CRole { Id = item.Employee_Role.Role_Id, Name = item.Employee_Role.Role_Name, Salary = (float)item.Employee_Role.Role_Salary };
                        //tạo mới
                        CEmployee Employee = new CEmployee
                        {
                            Id = item.Employee_Id,
                            Name = item.Employee_Name,
                            Address = item.Employee_Address,
                            Phone = item.Employee_Phone,
                            Email = item.Employee_Email,
                            BirthDay = (DateTime)item.Employee_BirthDay,
                            Identity = item.Employee_Identity,
                            Role = role,
                            //Info = item.Employee_Info

                        };

                        //Thêm vào danh sách
                        List.Add(Employee);
                    }
                }
            }
            catch
            {

            }



            return List;
        }

        /// <summary>
        /// Hàm cập nhật loại nhân viên
        /// </summary>
        /// <param name="Role"></param>
        /// <returns></returns>
        public bool UpdateEmployee(CEmployee Employee)
        {
            try
            {
                if (!string.IsNullOrEmpty(Employee.Name) && !string.IsNullOrEmpty(Employee.Address) && !string.IsNullOrEmpty(Employee.Phone) && !string.IsNullOrEmpty(Employee.Email) && !string.IsNullOrEmpty(Employee.Identity))
                {
                    //Tìm đối tượng cần update theo khóa chính
                    var find = DataProvider.Ins.DB.Employees.Find(Employee.Id);
                    if (find != null)
                    {
                        //Kiểm tra tên nhân viên
                        if (find.Employee_Name.ToLower() != Employee.Name.ToLower())
                        {
                            //Cập nhật lại
                            find.Employee_Name = Employee.Name;
                        }

                        //Kiểm tra địa chỉ nhân viên
                        if (find.Employee_Address != Employee.Address)
                        {
                            //Cập nhật lại
                            find.Employee_Address = Employee.Address;
                        }

                        //Kiểm tra số điện thoại nhân viên
                        if (find.Employee_Phone != Employee.Phone)
                        {
                            //Cập nhật lại
                            find.Employee_Phone = Employee.Phone;
                        }

                        //Kiểm tra email nhân viên
                        if (find.Employee_Email != Employee.Email)
                        {
                            //Cập nhật lại
                            find.Employee_Email = Employee.Email;
                        }

                        //Kiểm tra CMND nhân viên
                        if (find.Employee_Identity != Employee.Identity)
                        {
                            //Cập nhật lại
                            find.Employee_Identity = Employee.Identity;
                        }

                        //Kiểm tra loại nhân 
                       
                        if (find.Employee_Role.Role_Name != Employee.Role.Name)
                        {
                            var findId = DataProvider.Ins.DB.Employee_Role.Where(x => x.Role_Name == Employee.Role.Name).Select(x => x.Role_Id).FirstOrDefault();
                            find.Role_Id = findId;

                        }

                        //Lưu thay đổi
                        DataProvider.Ins.DB.SaveChanges();
                        return true;
                    }
                    else
                    {
                        return false;
                    }
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


        public bool AddEmployee(CEmployee newEmployee)
        {
            try
            {
                
                //Tìm loại dựa vào tên loại
                var role = DataProvider.Ins.DB.Employee_Role.Where(x => x.Role_Name == newEmployee.Role.Name).Select(x => x.Role_Id).FirstOrDefault();
                //Tạo nhân viên mới
                Employee employee = new Employee
                {
                    Employee_Name = newEmployee.Name,
                    Employee_Address = newEmployee.Address,
                    Employee_Phone = newEmployee.Phone,
                    Employee_Email = newEmployee.Email,
                    Employee_BirthDay = newEmployee.BirthDay,
                    Employee_Identity = newEmployee.Identity,
                    Role_Id = role
                };

                //Thêm
                DataProvider.Ins.DB.Employees.Add(employee);

                //Lưu thay đổi
                DataProvider.Ins.DB.SaveChanges();

                return true;

            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
            return false;
        }

        //Hàm trả về thông tin của nhân viên tương ứng với id của nhân viên đó
        public CEmployee EmployeeInfo(int EmployeeId)
        {
            CEmployee Employee = new CEmployee();

            try
            {
                //Kiểm tra nhân viên có tồn tại trong csdl không
                var find = DataProvider.Ins.DB.Employees.Find(EmployeeId);
                if (find != null)
                {
                    Employee = new CEmployee
                    {
                        Name = find.Employee_Name,
                        Id = find.Employee_Id,
                        Identity = find.Employee_Identity,
                        Role = new CRole { Name = find.Employee_Role.Role_Name, Id = find.Employee_Role.Role_Id },
                        BirthDay = (DateTime)find.Employee_BirthDay,
                        Address = find.Employee_Address,
                        Email = find.Employee_Email,
                        Phone = find.Employee_Phone
                    };
                }
                else
                {
                    //do something
                }
            }
            catch
            {

            }
            return Employee;
        }

        /// <summary>
        /// Hàm trả về danh sách lương của nhân viên
        /// </summary>
        /// <returns></returns>
        public List<CEmployee> ListSalary()
        {
            List<CEmployee> List = new List<CEmployee>();
            try
            {
                //Lấy ra danh sách dữ liệu
                var data = DataProvider.Ins.DB.Employee_Info;

                if (data.Count() > 0)
                {
                    foreach (var item in data)
                    {

                        //tạo mới
                        CEmployee Employee = new CEmployee
                        {
                            Id = item.Employee.Employee_Id,
                            Name = item.Employee.Employee_Name,
                            Role = new CRole
                            {
                                Name = item.Employee.Employee_Role.Role_Name,
                                Salary = (float)item.Employee.Employee_Role.Role_Salary
                            },

                            Info = new CEmployee_Info
                            {
                                DateStart = item.Date_Start,
                                SumDay = (int)item.Sum_Date,
                                DateWork = (int)item.Date_In_Month,
                                Salary = (float)item.Employee.Employee_Role.Role_Salary * (float)item.Date_In_Month
                            }

                        };

                        //Thêm vào List
                        List.Add(Employee);

                    }
                }
            }
            catch
            {

            }

            return List;
        }

        /// <summary>
        /// Hàm thanh toán lương cho nhân viên
        /// </summary>
        /// <param name="Employee"></param>
        /// <returns></returns>
        public bool Payment(CEmployee Employee)
        {
            try
            {
                var find = DataProvider.Ins.DB.Employee_Info.Where(x => x.Employee_Id == Employee.Id).FirstOrDefault();
                if (find != null)
                {
                    //Thanh toán xong reset lại số ngày làm việc bằng 0
                    find.Date_In_Month = 0;

                    //Lưu thay đổi
                    DataProvider.Ins.DB.SaveChanges();
                }
                else
                {
                    return false;
                }
                
                PayWage_History paywage = new PayWage_History
                {

                    Employee_Id = Employee.Id,
                    PayWage_Date = DateTime.Now,
                    Employee_Salary = Employee.Info.Salary,                    
                };

                //Thêm vào lịch sử thanh toán lương
                DataProvider.Ins.DB.PayWage_History.Add(paywage);

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
