using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Model.MyClass
{
    public class CSalesman : CEmployee
    {
        #region design pattern singleton

        private static CSalesman _ins;
        public static CSalesman Ins_Salesman
        {
            get
            {
                if (_ins == null)
                    _ins = new CSalesman();
                return _ins;
            }
            set
            {
                _ins = value;
            }
        }

        #endregion

        #region method

        public int AddBill(CBill dataBill)
        {
            int BillId = 0;
            try
            {
                //Tạo mới Bill
                Bill bill = new Bill
                {
                    Employee_Id = dataBill.Salesman.Id,
                    Customer_Id=dataBill.Customer.Id,
                    Bill_Total_Money=dataBill.TotalMoney,
                    Bill_Type = dataBill.Type,
                    Bill_Date=dataBill.Date
                };

                //Thêm mới
                DataProvider.Ins.DB.Bills.Add(bill);

                //Lưu thay đổi
                DataProvider.Ins.DB.SaveChanges();

                //Lấy ra id của bill vừa tạo
                BillId = DataProvider.Ins.DB.Bills.Where(x => x.Customer_Id == dataBill.Customer.Id
                && x.Employee_Id == dataBill.Salesman.Id).OrderByDescending(x => x.Bill_Date).Select(x => x.Bill_Id).FirstOrDefault();

                //Thêm vào bill_info
                foreach(var item in dataBill.ListBook)
                {
                    //tạo mới bill_info
                    Bill_Info info = new Bill_Info
                    {
                        Bill_Id = BillId,
                        Book_Id = item.Id,
                        Book_Count = item.Count,
                        Price = item.PricePromotion
                    };

                    //Thêm mới
                    DataProvider.Ins.DB.Bill_Info.Add(info);

                    //Lưu thay đổi 
                    DataProvider.Ins.DB.SaveChanges();

                    //Trừ số lượng sách tương ứng với id
                    CBook.Ins.DecreaseNumberOfBook(item.Id, item.Count);
                }

                //Thêm vào trong output_info
                DataProvider.Ins.DB.Output_Info.Add(new Output_Info { Bill_Id = BillId });

                //Lưu thay đổi
                DataProvider.Ins.DB.SaveChanges();

                return BillId;
            }
            catch
            {

            }
            return BillId;
        }

        #endregion

    }
}
