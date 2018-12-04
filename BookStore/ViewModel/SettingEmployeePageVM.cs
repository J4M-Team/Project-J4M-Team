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

        #endregion


        #region command binding

        public ICommand loadCommand { get; set; }
        public ICommand CheckCommand { get; set; }
        public ICommand EditCommand { get; set; }

        #endregion

        public SettingEmployeePageVM()
        {
            loadCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                //Ẩn cột chỉnh sửa
                EditColumnVisibility = Visibility.Hidden;

                //Ẩn chỉnh sửa
                IsChecked = false;

                //lấy data từ cơ sở dữ liệu
                ListRole = new ObservableCollection<CRole>(CRole.Ins.ListRole());            
            }
               );

            CheckCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                
                if (IsChecked == true)
                {
                    EditColumnVisibility = Visibility.Visible;
                    SalaryReadOnly = false;
                }
                else
                {
                    EditColumnVisibility = Visibility.Hidden;                   
                    SalaryReadOnly = true;
                }
                              
            }
               );

            EditCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                if (SelectedItem != null)
                {
                    CRole.Ins.ChangeSalary(SelectedItem);
                    
                }          
            }
               );
        }
    }
}
