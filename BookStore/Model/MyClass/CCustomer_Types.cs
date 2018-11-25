using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Model.MyClass
{
    public class CCustomer_Types
    {
        #region design pattern singleton

        private static CCustomer_Types _ins;
        public static CCustomer_Types Ins
        {
            get
            {
                if (_ins == null)
                    _ins = new CCustomer_Types();
                return _ins;
            }
            set
            {
                _ins = value;
            }
        }

        #endregion

        #region private properties

        private int _Id;
        private string _Name;
        private float _Promotion;

        #endregion

        #region public private properties

        public int Id { get { return _Id; } set { _Id = value; } }
        public string Name { get { return _Name; } set { _Name = value; } }
        public float Promotion { get { return _Promotion; } set { _Promotion = value; } }

        #endregion
    }
}
