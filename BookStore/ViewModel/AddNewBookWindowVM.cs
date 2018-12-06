using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using System.Collections.ObjectModel;
using BookStore.Model.MyClass;

namespace BookStore.ViewModel
{
    public class AddNewBookWindowVM:BaseViewModel
    {
        #region data binding

        private ObservableCollection<string> _ListTheme;
        public ObservableCollection<string> ListTheme
        {
            get { return _ListTheme; }
            set
            {
                _ListTheme = value;
                OnPropertyChanged(nameof(ListTheme));
            }
        }

        private ObservableCollection<string> _ListType;
        public ObservableCollection<string> ListType
        {
            get { return _ListType; }
            set
            {
                _ListType = value;
                OnPropertyChanged(nameof(ListType));
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

        private string _Type;
        public string Type
        {
            get { return _Type; }
            set
            {
                _Type = value;
                OnPropertyChanged(nameof(Type));
            }
        }

        private string _Theme;
        public string Theme
        {
            get { return _Theme; }
            set
            {
                _Theme = value;
                OnPropertyChanged(nameof(Theme));
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

        private string _Price;
        public string Price
        {
            get { return _Price; }
            set
            {
                _Price = value;
                OnPropertyChanged(nameof(Price));
            }
        }

        #endregion

        #region command binding

        public ICommand AddCommand { get; set; }
        public ICommand loadCommand { get; set; }

        #endregion

        #region method

        public AddNewBookWindowVM()
        {
            loadCommand = new RelayCommand<object>((p) => { return true; },(p) =>
            {
                ListTheme = new ObservableCollection<string>(CBook.Ins.ListTheme());
                ListType = new ObservableCollection<string>(CBook.Ins.ListType());
            }
               );

            AddCommand = new RelayCommand<object>((p) => {
                if (string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(Author) || string.IsNullOrEmpty(Type) || string.IsNullOrEmpty(Theme) && string.IsNullOrEmpty(Count) || string.IsNullOrEmpty(Price))
                {
                    return false;
                }
                else
                {
                    int ICount;
                    float FPrice;
                    if (int.TryParse(Count, out ICount) == false || float.TryParse(Price, out FPrice) == false)
                    {
                        return false;
                    }

                    if (ICount <= 0 || FPrice <= 0)
                    {
                        return false;
                    }

                    CBook Book = new CBook
                    {
                        Name = Name,
                        Author = Author,
                        Theme = Theme,
                        Type = Type,
                        Count = ICount,
                        Price = new CBook_Price { InputPrice = FPrice }
                    };

                    //Kiểm tra sách đã tồn tại trong kho chưa
                    if (CBook.Ins.isExist(Book) == true)
                    {
                        return false;
                    }
                    
                    return true;
                }
                
            },
            (p) =>
            {
                CBook Book = new CBook
                {
                    Name = Name,
                    Author = Author,
                    Theme = Theme,
                    Type = Type,
                    Count = int.Parse(Count),
                    Price = new CBook_Price { InputPrice = float.Parse(Price) }
                };

                //Thêm sách mới
                CBook.Ins.Add(Book);

                //tạo trắng bảng
                Name = "";
                Author = "";
                Theme = "";
                Type = "";
                Price = "";
                Count = "";
            }
               );
        }

        #endregion
    }
}
