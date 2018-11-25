using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Model.MyClass
{
    public class CWarehouse : CEmployee
    {
        #region design pattern singleton

        private static CWarehouse _ins;
        public static CWarehouse Ins_Warehouse
        {
            get
            {
                if (_ins == null)
                    _ins = new CWarehouse();
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
