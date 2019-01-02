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
    public class IncreaseBookWindowVM:BaseViewModel
    {
        #region data binding

        private ObservableCollection<CBook> _ListBook;
        public ObservableCollection<CBook> ListBook
        {
            get { return _ListBook; }
            set
            {
                _ListBook = value;
                OnPropertyChanged(nameof(ListBook));
            }
        }

        private CBook _SelectedItem;
        public CBook SelectedItem
        {
            get { return _SelectedItem; }
            set
            {
                _SelectedItem = value;
                OnPropertyChanged(nameof(SelectedItem));
            }
        }

        private int _SelectedIndex;
        public int SelectedIndex
        {
            get { return _SelectedIndex; }
            set
            {
                _SelectedIndex = value;
                OnPropertyChanged(nameof(SelectedIndex));
            }
        }

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

        private string _Name;
        public string Name
        {
            get { return _Name; }
            set
            {
                _Name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        private string _Author;
        public string Author
        {
            get { return _Author; }
            set
            {
                _Author = value;
                OnPropertyChanged(nameof(Author));
            }
        }

        private string _Count;
        public string Count
        {
            get { return _Count; }
            set
            {
                _Count = value;
                OnPropertyChanged(nameof(Count));
            }
        }

        private string _MinCount;
        public string MinCount
        {
            get { return _MinCount; }
            set
            {
                _MinCount = value;
                OnPropertyChanged(nameof(MinCount));
            }
        }

        private string _MinExist;
        public string MinExist
        {
            get { return _MinExist; }
            set
            {
                _MinExist = value;
                OnPropertyChanged(nameof(MinExist));
            }
        }

        #endregion

        #region command binding

        public ICommand AddListCommand { get; set; }
        public ICommand AddCommand { get; set; }
        public ICommand loadCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand SelectionChangedCommand { get; set; }
        public ICommand DeleteCommand { get; set; }

        #endregion

        #region method

        public IncreaseBookWindowVM()
        {
            loadCommand = new RelayCommand<object>((p) =>{ return true;},(p) =>
            {
                if (DataTransfer.Employee_Id > 0)
                {
                    EmployeeName = CEmployee.Ins.EmployeeInfo(DataTransfer.Employee_Id).Name;
                }

                MinCount = "150";
                MinExist = "300";
                ListBook = new ObservableCollection<CBook>();
            }
               );

            DeleteCommand = new RelayCommand<object>((p) => {
                if (SelectedItem == null)
                {
                    return false;
                }
                return true;
            }, 
            (p) =>
            {
                //Xóa theo index
                ListBook.RemoveAt(SelectedIndex);
            }
               );

            SelectionChangedCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                if (SelectedItem != null)
                {
                    Name = SelectedItem.Name;
                    Author = SelectedItem.Author;
                    Count = SelectedItem.Count.ToString();
                }
            }
               );

            AddCommand = new RelayCommand<object>((p) => {
                //Kiểm tra danh sách đã có dữ liệu
                if (ListBook.Count() == 0)
                {
                    return false;
                }
                return true;
            },
            (p) =>
            {
                //Duyệt từng phần tử trong list và thêm vào số lượng
                foreach(var item in ListBook)
                {
                    CBook.Ins.IncreaseNumberOfBook(item.Id, item.Count);

                    //Thêm vào lịch sử nhập kho
                    
                    if (DataTransfer.Employee_Id > 0)
                    {
                        CWarehouse.Ins_Warehouse.AddHistoryInput(DataTransfer.Employee_Id, item);
                    }              
                }

                //Thông báo thêm thành công
                System.Windows.MessageBox.Show("Thêm thành công", "Thông báo", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Asterisk);
                //Trả về trang trắng
                Name = "";
                Author = "";
                Count = "";
                ListBook.Clear();
            }
               );

            EditCommand = new RelayCommand<object>((p) => 
            {
                if (SelectedItem == null)
                {
                    return false;
                }
                //Kiểm tra chưa nhập đủ thông tin
                if (string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(Author) || string.IsNullOrEmpty(Count))
                {
                    return false;
                }

                //Kiểm tra thông tin nhập vào ở cột số lượng, lượng nhập tối thiểu, tồn kho tối thiểu không phải là số
                int ICount;
                int IMinCount;
                int IMinExist;

                if (int.TryParse(Count, out ICount) == false || int.TryParse(MinCount, out IMinCount) == false || int.TryParse(MinExist, out IMinExist) == false)
                {
                    return false;
                }

                //Tạo sách mới 
                CBook Book = new CBook { Name = Name, Author = Author, Count = ICount };

                //Kiểm tra sách có tồn tại trong kho hay chưa
                int Book_Id = CBook.Ins.isExist(Book);
                if (Book_Id == 0)
                {
                    return false;
                }

                //Kiểm tra thỏa mãn điều kiện lượng nhập tối thiểu
                if (ICount < IMinCount)
                {
                    return false;
                }

                //Kiểm tra thỏa mãn điều kiện lượng tồn tối thiểu
                if (CBook.Ins.InventoryNumber(Book_Id) > IMinExist)
                {
                    return false;
                }


                return true;
            }, 
            (p) =>
            {
                if (SelectedItem != null)
                {
                    CBook Book = new CBook { Name = Name, Author = Author };
                    //Sửa lại id sách
                    //System.Windows.MessageBox.Show(SelectedIndex.ToString());
                    ListBook[SelectedIndex].Id = CBook.Ins.isExist(Book);

                    ListBook[SelectedIndex].Name = Name;
                    ListBook[SelectedIndex].Author = Author;
                    ListBook[SelectedIndex].Count = int.Parse(Count);
                }
            }
               );

            AddListCommand = new RelayCommand<object>((p) => 
            {
                //Kiểm tra chưa nhập đủ thông tin
                if (string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(Author) || string.IsNullOrEmpty(Count))
                {
                    return false;
                }

                //Kiểm tra thông tin nhập vào ở cột số lượng, lượng nhập tối thiểu, tồn kho tối thiểu không phải là số
                int ICount;
                int IMinCount;
                int IMinExist;

                if (int.TryParse(Count, out ICount) == false || int.TryParse(MinCount, out IMinCount) == false || int.TryParse(MinExist, out IMinExist) == false)
                {
                    return false;
                }

                //Tạo sách mới 
                CBook Book = new CBook { Name = Name, Author = Author, Count = ICount };

                //Kiểm tra sách có tồn tại trong kho hay chưa
                int Book_Id = CBook.Ins.isExist(Book);
                if (Book_Id == 0)
                {
                    return false;
                }

                //Kiểm tra thỏa mãn điều kiện lượng nhập tối thiểu
                if (ICount < IMinCount)
                {
                    return false;
                }

                //Kiểm tra thỏa mãn điều kiện lượng tồn tối thiểu
                if (CBook.Ins.InventoryNumber(Book_Id) > IMinExist)
                {
                    return false;
                }

                //Kiểm tra xem có tồn tại trong List đã nhập bên dưới chưa
                if (ListBook.Count > 0)
                {
                    var find = ListBook.Where(x => x.Name == Name && x.Author == Author);
                    if (find.Count() > 0)
                        return false;
                }
               
                return true;
            }, 
            (p) =>
            {
                //Tạo sách mới 
                CBook Book = new CBook { Name = Name, Author = Author, Count = int.Parse(Count) };
                Book.Id = CBook.Ins.isExist(Book);
                ListBook.Add(Book);
            }
               );
        }


        #endregion
    }
}
