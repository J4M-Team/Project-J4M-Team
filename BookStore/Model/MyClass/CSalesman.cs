using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Model.MyClass
{
    public class CSalesman : CEmployee
    {
        #region design pattern singleton

        private static CSalesman _ins;
        public static CSalesman Ins_Salesman
        {
            get
            {
                if (_ins == null)
                    _ins = new CSalesman();
                return _ins;
            }
            set
            {
                _ins = value;
            }
        }

        #endregion

    }
}
