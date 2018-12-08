using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Model.MyClass
{
    public class CHuman
    {
        #region private properties

        private int _Id;
        private string _Name;
        private string _Address;
        private string _Phone;
        private string _Email;

        #endregion

        #region public properties

        public int Id { get { return _Id; } set { _Id = value; } }
        public string Name { get { return _Name; } set { _Name = value; } }
        public string Address { get { return _Address; } set { _Address = value; } }
        public string Phone { get { return _Phone; } set { _Phone = value; } }
        public string Email { get { return _Email; } set { _Email = value; } }

        #endregion

        #region constructor
        public CHuman()
        {

        }

        public CHuman(string Name, string Address, string Phone, string Email)
        {

            this.Name = Name;
            this.Address = Address;
            this.Phone = Phone;
            this.Email = Email;
        }

        #endregion
    }
}
