using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Model.MyClass
{
    public class CBook
    {
        #region design pattern singleton

        private static CBook _ins;
        public static CBook Ins
        {
            get
            {
                if (_ins == null)
                    _ins = new CBook();
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
        private string _Author;
        private string _Theme;
        private string _Type;
        private string _Content;
        private int _Count;
        //imgage
        private CBook_Price _Price;

        #endregion

        #region public properties

        public int Id { get { return _Id; } set { _Id = value; } }
        public string Name { get { return _Name; } set { _Name = value; } }
        public string Author { get { return _Author; } set { _Author = value; } }
        public string Theme { get { return _Theme; } set { _Theme = value; } }
        public string Type { get { return _Type; } set { _Type = value; } }
        public string Content { get { return _Content; } set { _Content = value; } }
        public int Count { get { return _Count; } set { _Count = value; } }
        public CBook_Price Price { get { return _Price; } set { _Price = value; } }

        #endregion
    }
}
