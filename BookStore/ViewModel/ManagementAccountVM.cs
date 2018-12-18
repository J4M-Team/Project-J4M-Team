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
    class ManagementAccountVM : BaseViewModel
    {
        #region data binding

        private ObservableCollection<CAccount> _ListAccount;
        public ObservableCollection<CAccount> ListAccount
        {
            get { return _ListAccount; }
            set
            {
                _ListAccount = value;
                OnPropertyChanged(nameof(ListAccount));
            }
        }

        private CAccount _SelectedItem;
        public CAccount SelectedItem
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

        #endregion

        #region command binding
        public ICommand loadCommand { get; set; }
        public ICommand ResetCommand { get; set; }
        #endregion

        public ManagementAccountVM()
        {
            loadCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                //lấy data từ cơ sở dữ liệu
                ListAccount = new ObservableCollection<CAccount>(CAccount.Ins.ListAccount());

            }
               );
            ResetCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                //Reset lại mật khẩu 
                if (SelectedItem != null)
                {
                    CAccount.Ins.ResetPassword(SelectedItem);
                }
                //load lại cơ sở dữ liệu
                ListAccount = new ObservableCollection<CAccount>(CAccount.Ins.ListAccount());

            }
               );
        }

    }
}
