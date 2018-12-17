using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Model.MyClass
{
    public class CInput_History
    {
        #region private properties

        private CEmployee _WareHouse;
        private CBook _Book;
        private DateTime _Date;

        #endregion

        #region public properties

        public CEmployee WareHouse { get { return _WareHouse; } set { _WareHouse = value; } }
        public CBook Book { get { return _Book; } set { _Book = value; } }
        public DateTime Date { get { return _Date; } set { _Date = value; } }

        #endregion

        #region method



        #endregion
    }
}
