using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
namespace BookStore.ViewModel
{
    public class InHoaDonVM:BaseViewModel

    {
        public ICommand AddBookCommand { get; set; }
        public InHoaDonVM()
        {
            AddBookCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                AddBookBillInformation wd = new AddBookBillInformation();
                object c=wd.DataContext;
                wd.ShowDialog();
                
            }
              );
        }
    }
}
