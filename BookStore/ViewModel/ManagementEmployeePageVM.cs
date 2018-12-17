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
    class ManagementEmployeePageVM : BaseViewModel
    {
        #region data binding

        private ObservableCollection<CEmployee> _ListEmployee;
        public ObservableCollection<CEmployee> ListEmployee
        {
            get { return _ListEmployee; }
            set
            {
                _ListEmployee = value;
                OnPropertyChanged(nameof(ListEmployee));
            }
        }

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

        private CEmployee _SelectedItem;
        public CEmployee SelectedItem
        {
            get { return _SelectedItem; }
            set
            {

                _SelectedItem = value;
                OnPropertyChanged(nameof(SelectedItem));
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

        private bool _AddressReadOnly;
        public bool AddressReadOnly
        {
            get { return _AddressReadOnly; }
            set
            {
                _AddressReadOnly = value;
                OnPropertyChanged(nameof(AddressReadOnly));
            }
        }

        private bool _PhoneReadOnly;
        public bool PhoneReadOnly
        {
            get { return _PhoneReadOnly; }
            set
            {
                _PhoneReadOnly = value;
                OnPropertyChanged(nameof(PhoneReadOnly));
            }
        }

        private bool _EmailReadOnly;
        public bool EmailReadOnly
        {
            get { return _EmailReadOnly; }
            set
            {
                _EmailReadOnly = value;
                OnPropertyChanged(nameof(EmailReadOnly));
            }
        }

        

        private bool _IdentityReadOnly;
        public bool IdentityReadOnly
        {
            get { return _IdentityReadOnly; }
            set
            {
                _IdentityReadOnly = value;
                OnPropertyChanged(nameof(IdentityReadOnly));
            }
        }

        //Thuộc tính ẩn cột loại mới (combobox)
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

        //Thuộc tính ẩn cột loại cũ (textbox)
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



        private CRole _EmployeeRole;
        public CRole EmployeeRole
        {
            get { return _EmployeeRole; }
            set
            {
                _EmployeeRole = value;
                OnPropertyChanged(nameof(EmployeeRole));
            }
        }



        #endregion

        #region command binding

        public ICommand loadCommand { get; set; }
        public ICommand CheckCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand AddEmployeeCommand { get; set; }

        #endregion

        public ManagementEmployeePageVM()
        {
            loadCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                //Không cho chỉnh sửa      
                NameReadOnly = true;
                AddressReadOnly = true;
                PhoneReadOnly = true;
                EmailReadOnly = true;
               
                IdentityReadOnly = true;

                //Ẩn cột Combobox trong datagrid
                ComboboxColumnVisibility = Visibility.Hidden;

                //Hiện cột text
                TextBlockColumnVisibility = Visibility.Visible;

                //Ẩn cột chỉnh sửa
                EditColumnVisibility = Visibility.Hidden;

                //Ẩn chỉnh sửa
                IsChecked = false;

                //Thêm data vào combobox
                ListRoleName = new ObservableCollection<string>(CRole.Ins.ListRoleName());

                //lấy data từ cơ sở dữ liệu
                ListEmployee = new ObservableCollection<CEmployee>(CEmployee.Ins.ListEmployee());

            }
                );
            CheckCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {

                if (IsChecked == true)
                {
                    //Hiện cột button edit
                    EditColumnVisibility = Visibility.Visible;

                    NameReadOnly = false;
                    AddressReadOnly = false;
                    PhoneReadOnly = false;
                    EmailReadOnly = false;
                   
                    IdentityReadOnly = false;

                    //Hiện cột combobox
                    ComboboxColumnVisibility = Visibility.Visible;

                    //Ẩn cột text
                    TextBlockColumnVisibility = Visibility.Hidden;

                }
                else
                {
                    //Ẩn cột button
                    EditColumnVisibility = Visibility.Hidden;

                    NameReadOnly = true;
                    AddressReadOnly = true;
                    PhoneReadOnly = true;
                    EmailReadOnly = true;
                   
                    IdentityReadOnly = true;

                    ComboboxColumnVisibility = Visibility.Hidden;
                    TextBlockColumnVisibility = Visibility.Visible;
                }

            }
               );

            EditCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                if (SelectedItem != null)
                {
                    CEmployee.Ins.UpdateEmployee(SelectedItem);
                }
            }
               );

            AddEmployeeCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                AddEmployee wd = new AddEmployee();
                wd.ShowDialog();

                //Load lại csdl
                ListEmployee = new ObservableCollection<CEmployee>(CEmployee.Ins.ListEmployee());
            }
                );
        }

    }
}
