using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Model.MyClass
{
    public class CReport
    {
        #region design pattern singleton

        private static CReport _ins;
        public static CReport Ins
        {
            get
            {
                if (_ins == null)
                    _ins = new CReport();
                return _ins;
            }
            set
            {
                _ins = value;
            }
        }

        #endregion

        #region private properties

        private DateTime _Date;//Ngày,tháng cần thống kê
        private int _CountBook;//Số lượng sách bán ra 
        private float _TotalMoney;//Tổng tiền thu được từ việc bán sách
        private float _Profit;//Lợi nhuận 
        private int _CountBookInput;//Tổng sách nhập vào
        private float _TotalMoneyInput;//tiền nhập sách
        private float _SumSalary;//Tiền lương trả cho nhân viên

        //Bỏ qua tiền điện, tiền nhà, tiền dịch vụ ..............Nhiều quá

        #endregion

        #region public properties

        public DateTime Date { get { return _Date; } set { _Date = value; } }
        public int CountBook { get { return _CountBook; } set { _CountBook = value; } }
        public float Profit { get { return _Profit; } set { _Profit = value; } }
        public float TotalMoney { get { return _TotalMoney; } set { _TotalMoney = value; } }
        public int CountBookInput { get { return _CountBookInput; } set { _CountBookInput = value; } }
        public float ToltalMoneyInput { get { return _TotalMoneyInput; } set { _TotalMoneyInput = value; } }
        public float SumSalary { get { return _SumSalary; } set { _SumSalary = value; } }


        #endregion

        #region method

        /// <summary>
        /// hàm trả về danh sách sách bán được trong ngày, tổng tiền thu được, lợi nhuận một cách tương đối
        /// </summary>
        /// <returns></returns>
        public List<CReport> ListReport()
        {
            List<CReport> List = new List<CReport>();
            try
            {
                //Lấy ra list date trong danh sách hóa đơn (lấy ra những ngày khác nhau không lấy ra giờ vì trong một ngày có thể có nhiều hóa đơn)
                var ListDate = DataProvider.Ins.DB.Bills.Select(x => EntityFunctions.TruncateTime(x.Bill_Date)).Distinct();
                if (ListDate.Count() > 0)
                {
                    //Duyệt trong listday và so sánh với 
                    foreach (var date in ListDate)
                    {
                        //Lấy ra danh sách thông tin hóa đơn theo ngày (như là groupby)
                        var dataBillInfo = DataProvider.Ins.DB.Bill_Info.Where(x => EntityFunctions.TruncateTime(x.Bill.Bill_Date) == date);

                        //Lấy ra tổng số sách                            
                        int CountBook = dataBillInfo.Sum(x => x.Book_Count);
                        Console.WriteLine(CountBook);

                        //Lấy ra tổng số tiền bán ra trong ngày
                        float TotalMoney = (float)dataBillInfo.Sum(x => x.Price);
                        Console.WriteLine(TotalMoney);

                        //Tính toán tổng lợi nhuận dựa trên giá bán của sách trừ cho giá nhập của sách
                        float sum = 0;
                        foreach (var item in dataBillInfo)
                        {
                            //Lấy ra giá sách nhập vào với ngày cài đặt gần nhất so với ngày cần truy vấn
                            var inputPrice = item.Book.Book_Input_Price.Where(x => x.Date_Set <= date).OrderByDescending(x => x.Date_Set).Select(x => x.Input_Price).FirstOrDefault();
                            //Console.WriteLine($"fffffffffff{inputPrice} fsf {item.Book.Book_Name}");

                            //Console.WriteLine($"{item.Price} giá bán lúc đó");
                            sum += (float)(item.Price - inputPrice);
                        }

                        //Tạo mới CReport
                        CReport Report = new CReport
                        {
                            Date = (DateTime)date,
                            CountBook = CountBook,
                            TotalMoney = TotalMoney,
                            Profit = sum
                        };

                        //Thêm vào list
                        List.Add(Report);
                    }
                }             
            }
            catch
            {

            }
            return List;
        }

        /// <summary>
        /// Hàm trả về ListReport theo tháng
        /// </summary>
        /// <param name="Month"></param>
        /// <returns></returns>
        public List<CReport> MonthlyReport(int Month,int Year)
        {
            List<CReport> List = new List<CReport>();

            try
            {
                //Lấy ra list date trong danh sách hóa đơn (lấy ra những ngày khác nhau không lấy ra giờ vì trong một ngày có thể có nhiều hóa đơn)
                //Lấy ra những tháng và năm như điều kiện nhập vào
                //https://stackoverflow.com/questions/30588033/get-date-part-only-from-datetime-value-using-entity-framework

                var ListDate = DataProvider.Ins.DB.Bills.Where(x => SqlFunctions.DatePart("year", x.Bill_Date) == Year
                && SqlFunctions.DatePart("month", x.Bill_Date) == Month).Select(x => EntityFunctions.TruncateTime(x.Bill_Date)).Distinct();
               
                if (ListDate.Count() > 0)
                {
                    //Duyệt trong listday và so sánh với 
                    foreach (var date in ListDate)
                    {
                        //Lấy ra danh sách thông tin hóa đơn theo ngày (như là groupby)
                        var dataBillInfo = DataProvider.Ins.DB.Bill_Info.Where(x => EntityFunctions.TruncateTime(x.Bill.Bill_Date) == date);

                        //Lấy ra tổng số sách                            
                        int CountBook = dataBillInfo.Sum(x => x.Book_Count);                       

                        //Lấy ra tổng số tiền bán ra trong ngày
                        float TotalMoney = 0;
                        foreach (var item in dataBillInfo)
                        {
                            TotalMoney += (float)item.Price * item.Book_Count;                        
                        }
                        
                        //Tính toán tổng lợi nhuận dựa trên giá bán của sách trừ cho giá nhập của sách
                        float sum = 0;
                        foreach (var item in dataBillInfo)
                        {
                            //Lấy ra giá sách nhập vào với ngày cài đặt gần nhất so với ngày cần truy vấn
                            var inputPrice = item.Book.Book_Input_Price.Where(x => x.Date_Set <= date).OrderByDescending(x => x.Date_Set).Select(x => x.Input_Price).FirstOrDefault();                            
                            
                            sum += (float)(item.Price - inputPrice)*item.Book_Count;//Nhân cho số lượng sách
                        }

                        //Tạo mới CReport
                        CReport Report = new CReport
                        {
                            Date = (DateTime)date,
                            CountBook = CountBook,
                            TotalMoney = TotalMoney,
                            Profit = sum
                        };

                        //Thêm vào list
                        List.Add(Report);
                    }
                }
            }
            catch
            {

            }

            return List;
        }

        /// <summary>
        /// Hàm trả về báo cáo số lượng sách nhập, bán ra trong tháng, tính toán lợi nhuận trong tháng
        /// </summary>
        /// <returns></returns>
        public List<CReport> ListReportAllMonth(int Year)
        {
            List<CReport> List = new List<CReport>();
            try
            {
                //Duyệt 12 tháng trong năm
                for (int Month = 1; Month <= 12; Month++)
                {
                    //Lấy ra tổng số sách nhập trong tháng
                    int CountBookInput = DataProvider.Ins.DB.Book_Input.Where(x => SqlFunctions.DatePart("year", x.Input_Date) == Year &&
                    SqlFunctions.DatePart("month", x.Input_Date) == Month).Select(x => x.Book_Count).DefaultIfEmpty(0).Sum();

                    //Lấy ra tổng số tiền nhập sách trong tháng
                    float TotalMoneyInput = (float)DataProvider.Ins.DB.Book_Input.Where(x => SqlFunctions.DatePart("year", x.Input_Date) == Year &&
                    SqlFunctions.DatePart("month", x.Input_Date) == Month).Select(x => x.Book_Count * x.Input_Price).DefaultIfEmpty(0).Sum();

                    //Lấy ra tổng tiền bán sách trong tháng
                    float TotalMoney = (float)DataProvider.Ins.DB.Bill_Info.Where(x => SqlFunctions.DatePart("year", x.Bill.Bill_Date) == Year &&
                    SqlFunctions.DatePart("month", x.Bill.Bill_Date) == Month).Select(x => x.Book_Count * x.Price).DefaultIfEmpty(0).Sum();

                    //Lấy ra tổng số sách bán được trong tháng
                    int CountBook = DataProvider.Ins.DB.Bill_Info.Where(x => SqlFunctions.DatePart("year", x.Bill.Bill_Date) == Year &&
                    SqlFunctions.DatePart("month", x.Bill.Bill_Date) == Month).Select(x => x.Book_Count).DefaultIfEmpty(0).Sum();

                    //Lấy ra tổng tiền lương trả cho nhân viên trong tháng
                    float SumSalary = (float)DataProvider.Ins.DB.PayWage_History.Where(x => SqlFunctions.DatePart("year", x.PayWage_Date) == Year &&
                      SqlFunctions.DatePart("month", x.PayWage_Date) == Month).Select(x => x.Employee_Salary).DefaultIfEmpty(0).Sum();

                    //Tính toán lợi nhuận bằng tiền bán sách trừ cho tiền nhập sách với tiền lương trả cho nhân viên trong tháng đó
                    float Profit = TotalMoney - TotalMoneyInput - SumSalary;

                    //Tạo mới CReport
                    CReport Report = new CReport
                    {
                        Date = new DateTime(Year, Month, 1),
                        CountBook = CountBook,
                        CountBookInput = CountBookInput,
                        TotalMoney = TotalMoney,
                        ToltalMoneyInput = TotalMoneyInput,
                        SumSalary = SumSalary,
                        Profit = Profit
                    };

                    //Thêm vào List
                    List.Add(Report);                    
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
