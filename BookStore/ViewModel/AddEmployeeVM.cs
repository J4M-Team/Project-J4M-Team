using BookStore.Model;
using BookStore.Model.MyClass;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BookStore.ViewModel
{
    class AddEmployeeVM : BaseViewModel
    {
        #region data binding
        private ObservableCollection<string> _ListRoleName;
        public ObservableCollection<string> ListRoleName
        {
            get { return _ListRoleName; }
            set
            {
                _ListRoleName = value;
                OnPropertyChanged(nameof(ListRoleName));
            }
        }

        private string _ComboBoxSelectedItem;
        public string ComboBoxSelectedItem
        {
            get { return _ComboBoxSelectedItem; }
            set
            {
                _ComboBoxSelectedItem = value;
                OnPropertyChanged(nameof(ComboBoxSelectedItem));
            }
        }


        #endregion

        #region properties binding


        //Thông tin nhân viên mới được thêm
        private string _EmployeeName;
        public string EmployeeName
        {
            get { return _EmployeeName; }
            set
            {
                _EmployeeName = value;
                OnPropertyChanged(nameof(EmployeeName));
            }
        }

        private string _EmployeeAddress;
        public string EmployeeAddress
        {
            get { return _EmployeeAddress; }
            set
            {
                _EmployeeAddress = value;
                OnPropertyChanged(nameof(EmployeeAddress));
            }
        }

        private string _EmployeeEmail;
        public string EmployeeEmail
        {
            get { return _EmployeeEmail; }
            set
            {
                _EmployeeEmail = value;
                OnPropertyChanged(nameof(EmployeeEmail));
            }
        }

        private string _EmployeePhone;
        public string EmployeePhone
        {
            get { return _EmployeePhone; }
            set
            {
                _EmployeePhone = value;
                OnPropertyChanged(nameof(EmployeePhone));
            }
        }

        private DateTime _EmployeeBirthday;
        public DateTime EmployeeBirthday
        { 
            get { return _EmployeeBirthday; }
            set
            {
                _EmployeeBirthday = value;
                OnPropertyChanged(nameof(EmployeeBirthday));
            }
        }

        private string _EmployeeIdentity;
        public string EmployeeIdentity
        {
            get { return _EmployeeIdentity; }
            set
            {
                _EmployeeIdentity = value;
                OnPropertyChanged(nameof(EmployeeIdentity));
            }
        }

        #endregion

        #region command binding

        public ICommand loadCommand { get; set; }
        public ICommand AddEmployeeCommand { get; set; }


        #endregion

        #region method

        public AddEmployeeVM()
        {
            loadCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                //Thêm data vào combobox
                ListRoleName = new ObservableCollection<string>(CRole.Ins.ListRoleName());
                EmployeeBirthday = DateTime.Now;
            }
                );
            AddEmployeeCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                if (string.IsNullOrEmpty(EmployeeName) || string.IsNullOrEmpty(EmployeeAddress) || string.IsNullOrEmpty(EmployeeEmail) || string.IsNullOrEmpty(EmployeePhone) || string.IsNullOrEmpty(EmployeeIdentity))
                {
                    return;
                }
                else
                {
                    //Kiểm tra nhập số cho SĐT và CMND
                    double test;
                    if(!double.TryParse(EmployeePhone, out test) || !double.TryParse(EmployeeIdentity, out test))
                    {
                        MessageBox.Show("Cần nhập SĐT hoặc CMND bằng số !!", "Thông báo");
                        return;
                    }

                    //Kiểm tra nhập email

                    string strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" 
                                           + @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" 
                                           + @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
                    Regex re = new Regex(strRegex);
                    if (!re.IsMatch(EmployeeEmail))
                    {
                        MessageBox.Show("Nhập sai định dạng email !!", "Thông báo");
                        return;
                    }

                    //Tạo mới CRole
                    CRole role = new CRole
                    {
                        Name = ComboBoxSelectedItem
                    };
                    if(role.Name == null)
                    {
                        MessageBox.Show("Chưa nhập loại nhân viên !!", "Thông báo");
                        return;
                    }
                    CEmployee Employee = new CEmployee

                    {
                        Name = EmployeeName,
                        Address = EmployeeAddress,
                        Email = EmployeeEmail,
                        Phone = EmployeePhone,
                        BirthDay = EmployeeBirthday,
                        Identity = EmployeeIdentity,
                        Role = role,
                        Info = new CEmployee_Info
                        {
                            DateStart = DateTime.Now,
                            DateWork = 0,
                            SumDay = 0
                        }
                    };

                    //Thêm vào cơ sở dữ liệu
                    CEmployee.Ins.AddEmployee(Employee);

                    MessageBox.Show("Thêm vào thành công ", "Thông báo !!");

                    //Lấy ID đc khởi tạo tự động trong cơ sở dữ liệu để truyền qua cửa sổ tạo account
                    int employeeID = DataProvider.Ins.DB.Employees.Where(x => x.Employee_Identity == Employee.Identity).Select(x => x.Employee_Id).First();
                    //Hiện cửa sổ thêm account cho nhân viên
                    DataTransfer.IDEmployee =  employeeID;
                    AddAccountEmployee wd = new AddAccountEmployee();
                    wd.ShowDialog();



                }


            }
                );
        }

        #endregion
    }
}

