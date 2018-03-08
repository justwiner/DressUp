using DressUp_Scl_Data.Data;
using DressUp_Scl_Service.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DressUp_Scl_Service.Service.BackStageService
{
    public class GeneralOperationService
    {
        private DressUpWebDbEntities db = new DressUpWebDbEntities();
        public Boolean CheckPassword(Users user, String oldPassword, String newPassword)
        {
            if (user.Password == oldPassword)
            {
                Users findUser = db.Users.ToList().Find(m => m.UserId == user.UserId);
                findUser.Password = newPassword;
                db.SaveChanges();
                return true;
            }
            else {
                return false;
            }
        }
        public List<OperationRecords_> GetAllRecords() {
            List<OperationRecords_> list = new List<OperationRecords_>();
            foreach (OperationRecords item in db.OperationRecords) {
                Users user = db.Users.ToList().Find(m => m.UserId == item.UserId);
                string mode = "";
                foreach (Roles m in user.Roles) {
                    mode += (m.Name + " ");
                }
                list.Add(new OperationRecords_()
                {
                    RecordId = item.RecordId,
                    UserId = item.UserId,
                    UserName = user.Name,
                    Time = item.RecordingTime.ToString(),
                    Detail = item.Details,
                    Mode = mode,
                });
            }
            return list;
        }
        public List<OperationRecords_> GetMyRecords(Users user) {
            return GetAllRecords().Where(m => m.UserId == user.UserId).ToList();
        }
        public List<OperationRecords_> FindByTime(int timeId, List<OperationRecords_> myAllRecords) {
            List<OperationRecords_> list = new List<OperationRecords_>();
            DateTime nowTime = DateTime.Now;
            if (timeId == 400)
            {
                list = myAllRecords;
            }
            else {
                switch (timeId) {
                    case 1: list = GetRecordsListByTime(myAllRecords, DateTime.Parse(nowTime.AddDays(1).ToString("yyyy-MM-dd")), DateTime.Parse(nowTime.ToString("yyyy-MM-dd"))); break;
                    case 2: list = GetRecordsListByTime(myAllRecords, DateTime.Parse(nowTime.ToString("yyyy-MM-dd")), DateTime.Parse(nowTime.AddDays(-1).ToString("yyyy-MM-dd"))); break;
                    case 3:
                        {
                            int weekNow = Convert.ToInt32(nowTime.DayOfWeek);
                            weekNow = (weekNow == 0 ? (7 - 1) : (weekNow - 1));
                            int daySpan = (-1) * weekNow;
                            list = GetRecordsListByTime(myAllRecords, nowTime.AddDays(1), nowTime.AddDays(daySpan));
                            break;
                        }
                    default: break;
                }
            }
            return list;
        }
        public List<OperationRecords_> GetRecordsListByTime(List<OperationRecords_> myAllRecords, DateTime time1,DateTime time2) {
            List<OperationRecords_> list = new List<OperationRecords_>();
            foreach (OperationRecords_ item in myAllRecords) {
                if (DateTime.Parse(item.Time) >= DateTime.Parse(time2.ToString("yyyy-MM-dd")) && DateTime.Parse(item.Time) < (DateTime.Parse(time1.ToString("yyyy-MM-dd")))) {
                    list.Add(item);
                }
            }
            return list;
        }
        public List<OperationRecords> GetMyOperationRecords(Users user)
        {
            List<OperationRecords> records = new List<OperationRecords>();
            DateTime minTime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));
            DateTime maxTime = DateTime.Parse(DateTime.Now.AddDays(1).ToString("yyyy-MM-dd"));
            List<OperationRecords> newRecords = db.OperationRecords
                .Where(m => m.UserId == user.UserId)
                .OrderByDescending(m => m.RecordingTime)
                .ToList();
            if (newRecords.Count < 3)
            {
                records.AddRange(newRecords);
            }
            else
            {
                records.Add(newRecords[0]);
                records.Add(newRecords[1]);
                records.Add(newRecords[2]);
            }
            return records;
        }
        public List<Goods> GetSalesRanking()
        {
            return db.Goods.OrderByDescending(m => m.GoodsSaleNum).ToList();
        }
        public List<Goods> GetStockSubtotalList()
        {
            return db.Goods.OrderBy(m => m.Stock).ToList();
        }
        public int GetNewMessageNum(Users user)
        {
            int messageNum = 0;
            if (user.Roles.Count == 1) {
                if (user.Roles.ElementAt(0).RoleId.ToString() == "9c2afc0e-8088-42cd-8cf4-91afbb8bcf31")
                {
                    messageNum = db.Orders.Where(m => m.OrderStatus == "已支付").Count();
                    messageNum += db.Orders.Where(m => m.OrderStatus == "未支付").Count();
                }
                else if (user.Roles.ElementAt(0).RoleId.ToString() == "f4ca2d2c-3ff4-473a-87a0-2e1d4d9ed328")
                {
                    messageNum = db.PurchaseList.Where(m => m.OrderStatus == "未处理").Count();
                    messageNum += db.ShipmentList.Where(m => m.OrderStatus == "未处理").Count();
                }
            }
            return messageNum;
        }
        public void RecordOperationRecords(Users user, string detail, string type)
        {
            OperationRecords record = new OperationRecords()
            {
                RecordId = Guid.NewGuid(),
                UserId = user.UserId,
                RecordingTime = DateTime.Now,
                Details = detail,
                OperationType = type,
            };
            db.OperationRecords.Add(record);
            db.SaveChanges();
        }
    }
}
