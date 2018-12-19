using BookStore.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace BookStore.Model.MyClass
{
    public class CBook : BaseViewModel
    {
        #region design pattern singleton

        private static CBook _ins;
        public static CBook Ins
        {
            get
            {
                if (_ins == null)
                    _ins = new CBook();
                return _ins;
            }
            set
            {
                _ins = value;
            }
        }

        #endregion

        #region private properties

        private int _Id;
        private string _Name;
        private string _Author;
        private string _Theme;
        private string _Type;
        private string _Content;
        private int _Count;
        private BitmapImage _Image;
        //imgage
        private CBook_Price _Price;
        private float _Promotion;//Phần trăm khuyến mãi
        private float _PricePromotion;//Giá bán sách sau khuyến mãi
        private int _TotalCount;//tổng lượt mua của sách
        private float _TotalPrice;

        //Thuộc tính ẩn của cột giá sách gốc
        private Visibility _PriceVisibility;

        #endregion

        #region public properties

        public int Id { get { return _Id; } set { _Id = value; OnPropertyChanged(nameof(Id)); } }
        public string Name { get { return _Name; } set { _Name = value; OnPropertyChanged(nameof(Name)); } }
        public string Author { get { return _Author; } set { _Author = value; OnPropertyChanged(nameof(Author)); } }
        public string Theme { get { return _Theme; } set { _Theme = value; } }
        public string Type { get { return _Type; } set { _Type = value; } }
        public string Content { get { return _Content; } set { _Content = value; } }
        public int Count { get { return _Count; } set { _Count = value; OnPropertyChanged(nameof(Count)); } }
        public CBook_Price Price { get { return _Price; } set { _Price = value; } }
        public BitmapImage Image { get { return _Image; } set { _Image = value; } }
        public float Promotion { get { return _Promotion; } set { _Promotion = value; } }
        public float PricePromotion { get { return _PricePromotion; } set { _PricePromotion = value; } }
        public int TotalCount { get { return _TotalCount; } set { _TotalCount = value; } }
        public float TotalPrice { get { return _TotalPrice; } set { _TotalPrice = value; } }

        public Visibility PriceVisibility { get { return _PriceVisibility; } set { _PriceVisibility = value; } }

        #endregion

        #region method

        /// <summary>
        /// Hàm trả về danh sách toàn bộ sách trong cơ sở dữ liệu
        /// </summary>
        /// <returns></returns>
        public List<CBook> Load()
        {
            var List = new List<CBook>();
            try
            {
                var sql = DataProvider.Ins.DB.Books;
                foreach (var item in sql)
                {
                    CBook Book;
                    if (item.Book_Image == null)
                    {
                        BitmapImage image = new BitmapImage(new Uri("pack://application:,,,/" + "./Image/Book.png"));
                        Book = new CBook
                        {
                            Id = item.Book_Id,
                            Name = item.Book_Name,
                            Author = item.Book_Author,
                            Type = item.Book_Type,
                            Theme = item.Book_Theme,
                            Count = (int)item.Book_Count,
                            Image = image,
                            Price = new CBook_Price { OutputPrice = (float)item.Book_Output_Price.OrderByDescending(x => x.Date_Set).Select(x => x.Output_Price).FirstOrDefault() },
                            TotalCount = item.Bill_Info.Select(x => x.Book_Count).Sum()
                        };
                    }
                    else
                    {
                        Book = new CBook
                        {
                            Id = item.Book_Id,
                            Name = item.Book_Name,
                            Author = item.Book_Author,
                            Type = item.Book_Type,
                            Theme = item.Book_Theme,
                            Count = (int)item.Book_Count,
                            Image = Help.ByteToImage(item.Book_Image),
                            Price = new CBook_Price { OutputPrice = (float)item.Book_Output_Price.OrderByDescending(x => x.Date_Set).Select(x => x.Output_Price).FirstOrDefault() },
                            TotalCount = item.Bill_Info.Select(x => x.Book_Count).Sum()
                        };
                    }

                    List.Add(Book);
                }
            }
            catch
            {

            }
            return List;
        }

        /// <summary>
        /// Hàm thêm mới một sách dưới cơ sở dữ liệu
        /// </summary>
        /// <param name="BookData">Dữ liệu sách cần thêm mới</param>
        /// <returns>nếu thành công trả về id sách vừa thêm, thất bại trả về 0</returns>
        public int Add(CBook BookData)
        {
            try
            {
                Book Book;
                if (BookData.Image == null)
                {
                    Book = new Book
                    {
                        Book_Name = BookData.Name,
                        Book_Author = BookData.Author,
                        Book_Type = BookData.Type,
                        Book_Theme = BookData.Theme,
                        Book_Count = BookData.Count
                    };
                }
                else
                {
                    Book = new Book
                    {
                        Book_Name = BookData.Name,
                        Book_Author = BookData.Author,
                        Book_Type = BookData.Type,
                        Book_Theme = BookData.Theme,
                        Book_Count = BookData.Count,
                        Book_Image = Help.ImageToByte(BookData.Image)
                    };
                }

                DataProvider.Ins.DB.Books.Add(Book);
                DataProvider.Ins.DB.SaveChanges();

                //Tìm id sách vừa mới được tạo theo tên và tác giả và thể loại
                var Book_Id = DataProvider.Ins.DB.Books.Where(x => x.Book_Name.ToLower() == BookData.Name.ToLower() && x.Book_Author == BookData.Author && x.Book_Type == BookData.Type).Select(x => x.Book_Id).FirstOrDefault();

                //Kiểm tra tồn tại id chưa
                if (Book_Id == 0)
                {
                    return 0;
                }

                //Thêm giá nhập sách
                this.ChangeInputPrice(Book_Id, BookData.Price.InputPrice);

                //Thêm giá bán sách mặc định ban đầu giá bán sách sẽ bằng 140% giá của giá nhập sách
                this.ChangeOutputPrice(Book_Id, BookData.Price.OutputPrice);

                return Book_Id;
            }
            catch
            {
                return 0;
            }

        }

        /// <summary>
        /// Hàm cập nhật thông tin của sách dưới cơ sở dữ liệu
        /// </summary>
        /// <param name="BookData">Dữ liệu cần thay đổi</param>
        /// <returns>nếu thành công trả về true, thất bại trả về false</returns>
        public bool Update(CBook BookData)
        {
            try
            {
                //Tìm đối tượng cần update theo khóa chính
                var BooktoUpdate = DataProvider.Ins.DB.Books.Find(BookData.Id);
                if (BooktoUpdate != null)
                {
                    //update dữ liệu mới
                    BooktoUpdate.Book_Name = BookData.Name;
                    BooktoUpdate.Book_Theme = BookData.Theme;
                    BooktoUpdate.Book_Author = BookData.Author;
                    BooktoUpdate.Book_Type = BookData.Type;
                    BooktoUpdate.Book_Image = Help.ImageToByte(BookData.Image);
                    //lưu thay đổi
                    DataProvider.Ins.DB.SaveChanges();
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Hàm trả về danh sách chủ đề 
        /// </summary>
        /// <returns>List</returns>
        public List<string> ListTheme()
        {
            var List = new List<string>();

            try
            {
                //Lấy ra danh sách chủ đề
                var Theme = (from item in DataProvider.Ins.DB.Books select new { item.Book_Theme }).Distinct();

                foreach (var item in Theme)
                {
                    List.Add(item.Book_Theme);
                }
            }
            catch
            {

            }

            return List;
        }

        /// <summary>
        /// Hàm trả về danh sách thể loại của sách trong cơ sở dữ liệu
        /// </summary>
        /// <returns></returns>
        public List<string> ListType()
        {
            var List = new List<string>();

            try
            {
                //Lấy ra danh sách loại
                var Type = (from item in DataProvider.Ins.DB.Books select new { item.Book_Type }).Distinct();

                foreach (var item in Type)
                {
                    List.Add(item.Book_Type);
                }
            }
            catch
            {

            }

            return List;
        }

        public BitmapImage ImageOfId(int Id)
        {
            BitmapImage result = new BitmapImage();
            try
            {
                var data = DataProvider.Ins.DB.Books.Where(x => x.Book_Id == Id).Select(x => x.Book_Image).First();
                result = Help.ByteToImage(data);
                return result;
            }
            catch
            {

            }
            result = null;
            return result;
        }

        /// <summary>
        /// Hàm trả về danh sách tác giả trong cơ sở dữ liệu
        /// </summary>
        /// <returns></returns>
        public List<string> ListAuthor()
        {
            List<string> List = new List<string>();
            try
            {
                //Lấy ra danh sách loại
                var data = (from item in DataProvider.Ins.DB.Books select new { item.Book_Author }).Distinct();

                foreach (var item in data)
                {
                    List.Add(item.Book_Author);
                }
            }
            catch
            {

            }

            return List;
        }

        /// <summary>
        /// Hàm giảm số lượng của sách trong kho
        /// </summary>
        /// <param name="Book_Id">mã sách cần giảm</param>
        /// <param name="Quantity">số lượng giảm</param>
        /// <returns></returns>
        public bool DecreaseNumberOfBook(int Book_Id, int Quantity)
        {
            try
            {
                //Tìm kiếm sách
                var data = DataProvider.Ins.DB.Books.Find(Book_Id);
                if (data != null)
                {
                    //Lấy ra số lượng trong kho
                    int oldQuantity = (int)data.Book_Count;

                    //Kiểm tra nếu lớn hơn thì tiến hành trừ
                    if (oldQuantity >= Quantity)
                    {
                        int newQuantity = oldQuantity - Quantity;
                        //Cập nhật lại số lượng mới
                        data.Book_Count = newQuantity;
                        //lưu lại
                        DataProvider.Ins.DB.SaveChanges();
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch
            {

            }
            return false;
        }

        /// <summary>
        /// Hàm thêm số lượng sách trong kho
        /// </summary>
        /// <param name="Book_Id">mã sách cần thêm</param>
        /// <param name="Quantity">Số lượng sách cần thêm</param>
        /// <returns></returns>
        public bool IncreaseNumberOfBook(int Book_Id, int Quantity)
        {
            try
            {
                //Tìm kiếm sách
                var data = DataProvider.Ins.DB.Books.Find(Book_Id);
                if (data != null)
                {
                    //Lấy ra số lượng trong kho
                    int oldQuantity = (int)data.Book_Count;

                    //Thêm số lượng
                    int newQuantity = oldQuantity + Quantity;

                    //Cập nhật lại số lượng mới
                    data.Book_Count = newQuantity;
                    //lưu lại
                    DataProvider.Ins.DB.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {

            }
            return false;
        }

        /// <summary>
        /// Hàm trả về danh sách giá nhập và giá bán của toàn bộ sách trong kho
        /// </summary>
        /// <returns></returns>
        public List<CBook> ListPriceBook()
        {
            List<CBook> List = new List<CBook>();
            try
            {
                //Lấy ra danh sách giá mới nhất ở bảng outputprice
                var OutputPrice = from item in DataProvider.Ins.DB.Book_Output_Price
                                  group item by item.Book_Id into Group
                                  from item2 in Group
                                  where item2.Date_Set == Group.Max(x => x.Date_Set)
                                  select new { item2.Book_Id, item2.Date_Set, item2.Output_Price };

                //Lấy ra danh sách giá mới nhất ở bảng inputprice
                var InputPrice = from item in DataProvider.Ins.DB.Book_Input_Price
                                 group item by item.Book_Id into Group
                                 from item2 in Group
                                 where item2.Date_Set == Group.Max(x => x.Date_Set)
                                 select new { item2.Book_Id, item2.Date_Set, item2.Input_Price };

                //join 3 bảng lại nếu như giá chưa được cài đặt thì trả về là 0
                var data = from book in DataProvider.Ins.DB.Books
                           join output in OutputPrice on book.Book_Id equals output.Book_Id into outputtable
                           join input in InputPrice on book.Book_Id equals input.Book_Id into inputable
                           from inp in inputable.DefaultIfEmpty()
                           from outp in outputtable.DefaultIfEmpty()
                           select new
                           {
                               book.Book_Id,
                               book.Book_Name,
                               book.Book_Author,
                               book.Book_Count,
                               input = inp == null ? 0 : inp.Input_Price,
                               output = outp == null ? 0 : outp.Output_Price
                           };


                foreach (var item in data)
                {

                    CBook_Price price = new CBook_Price { OutputPrice = (float)item.output, InputPrice = (float)item.input };
                    CBook Book = new CBook { Id = item.Book_Id, Name = item.Book_Name, Author = item.Book_Author, Count = (int)item.Book_Count, Price = price };
                    List.Add(Book);
                }
                return List;
            }
            catch
            {

            }
            return List;
        }

        /// <summary>
        /// Hàm thay đổi giá nhập của sách 
        /// </summary>
        /// <param name="Book_Id">id sách</param>
        /// <param name="NewPrice">giá mới</param>
        /// <returns></returns>
        public bool ChangeInputPrice(int Book_Id, float NewPrice)
        {
            try
            {
                if (NewPrice <= 0)
                {
                    return false;
                }
                else
                {
                    //Tìm sách trong danh sách sách
                    var data = DataProvider.Ins.DB.Books.Find(Book_Id);
                    if (data != null)
                    {
                        //Kiểm tra xem giá có được cài đặt chưa nếu chưa thì thêm mới                       
                        if (DataProvider.Ins.DB.Book_Input_Price.Where(x => x.Book_Id == Book_Id).Count() > 0)
                        {
                            //Kiểm tra xem giá có giống với giá tạo sau cùng hay không
                            var lastPrice = DataProvider.Ins.DB.Book_Input_Price.Where(x => x.Book_Id == Book_Id && x.Date_Set == DataProvider.Ins.DB.Book_Input_Price.Where(y => y.Book_Id == Book_Id).Max(y => y.Date_Set)).First();
                            if (lastPrice != null)
                            {
                                if (lastPrice.Input_Price == NewPrice)
                                {
                                    return false;
                                }
                                else
                                {
                                    //Thêm mới
                                    goto addnew;
                                }
                            }
                            else
                            {
                                goto addnew;
                            }
                        }
                        else
                        {
                            goto addnew;
                        }

#pragma warning disable CS0164 // This label has not been referenced
                        addnew:
#pragma warning restore CS0164 // This label has not been referenced
                        //Tạo giá mới
                        Book_Input_Price newPrice = new Book_Input_Price { Book_Id = Book_Id, Input_Price = NewPrice, Date_Set = DateTime.Now };
                        //Thêm vào
                        DataProvider.Ins.DB.Book_Input_Price.Add(newPrice);
                        //Lưu lại
                        DataProvider.Ins.DB.SaveChanges();
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }

            }
            catch
            {

            }
            return false;
        }

        /// <summary>
        /// Hàm thay đổi giá bán của sách
        /// </summary>
        /// <param name="Book_Id">id sách</param>
        /// <param name="NewPrice">giá mới</param>
        /// <returns></returns>
        public bool ChangeOutputPrice(int Book_Id, float NewPrice)
        {
            try
            {
                if (NewPrice <= 0)
                {
                    return false;
                }
                else
                {
                    //Tìm sách trong cơ sở dữ liệu
                    var find = DataProvider.Ins.DB.Books.Find(Book_Id);
                    if (find != null)
                    {
                        //Kiểm tra xem giá có được cài đặt chưa nếu chưa thì thêm mới                       
                        if (DataProvider.Ins.DB.Book_Output_Price.Where(x => x.Book_Id == Book_Id).Count() > 0)
                        {
                            //Kiểm tra xem giá có giống với giá tạo sau cùng hay không
                            var lastPrice = DataProvider.Ins.DB.Book_Output_Price.Where(x => x.Book_Id == Book_Id && x.Date_Set == DataProvider.Ins.DB.Book_Output_Price.Where(y => y.Book_Id == Book_Id).Max(y => y.Date_Set)).First();
                            if (lastPrice != null)
                            {
                                if (lastPrice.Output_Price == NewPrice)
                                {
                                    return false;
                                }
                                else
                                {
                                    //Thêm mới
                                    goto addnew;
                                }
                            }
                        }
                        else
                        {
                            goto addnew;
                        }

#pragma warning disable CS0164 // This label has not been referenced
                        addnew:
#pragma warning restore CS0164 // This label has not been referenced
                        //Tạo mới
                        Book_Output_Price newPrice = new Book_Output_Price { Book_Id = Book_Id, Output_Price = NewPrice, Date_Set = DateTime.Now };
                        //Thêm
                        DataProvider.Ins.DB.Book_Output_Price.Add(newPrice);
                        //Lưu
                        DataProvider.Ins.DB.SaveChanges();
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch
            {

            }
            return false;
        }

        /// <summary>
        /// Hàm trả về List lịch sử thay đổi giá bán của sách
        /// </summary>
        /// <param name="Book_Id"></param>
        /// <returns></returns>
        public List<CBook> ListHistoryOutputPrice(int Book_Id)
        {
            List<CBook> List = new List<CBook>();
            try
            {
                //Lấy ra danh sách
                var data = DataProvider.Ins.DB.Book_Output_Price.Where(x => x.Book_Id == Book_Id);
                if (data != null)
                {
                    foreach (var item in data)
                    {
                        CBook_Price price = new CBook_Price { OutputPrice = (float)item.Output_Price, Output_Date_Set = item.Date_Set };
                        CBook Book = new CBook { Id = item.Book_Id, Price = price };
                        List.Add(Book);
                    }
                }
            }
            catch
            {

            }
            return List;
        }

        /// <summary>
        /// hàm trả về List lịch sử thay đổi giá nhập của sách
        /// </summary>
        /// <param name="Book_Id"></param>
        /// <returns></returns>
        public List<CBook> ListHistoryInputPrice(int Book_Id)
        {
            List<CBook> List = new List<CBook>();
            try
            {
                //Lấy ra danh sách
                var data = DataProvider.Ins.DB.Book_Input_Price.Where(x => x.Book_Id == Book_Id);
                if (data != null)
                {
                    foreach (var item in data)
                    {
                        CBook_Price price = new CBook_Price { InputPrice = (float)item.Input_Price, Input_Date_Set = item.Date_Set };
                        CBook Book = new CBook { Id = item.Book_Id, Price = price };
                        List.Add(Book);
                    }
                }

            }
            catch
            {

            }
            return List;
        }

        /// <summary>
        /// Hàm kiếm tra xem sách đã tồn tại trong cơ sở dữ liệu chưa, kiểm tra theo tên sách và tên tác giả
        /// </summary>
        /// <param name="Book"></param>
        /// <returns>trả về Id sách</returns>
        public int isExist(CBook Book)
        {

            try
            {
                if (!string.IsNullOrEmpty(Book.Name) && !string.IsNullOrEmpty(Book.Author))
                {
                    //Tìm theo tên và tác giả
                    var find = DataProvider.Ins.DB.Books.Where(x => x.Book_Name.ToLower() == Book.Name.ToLower() && x.Book_Author.ToLower() == Book.Author.ToLower());

                    if (find.Count() > 0)
                    {
                        //Đã tồn tại
                        return find.Select(x => x.Book_Id).First();
                    }
                    else
                    {
                        return 0;
                    }
                }
                else
                {
                    return 0;
                }
            }
            catch
            {

            }
            return 0;
        }

        /// <summary>
        /// Hàm trả về số lượng sách có trong kho của sách theo id
        /// </summary>
        /// <param name="Book_Id"></param>
        /// <returns></returns>
        public int InventoryNumber(int Book_Id)
        {
            try
            {
                var find = DataProvider.Ins.DB.Books.Find(Book_Id);
                if (find != null)
                {
                    return (int)find.Book_Count;
                }
            }
            catch
            {

            }
            //Trả về -1 nếu không còn sách tồn trong kho
            return -1;
        }

        /// <summary>
        /// Hàm trả về danh sách sách kèm theo giá bán và giá khuyến mãi
        /// </summary>
        /// <returns></returns>
        public List<CBook> ListPromotionBook()
        {
            List<CBook> List = new List<CBook>();
            try
            {
                //Lấy ra danh sách giá mới nhất ở bảng outputprice
                var OutputPrice = from item in DataProvider.Ins.DB.Book_Output_Price
                                  group item by item.Book_Id into Group
                                  from item2 in Group
                                  where item2.Date_Set == Group.Max(x => x.Date_Set)
                                  select new { item2.Book_Id, item2.Date_Set, item2.Output_Price };

                //join 3 bảng lại nếu như giá chưa được cài đặt hoặc sách không có khuyến mãi thì trả về là 0
                var data = from book in DataProvider.Ins.DB.Books
                           join output in OutputPrice on book.Book_Id equals output.Book_Id into outputtable
                           join promotion in DataProvider.Ins.DB.Book_Output_PromotionPrice on book.Book_Id equals promotion.Book_Id into promotiontable
                           from pro in promotiontable.DefaultIfEmpty()
                           from outp in outputtable.DefaultIfEmpty()
                           select new
                           {
                               book.Book_Id,
                               book.Book_Name,
                               book.Book_Author,
                               book.Book_Count,
                               promotion = pro == null ? 0 : pro.Promotion,
                               output = outp == null ? 0 : outp.Output_Price
                           };

                //Thêm vào list
                foreach (var item in data)
                {
                    CBook Book = new CBook
                    {
                        Id = item.Book_Id,
                        Name = item.Book_Name,
                        Author = item.Book_Author,
                        Count = (int)item.Book_Count,
                        Price = new CBook_Price { OutputPrice = (float)item.output },
                        Promotion = (float)item.promotion,
                        PricePromotion = item.promotion == 0 ? (float)item.output : (float)item.output - (float)item.output * (float)item.promotion
                    };
                    List.Add(Book);
                }
            }
            catch
            {

            }
            return List;
        }

        /// <summary>
        /// Hàm thay đổi giá khuyễn mãi của sách theo Id
        /// </summary>
        /// <param name="Book_Id"></param>
        /// <param name="NewPromotion"></param>
        /// <returns></returns>
        public bool ChangePromotion(int Book_Id, float NewPromotion)
        {
            try
            {
                //Kiểm tra điều kiện nhập
                if (Book_Id <= 0 || NewPromotion <= 0)
                {
                    return false;
                }

                //Tìm kiếm sách trong danh sách sách
                if (DataProvider.Ins.DB.Books.Find(Book_Id) == null)
                {
                    return false;
                }

                //Kiểm tra trong bảng PromotionPrice, nếu đã tồn tại rồi thì cập nhật giá mới, nếu như chưa tồn tại thì thêm mới
                var find = DataProvider.Ins.DB.Book_Output_PromotionPrice.Where(x => x.Book_Id == Book_Id).FirstOrDefault();

                if (find == null)
                {
                    //Tạo mới
                    Book_Output_PromotionPrice Promotion = new Book_Output_PromotionPrice { Book_Id = Book_Id, Promotion = NewPromotion };

                    //Thêm mới
                    DataProvider.Ins.DB.Book_Output_PromotionPrice.Add(Promotion);

                    //Lưu thay đổi
                    DataProvider.Ins.DB.SaveChanges();

                    return true;
                }
                else
                {
                    //Kiểm tra nếu như khuyến mãi mới giống khuyến mãi cũ thì không cập nhật
                    if ((float)find.Promotion == NewPromotion)
                    {
                        return false;
                    }
                    else
                    {
                        //Cập nhật
                        find.Promotion = NewPromotion;

                        //Lưu thay đổi
                        DataProvider.Ins.DB.SaveChanges();
                        return true;
                    }
                }
            }
            catch
            {

            }
            return false;
        }

        /// <summary>
        /// Hàm trả về Top sách bán ra nhiều nhất theo tháng
        /// </summary>
        /// <param name="Year">Năm</param>
        /// <param name="Month">Tháng</param>
        /// <param name="Top">Số lượng sách cần lấy</param>
        /// <returns></returns>
        public List<CBook> ListTopBook(int Year, int Month, int Top)
        {
            List<CBook> List = new List<CBook>();

            try
            {
                //https://stackoverflow.com/questions/37338860/linq-sum-with-group-by?rq=1

                //Lấy ra danh sách id sách kèm tổng số lượng bán ra
                var ListBook = from item in DataProvider.Ins.DB.Bill_Info.Where(x => SqlFunctions.DatePart("year", x.Bill.Bill_Date) == Year &&
                    SqlFunctions.DatePart("month", x.Bill.Bill_Date) == Month)
                               group item by item.Book_Id into Group
                               select new
                               {
                                   Book_Id = Group.Key,
                                   Book_Sum = Group.Select(x => x.Book_Count).DefaultIfEmpty(0).Sum()
                               };

                //Lấy ra thông tin của sách bằng cách join với bảng Book
                var dataListBook = from item1 in ListBook.OrderByDescending(x => x.Book_Sum).Take(Top)
                                   join item2 in DataProvider.Ins.DB.Books on item1.Book_Id equals item2.Book_Id
                                   select new
                                   {
                                       item1.Book_Id,
                                       item2.Book_Name,
                                       item2.Book_Author,
                                       item1.Book_Sum
                                   };

                if (dataListBook.Count() > 0)
                {
                    //Thêm dữ liệu
                    foreach (var item in dataListBook)
                    {
                        //Tạo mới
                        CBook Book = new CBook
                        {
                            Id = item.Book_Id,
                            Name = item.Book_Name,
                            Author = item.Book_Author,
                            Count = item.Book_Sum
                        };

                        //Thêm vào List
                        List.Add(Book);
                    }
                }

            }
            catch
            {

            }

            return List;
        }

        public List<CBook> ListTopBookAll(int Top)
        {
            List<CBook> List = new List<CBook>();

            try
            {
                //https://stackoverflow.com/questions/37338860/linq-sum-with-group-by?rq=1

                //Lấy ra danh sách id sách kèm tổng số lượng bán ra
                var ListBook = from item in DataProvider.Ins.DB.Bill_Info
                               group item by item.Book_Id into Group
                               select new
                               {
                                   Book_Id = Group.Key,
                                   Book_Sum = Group.Select(x => x.Book_Count).DefaultIfEmpty(0).Sum()
                               };

                //Lấy ra thông tin của sách bằng cách join với bảng Book
                var dataListBook = from item1 in ListBook.OrderByDescending(x => x.Book_Sum).Take(Top)
                                   join item2 in DataProvider.Ins.DB.Books on item1.Book_Id equals item2.Book_Id
                                   select new
                                   {
                                       item1.Book_Id,
                                       item2.Book_Name,
                                       item2.Book_Author,
                                       item1.Book_Sum
                                   };

                if (dataListBook.Count() > 0)
                {
                    //Thêm dữ liệu
                    foreach (var item in dataListBook)
                    {
                        //Tạo mới
                        CBook Book = new CBook
                        {
                            Id = item.Book_Id,
                            Name = item.Book_Name,
                            Author = item.Book_Author,
                            Count = item.Book_Sum
                        };

                        //Thêm vào List
                        List.Add(Book);
                    }
                }
            }

            catch
            {

            }

            return List;
        }

        /// <summary>
        /// Hàm trả về list sách tìm kiếm theo thông tin tác giả.............
        /// </summary>
        /// <param name="bookdatt"></param>
        /// <returns></returns>
        public List<CBook> FindBookName(string BookName)
        {
            List<CBook> List = new List<CBook>();

            if (string.IsNullOrEmpty(BookName))
            {
                return List;
            }

            try
            {
                var data = DataProvider.Ins.DB.Books.Where(x => x.Book_Name.ToLower().Contains(BookName.ToLower()));

                foreach (var item in data)
                {
                    CBook Book;
                    if (item.Book_Image == null)
                    {
                        BitmapImage image = new BitmapImage(new Uri("pack://application:,,,/" + "./Image/Book.png"));
                        Book = new CBook
                        {
                            Id = item.Book_Id,
                            Name = item.Book_Name,
                            Author = item.Book_Author,
                            Type = item.Book_Type,
                            Theme = item.Book_Theme,
                            Count = (int)item.Book_Count,
                            Image = image,
                            Price = new CBook_Price { OutputPrice = (float)item.Book_Output_Price.OrderByDescending(x => x.Date_Set).Select(x => x.Output_Price).FirstOrDefault() },
                            TotalCount = item.Bill_Info.Select(x => x.Book_Count).Sum()
                        };
                    }
                    else
                    {
                        Book = new CBook
                        {
                            Id = item.Book_Id,
                            Name = item.Book_Name,
                            Author = item.Book_Author,
                            Type = item.Book_Type,
                            Theme = item.Book_Theme,
                            Count = (int)item.Book_Count,
                            Image = Help.ByteToImage(item.Book_Image),
                            Price = new CBook_Price { OutputPrice = (float)item.Book_Output_Price.OrderByDescending(x => x.Date_Set).Select(x => x.Output_Price).FirstOrDefault() },
                            TotalCount = item.Bill_Info.Select(x => x.Book_Count).Sum()
                        };
                    }

                    //Thêm sách vào List
                    List.Add(Book);

                }
            }
            catch
            {

            }

            return List;
        }

        /// <summary>
        /// Hàm trả về list sách tìm kiếm theo tên sách và tác giả
        /// </summary>
        /// <param name="BookName"></param>
        /// <param name="Author"></param>
        /// <returns></returns>
        public List<CBook> FindBookAuthor(string Author)
        {
            List<CBook> List = new List<CBook>();

            if (string.IsNullOrEmpty(Author))
            {
                return List;
            }

            try
            {
                var data = DataProvider.Ins.DB.Books.Where(x => x.Book_Author.ToLower().Contains(Author.ToLower()));

                foreach (var item in data)
                {
                    CBook Book;
                    if (item.Book_Image == null)
                    {
                        BitmapImage image = new BitmapImage(new Uri("pack://application:,,,/" + "./Image/Book.png"));
                        Book = new CBook
                        {
                            Id = item.Book_Id,
                            Name = item.Book_Name,
                            Author = item.Book_Author,
                            Type = item.Book_Type,
                            Theme = item.Book_Theme,
                            Count = (int)item.Book_Count,
                            Image = image,
                            Price = new CBook_Price { OutputPrice = (float)item.Book_Output_Price.OrderByDescending(x => x.Date_Set).Select(x => x.Output_Price).FirstOrDefault() },
                            TotalCount = item.Bill_Info.Select(x => x.Book_Count).Sum()
                        };
                    }
                    else
                    {
                        Book = new CBook
                        {
                            Id = item.Book_Id,
                            Name = item.Book_Name,
                            Author = item.Book_Author,
                            Type = item.Book_Type,
                            Theme = item.Book_Theme,
                            Count = (int)item.Book_Count,
                            Image = Help.ByteToImage(item.Book_Image),
                            Price = new CBook_Price { OutputPrice = (float)item.Book_Output_Price.OrderByDescending(x => x.Date_Set).Select(x => x.Output_Price).FirstOrDefault() },
                            TotalCount = item.Bill_Info.Select(x => x.Book_Count).Sum()
                        };
                    }

                    //Thêm sách vào List
                    List.Add(Book);

                }
            }
            catch
            {

            }

            return List;
        }

        /// <summary>
        /// Hàm trả về List sách tìm kiếm theo chủ đề
        /// </summary>
        /// <param name="Theme"></param>
        /// <returns></returns>
        public List<CBook> FindBookTheme(string Theme)
        {
            List<CBook> List = new List<CBook>();

            if (string.IsNullOrEmpty(Theme))
            {
                return List;
            }

            try
            {
                var data = DataProvider.Ins.DB.Books.Where(x => x.Book_Theme.ToLower().Contains(Theme.ToLower()));

                foreach (var item in data)
                {
                    CBook Book;
                    if (item.Book_Image == null)
                    {
                        BitmapImage image = new BitmapImage(new Uri("pack://application:,,,/" + "./Image/Book.png"));
                        Book = new CBook
                        {
                            Id = item.Book_Id,
                            Name = item.Book_Name,
                            Author = item.Book_Author,
                            Type = item.Book_Type,
                            Theme = item.Book_Theme,
                            Count = (int)item.Book_Count,
                            Image = image,
                            Price = new CBook_Price { OutputPrice = (float)item.Book_Output_Price.OrderByDescending(x => x.Date_Set).Select(x => x.Output_Price).FirstOrDefault() },
                            TotalCount = item.Bill_Info.Select(x => x.Book_Count).Sum()
                        };
                    }
                    else
                    {
                        Book = new CBook
                        {
                            Id = item.Book_Id,
                            Name = item.Book_Name,
                            Author = item.Book_Author,
                            Type = item.Book_Type,
                            Theme = item.Book_Theme,
                            Count = (int)item.Book_Count,
                            Image = Help.ByteToImage(item.Book_Image),
                            Price = new CBook_Price { OutputPrice = (float)item.Book_Output_Price.OrderByDescending(x => x.Date_Set).Select(x => x.Output_Price).FirstOrDefault() },
                            TotalCount = item.Bill_Info.Select(x => x.Book_Count).Sum()
                        };
                    }

                    //Thêm sách vào List
                    List.Add(Book);

                }
            }
            catch
            {

            }

            return List;
        }

        /// <summary>
        /// Hàm trả về List sách tìm kiếm theo thể loại
        /// </summary>
        /// <param name="Type"></param>
        /// <returns></returns>
        public List<CBook> FindBookType(string Type)
        {
            List<CBook> List = new List<CBook>();

            if (string.IsNullOrEmpty(Type))
            {
                return List;
            }

            try
            {
                var data = DataProvider.Ins.DB.Books.Where(x => x.Book_Type
.ToLower().Contains(Type.ToLower()));

                foreach (var item in data)
                {
                    CBook Book;
                    if (item.Book_Image == null)
                    {
                        BitmapImage image = new BitmapImage(new Uri("pack://application:,,,/" + "./Image/Book.png"));
                        Book = new CBook
                        {
                            Id = item.Book_Id,
                            Name = item.Book_Name,
                            Author = item.Book_Author,
                            Type = item.Book_Type,
                            Theme = item.Book_Theme,
                            Count = (int)item.Book_Count,
                            Image = image,
                            Price = new CBook_Price { OutputPrice = (float)item.Book_Output_Price.OrderByDescending(x => x.Date_Set).Select(x => x.Output_Price).FirstOrDefault() },
                            TotalCount = item.Bill_Info.Select(x => x.Book_Count).Sum()
                        };
                    }
                    else
                    {
                        Book = new CBook
                        {
                            Id = item.Book_Id,
                            Name = item.Book_Name,
                            Author = item.Book_Author,
                            Type = item.Book_Type,
                            Theme = item.Book_Theme,
                            Count = (int)item.Book_Count,
                            Image = Help.ByteToImage(item.Book_Image),
                            Price = new CBook_Price { OutputPrice = (float)item.Book_Output_Price.OrderByDescending(x => x.Date_Set).Select(x => x.Output_Price).FirstOrDefault() },
                            TotalCount = item.Bill_Info.Select(x => x.Book_Count).Sum()
                        };
                    }

                    //Thêm sách vào List
                    List.Add(Book);

                }
            }
            catch
            {

            }

            return List;
        }

        /// <summary>
        /// Hàm trả về List sách tìm kiếm theo giá bán ra từ Min đến Max
        /// </summary>
        /// <param name="MinPrice"></param>
        /// <param name="MaxPrice"></param>
        /// <returns></returns>
        public List<CBook> FindBookPrice(float MinPrice, float MaxPrice)
        {
            List<CBook> List = new List<CBook>();
            try
            {
                //Lấy ra danh sách giá mới nhất ở bảng outputprice
                var OutputPrice = from item in DataProvider.Ins.DB.Book_Output_Price
                                  group item by item.Book_Id into Group
                                  from item2 in Group
                                  where item2.Date_Set == Group.Max(x => x.Date_Set)
                                  select new { item2.Book_Id, item2.Date_Set, item2.Output_Price };

                //join 3 bảng lại nếu như giá chưa được cài đặt hoặc sách không có khuyến mãi thì trả về là 0
                var data = from book in DataProvider.Ins.DB.Books
                           join output in OutputPrice on book.Book_Id equals output.Book_Id into outputtable
                           join promotion in DataProvider.Ins.DB.Book_Output_PromotionPrice on book.Book_Id equals promotion.Book_Id into promotiontable
                           from pro in promotiontable.DefaultIfEmpty()
                           from outp in outputtable.DefaultIfEmpty()
                           select new
                           {
                               book.Book_Id,
                               book.Book_Name,
                               book.Book_Author,
                               book.Book_Count,
                               promotion = pro == null ? 0 : pro.Promotion,
                               output = outp == null ? 0 : outp.Output_Price
                           };

                //Thêm vào list
                foreach (var item in data)
                {
                    CBook Book = new CBook
                    {
                        Id = item.Book_Id,
                        Name = item.Book_Name,
                        Author = item.Book_Author,
                        Count = (int)item.Book_Count,
                        Price = new CBook_Price { OutputPrice = (float)item.output },
                        Promotion = (float)item.promotion,
                        PricePromotion = item.promotion == 0 ? (float)item.output : (float)item.output - (float)item.output * (float)item.promotion
                    };

                    //Kiểm tra nếu như sách có giá khuyến mãi thì kiểm tra theo giá gốc, nếu như có khuyến mãi thì kiểm tra theo giá khuyến mãi
                    float Check = Book.Promotion == 0 ? Book.Price.OutputPrice : Book.PricePromotion;
                    if (Check >= MinPrice && Check <= MinPrice)
                    {
                        List.Add(Book);
                    }
                }
            }
            catch
            {

            }
            return List;
        }

        /// <summary>
        /// Hàm trả về danh sách sách theo điều kiện nhập vào
        /// </summary>
        /// <param name="Name">Tên sách</param>
        /// <param name="Author">Tác giả</param>
        /// <param name="Theme">Chủ đề</param>
        /// <param name="Type">Thể loại</param>
        /// <param name="MinPrice">Giá thấp nhất</param>
        /// <param name="MaxPrice">Giá cao nhất</param>
        /// <param name="currentPage">Trang cần lấy</param>
        /// <param name="NumberPage">Số lượng sách ở mỗi trang</param>
        /// <returns></returns>
        public List<CBook> FindBook(string Name, string Author, string Theme, string Type, float MinPrice, float MaxPrice, int currentPage, int NumberPage,bool isSale)
        {
            List<CBook> List = new List<CBook>();

            try
            {
                List<Book> Data = DataProvider.Ins.DB.Books.ToList();
 
                //Lọc theo tên
                if (string.IsNullOrEmpty(Name) || Name.ToLower() == "tất cả")
                {
                    //do nothing
                }
                else
                {
                    Data = Data.Where(x => x.Book_Name.ToLower().Contains(Name.ToLower())).ToList();
                }

                //Lọc theo tác giả
                if (string.IsNullOrEmpty(Author) || Author.ToLower() == "tất cả")
                {
                    //do nothing
                }
                else
                {
                    Data = Data.Where(x => x.Book_Author.ToLower() == Author.ToLower()).ToList();
                }

                //Lọc theo Chủ đề
                if (string.IsNullOrEmpty(Theme) || Theme.ToLower() == "tất cả")
                {
                    //do nothing
                }
                else
                {
                    Data = Data.Where(x => x.Book_Theme.ToLower() == Theme.ToLower()).ToList();
                }

                //Lọc theo thể loại
                if (string.IsNullOrEmpty(Type) || Type.ToLower() == "tất cả")
                {
                    //do nothing
                }
                else
                {
                    Data = Data.Where(x => x.Book_Type.ToLower() == Type.ToLower()).ToList();
                }

                //Lấy từng trang
                if (currentPage > 0 && NumberPage > 0)
                {
                    Data = Data.Skip((NumberPage - 1) * currentPage).Take(currentPage).ToList();
                }

                //Thêm vào List sách lọc theo tên, tác giả,thể loại,chủ đề
                foreach (var item in Data)
                {
                    CBook Book;
                    if (item.Book_Image == null)
                    {
                        BitmapImage image = new BitmapImage(new Uri("pack://application:,,,/" + "./Image/Book.png"));
                        float Promotion = (float)item.Book_Output_PromotionPrice.Select(x => x.Promotion).FirstOrDefault();
                        float OutputPrice = (float)item.Book_Output_Price.OrderByDescending(x => x.Date_Set).Select(x => x.Output_Price).FirstOrDefault();

                        if (isSale == true)
                        {
                            if (Promotion == 0)
                            {
                                goto Endloop;
                            }
                        }

                        Book = new CBook
                        {
                            Id = item.Book_Id,
                            Name = item.Book_Name,
                            Author = item.Book_Author,
                            Type = item.Book_Type,
                            Theme = item.Book_Theme,
                            Count = (int)item.Book_Count,
                            Image = image,
                            Promotion = Promotion,
                            Price = new CBook_Price { OutputPrice = (float)item.Book_Output_Price.OrderByDescending(x => x.Date_Set).Select(x => x.Output_Price).FirstOrDefault() },
                            PricePromotion = Promotion == 0 ? OutputPrice : OutputPrice - OutputPrice * Promotion,
                            TotalCount = item.Bill_Info.Select(x => x.Book_Count).Sum(),

                            PriceVisibility = Promotion == 0 ? Visibility.Collapsed : Visibility.Visible
                        };
                        
                        List.Add(Book);
                    }
                    else
                    {
                        float Promotion = (float)item.Book_Output_PromotionPrice.Select(x => x.Promotion).FirstOrDefault();
                        float OutputPrice = (float)item.Book_Output_Price.OrderByDescending(x => x.Date_Set).Select(x => x.Output_Price).FirstOrDefault();

                        if (isSale == true)
                        {
                            if (Promotion == 0)
                            {
                                goto Endloop;
                            }
                        }

                        Book = new CBook
                        {
                            Id = item.Book_Id,
                            Name = item.Book_Name,
                            Author = item.Book_Author,
                            Type = item.Book_Type,
                            Theme = item.Book_Theme,
                            Count = (int)item.Book_Count,
                            Image = Help.ByteToImage(item.Book_Image),
                            Promotion = Promotion,
                            Price = new CBook_Price { OutputPrice = OutputPrice },
                            PricePromotion = Promotion == 0 ? OutputPrice : OutputPrice - OutputPrice * Promotion,
                            TotalCount = item.Bill_Info.Select(x => x.Book_Count).Sum(),

                            PriceVisibility = Promotion == 0 ? Visibility.Collapsed : Visibility.Visible
                        };

                        List.Add(Book);
                    }

#pragma warning disable CS0164 // This label has not been referenced
                Endloop:
                    continue;
#pragma warning restore CS0164 // This label has not been referenced
                }

            }
            catch
            {

            }
            return List;
        }

        #endregion
    }
}
