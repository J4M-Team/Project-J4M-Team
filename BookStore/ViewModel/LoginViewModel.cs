using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BookStore.ViewModel
{
    public class LoginViewModel : BaseViewModel
    {
        public ICommand LoginCommand { get; set; }
       // public string txt

        public LoginViewModel()
        {
            LoginCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                BookWindow wd = new BookWindow();
                wd.ShowDialog();
            }
                );
           // txt = "qui";
        }
    }
}
