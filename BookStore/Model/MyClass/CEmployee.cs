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
       


    }
}
