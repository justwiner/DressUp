using DressUp.Scl.Model.ServiceModel;
using DressUp_Scl_Data.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DressUp.Scl.Service
{
    public class NewMessageService
    {
        private DressUpWebDbEntities db = new DressUpWebDbEntities();
        public List<StoreNewMessageSVM> TogetherNewMessage()
        {
            List<ShipmentList> shipmentList = db.ShipmentList.Where(m => m.OrderStatus == "未处理").ToList();
            List<PurchaseList> purchaseList = db.PurchaseList.Where(m => m.OrderStatus == "未处理").ToList();
            List<Orders> orderList = db.Orders.ToList();
            List<StoreNewMessageSVM> storeNewMessages = new List<StoreNewMessageSVM>();
            foreach (ShipmentList item in shipmentList)
            {
                Orders order = orderList.SingleOrDefault(m => m.OrderNum == item.OrderNum);
                storeNewMessages.Add(new StoreNewMessageSVM()
                {
                    Id = item.OrderId,
                    Type = item.Type,
                    GoodsId = order.GoodsId,
                    GoodsName = order.GoodsName,
                    Num = order.GoodsNum
                });
            }
            foreach (PurchaseList item in purchaseList)
            {
                storeNewMessages.Add(new StoreNewMessageSVM()
                {
                    Id = item.OrderNum,
                    Type = item.OrderType,
                    GoodsId = (Guid)item.GoodsId,
                    GoodsName = item.GoodsName,
                    Num = item.StorageNum
                });
            }
            return storeNewMessages;
        }
        public List<StoreNewMessageSVM> FindByMessageType(int typeId, List<StoreNewMessageSVM> list)
        {
            switch (typeId)
            {
                case 400: return list;
                case 1: return list.Where(m => m.Type == "入库").ToList();
                case -1: return list.Where(m => m.Type == "出库").ToList();
            }
            return null;
        }
        public StoreNewMessageSVM GetMessageInfoById(Guid id)
        {
            return TogetherNewMessage().SingleOrDefault(m => m.Id == id);
        }
    }
}