﻿using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
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
        private CAccount _Account;//Lưu tài khoản của nhân viên

        #endregion

        #region public properties

        public DateTime BirthDay { get { return _BirthDay; } set { _BirthDay = value; } }
        public string Identity { get { return _Identity; } set { _Identity = value; } }
        public CRole Role { get { return _Role; } set { _Role = value; } }
        public CEmployee_Info Info { get { return _Info; } set { _Info = value; } }
        public CAccount Acount { get { return _Account; } set { _Account = value; } }

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

        /// <summary>
        /// hàm thêm nhân viên mới vào csdl
        /// </summary>
        /// <param name="newEmployee"></param>
        /// <returns></returns>
        public int AddEmployee(CEmployee newEmployee)
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

                //Lấy ra id vừa tạo
                var Id = DataProvider.Ins.DB.Employees.Where(x => x.Employee_Name == newEmployee.Name && x.Employee_Identity == newEmployee.Identity 
                && x.Employee_Phone == newEmployee.Phone).Select(x => x.Employee_Id).FirstOrDefault();

                //Thêm vào bảng info
                Employee_Info info = new Employee_Info
                {
                    Employee_Id = Id,
                    Date_Start = newEmployee.Info.DateStart,
                    Sum_Date = newEmployee.Info.SumDay,
                    Date_In_Month = newEmployee.Info.DateWork
                };

                //Thêm vào bảng info
                DataProvider.Ins.DB.Employee_Info.Add(info);

                //Lưu thay đổi
                DataProvider.Ins.DB.SaveChanges();

                return Id;

            }
            catch (Exception)
            {
                
            }
            return 0;
        }

        public int isEmployee(string Identity)
        {
            try
            {
                var find = DataProvider.Ins.DB.Employees.Where(x => x.Employee_Identity == Identity).FirstOrDefault();
                if (find != null)
                {
                    return find.Employee_Id;
                }
                return 0;
            }
            catch
            {

            }
            return 0;
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
                        Role = new CRole
                        {
                            Name = find.Employee_Role.Role_Name,
                            Id = find.Employee_Role.Role_Id,
                            Salary = (float)find.Employee_Role.Role_Salary,
                            Decentralization = find.Employee_Role.Decentralization1.Describe
                        },
                        BirthDay = (DateTime)find.Employee_BirthDay,
                        Address = find.Employee_Address,
                        Email = find.Employee_Email,
                        Phone = find.Employee_Phone,
                        Info = new CEmployee_Info
                        {
                            DateStart = find.Employee_Info.Where(x => x.Employee_Id == find.Employee_Id).Select(x => x.Date_Start).FirstOrDefault(),
                            SumDay = (int)find.Employee_Info.Where(x => x.Employee_Id == find.Employee_Id).Select(x => x.Sum_Date).FirstOrDefault(),
                            DateWork = (int)find.Employee_Info.Where(x => x.Employee_Id == find.Employee_Id).Select(x => x.Date_In_Month).FirstOrDefault(),
                            Salary = (float)find.Employee_Role.Role_Salary * (int)find.Employee_Info.Where(x => x.Employee_Id == find.Employee_Id).Select(x => x.Date_In_Month).FirstOrDefault()
                        },
                        Acount = new CAccount
                        {
                            User = find.Employee_Account.Where(x => x.Employee_Id == find.Employee_Id).Select(x => x.Account_User).FirstOrDefault(),
                            Password = find.Employee_Account.Where(x => x.Employee_Id == find.Employee_Id).Select(x => x.Account_Password).FirstOrDefault()
                        }
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

        /// <summary>
        /// Hàm tăng số ngày làm việc (điểm danh) của nhân viên khi đăng nhập 1, chỉ tính lần đăng nhập đầu tiên trong ngày
        /// </summary>
        /// <param name="Employee_Id">Id nhân viên</param>
        /// <returns></returns>
        public bool CheckIn(int Employee_Id)
        {
            try
            {
                var find = DataProvider.Ins.DB.Employee_Account.Where(x => x.Employee_Id == Employee_Id).First();
                if (find != null)
                {
                    //Kiểm tra đã từng đăng nhập chưa
                    var LastLogin = find.LastLogin;
                    if (LastLogin != null)
                    {
                        //Kiểm tra ngày đăng nhập cuối cùng có trùng với ngày hôm này hay không
                        if(EntityFunctions.TruncateTime(find.LastLogin)== EntityFunctions.TruncateTime(DateTime.Now))
                        {
                            return false;
                        }
                        else
                        {
                            //Tạo mới một đăng nhập
                            find.LastLogin = DateTime.Now;

                            //Lưu lại thay đổi
                            DataProvider.Ins.DB.SaveChanges();

                            //Lấy ra thông tin bên bảng employee_info
                            var FindInfo = DataProvider.Ins.DB.Employee_Info.Where(x => x.Employee_Id == Employee_Id).First();
                            if (FindInfo != null)
                            {
                                //Thêm ngày làm việc vào
                                FindInfo.Sum_Date = FindInfo.Sum_Date + 1;
                                FindInfo.Date_In_Month = FindInfo.Date_In_Month + 1;

                                //Lưu lại thay đổi
                                DataProvider.Ins.DB.SaveChanges();

                                return true;
                                
                            }
                        }
                    }
                    else
                    {
                        //Tạo mới một đăng nhập
                        find.LastLogin = DateTime.Now;

                        //Lưu lại thay đổi
                        DataProvider.Ins.DB.SaveChanges();

                        //Lấy ra thông tin bên bảng employee_info
                        var FindInfo = DataProvider.Ins.DB.Employee_Info.Where(x => x.Employee_Id == Employee_Id).First();
                        if (FindInfo != null)
                        {
                            //Thêm ngày làm việc vào
                            FindInfo.Sum_Date = FindInfo.Sum_Date + 1;
                            FindInfo.Date_In_Month = FindInfo.Date_In_Month + 1;

                            //Lưu lại thay đổi
                            DataProvider.Ins.DB.SaveChanges();

                            return true;
;                        }
                    }
                }
            }
            catch
            {

            }

            return false;
        }
        #endregion


    }
}
