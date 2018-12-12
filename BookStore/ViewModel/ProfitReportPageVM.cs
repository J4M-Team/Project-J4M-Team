using BookStore.Model.MyClass;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BookStore.ViewModel
{
    public class ProfitReportPageVM:BaseViewModel
    {
        #region data binding

        private ObservableCollection<CReport> _ListReport;
        public ObservableCollection<CReport> ListReport
        {
            get { return _ListReport; }
            set
            {
                _ListReport = value;
                OnPropertyChanged(nameof(ListReport));
            }
        }

        #endregion

        #region command binding

        public ICommand loadCommand { get; set; }

        #endregion

        #region method

        public ProfitReportPageVM()
        {
            loadCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                ListReport = new ObservableCollection<CReport>(CReport.Ins.ListReport());
            }
              );
        }

        #endregion
    }
}
