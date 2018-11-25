using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Model.MyClass
{
    public class CEmployee_Info
    {
        #region private properties

        private DateTime _DateStart;
        private int _SumDay;
        private int _DateWork;


        #endregion

        #region public properties

        public DateTime DateStart { get { return _DateStart; } set { _DateStart = value; } }
        public int SumDay { get { return _SumDay; } set { _SumDay = value; } }
        public int DateWork { get { return _DateWork; } set { _DateWork = value; } }

        #endregion
    }
}
