using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Model.MyClass
{
    public class CAccount
    {
        #region design pattern singleton

        private static CAccount _ins;
        public static CAccount Ins
        {
            get
            {
                if (_ins == null)
                    _ins = new CAccount();
                return _ins;
            }
            set
            {
                _ins = value;
            }
        }

        #endregion

        #region private properties

        private int _IdAccount;
        private string _User;
        private string _Password;
        private int _EmployeeID;

        #endregion

        #region public properties

        public int IdAccount { get { return _IdAccount; } set { _IdAccount = value; } }
        public string User { get { return _User; } set { _User = value; } }
        public string Password { get { return _Password; } set { _Password = value; } }
        public int EmployeeID { get { return _EmployeeID; } set { _EmployeeID = value; } }

        #endregion

        #region method

        /// <summary>
        /// Hàm trả về Id của nhân viên tương ứng với Account nếu như tồn tại trong csdl
        /// </summary>
        /// <param name="Account"></param>
        /// <returns></returns>
        public int IsAccount(CAccount Account)
        {
            try
            {
                var find = DataProvider.Ins.DB.Employee_Account.Where(x => x.Account_User == Account.User && x.Account_Password == Account.Password).FirstOrDefault();

                if (find != null)
                {
                    return find.Employee_Id;
                }
            }
            catch
            {

            }
            return 0;
        }

        /// <summary>
        /// Hàm trả về quyền truy cập của tài khoản nhân viên theo theo id nhân viên
        /// </summary>
        /// <param name="Employee_Id"></param>
        /// <returns>trả về 0 khi nhân viên không tồn tại, trả về 1 khi nhân viên có quyền truy cập tồn bộ hệ thống
        /// trả về 2 khi nhân viên có quyền truy cập quản lý kho, trả về 3 khi nhân viên có quyền truy cập quản lý bán hàng
        /// Trả về 4 khi nhân viên có toàn quyền truy cập hệ thống</returns>
        public int Decentralization(int Employee_Id)
        {
            try
            {
                var find = DataProvider.Ins.DB.Employees.Find(Employee_Id).Employee_Role.Decentralization1.Decentralization_Id;
                return find;
            }
            catch
            {

            }
            return 0;
        }

        /// <summary>
        /// Hàm trả về danh sách account trong csdl
        /// </summary>
        public List<CAccount> ListAccount()
        {

            List<CAccount> List = new List<CAccount>();
            try
            {
                var data = DataProvider.Ins.DB.Employee_Account;
                if (data.Count() > 0)
                {
                    foreach (var item in data)
                    {
                        CAccount account = new CAccount
                        {
                            IdAccount = item.Account_Id,
                            User = item.Account_User,
                            Password = item.Account_Password,
                            EmployeeID = item.Employee_Id,
                        };
                        //Thêm vào danh sách
                        List.Add(account);
                    }
                }

               
            }
            catch
            {

            }
            return List;
        }

        /// <summary>
        /// Hàm reset mới password của tìa khoản nhân viên
        /// </summary>
        public bool ResetPassword(CAccount account)
        {
            try
            {
                var find = DataProvider.Ins.DB.Employee_Account.Find(account.IdAccount);
                if (find != null)
                {
                    string newpass = Help.RandomPassword();
                    newpass = Help.Base64Encode(newpass); //mã hóa 
                    find.Account_Password = newpass;
                    //Lưu lại
                    DataProvider.Ins.DB.SaveChanges();
                    return true;
                }
                else return false;
            }
            catch
            {

            }
            return false;
        }

        public bool ChangePassword(int Employee_Id,string newPass)
        {
            try
            {
                var find = DataProvider.Ins.DB.Employee_Account.Where(x => x.Employee_Id == Employee_Id).First();
                if (find != null)
                {
                    //Thay đổi
                    find.Account_Password = Help.Base64Encode(newPass);

                    //Lưu lại
                    DataProvider.Ins.DB.SaveChanges();

                    return true;
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
