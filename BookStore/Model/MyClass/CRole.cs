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

        private string _Name;
        private float _Salary;

        #endregion

        #region public properties

        public string Name { get { return _Name; } set { _Name = value; } }
        public float Salary { get { return _Salary; } set { _Salary = value; } }

        #endregion
    }
}
