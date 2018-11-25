using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Model.MyClass
{
    public class CBill
    {
        #region design pattern singleton

        private static CBill _ins;
        public static CBill Ins
        {
            get
            {
                if (_ins == null)
                    _ins = new CBill();
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
        private CSalesman _Salesman;
        private CCustomer _Customer;
        private float _TotalMoney;
        private int _Type;
        private float _Promotion;
        private DateTime _Date;
        private List<CBook> _ListBook;

        #endregion

        #region public properties

        public int Id { get { return _Id; } set { _Id = value; } }
        public CSalesman Salesman { get { return _Salesman; } set { _Salesman = value; } }
        public CCustomer Customer { get { return _Customer; } set { _Customer = value; } }
        public float TotalMoney { get { return _TotalMoney; } set { _TotalMoney = value; } }
        public int Type { get { return _Type; } set { _Type = value; } }
        public float Promotion { get { return _Promotion; } set { _Promotion = value; } }
        public DateTime Date { get { return _Date; } set { _Date = value; } }
        public List<CBook> ListBook { get { return _ListBook; } set { _ListBook = value; } }

        #endregion
    }
}
