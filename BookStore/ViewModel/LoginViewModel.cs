using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using BookStore.Model.MyClass;
using System.Windows.Controls;

namespace BookStore.ViewModel
{
    public class LoginViewModel : BaseViewModel
    {
        #region data binding

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

        private string _PassWord;
        public string PassWord
        {
            get { return _PassWord; }
            set { _PassWord = value; }
        }

        #endregion

        #region command binding

        public ICommand LoginCommand { get; set; }
        public ICommand PasswordChangedCommand { get; set; }

        #endregion

        public LoginViewModel()
        {
            LoginCommand = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                
                if (UserName == "1")
                {
                    p.Hide();
                    BookWindow wd = new BookWindow();
                    wd.ShowDialog();
                    p.Show();

                }
                else if (UserName == "2")
                {
                    p.Hide();
                    ManagementWindow wd = new ManagementWindow();
                    wd.ShowDialog();
                    p.Show();
                }
                else if(UserName =="3")
                {
                    p.Hide();
                    BanHangMain wd = new BanHangMain();
                    wd.ShowDialog();
                    p.Show();
                }
                else
                {
                    int EmployeeId = CAccount.Ins.IsAccount(new CAccount { User = UserName, Password = PassWord });
                    if (EmployeeId == 0)
                    {
                        MessageBox.Show("Sai mật khẩu hoặc tài khoản,vui lòng kiểm tra lại!");
                    }
                    else
                    {
                        //Truyền Id của nhân viên qua màn hình 2 
                        DataTransfer.Employee_Id = EmployeeId;

                        p.Hide();
                        BookWindow wd = new BookWindow();
                        wd.ShowDialog();
                        p.Show();
                    }
                }                                            
            }
                );

            PasswordChangedCommand = new RelayCommand<PasswordBox>((p) => { return true; }, (p) =>
            {
                PassWord = p.Password;
            }
                );

        }
    }
}
