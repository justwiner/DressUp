using DressUp.Scl.Model.ServiceModel;
using DressUp_Scl_Data.Data;
using DressUp_Scl_Service.Service.ReceptionistService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DressUp.Scl.Service
{
    public class SimpleOrdersServcie
    {
        public ShowGoodsService service = new ShowGoodsService();
        public List<SimpleOrdersSVM> CreatSimpleOrders(List<ConciseOrder> conciseOrders) {
            List<Goods> goodsList = service.GetAllGoods();
            List<SimpleOrdersSVM> simpleOrdersList = new List<SimpleOrdersSVM>();
            List<ConciseOrder> showConciseOrder = conciseOrders.Where(m => m.IfChecked == "true").ToList();
            foreach (ConciseOrder item in showConciseOrder) {
                Goods goods = goodsList.SingleOrDefault(m => m.GoodsId == item.GoodsId);
                simpleOrdersList.Add(new SimpleOrdersSVM() {
                    OrderNum = Guid.NewGuid(),
                    GoodsId = item.GoodsId,
                    GoodsImg = goods.GoodsSimpleGraph,
                    GoodsName = goods.GoodsName,
                    GoodsNum = item.GoodsNum,
                    GoodsPrice = goods.Price,
                    GoodsTotalPrice = (item.GoodsNum * goods.Price),
                    ShopCartsId = item.ShopCartsId
                });
            }
            return simpleOrdersList;
        }
    }
}