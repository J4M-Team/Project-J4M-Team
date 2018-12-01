using BookStore.Model.MyClass;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;

namespace BookStore.ViewModel
{
    public class AccepBillPageVM:BaseViewModel
    {
        #region data binding

        private ObservableCollection<CBill> _ListBill;
        public ObservableCollection<CBill> ListBill
        {
            get { return _ListBill; }
            set
            {
                _ListBill = value;
                OnPropertyChanged(nameof(ListBill));
            }
        }

        private CBill _SelectedItem;
        public CBill SelectedItem
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

        public ICommand loadCommand { get; set; }
        public ICommand AccepCommand { get; set; }

        #endregion

        public AccepBillPageVM()
        {
            loadCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                ListBill = new ObservableCollection<CBill>(CBill.Ins.ListNewBill());
            }
               );

            AccepCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                if (SelectedItem != null)
                {
                    MessageBox.Show(SelectedItem.Id.ToString());
                }
                else
                {
                   //2 sự kiện bấm button và selectionchange bị gọi chồng vào nhau
                }
               
            }
               );
        }
    }
}
