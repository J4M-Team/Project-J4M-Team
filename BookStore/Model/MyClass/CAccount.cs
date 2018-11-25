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
    }
}
