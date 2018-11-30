using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;

namespace BookStore.ViewModel
{
    public class LoginViewModel : BaseViewModel
    {
        public ICommand LoginCommand { get; set; }
        // public string txt
        private string _UserName;
        public string UserName
        {
            get { return _UserName; }
            set
            {


                _UserName = value;
                OnPropertyChanged(nameof(UserName));

            }
        }
        

        public LoginViewModel()
        {
            LoginCommand = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                p.Hide();
                if (UserName == "1")
                {
                    BookWindow wd = new BookWindow();
                    wd.ShowDialog();
                }
                else
                {
                    BanHangMain wd = new BanHangMain();
                    wd.ShowDialog();
                }
                
                p.Show();
            }
                );
           // txt = "qui";
        }
    }
}
