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
      
        private ComboBoxItem _SelectedItem;
        public ComboBoxItem SelectedItem
        {
            get { return _SelectedItem; }
            set
            {
                _SelectedItem = value;
                OnPropertyChanged(nameof(SelectedItem));
            }
        }

        #endregion

        #region command binding

        public ICommand LoginCommand { get; set; }
        public ICommand PasswordChangedCommand { get; set; }
        public ICommand CloseCommand { get; set; }

        #endregion

        public LoginViewModel()
        {
            LoginCommand = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
              
                if (!string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty(PassWord))
                {
                    if (SelectedItem != null)
                    {
                        PassWord = Help.Base64Encode(PassWord);
                        int EmployeeId = CAccount.Ins.IsAccount(new CAccount { User = UserName, Password = PassWord });
                        //Kiểm tra tài khoản có tồn tại trong cơ sở dữ liệu
                        if (EmployeeId == 0)
                        {
                            MessageBox.Show("Sai mật khẩu hoặc tài khoản,vui lòng kiểm tra lại!");
                        }
                        else
                        {
                            //Kiểm tra quyền của tài khoản
                            if (SelectedItem.Content.ToString() == "Màn hình quản lý")
                            {
                                if (4 == CAccount.Ins.Decentralization(EmployeeId) || 1 == CAccount.Ins.Decentralization(EmployeeId))
                                {
                                    //Truyền Id của nhân viên qua màn hình 2 
                                    DataTransfer.Employee_Id = EmployeeId;

                                    //CheckIn
                                    CEmployee.Ins.CheckIn(EmployeeId);

                                    p.Hide();
                                    ManagementWindow wd = new ManagementWindow();
                                    
                                    wd.ShowDialog();
                                    p.Show();
                                }
                                else
                                {
                                    MessageBox.Show("Tài khoản của bạn không có quyền truy cập chức năng này");
                                }

                            }
                            else if (SelectedItem.Content.ToString() == "Màn hình quản lý kho")
                            {
                                if (4 == CAccount.Ins.Decentralization(EmployeeId) || 1 == CAccount.Ins.Decentralization(EmployeeId)|| 2 == CAccount.Ins.Decentralization(EmployeeId))
                                {
                                    //Truyền Id của nhân viên qua màn hình 2 
                                    DataTransfer.Employee_Id = EmployeeId;

                                    //CheckIn
                                    CEmployee.Ins.CheckIn(EmployeeId);

                                    p.Hide();
                                    BookWindow wd = new BookWindow();
                                    wd.ShowDialog();
                                    p.Show();
                                }
                                else
                                {
                                    MessageBox.Show("Tài khoản của bạn không có quyền truy cập chức năng này");
                                }

                            }
                            else if (SelectedItem.Content.ToString() == "Màn hình bán hàng")
                            {
                                if (4 == CAccount.Ins.Decentralization(EmployeeId) || 1 == CAccount.Ins.Decentralization(EmployeeId) || 3 == CAccount.Ins.Decentralization(EmployeeId))
                                {
                                    //Truyền Id của nhân viên qua màn hình 2 
                                    DataTransfer.Employee_Id = EmployeeId;

                                    //CheckIn
                                    CEmployee.Ins.CheckIn(EmployeeId);

                                    p.Hide();
                                    BanHangMain wd = new BanHangMain();
                                    wd.ShowDialog();
                                    p.Show();
                                }
                                else
                                {
                                    MessageBox.Show("Tài khoản của bạn không có quyền truy cập chức năng này");
                                }
                            }
                            else if (SelectedItem.Content.ToString() == "Màn hình tìm kiếm sách")
                            {
                                //Chưa code
                            }
                            
                        }                       
                    }
                    else
                    {
                        MessageBox.Show("Vui lòng chọn màn hình cần đăng nhập");
                    }
                }
                else
                {
                    MessageBox.Show("Vui lòng điền đầy đủ thông tin");
                }                             
            }
                );

            PasswordChangedCommand = new RelayCommand<PasswordBox>((p) => { return true; }, (p) =>
            {
                PassWord = p.Password;
            }
                );

            CloseCommand = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                //đóng cửa sổ
                p.Close();
            }
                );

        }
    }
}
