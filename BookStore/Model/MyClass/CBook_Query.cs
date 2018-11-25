using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Model.MyClass
{
    public class CBook_Query
    {
        #region design pattern singleton

        private static CBook_Query _ins;
        public static CBook_Query Ins
        {
            get
            {
                if (_ins == null)
                    _ins = new CBook_Query();
                return _ins;
            }
            set
            {
                _ins = value;
            }
        }

        #endregion

        #region private properties

        private CBook _Book;
        private DateTime _Date;

        #endregion

        #region public properties

        public CBook Book { get { return _Book; } set { _Book = value; } }
        public DateTime Date { get { return _Date; } set { _Date = value; } }

        #endregion
    }
}
