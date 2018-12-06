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
    public class SettingEmployeePageVM:BaseViewModel
    {
        #region data binding

        private ObservableCollection<CRole> _ListRole;
        public ObservableCollection<CRole> ListRole
        {
            get { return _ListRole; }
            set
            {
                _ListRole = value;
                OnPropertyChanged(nameof(ListRole));
            }
        }
        private ObservableCollection<string> _ListDecentralization;
        public ObservableCollection<string> ListDecentralization
        {
            get { return _ListDecentralization; }
            set
            {
                _ListDecentralization = value;
                OnPropertyChanged(nameof(ListDecentralization));
            }
        }

        private CRole _SelectedItem;
        public CRole SelectedItem
        {
            get { return _SelectedItem; }
            set
            {
                _SelectedItem = value;
                OnPropertyChanged(nameof(SelectedItem));
            }
        }

        //Tên loại nhân viên mới
        private string _EmployeeRole;
        public string EmployeeRole
        {
            get { return _EmployeeRole; }
            set
            {
                _EmployeeRole = value;
                OnPropertyChanged(nameof(EmployeeRole));
            }
        }

        //Lương của loại nhân viên mới
        private string _EmployeeSalary;
        public string EmployeeSalary
        {
            get { return _EmployeeSalary; }
            set
            {
                _EmployeeSalary = value;
                OnPropertyChanged(nameof(EmployeeSalary));
            }
        }

        //Quyên hạn của loại nhân viên mới
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

        //Thuộc tính ischecked của toggle button
        private bool _IsChecked;
        public bool IsChecked
        {
            get { return _IsChecked; }
            set
            {
                _IsChecked = value;
                OnPropertyChanged(nameof(IsChecked));
            }
        }

        //https://social.msdn.microsoft.com/Forums/vstudio/en-US/c7b3f3d2-5842-4460-bc3c-dda4073eab51/setting-a-datagrid-columns-readonly-property-to-a-binding?forum=wpf
        //binding thuộc tính isreadOnly của cột hiển thị lương trong datagrid
        private bool _SalaryReadOnly;
        public bool SalaryReadOnly
        {
            get { return _SalaryReadOnly; }
            set
            {
                _SalaryReadOnly = value;
                OnPropertyChanged(nameof(SalaryReadOnly));
            }
        }

        //Thuộc tính isreadOnly của cột hiển thị loại nhân viên
        private bool _NameReadOnly;
        public bool NameReadOnly
        {
            get { return _NameReadOnly; }
            set
            {
                _NameReadOnly = value;
                OnPropertyChanged(nameof(NameReadOnly));
            }
        }

        //Thuộc tính ẩn của cột button
        private Visibility _EditColumnVisibility;
        public Visibility EditColumnVisibility
        {
            get { return _EditColumnVisibility; }
            set
            {
                _EditColumnVisibility = value;
                OnPropertyChanged(nameof(EditColumnVisibility));
            }
        }

        //Thuộc tính ẩn cột quyền hạn mới (combobox)
        private Visibility _ComboboxColumnVisibility;
        public Visibility ComboboxColumnVisibility
        {
            get { return _ComboboxColumnVisibility; }
            set
            {
                _ComboboxColumnVisibility = value;
                OnPropertyChanged(nameof(ComboboxColumnVisibility));
            }
        }

        //Thuộc tính ẩn cột quyền hạn cũ (textbox)
        private Visibility _TextBlockColumnVisibility;
        public Visibility TextBlockColumnVisibility
        {
            get { return _TextBlockColumnVisibility; }
            set
            {
                _TextBlockColumnVisibility = value;
                OnPropertyChanged(nameof(TextBlockColumnVisibility));
            }
        }


        #endregion


        #region command binding

        public ICommand loadCommand { get; set; }
        public ICommand CheckCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand AddRoleCommand { get; set; }

        #endregion

        #region method

        public SettingEmployeePageVM()
        {
            loadCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                //Không cho chỉnh sửa
                SalaryReadOnly = true;
                NameReadOnly = true;

                //Ẩn cột chỉnh sửa
                EditColumnVisibility = Visibility.Hidden;

                //Ẩn cột Combobox trong datagrid
                ComboboxColumnVisibility = Visibility.Hidden;

                //Hiện cột text
                TextBlockColumnVisibility = Visibility.Visible;

                //Ẩn chỉnh sửa
                IsChecked = false;

                //lấy data từ cơ sở dữ liệu
                ListRole = new ObservableCollection<CRole>(CRole.Ins.ListRole());

                //Thêm data vào combobox
                ListDecentralization = new ObservableCollection<string>(CRole.Ins.ListDecentralization());

                
            }
               );

            CheckCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                
                if (IsChecked == true)
                {
                    //Hiện cột button
                    EditColumnVisibility = Visibility.Visible;

                    //Hiện cột combobox
                    ComboboxColumnVisibility = Visibility.Visible;

                    //ẩn cột text
                    TextBlockColumnVisibility = Visibility.Hidden;

                    SalaryReadOnly = false;

                    NameReadOnly = false;
                }
                else
                {
                    //Ẩn cột button và cột combobox, hiện cột text
                    EditColumnVisibility = Visibility.Hidden;
                    ComboboxColumnVisibility = Visibility.Hidden;
                    TextBlockColumnVisibility = Visibility.Visible;
                    SalaryReadOnly = true;
                    NameReadOnly = true;
                }
                              
            }
               );

            EditCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                if (SelectedItem != null)
                {
                    CRole.Ins.UpdateRole(SelectedItem);              
                }          
            }
               );

            AddRoleCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                if (string.IsNullOrEmpty(EmployeeRole) || string.IsNullOrEmpty(EmployeeSalary) || string.IsNullOrEmpty(ComboBoxSelectedItem))
                {
                    return;
                }
                else
                {
                    //Kiểm tra
                    float RoleSalary;

                    if(float.TryParse(EmployeeSalary,out RoleSalary) == true)
                    {
                        //Tạo mới CRole
                        CRole Role = new CRole { Name = EmployeeRole, Salary = RoleSalary, Decentralization = ComboBoxSelectedItem };

                        //Thêm vào cơ sở dữ liệu
                        CRole.Ins.AddRole(Role);

                        //load lại bảng
                        ListRole = new ObservableCollection<CRole>(CRole.Ins.ListRole());
                    }
                    else
                    {
                        return;
                    }
                }
                
            }
               );
        }

        #endregion
    }
}
