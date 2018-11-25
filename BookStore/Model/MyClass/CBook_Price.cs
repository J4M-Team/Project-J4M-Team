using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Model.MyClass
{
    public class CBook_Price
    {
        #region private properties

        private float _InputPrice;
        private float _OutputPrice;

        #endregion

        #region public properties

        public float InputPrice { get { return _InputPrice; } set { _InputPrice = value; } }
        public float OutputPrice { get { return _OutputPrice; } set { _OutputPrice = value; } }

        #endregion
    }
}
