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

        private string _User;
        private string _Password;

        #endregion

        #region public properties

        public string User { get { return _User; } set { _User = value; } }
        public string Password { get { return _Password; } set { _Password = value; } }

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

        #endregion
    }
}
