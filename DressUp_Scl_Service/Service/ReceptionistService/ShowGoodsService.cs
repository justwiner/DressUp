using DressUp_Scl_Data.Data;
using DressUp_Scl_Service.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DressUp_Scl_Service.Service.ReceptionistService
{
    public class ShowGoodsService
    {
        private DressUpWebDbEntities db = new DressUpWebDbEntities();
        public List<GoodsTypes> GetDataGoodsTypes()
        {
            return db.GoodsTypes.ToList();
        }
        public List<GoodsTypes_> GetAllGoodsTypes()
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
        public List<GoodsTypes_> GetGoodsTypesChildren(GoodsTypes type)
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
        public Boolean IfhasParent(GoodsTypes type)
        {
            if (db.GoodsTypes.SingleOrDefault(m => m.TypeId == type.FatherTypeId) != null)
            {
                return true;
            }
            return false;
        }
        public List<Goods> GetHotGoods() {
            List<Goods> list = db.Goods.OrderByDescending(m => m.GoodsSaleNum).Where(m => m.GoodsStatus == "已上架").ToList();
            return list.GetRange(0, 9);
        }
        public List<Goods> GetGoodsByTypeId(int typeId) {

            List<Goods> list = db.Goods.Where(m => m.ClassificationId == typeId).Where(m => m.GoodsStatus == "已上架").ToList();
            if (list.Count >= 9)
            {
                return list.GetRange(0, 9);
            }
            else {
                return list;
            }
        }
        public List<Goods> GetGoodsBySearch(string keyWords) {
            List<Goods> list = new List<Goods>();
            List<Goods> goodsList = db.Goods.Where(m => m.GoodsStatus == "已上架").ToList();
            foreach (Goods item in goodsList) {
                if (item.GoodsName.Contains(keyWords)) {
                    list.Add(item);
                }
            }
            return list;
        }
        public List<Goods> GetAllGoods()
        {
            return db.Goods.ToList();
        }
        public object GetTypeEnglishName(int typeId)
        {
            return db.GoodsTypes.SingleOrDefault(m => m.TypeId == typeId).TypeEnglishName;
        }
        public object GetTypeName(int typeId)
        {
            return db.GoodsTypes.SingleOrDefault(m => m.TypeId == typeId).TypeName;
        }
        public Goods GetGoodsById(Guid goodsId) {
            return db.Goods.SingleOrDefault(m => m.GoodsId == goodsId);
        }
        public List<GoodsAtlas> GetGoodsAtlas(Guid goodsId) {
            return db.GoodsAtlas.Where(m => m.GoodsId == goodsId).ToList();
        }
        public List<GoodsTypes> GetGoodsTypes() {
            return db.GoodsTypes.ToList();
        }
        public List<ShoppingCarts> GetShoppingCarts(Users buyer) {
            return db.ShoppingCarts.Where(m => m.UserId == buyer.UserId).ToList();
        }
        public List<ReceivingInfos> GetAllReceivingInfos() {
            return db.ReceivingInfos.ToList();
        }
    }
}
