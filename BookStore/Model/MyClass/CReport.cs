using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Model.MyClass
{
    public class CReport
    {
        #region private properties

        private DateTime _Date;//Ngày cần thống kê
        private int _CountBook;//Số lượng sách bán ra tương ứng theo ngày
        private float _TotalMoney;//Tổng tiền thu được từ việc bán sách
        private float _Profit;//Lợi nhuận trong ngày

        #endregion

        #region public properties

        public DateTime Date { get { return _Date; } set { _Date = value; } }
        public int CountBook { get { return _CountBook; } set { _CountBook = value; } }
        public float Profit { get { return _Profit; } set { _Profit = value; } }
        public float TotalMoney { get { return _TotalMoney; } set { _TotalMoney = value; } }

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

        #endregion
    }
}
