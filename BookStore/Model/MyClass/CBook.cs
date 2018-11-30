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

        #endregion
    }
}
