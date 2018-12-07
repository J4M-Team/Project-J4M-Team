using BookStore.Model;
using BookStore.Model.MyClass;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows;
using Microsoft.Win32;
using System.Windows.Media.Imaging;
using System.IO;

namespace BookStore.ViewModel
{

    public class EditBookPageVM:BaseViewModel
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

        

        private BitmapImage _CoverImage;
        public BitmapImage CoverImage
        {
            get { return _CoverImage; }
            set
            {
                _CoverImage = value;
                OnPropertyChanged(nameof(CoverImage));
            }
        }

        #endregion

        #region properties binding

        //Thuộc tính isreadonly cột tên sách
        private bool _NameisReadOnly;
        public bool NameisReadOnly
        {
            get { return _NameisReadOnly; }
            set
            {
                _NameisReadOnly = value;
                OnPropertyChanged(nameof(NameisReadOnly));
            }
        }

        //Thuộc tính isreadonly cột tác giả
        private bool _AuthorisReadOnly;
        public bool AuthorisReadOnly
        {
            get { return _AuthorisReadOnly; }
            set
            {
                _AuthorisReadOnly = value;
                OnPropertyChanged(nameof(AuthorisReadOnly));
            }
        }

        //Thuộc tính isreadonly cột thể loại
        private bool _TypeisReadOnly;
        public bool TypeisReadOnly
        {
            get { return _TypeisReadOnly; }
            set
            {
                _TypeisReadOnly = value;
                OnPropertyChanged(nameof(TypeisReadOnly));
            }
        }

        //Thuộc tính isreadonly cột chủ đề
        private bool _ThemeisReadOnly;
        public bool ThemeisReadOnly
        {
            get { return _ThemeisReadOnly; }
            set
            {
                _ThemeisReadOnly = value;
                OnPropertyChanged(nameof(ThemeisReadOnly));
            }
        }

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

        //Thuộc tính ẩn của nút chọn ảnh
        private Visibility _ImageButtonVisibility;
        public Visibility ImageButtonVisibility
        {
            get { return _ImageButtonVisibility; }
            set
            {
                _ImageButtonVisibility = value;
                OnPropertyChanged(nameof(ImageButtonVisibility));
            }
        }

        #endregion

        #region command binding

        public ICommand loadCommand { get; set; }
        public ICommand AddnewBookCommand { get; set; }
        public ICommand IncreaseBookCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand ImageCommand { get; set; }
        public ICommand SelectionChangedCommand { get; set; }
        public ICommand CheckCommand { get; set; }

        #endregion

        #region global variables

        public string FileName;

        #endregion

        public EditBookPageVM()
        {
            loadCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                AuthorisReadOnly = true;
                NameisReadOnly = true;
                ThemeisReadOnly = true;
                TypeisReadOnly = true;

                IsChecked = false;

                EditColumnVisibility = Visibility.Hidden;
                ImageButtonVisibility = Visibility.Hidden;

                ListBook = new ObservableCollection<CBook>(CBook.Ins.Load());               
            }
               );

            AddnewBookCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                AddNewBookWindow wd = new AddNewBookWindow();
                wd.ShowDialog();

                //load lại bảng sau khi đã thêm
                ListBook = new ObservableCollection<CBook>(CBook.Ins.Load());
            }
               );

            IncreaseBookCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                IncreaseBookWindow wd = new IncreaseBookWindow();
                wd.ShowDialog();

                //load lại bảng sau khi đã thêm
                ListBook = new ObservableCollection<CBook>(CBook.Ins.Load());
            }
               );

            EditCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                
                if(string.IsNullOrEmpty(SelectedItem.Name)|| string.IsNullOrEmpty(SelectedItem.Author)|| string.IsNullOrEmpty(SelectedItem.Theme)|| string.IsNullOrEmpty(SelectedItem.Type))
                {
                    return;
                }
                else
                {
                    if (SelectedItem != null)
                    {
                        CBook Book = new CBook
                        {
                            Id = SelectedItem.Id,
                            Name = SelectedItem.Name,
                            Author = SelectedItem.Author,
                            Theme = SelectedItem.Theme,
                            Type = SelectedItem.Type,
                            Image = CoverImage
                        };
                        CBook.Ins.Update(Book);

                        //load lại dữ liệu
                        ListBook = new ObservableCollection<CBook>(CBook.Ins.Load());
                        
                        // MessageBox.Show("chọn vào button");
                    }
                }

            }
               );

            ImageCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
               
                OpenFileDialog ofd = new OpenFileDialog() { Filter = "JPEG|*.jpg|PNG|*.png", ValidateNames = true, Multiselect = false };
                
                var dialogOk = ofd.ShowDialog();
                if(dialogOk == true)
                {
                    FileName = ofd.FileName;
                    CoverImage = new BitmapImage(new Uri(FileName));
                }
                
            }
               );

            SelectionChangedCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                // MessageBox.Show("chọn vào change");
                if(ListBook!=null)
                {
                    //Thêm vào lí do khi load lại màn hình nó nhày vào hàm này khi đó selecteditem bị rỗng => chương trình bị treo
                    if (SelectedItem != null)
                    {
                        var querry = (from item in ListBook where item.Id == SelectedItem.Id select item.Image).First();
                        if (querry != null)
                        {
                            CoverImage = querry;
                        }
                        else
                        {
                            CoverImage = new BitmapImage(new Uri("pack://application:,,,/" + "./Image/Book.png"));
                        }
                    }
                    
                }
                
            }
               );

            CheckCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {

                if (IsChecked == true)
                {
                    AuthorisReadOnly = false;
                    NameisReadOnly = false;
                    ThemeisReadOnly = false;
                    TypeisReadOnly = false;

                    EditColumnVisibility = Visibility.Visible;
                    ImageButtonVisibility = Visibility.Visible;
                }
                else
                {

                    AuthorisReadOnly = true;
                    NameisReadOnly = true;
                    ThemeisReadOnly = true;
                    TypeisReadOnly = true;

                    EditColumnVisibility = Visibility.Hidden;
                    ImageButtonVisibility = Visibility.Hidden;
                }

            }
               );
        }



    }
}
