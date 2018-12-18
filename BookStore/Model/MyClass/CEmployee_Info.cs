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
       
        private DateTime _DateStart;
        private int _SumDay;
        private int _DateWork;
        //Tính lương trong tháng hiện tại
        private float _Salary;


        #endregion

        #region public properties
              
        public DateTime DateStart { get { return _DateStart; } set { _DateStart = value; } }
        public int SumDay{ get { return _SumDay; } set { _SumDay = value; } }
        public int DateWork { get { return _DateWork; } set { _DateWork = value; } }
        //Tính lương trong tháng hiện tại
        public float Salary { get { return _Salary; } set { _Salary = value; } }
        #endregion

        #region method
     

        #endregion
    }
}
