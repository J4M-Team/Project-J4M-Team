using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Model.MyClass
{
    public class CRole
    {
        #region design pattern singleton

        private static CRole _ins;
        public static CRole Ins
        {
            get
            {
                if (_ins == null)
                    _ins = new CRole();
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
        private string _Name;
        private int _Count;
        private float _Salary;
        private string _Decentralization;//Thông tin quyền hạn của chức vụ này

        #endregion

        #region public properties

        public int Id { get { return _Id; } set { _Id = value; } }
        public string Name { get { return _Name; } set { _Name = value; } }
        //Số lượng nhân viên này hiện tại
        public int Count { get { return _Count; } set { _Count = value; } }
        public float Salary { get { return _Salary; } set { _Salary = value; } }
        public string Decentralization { get { return _Decentralization; } set { _Decentralization = value; } }

        #endregion

        #region method

        /// <summary>
        /// Hàm trả về danh sách chức vụ nhân viên trong cửa hàng
        /// </summary>
        /// <returns></returns>
        public List<CRole> ListRole()
        {
            List<CRole> List = new List<CRole>();
            try
            {
                //Lấy ra danh sách dữ liệu
                var data = DataProvider.Ins.DB.Employee_Role;
                if (data.Count() > 0)
                {
                    foreach(var item in data)
                    {
                        //Lấy ra số lượng nhân viên cùng loại 
                        int count = DataProvider.Ins.DB.Employees.Where(x => x.Role_Id == item.Role_Id).Count();

                        //tạo mới
                        CRole Role = new CRole
                        {
                            Id=item.Role_Id,
                            Name = item.Role_Name,
                            Salary = (float)item.Role_Salary,
                            Decentralization = item.Decentralization1.Describe,
                            Count = count
                        };

                        //Thêm vào danh sách
                        List.Add(Role);
                    }
                }        
            }
            catch
            {

            }

           
            return List;
        }
       
        /// <summary>
        /// Hàm cập nhật lương mới cho nhân viên
        /// </summary>
        /// <param name="Role"></param>
        /// <returns></returns>
        public bool ChangeSalary(CRole Role)
        {
            try
            {
                if (Role.Salary > 0)
                {
                    //Tìm đối tượng cần update theo khóa chính
                    var find = DataProvider.Ins.DB.Employee_Role.Find(Role.Id);
                    if (find != null)
                    {
                        //Kiểm tra nếu lương mới bằng lương cũ thì không cập nhật
                        if (find.Role_Salary != Role.Salary)
                        {
                            //Cập nhật giá mới
                            find.Role_Salary = Role.Salary;

                            //Lưu lại thay đổi
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
        /// Hàm trả về danh sách quyền của nhân viên
        /// </summary>
        /// <returns></returns>
        public List<string> ListDecentralization()
        {
            List<string> List = new List<string>();
            try
            {
                var data = DataProvider.Ins.DB.Decentralizations.Select(x => x.Describe);
                if (data.Count() > 0)
                {
                    foreach(var item in data)
                    {
                        List.Add(item);
                    }
                }
            }
            catch
            {

            }
            return List;
        }

        public bool AddRole(CRole newRole)
        {
            
            try
            {
                if (newRole.Salary > 0)
                {
                    //Kiểm tra xem loại nhân viên đó có tồn tại trong cơ sở dữ liệu hay chưa
                    var find = DataProvider.Ins.DB.Employee_Role.Where(x => x.Role_Name.ToLower() == newRole.Name.ToLower());
                    if (find.Count() == 0)
                    {
                        //Tìm kiếm id tương ứng với loại quyền của nhân viên
                        var IdDecentralization = DataProvider.Ins.DB.Decentralizations.Where(x => x.Describe.ToLower() == newRole.Decentralization.ToLower()).First();
                        if (IdDecentralization != null)
                        {
                            //Tạo mới Role
                            Employee_Role Role = new Employee_Role { Role_Name = newRole.Name, Role_Salary = newRole.Salary, Decentralization = IdDecentralization.Decentralization_Id };

                            //Thêm
                            DataProvider.Ins.DB.Employee_Role.Add(Role);

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

        #endregion
    }
}
