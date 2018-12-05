using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace BookStore.Model.MyClass
{
    public class CBook
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

        #endregion

        #region public properties

        public int Id { get { return _Id; } set { _Id = value; } }
        public string Name { get { return _Name; } set { _Name = value; } }
        public string Author { get { return _Author; } set { _Author = value; } }
        public string Theme { get { return _Theme; } set { _Theme = value; } }
        public string Type { get { return _Type; } set { _Type = value; } }
        public string Content { get { return _Content; } set { _Content = value; } }
        public int Count { get { return _Count; } set { _Count = value; } }
        public CBook_Price Price { get { return _Price; } set { _Price = value; } }
        public BitmapImage Image { get { return _Image; } set { _Image = value; } }

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
                foreach(var item in sql)
                {
                    CBook Book;
                    if(item.Book_Image == null)
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
                            Image = image
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
                            Image = Help.ByteToImage(item.Book_Image)
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
        /// <returns>nếu thành công trả về true, thất bại trả về false</returns>
        public bool Add(CBook BookData)
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
                return true;
            }
            catch
            {
                return false;
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
                var data = DataProvider.Ins.DB.Books.Where(x => x.Book_Id == Id).Select(x=>x.Book_Image).First();
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
        public List<CBook>ListHistoryOutputPrice(int Book_Id)
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
                        CBook Book = new CBook { Id = item.Book_Id,Price=price };
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

        #endregion
    }
}
