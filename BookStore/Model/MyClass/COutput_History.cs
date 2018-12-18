using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Model.MyClass
{
    public class COutput_History
    {
        #region private properties

        private int _Id;
        private CBill _Bill;
        private CWarehouse _WareHouse;
        private DateTime _Date;

        #endregion

        #region public properties

        public int Id { get { return _Id; } set { _Id = value; } }
        public CBill Bill { get { return _Bill; } set { _Bill = value; } }
        public CWarehouse WareHouse { get { return _WareHouse; } set { _WareHouse = value; } }
        public DateTime Date { get { return _Date; } set { _Date = value; } }

        #endregion

    }
}
