using BookStore.Model;
using BookStore.Model.MyClass;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BookStore.ViewModel
{
    class AddAccountEmployeeVM : BaseViewModel
    {
        #region data binding

        #endregion

        #region properties binding
        //Thông tin tài khoản vừa được khởi tạo

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

        private string _Password;
        public string Password
        {
            get { return _Password; }
            set
            {
                _Password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        private string _PasswordAgain;
        public string PasswordAgain
        {
            get { return _PasswordAgain; }
            set
            {
                _PasswordAgain = value;  
                OnPropertyChanged(nameof(PasswordAgain));
            }
        }

        #endregion

        #region command binding


        public ICommand AddAccountCommand { get; set; }
        public ICommand PasswordCommand { get; set; }
        public ICommand PasswordAgainCommand { get; set; }



        #endregion

        public AddAccountEmployeeVM()
        {

            AddAccountCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {

                if (string.IsNullOrEmpty(UserName) || string.IsNullOrEmpty(Password) || string.IsNullOrEmpty(PasswordAgain))
                {
                    return;
                }
                else
                {
                    //Tìm trong csdl tên đăng nhập có tồn tại không
                    var find = DataProvider.Ins.DB.Employee_Account.Where(x => x.Account_User == UserName).FirstOrDefault();
                    if (find != null)
                    {
                        MessageBox.Show("Tên tài khoản đã được sử dụng", "Thông báo");
                        return;
                    }
                    //Kiểm tra việc nhập lại password đúng không
                    if (Password != PasswordAgain)
                    {
                        MessageBox.Show("Mật khẩu nhập lại không đúng !!", "Thông báo");
                        return;
                    }

                    Employee_Account account = new Employee_Account
                    {
                        Account_User = UserName,
                        Account_Password = Password,
                        Employee_Id = DataTransfer.IDEmployee,
                    };

                    //Thêm vào danh sách account trong csdl
                    DataProvider.Ins.DB.Employee_Account.Add(account);

                    //Lưu lại
                    DataProvider.Ins.DB.SaveChanges();

                    MessageBox.Show("Đã thêm account mới cho nhân viên", "Thông báo");

                }

            }
                );
            PasswordCommand = new RelayCommand<PasswordBox>((p) => { return true; }, (p) =>
            {
                Password = p.Password;
            }
              );
            PasswordAgainCommand = new RelayCommand<PasswordBox>((p) => { return true; }, (p) =>
            {
                PasswordAgain = p.Password;
            }
              );
        }
    }
}
