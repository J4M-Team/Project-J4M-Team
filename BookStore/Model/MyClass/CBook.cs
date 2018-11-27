﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                    var Book = new CBook { Id = item.Book_Id, Name = item.Book_Name, Author = item.Book_Author, Type = item.Book_Type, Theme = item.Book_Theme, Count = (int)item.Book_Count };
                    List.Add(Book);
                }
            }
            catch
            {

            }
            return List;
        }

        public bool Add(CBook BookData)
        {
            try
            {
                var Book = new Book { Book_Name = BookData.Name, Book_Author = BookData.Author, Book_Type = BookData.Type, Book_Theme = BookData.Theme, Book_Count = BookData.Count };
                DataProvider.Ins.DB.Books.Add(Book);
                DataProvider.Ins.DB.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }

        }

        #endregion
    }
}
