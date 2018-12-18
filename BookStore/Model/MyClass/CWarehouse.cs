using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Model.MyClass
{
    public class CWarehouse : CEmployee
    {
        #region design pattern singleton

        private static CWarehouse _ins;
        public static CWarehouse Ins_Warehouse
        {
            get
            {
                if (_ins == null)
                    _ins = new CWarehouse();
                return _ins;
            }
            set
            {
                _ins = value;
            }
        }

        #endregion

        #region method

        /// <summary>
        /// Hàm thêm lịch sử xuất kho khi nhân viên quản kho xác nhận hóa đơn từ khách hàng
        /// </summary>
        /// <param name="Bill_Id"></param>
        /// <param name="Employee_Id"></param>
        /// <returns></returns>
        public bool AddHistoryOutput(int Bill_Id, int Employee_Id)
        {
            try
            {
                Book_Output data = new Book_Output { Bill_Id = Bill_Id, Employee_Id = Employee_Id, Output_Date = DateTime.Now };
                DataProvider.Ins.DB.Book_Output.Add(data);
                DataProvider.Ins.DB.SaveChanges();
                return true;
            }
            catch
            {

            }
            return false;
        }

        /// <summary>
        /// Hàm thêm vào lịch sử nhập kho khi nhân viên nhập thêm sách mới, hoặc thêm sách vào kho
        /// </summary>
        /// <param name="Employee_Id"></param>
        /// <param name="BookData"></param>
        /// <returns></returns>
        public bool AddHistoryInput(int Employee_Id,CBook BookData)
        {
            try
            {
                //Kiểm tra data của sách, id nhân viên truyền vào
                if (BookData.Id <= 0 || BookData.Count <= 0 || Employee_Id <= 0)
                {
                    return false;
                }

                //Tìm giá nhập của sách theo id,Lấy giá nhập mới nhất trong bảng input_Price (giá nhập mới nhất có ngày cài đặt là ngày gần nhất)
                float Input_Price = (float)DataProvider.Ins.DB.Book_Input_Price.Where(x => x.Book_Id == BookData.Id).OrderByDescending(x => x.Date_Set).Select(x => x.Input_Price).FirstOrDefault();

                //Kiểm tra nếu giá nhập không tồn tại 
                if(Input_Price<=0)
                {
                    return false;
                }

                //Tạo mới
                Book_Input data = new Book_Input
                {
                    Book_Id = BookData.Id,
                    Book_Count = BookData.Count,
                    Employee_Id = Employee_Id,
                    Input_Price = Input_Price,
                    Input_Date = DateTime.Now
                };

                //Thêm vào
                DataProvider.Ins.DB.Book_Input.Add(data);

                //Lưu thay đổi
                DataProvider.Ins.DB.SaveChanges();

                return true;
            }
            catch
            {

            }
            return false;
        }

        /// <summary>
        /// Hàm trả về lịch sử nhập kho
        /// </summary>
        /// <param name="Employee_Name"></param>
        /// <param name="MinDate"></param>
        /// <param name="MaxDate"></param>
        /// <returns></returns>
        public List<CInput_History> InputHistory(string Employee_Name, DateTime MinDate, DateTime MaxDate)
        {
            List<CInput_History> List = new List<CInput_History>();

            try
            {
                IQueryable<Book_Input> data = DataProvider.Ins.DB.Book_Input.Select(x => x);

                if (MinDate > MaxDate)
                {
                    return List;
                }
                else
                {
                    if (!string.IsNullOrEmpty(Employee_Name))
                    {
                        data = DataProvider.Ins.DB.Book_Input.Where(x => x.Employee.Employee_Name.ToLower().Contains(Employee_Name.ToLower())
                        && EntityFunctions.TruncateTime(x.Input_Date) >= EntityFunctions.TruncateTime(MinDate) &&
                        EntityFunctions.TruncateTime(x.Input_Date) <= EntityFunctions.TruncateTime(MaxDate));
                    }
                    else
                    {
                        data = DataProvider.Ins.DB.Book_Input.Where(x => EntityFunctions.TruncateTime(x.Input_Date) >= EntityFunctions.TruncateTime(MinDate) &&
                        EntityFunctions.TruncateTime(x.Input_Date) <= EntityFunctions.TruncateTime(MaxDate));
                    }
                }


                if (data.Count() > 0)
                {
                    foreach (var item in data)
                    {
                        //Tạo mới

                        CInput_History History = new CInput_History
                        {
                            Id=item.Input_Id,
                            WareHouse = new CWarehouse
                            {
                                Id = item.Employee_Id,
                                Name = item.Employee.Employee_Name
                            },
                            Book = new CBook
                            {
                                Id = item.Book_Id,
                                Name = item.Book.Book_Name,
                                Count = item.Book_Count,
                                Price = new CBook_Price
                                {
                                    InputPrice = (float)item.Input_Price
                                },
                                TotalPrice = item.Book_Count * (float)item.Input_Price
                            },
                            Date = (DateTime)item.Input_Date
                        };

                        //Thêm vào List
                        List.Add(History);
                    }
                }
            }
            catch
            {

            }

            return List;
        }

        /// <summary>
        /// Hàm trả về ngày nhỏ nhất trong lịch sử nhập kho
        /// </summary>
        /// <returns></returns>
        public DateTime MinDateInput()
        {
            DateTime MinDate = new DateTime();

            try
            {
                var data = DataProvider.Ins.DB.Book_Input.OrderBy(x => x.Input_Date).Select(x => x.Input_Date).FirstOrDefault();

                MinDate = (DateTime)data;

                return MinDate;
            }
            catch
            {

            }

            return MinDate;
        }

        /// <summary>
        /// Hàm trả về ngày lớn nhất trong lịch sử nhập kho
        /// </summary>
        /// <returns></returns>
        public DateTime MaxDateInput()
        {
            DateTime MaxDate = new DateTime();

            try
            {
                var data = DataProvider.Ins.DB.Book_Input.OrderByDescending(x => x.Input_Date).Select(x => x.Input_Date).FirstOrDefault();

                MaxDate = (DateTime)data;

                return MaxDate;
            }
            catch
            {

            }

            return MaxDate;
        }

        /// <summary>
        /// Hàm trả về lịch sử xuất kho
        /// </summary>
        /// <param name="Employee_Name"></param>
        /// <param name="MinDate"></param>
        /// <param name="MaxDate"></param>
        /// <returns></returns>
        public List<COutput_History> OutPutHistory(string Employee_Name, DateTime MinDate, DateTime MaxDate)
        {
            List<COutput_History> List = new List<COutput_History>();

            try
            {
                //IQueryable<Bill_Info> data = DataProvider.Ins.DB.Bill_Info.Select(x => x);

                IQueryable<Book_Output> data = DataProvider.Ins.DB.Book_Output.Select(x => x);

                if (MinDate > MaxDate)
                {
                    return List;
                }
                else
                {
                    if (!string.IsNullOrEmpty(Employee_Name))
                    {
                        data = DataProvider.Ins.DB.Book_Output.Where(x => x.Employee.Employee_Name.ToLower().Contains(Employee_Name.ToLower())
                        && EntityFunctions.TruncateTime(x.Output_Date) >= EntityFunctions.TruncateTime(MinDate) &&
                        EntityFunctions.TruncateTime(x.Output_Date) <= EntityFunctions.TruncateTime(MaxDate));
                    }
                    else
                    {
                        data = DataProvider.Ins.DB.Book_Output.Where(x => EntityFunctions.TruncateTime(x.Output_Date) >= EntityFunctions.TruncateTime(MinDate) &&
                        EntityFunctions.TruncateTime(x.Output_Date) <= EntityFunctions.TruncateTime(MaxDate));
                    }
                }

                if (data.Count() > 0)
                {
                    foreach (var item in data)
                    {
                        CBill Bill = new CBill
                        {
                            Id = item.Bill.Bill_Id,
                            Salesman = new CSalesman
                            {
                                Id = item.Bill.Employee_Id,
                                Name = item.Bill.Employee.Employee_Name
                            },
                            Date = (DateTime)item.Bill.Bill_Date,
                            Customer = new CCustomer
                            {
                                Name = item.Bill.Customer.Customer_Name
                            },
                            ListBook = new List<CBook>()

                        };
                        //Duyệt trong bảng Bill_info để lấy thông tin sách thêm vào list
                        foreach(var item2 in item.Bill.Bill_Info)
                        {
                            //Tạo mới Book
                            CBook Book = new CBook
                            {
                                Id = item2.Book_Id,
                                Name = item2.Book.Book_Name,
                                Count = item2.Book_Count
                            };

                            //Thêm vào trong List
                            Bill.ListBook.Add(Book);
                        }

                        //Tạo mới history
                        COutput_History History = new COutput_History
                        {
                            Id = item.Output_Id,
                            WareHouse = new CWarehouse
                            {
                                Id = item.Employee_Id,
                                Name = item.Employee.Employee_Name
                            },
                            Date = (DateTime)item.Output_Date,
                            Bill = Bill
                        };

                        //Thêm vào trong List
                        List.Add(History);
                                            
                    }
                }
            }
            catch
            {

            }
            return List;
        }

        /// <summary>
        /// Hàm trả về ngày nhỏ nhất trong lịch sử xuất kho
        /// </summary>
        /// <returns></returns>
        public DateTime MinDateOutput()
        {
            DateTime MinDate = new DateTime();

            try
            {
                var data = DataProvider.Ins.DB.Book_Output.OrderBy(x => x.Output_Date).Select(x => x.Output_Date).FirstOrDefault();

                MinDate = (DateTime)data;

                return MinDate;
            }
            catch
            {

            }

            return MinDate;
        }

        /// <summary>
        /// Hàm trả về ngày lớn nhất trong lịch sử xuất kho
        /// </summary>
        /// <returns></returns>
        public DateTime MaxDateOutput()
        {
            DateTime MaxDate = new DateTime();

            try
            {
                var data = DataProvider.Ins.DB.Book_Output.OrderByDescending(x => x.Output_Date).Select(x => x.Output_Date).FirstOrDefault();

                MaxDate = (DateTime)data;

                return MaxDate;
            }
            catch
            {

            }

            return MaxDate;
        }

        #endregion
    }
}
