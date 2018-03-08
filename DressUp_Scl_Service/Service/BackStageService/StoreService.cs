using DressUp_Scl_Data.Data;
using DressUp_Scl_Service.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DressUp_Scl_Service.Service.BackStageService
{
    public class StoreService
    {
        private DressUpWebDbEntities db = new DressUpWebDbEntities();
        private readonly int STOCK_LIMIT = 100;
        //******************************************库存查询******************************//
        public List<Goods> GetAllGoods() {
            return db.Goods.ToList();
        }
        public Goods GetGoodsById(Guid? id)
        {
            return GetAllGoods().SingleOrDefault(m => m.GoodsId == id);
        }
        public List<GoodsTypes> GetDataGoodsTypes() {
            return db.GoodsTypes.ToList();
        }
        public List<GoodsTypes_> GetAllGoodsTypes()
        {
            List<GoodsTypes_> list = new List<GoodsTypes_>();
            foreach (GoodsTypes item in db.GoodsTypes.ToList()) {
                if (!IfhasParent(item)) {
                    list.Add(new GoodsTypes_() {
                        FatherTypeId = item.FatherTypeId,
                        TypeId = item.TypeId,
                        TypeName = item.TypeName,
                        Children = GetGoodsTypesChildren(item)
                    });
                }
            }
            return list;
        }
        public List<GoodsTypes_> GetGoodsTypesChildren(GoodsTypes type) {
            List<GoodsTypes> list = db.GoodsTypes.ToList().FindAll(m => m.FatherTypeId == type.TypeId);
            List<GoodsTypes_> types = new List<GoodsTypes_>();
            foreach (GoodsTypes item in list) {
                types.Add(new GoodsTypes_()
                {
                    FatherTypeId = item.FatherTypeId,
                    TypeId = item.TypeId,
                    TypeName = item.TypeName,
                    Children = null,
                });
            }
            return types;
        }
        public Boolean IfhasParent(GoodsTypes type) {
            if (db.GoodsTypes.SingleOrDefault(m => m.TypeId == type.FatherTypeId) != null)
            {
                return true;
            }
            return false;
        }
        public List<Goods> GetListByType(int typeId) {
            if (typeId != 400)
            {
                GoodsTypes type = GetDataGoodsTypes().Find(m => m.TypeId == typeId);
                List<Goods> goodsList = new List<Goods>();
                GoodsTypes chlidType = GetDataGoodsTypes().Find(m => m.FatherTypeId == type.TypeId);
                if (chlidType == null)
                {
                    goodsList = GetAllGoods().FindAll(m => m.ClassificationId == typeId);
                }
                else
                {
                    List<GoodsTypes> list = GetDataGoodsTypes().FindAll(m => m.FatherTypeId == type.TypeId);
                    foreach (GoodsTypes item in list)
                    {
                        goodsList.AddRange(GetAllGoods().FindAll(m => m.ClassificationId == item.TypeId));
                    }
                }
                return goodsList;
            }
            else
                return GetAllGoods();
        }
        public List<Goods> GetListByStock(int stockId)
        {
            if (stockId != 400)
            {
                List<Goods> goodsList = new List<Goods>();
                if (stockId == 1)
                {
                    goodsList = GetAllGoods().FindAll(m => m.Stock >= STOCK_LIMIT);
                }
                if (stockId == -1)
                {
                    goodsList = GetAllGoods().FindAll(m => m.Stock < STOCK_LIMIT);
                }
                return goodsList;
            }
            else
                return GetAllGoods();
        }
        public List<Goods> GetListByStatus(int statusId)
        {
            if (statusId != 400)
            {
                List<Goods> goodsList = new List<Goods>();
                if (statusId == 1)
                {
                    goodsList = GetAllGoods().FindAll(m => m.GoodsStatus == "已上架");
                }
                if (statusId == -1)
                {
                    goodsList = GetAllGoods().FindAll(m => m.GoodsStatus == "已下架");
                }
                return goodsList;
            }
            else
                return GetAllGoods();
        }
        public List<Goods> FindIntersectionByGoodsId(List<Goods> list1,List<Goods> list2) {
            List<Goods> result = new List<Goods>();
            foreach (Goods m in list1) {
                foreach (Goods n in list2) {
                    if (m.GoodsId == n.GoodsId) {
                        result.Add(m);
                    }
                }
            }
            return result;
        }
        //******************************************入库操作******************************//
        public Boolean NewStorage(string goodsName, string goodsType, int goodsNum, int purchasePrice,Users user) {
            try
            {
                int typeId = GetDataGoodsTypes().Find(m => m.TypeName == goodsType).TypeId;
                Goods goods = new Goods()
                {
                    //**********************不可空字段************************//
                    GoodsId = Guid.NewGuid(),
                    GoodsName = goodsName,
                    ClassificationId = typeId,
                    GoodsSimpleGraph = "/Picture/FrontPic/未添加图片.png",
                    PurchasePrice = purchasePrice,
                    GoodsStatus = "已下架",
                    Stock = goodsNum,
                    GoodsSaleNum = 0,
                    //**********************可空字段************************//
                    GoodsDetail = "",
                    Price = 0,
                    PromotionPrice = 0,
                    ShelvesNum = 0,
                    IfHot = false,
                    IfNewGoods = false,
                };
                db.Goods.Add(goods);

                OperationRecords operationRecord = new OperationRecords()
                {
                    RecordId = Guid.NewGuid(),
                    UserId = user.UserId,
                    RecordingTime = DateTime.Now,
                    Details = "进购" + goods.GoodsName + "入库" + goodsNum + "件",
                    OperationType = "入库"
                };
                db.OperationRecords.Add(operationRecord);

                db.SaveChanges();
                return true;
            }
            catch {
                return false;
            }
        }
        //******************************************新消息处理****************************//
        public List<ShipmentList> GetAllShipmentList()
        {
            return db.ShipmentList.ToList();
        }

        //旧品入库
        public bool GoodsPurchase(Guid messageId,Users user,Guid id,int goodsNum)
        {
            try
            {
                Goods goods = GetAllGoods().SingleOrDefault(m => m.GoodsId == id);
                db.Goods.SingleOrDefault(m => m.GoodsId == id).Stock += goodsNum;
                db.SaveChanges();

                OperationRecords operationRecord = new OperationRecords()
                {
                    RecordId = Guid.NewGuid(),
                    UserId = user.UserId,
                    RecordingTime = DateTime.Now,
                    Details = "进购" + goods.GoodsName + "入库" + goodsNum + "件",
                    OperationType = "入库"
                };
                db.OperationRecords.Add(operationRecord);
                db.PurchaseList.SingleOrDefault(m => m.OrderNum == messageId).OrderStatus = "已处理";
                db.SaveChanges();
                
                return true;
            }
            catch {
                return false;
            }
        }
        //新品入库
        public bool GoodsPurchase(Guid messageId,Users user, string goodsName, string goodsType, int goodsNum, decimal purchasePrice)
        {
            try
            {
                int goodsTypeId = GetDataGoodsTypes().SingleOrDefault(m => m.TypeName == goodsType).TypeId;
                Goods goods = new Goods()
                {
                    GoodsId = Guid.NewGuid(),
                    GoodsName = goodsName,
                    ClassificationId = goodsTypeId,
                    Stock = goodsNum,
                    PurchasePrice  = purchasePrice,
                    GoodsSimpleGraph = "/Picture/FrontPic/未添加图片.png",
                    GoodsStatus = "已下架",
                    GoodsSaleNum = 0,

                    //**********************可空字段************************//
                    GoodsDetail = "",
                    Price = 0,
                    PromotionPrice = 0,
                    ShelvesNum = 0,
                    IfHot = false,
                    IfNewGoods = false,
                };
                db.Goods.Add(goods);
                OperationRecords operationRecord = new OperationRecords()
                {
                    RecordId = Guid.NewGuid(),
                    UserId = user.UserId,
                    RecordingTime = DateTime.Now,
                    Details = "进购" + goods.GoodsName + "入库" + goodsNum + "件",
                    OperationType = "入库"
                };
                db.OperationRecords.Add(operationRecord);
                db.PurchaseList.SingleOrDefault(m => m.OrderNum == messageId).OrderStatus = "已处理";
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        //出库
        public bool GoodsShipment(Guid orderID,Guid messageId,Users user,Guid id, string goodsName, string contactInfo, int goodsNum, string consignee, string receiptAddress)
        {
            try
            {
                db.Goods.SingleOrDefault(m => m.GoodsId == id).Stock -= goodsNum;
                db.Orders.SingleOrDefault(m => m.OrderNum == orderID).OrderStatus = "已发货";
                OperationRecords operationRecord = new OperationRecords()
                {
                    RecordId = Guid.NewGuid(),
                    UserId = user.UserId,
                    RecordingTime = DateTime.Now,
                    Details = goodsName + "出库" + goodsNum + "件",
                    OperationType = "出库"
                };
                db.OperationRecords.Add(operationRecord);
                db.ShipmentList.SingleOrDefault(m => m.OrderId == messageId).OrderStatus = "已处理";
                int num = db.Goods.FirstOrDefault(m => m.GoodsId == db.Orders.FirstOrDefault(p => p.OrderNum == orderID).GoodsId).GoodsSaleNum;
                db.Goods.FirstOrDefault(m => m.GoodsId == db.Orders.FirstOrDefault(p => p.OrderNum == orderID).GoodsId).GoodsSaleNum = num + goodsNum;
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        
    }
}
