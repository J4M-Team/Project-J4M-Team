using BookStore.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Model.MyClass
{
    public class CBook_Price:BaseViewModel
    {
        #region private properties

        private float _InputPrice;
        private DateTime _Input_Date_Set;
        private float _OutputPrice;
        private DateTime _Output_Date_Set;

        #endregion

        #region public properties

        public float InputPrice { get { return _InputPrice; } set { _InputPrice = value;OnPropertyChanged(nameof(InputPrice)); } }
        public float OutputPrice { get { return _OutputPrice; } set { _OutputPrice = value;OnPropertyChanged(nameof(OutputPrice)); } }
        public DateTime Input_Date_Set { get { return _Input_Date_Set; } set { _Input_Date_Set = value; } }
        public DateTime Output_Date_Set { get { return _Output_Date_Set; } set { _Output_Date_Set = value; } }

        #endregion
    }
}
