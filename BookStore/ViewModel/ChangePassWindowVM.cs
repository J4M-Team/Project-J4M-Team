using BookStore.Model.MyClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace BookStore.ViewModel
{
    public class ChangePassWindowVM:BaseViewModel
    {
        #region data binding

        private CEmployee _Employee;
        public CEmployee Employee
        {
            get { return _Employee; }
            set
            {
                _Employee = value;
                OnPropertyChanged(nameof(Employee));
            }
        }

        private string _OldPassWord;
        public string OldPassWord
        {
            get { return _OldPassWord; }
            set
            {
                _OldPassWord = value;
                OnPropertyChanged(nameof(OldPassWord));
            }
        }

        private string _NewPassWord;
        public string NewPassWord
        {
            get { return _NewPassWord; }
            set
            {
                _NewPassWord = value;
                OnPropertyChanged(nameof(NewPassWord));
            }
        }

        private string _ComfirmPassWord;
        public string ComfirmPassWord
        {
            get { return _ComfirmPassWord; }
            set
            {
                _ComfirmPassWord = value;
                OnPropertyChanged(nameof(ComfirmPassWord));
            }
        }

        #endregion

        #region command binding

        public ICommand AcceptCommand { get; set; }
        public ICommand OldPassCommand { get; set; }
        public ICommand NewPassCommand { get; set; }
        public ICommand ComfirmPassCommand { get; set; }
        public ICommand loadCommand { get; set; }

        #endregion

        #region method

        public ChangePassWindowVM()
        {
            loadCommand = new RelayCommand<PasswordBox>((p) => { return true; }, (p) =>
            {
                if (DataTransfer.Employee_Id > 0)
                {
                    Employee = CEmployee.Ins.EmployeeInfo(DataTransfer.Employee_Id);
                }
            }
               );

            AcceptCommand = new RelayCommand<PasswordBox>((p) => {
                if (string.IsNullOrEmpty(OldPassWord) || string.IsNullOrEmpty(NewPassWord) || string.IsNullOrEmpty(ComfirmPassWord))
                {
                    return false;
                }
                return true;
            }, 
            (p) =>
            {
                if(string.IsNullOrEmpty(OldPassWord) || string.IsNullOrEmpty(NewPassWord) || string.IsNullOrEmpty(ComfirmPassWord))
                {
                    return;
                }
                //Kiểm tra mật khẩu mới nhập có đúng hay không
                if(Help.Base64Encode(OldPassWord) != Employee.Acount.Password)
                {
                    System.Windows.MessageBox.Show("Mật khẩu không đúng, vui lòng nhập lại");
                    return;
                }

                //Kiểm tra mật khẩu mới có trùng với mật khẩu xác nhận hay không
                if (NewPassWord != ComfirmPassWord)
                {
                    System.Windows.MessageBox.Show("Mật khẩu xác nhận không đúng");
                    return;
                }

                //Thay đổi mật khẩu
                if (CAccount.Ins.ChangePassword(DataTransfer.Employee_Id, NewPassWord)==true)
                {
                    System.Windows.MessageBox.Show("Thay đổi thành công");
                }
                else
                {
                    System.Windows.MessageBox.Show("Thay đổi thất bại");
                }

                //trả về trắng thông tin
                NewPassWord = "";
                OldPassWord = "";
                ComfirmPassWord = "";
            }
               );

            OldPassCommand = new RelayCommand<PasswordBox>((p) => { return true; }, (p) =>
            {
                OldPassWord = p.Password;
            }
               );

            NewPassCommand = new RelayCommand<PasswordBox>((p) => { return true; }, (p) =>
            {
                NewPassWord = p.Password;
            }
               );

            ComfirmPassCommand = new RelayCommand<PasswordBox>((p) => { return true; }, (p) =>
            {
                ComfirmPassWord = p.Password;
            }
               );
        }

        #endregion
    }
}
