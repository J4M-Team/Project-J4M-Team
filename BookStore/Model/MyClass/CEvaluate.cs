using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Model.MyClass
{
    public class CEvaluate
    {
        #region design pattern singleton

        private static CEvaluate _ins;
        public static CEvaluate Ins
        {
            get
            {
                if (_ins == null)
                    _ins = new CEvaluate();
                return _ins;
            }
            set
            {
                _ins = value;
            }
        }

        #endregion

        #region private properties

        private CCustomer _Customer;
        private CEmployee _Employee;
        private DateTime _Date;
        private int _Content;

        #endregion

        #region public properties

        public CCustomer Customer { get { return _Customer; } set { _Customer = value; } }
        public CEmployee Employee { get { return _Employee; } set { _Employee = value; } }
        public DateTime Date { get { return _Date; } set { _Date = value; } }
        public int Content { get { return _Content; } set { _Content = value; } }

        #endregion
    }
}
