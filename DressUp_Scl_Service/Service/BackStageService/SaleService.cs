using DressUp_Scl_Data.Data;
using DressUp_Scl_Service.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DressUp_Scl_Service.Service.BackStageService
{
    public class SaleService
    {
        static DressUpWebDbEntities db = new DressUpWebDbEntities();
        private static readonly int STOCK_LIMIT = 100;
        #region 商品
        public static List<Goods> GetListByType(int typeId,List<Goods> list_)
        {
            if (typeId != 400)
            {
                GoodsTypes type = GetDataGoodsTypes().Find(m => m.TypeId == typeId);
                List<Goods> goodsList = new List<Goods>();
                GoodsTypes chlidType = GetDataGoodsTypes().Find(m => m.FatherTypeId == type.TypeId);
                if (chlidType == null)
                {
                    goodsList = list_.FindAll(m => m.ClassificationId == typeId);
                }
                else
                {
                    List<GoodsTypes> list = GetDataGoodsTypes().FindAll(m => m.FatherTypeId == type.TypeId);
                    foreach (GoodsTypes item in list)
                    {
                        goodsList.AddRange(list_.FindAll(m => m.ClassificationId == item.TypeId));
                    }
                }
                return goodsList;
            }
            else
                return list_;
        }
        public static List<Goods> GetListByStock(int stockId, List<Goods> list_)
        {
            if (stockId != 400)
            {
                List<Goods> goodsList = new List<Goods>();
                if (stockId == 1)
                {
                    goodsList = list_.FindAll(m => m.Stock >= STOCK_LIMIT);
                }
                if (stockId == -1)
                {
                    goodsList = list_.FindAll(m => m.Stock < STOCK_LIMIT);
                }
                return goodsList;
            }
            else
                return list_;
        }
        public List<Goods> GetListByStock_noStatic(int stockId)
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
        public static List<Goods> GetAllGoods()
        {
            return db.Goods.ToList();
        }
        public List<Goods> GetAllGoods_noStatic()
        {
            return db.Goods.ToList();
        }
        public static List<Goods> GetListByStatus(int statusId, List<Goods> list_)
        {
            if (statusId != 400)
            {
                List<Goods> goodsList = new List<Goods>();
                if (statusId == 1)
                {
                    goodsList = list_.FindAll(m => m.GoodsStatus == "已上架");
                }
                if (statusId == -1)
                {
                    goodsList = list_.FindAll(m => m.GoodsStatus == "已下架");
                }
                return goodsList;
            }
            else
                return list_;
        }
        public static List<GoodsTypes_> GetAllGoodsTypes()
        {
            List<GoodsTypes_> list = new List<GoodsTypes_>();
            foreach (GoodsTypes item in db.GoodsTypes.ToList())
            {
                if (!IfhasParent(item))
                {
                    list.Add(new GoodsTypes_()
                    {
                        FatherTypeId = item.FatherTypeId,
                        TypeId = item.TypeId,
                        TypeName = item.TypeName,
                        Children = GetGoodsTypesChildren(item)
                    });
                }
            }
            return list;
        }
        private static List<GoodsTypes_> GetGoodsTypesChildren(GoodsTypes type)
        {
            List<GoodsTypes> list = db.GoodsTypes.ToList().FindAll(m => m.FatherTypeId == type.TypeId);
            List<GoodsTypes_> types = new List<GoodsTypes_>();
            foreach (GoodsTypes item in list)
            {
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
        private static bool IfhasParent(GoodsTypes type)
        {
            if (db.GoodsTypes.SingleOrDefault(m => m.TypeId == type.FatherTypeId) != null)
            {
                return true;
            }
            return false;
        }
        public static List<Goods> FindIntersectionByGoodsId(List<Goods> list1, List<Goods> list2)
        {
            List<Goods> result = new List<Goods>();
            foreach (Goods m in list1)
            {
                foreach (Goods n in list2)
                {
                    if (m.GoodsId == n.GoodsId)
                    {
                        result.Add(m);
                    }
                }
            }
            return result;
        }
        public static List<GoodsTypes> GetDataGoodsTypes()
        {
            return db.GoodsTypes.ToList();
        }
        public List<GoodsAtlas> GetGoodsAtlas(Guid goodsId) {
            List < GoodsAtlas > atlasList = db.GoodsAtlas.Where(m => m.GoodsId == goodsId).ToList();
            return atlasList;
        }
        public Goods GetGoodsInfoById(Guid goodsId)
        {
            foreach (Goods item in GetAllGoods()) {
                if (item.GoodsId.Equals(goodsId)) {
                    return item;
                }
            }
            return null;
        }
        public Boolean GetIf(string str) {
            Boolean ifOrNot = false;
            if (str == "是")
            {
                ifOrNot = true;
            }
            else
            {
                ifOrNot = false;
            }
            return ifOrNot;
        }
        public int judgeShelfNum(int shelfNum ,int stock) {
            if (shelfNum > stock) {
                return stock;
            }
            return shelfNum;
        }
        public Goods ChangeGoodsInfo(Users user,Guid goodsId,string goodsName, string goodsDetail,decimal goodsPrice, decimal goodsPromotionPrice,Boolean ifPromotion,Boolean ifOnShelf,int shelfNum) {
            
            string detail = "";
            string type = "";
            if (ifOnShelf)
            {
                if (db.Goods.SingleOrDefault(m => m.GoodsId == goodsId).GoodsStatus == "已下架") {
                    db.Goods.SingleOrDefault(m => m.GoodsId == goodsId).OnShelfTime = DateTime.Now;
                    db.Goods.SingleOrDefault(m => m.GoodsId == goodsId).OffShelfTime = null;
                    db.Goods.SingleOrDefault(m => m.GoodsId == goodsId).GoodsStatus = "已上架";
                }
                db.Goods.SingleOrDefault(m => m.GoodsId == goodsId).ShelvesNum = judgeShelfNum(shelfNum, db.Goods.SingleOrDefault(m => m.GoodsId == goodsId).Stock);
                db.Goods.SingleOrDefault(m => m.GoodsId == goodsId).Stock = db.Goods.SingleOrDefault(m => m.GoodsId == goodsId).Stock - (int)db.Goods.SingleOrDefault(m => m.GoodsId == goodsId).ShelvesNum;
                detail += "为" + db.Goods.SingleOrDefault(m => m.GoodsId == goodsId).GoodsName + "上架了" + db.Goods.SingleOrDefault(m => m.GoodsId == goodsId).ShelvesNum + "件商品。";
                type += "上架.";
            }
            else {
                if (db.Goods.SingleOrDefault(m => m.GoodsId == goodsId).GoodsStatus == "已上架"){
                    db.Goods.SingleOrDefault(m => m.GoodsId == goodsId).OffShelfTime = DateTime.Now;
                    db.Goods.SingleOrDefault(m => m.GoodsId == goodsId).Stock = (int)db.Goods.SingleOrDefault(m => m.GoodsId == goodsId).ShelvesNum + db.Goods.SingleOrDefault(m => m.GoodsId == goodsId).Stock;
                    detail += "为" + db.Goods.SingleOrDefault(m => m.GoodsId == goodsId).GoodsName + "下架了" + db.Goods.SingleOrDefault(m => m.GoodsId == goodsId).ShelvesNum + "件商品。";
                    db.Goods.SingleOrDefault(m => m.GoodsId == goodsId).ShelvesNum = 0;
                    db.Goods.SingleOrDefault(m => m.GoodsId == goodsId).GoodsStatus = "已下架";
                    type += "下架.";
                }
            }
            db.Goods.SingleOrDefault(m => m.GoodsId == goodsId).GoodsName = goodsName;
            db.Goods.SingleOrDefault(m => m.GoodsId == goodsId).GoodsDetail = goodsDetail;
            db.Goods.SingleOrDefault(m => m.GoodsId == goodsId).Price = goodsPrice;
            db.Goods.SingleOrDefault(m => m.GoodsId == goodsId).PromotionPrice = goodsPromotionPrice;
            db.Goods.SingleOrDefault(m => m.GoodsId == goodsId).IfHot = ifPromotion;
            detail += "为" + db.Goods.SingleOrDefault(m => m.GoodsId == goodsId).GoodsName + "修改了商品的部分信息。";
            type += "修改信息。";
            RecordOperationRecords(user,detail,type);
            db.SaveChanges();
            Goods goods = db.Goods.SingleOrDefault(m => m.GoodsId == goodsId);
            return goods;
        }
        public void RecordOperationRecords(Users user,string detail,string type) {
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
        public bool CreatWarehousingOrder(Users user,int purchaseNum, Guid goodsId) {
            try
            {
                string goodsName = db.Goods.SingleOrDefault(m => m.GoodsId == goodsId).GoodsName;
                string detail = "";
                string type = "进货";
                detail += "为商品" + goodsName + "提交进货申请，数量：" + purchaseNum;
                RecordOperationRecords(user, detail, type);
                PurchaseList order = new PurchaseList()
                {
                    OrderNum = Guid.NewGuid(),
                    OrderType = "入库",
                    GoodsId = goodsId,
                    GoodsName = goodsName,
                    StorageNum = purchaseNum,
                    OrderStatus = "未处理",
                    OrderTime = DateTime.Now,
                };
                db.PurchaseList.Add(order);
                db.SaveChanges();
                return true;
            }
            catch  {
                return false;
            }
        }
        public string SetGoodsSimpleGraphPath(Goods goods,string fileName) {
            string path = "/Picture/FrontPic/Goods/";
            GoodsTypes goodsType = db.GoodsTypes.SingleOrDefault(m => m.TypeId == goods.ClassificationId);
            GoodsTypes goodsFatherType = db.GoodsTypes.SingleOrDefault(m => m.TypeId == goodsType.FatherTypeId);
            path += goodsFatherType.TypeEnglishName + "/" + goodsType.TypeEnglishName + "/" + goods.GoodsName;
            db.Goods.SingleOrDefault(m => m.GoodsId == goods.GoodsId).GoodsSimpleGraph = (path + "/" + fileName);
            db.SaveChanges();
            return path;
        }
        public void RemoveGoodsAtlas(Guid goodsID) {
            db.GoodsAtlas.RemoveRange(db.GoodsAtlas.Where(m => m.GoodsId == goodsID));
            db.SaveChanges();
        }
        public string SetGoodsAtlasPath(Goods goods, string fileName) {
            string path = "/Picture/FrontPic/Goods/";
            GoodsTypes goodsType = db.GoodsTypes.SingleOrDefault(m => m.TypeId == goods.ClassificationId);
            GoodsTypes goodsFatherType = db.GoodsTypes.SingleOrDefault(m => m.TypeId == goodsType.FatherTypeId);
            path += goodsFatherType.TypeEnglishName + "/" + goodsType.TypeEnglishName + "/" + goods.GoodsName + "/Atlas";
            db.GoodsAtlas.Add(new GoodsAtlas()
            {
                Img = (path + "/" + fileName),
                GoodsId = goods.GoodsId
            });
            db.SaveChanges();
            return path;
        }
        #endregion
        #region 订单
        //**************************所有订单*******************************//
        public List<Orders> GetAllOrders() {
            return db.Orders.ToList();
        }
        public List<Orders> GetOrdersByStatusId(int statusId,List<Orders> orderList) {
            List<Orders> list = new List<Orders>();
            switch (statusId) {
                case 0: list = orderList; break;
                case 1: list = orderList.Where(m => m.OrderStatus == "已发货").ToList(); break;
                case 2: list = orderList.Where(m => m.OrderStatus == "已支付").ToList(); break;
                case 3: list = orderList.Where(m => m.OrderStatus == "待发货").ToList(); break;
                case 4: list = orderList.Where(m => m.OrderStatus == "已退款").ToList(); break;
                case 5: list = orderList.Where(m => m.OrderStatus == "已完成").ToList(); break;
                case 6: list = orderList.Where(m => m.OrderStatus == "已失效").ToList(); break;
            }
            return list;
        }
        public List<Orders> GetOrdersByTime(DateTime minDate,DateTime maxDate,List<Orders> orderList) {
            List<Orders> list = new List<Orders>();
            foreach (Orders item in orderList) {
                if (item.OrderGenerationTime >= minDate && item.OrderGenerationTime <= maxDate) {
                    list.Add(item);
                }
            }
            return list;
        }
        //***************************新订单********************************//
        public List<Orders> GetNewOrders() {
            List<Orders> list = new List<Orders>();
            list.AddRange(GetAllOrders().Where(m => m.OrderStatus == "已支付"));
            return list;
        }
        public bool Library(Guid id,Users user) {
            try
            {
                db.Orders.SingleOrDefault(m => m.OrderNum == id).OrderStatus = "待发货";
                db.SaveChanges();
                Orders order = db.Orders.SingleOrDefault(m => m.OrderNum == id);
                string detail = "";
                string type = "进货";
                detail += "为商品" + order.GoodsName + "提交出库申请，数量：" + order.GoodsNum;
                RecordOperationRecords(user, detail, type);
                ShipmentList item = new ShipmentList()
                {
                    OrderId = Guid.NewGuid(),
                    Type = "出库",
                    OrderNum = id,
                    OrderStatus = "未处理",
                    OrderTime = DateTime.Now,
                };
                db.ShipmentList.Add(item);
                db.SaveChanges();
                return true;
            }
            catch {
                return false;
            }
        }
        #endregion
    }
}
