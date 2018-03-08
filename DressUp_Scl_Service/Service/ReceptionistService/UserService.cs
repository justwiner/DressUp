using DressUp_Scl_Data.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DressUp_Scl_Service.Service.ReceptionistService
{
    public class UserService
    {
        private DressUpWebDbEntities db = new DressUpWebDbEntities();
        public List<ShoppingCarts> GetBuyersShoppingCarts(Users buyer) {
            return db.ShoppingCarts.Where(m => m.UserId == buyer.UserId).ToList();
        }
        public Users BuyerLogin(string account ,string password) {
            List<Users> list = db.Users.Where(m => m.Roles.Count == 1).ToList();
            list = list.Where(m => m.Roles.ElementAt(0).RoleId.ToString() == "299dc18e-4347-41cc-b8b4-477ce0d49699").ToList();
            Users buyer = list.SingleOrDefault(m => m.Account == account);
            if (buyer == null)
            {
                return null;
            }
            else {
                if (password != buyer.Password) {
                    return null;
                }
            }
            return buyer;
        }
        public bool AddShopCartsGoods(Guid goodsId, int goodsNum,Guid buyerId)
        {
            try
            {
                Goods goods = db.Goods.SingleOrDefault(m => m.GoodsId == goodsId);
                db.ShoppingCarts.Add(new ShoppingCarts()
                {
                    UserId = buyerId,
                    GoodsNum = goodsNum,
                    AddTime = DateTime.Now,
                    GoodsStatus = goods.GoodsStatus,
                    GoodsId = goodsId
                });
                db.SaveChanges();
                return true;
            }
            catch {
                return false;
            }
        }
        public void DeleteShopCartsGoods(int shoppingCartsId) {
            db.ShoppingCarts.Remove(db.ShoppingCarts.SingleOrDefault(m => m.ShoppingCartId == shoppingCartsId));
            db.SaveChanges();
        }
        public bool NameIfExist(string name) {
            if (db.Users.SingleOrDefault(m => m.Account == name) != null) {
                return true;
            }
            return false;
        }
        public bool RegisterNewBuer(string nickname, string account, string password)
        {
            try
            {
                Users buyer = new Users()
                {
                    UserId = Guid.NewGuid(),
                    Name = nickname,
                    Account = account,
                    Password = password,
                    CreationTime = DateTime.Now
                };
                buyer.Roles.Add(db.Roles.SingleOrDefault(m => m.RoleId.ToString() == "299dc18e-4347-41cc-b8b4-477ce0d49699"));
                db.Users.Add(buyer);
                db.SaveChanges();
                return true;
            }
            catch {
                return false;
            }
        }
        public List<ReceivingInfos> GetReceivingInfo(Guid userId) {
            return db.ReceivingInfos.Where(m => m.UserId == userId).ToList();
        }
        public ReceivingInfos GetDefaultReceivingInfo(Guid userId)
        {
            return GetReceivingInfo(userId).SingleOrDefault(m => m.IsDefault == true);
        }
        public ReceivingInfos SaveReceivingInfo(Guid buyerId,string newConsignee, string newReceiptAddress, string newContactInfo) {
            ReceivingInfos receivingInfo = new ReceivingInfos()
            {
                UserId = buyerId,
                Consignee = newConsignee,
                ReceiptAddress = newReceiptAddress,
                ContactInfo = newContactInfo,
                IsDefault = false
            };
            db.ReceivingInfos.Add(receivingInfo);
            db.SaveChanges();
            return receivingInfo;
        }
        public ReceivingInfos GetReceivingInfoById(int receivingInfoId) {
            return db.ReceivingInfos.SingleOrDefault(m => m.ReceivingInfoId == receivingInfoId);
        }
        public bool SaveOrders(List<int> shopCartsIds,List<Orders> orders) {
            try
            {
                if (!(shopCartsIds.Count == 1 && shopCartsIds[0] == 0))
                {
                    foreach (int shopCartsId in shopCartsIds)
                    {
                        db.ShoppingCarts.Remove(
                            db.ShoppingCarts.SingleOrDefault(m => m.ShoppingCartId == shopCartsId)
                            );
                    }
                }
                db.Orders.AddRange(orders);
                db.SaveChanges();
                return true;
            }
            catch {
                return false;
            }
        }
        public int WaitReceivingOrdersNum(Guid buyerId)
        {
            return db.Orders.Where(m => m.UserId == buyerId && m.OrderStatus == "已支付").Count();
        }
        public List<Orders> GetNearlyOrder(Guid buyerId) {
            List<Orders> myOrders = db.Orders.Where(m => m.UserId == buyerId).ToList();
            if (myOrders.Count >= 3)
            {
                return myOrders.OrderByDescending(m => m.OrderGenerationTime).Take(3).ToList();
            }
            else {
                return myOrders;
            }
        }
        public List<Orders> GetAllOrder(Guid buyerId) {
            return db.Orders.Where(m => m.UserId == buyerId).ToList();
        }
        public List<Orders> GetOrderById(Guid buyerId,int ordersStatusId) {
            List<Orders> buyerOrders = GetAllOrder(buyerId);
            switch (ordersStatusId) {
                case 1: return buyerOrders;
                case 2: return buyerOrders.Where(m => m.OrderStatus == "已支付").ToList();
                case 3: return buyerOrders.Where(m => m.OrderStatus == "已发货").ToList();
                case 4: return buyerOrders.Where(m => m.OrderStatus == "已完成").ToList();
                case 5: return buyerOrders.Where(m => m.OrderStatus == "已失效").ToList();
                case 6: return buyerOrders.Where(m => m.OrderStatus == "退款中").ToList();
                case 7: return buyerOrders.Where(m => m.OrderStatus == "未退款").ToList();
                case 8: return buyerOrders.Where(m => m.OrderStatus == "已退款").ToList();
                case 9: return buyerOrders.Where(m => m.OrderStatus == "待发货").ToList();
            }
            return buyerOrders;
        }
        public bool ModifyOrderState(Guid orderId,string state) {
            try
            {
                db.Orders.SingleOrDefault(m => m.OrderNum == orderId).OrderStatus = state;
                db.SaveChanges();
                return true;
            }
            catch {
                return false;
            }
        }
        public List<ReceivingInfos> GetUserReceivingInfo(Guid buyerId) {
            return db.ReceivingInfos.Where(m => m.UserId == buyerId).ToList();
        }
        public Orders GetOrderById(Guid orderId) {
            return db.Orders.SingleOrDefault(m => m.OrderNum == orderId);
        }
        public Users ChangeBuyerInfo(Users buyer, string buyerName, string buyerContactInfo)
        {
            db.Users.SingleOrDefault(m => m.UserId == buyer.UserId).Name = buyerName;
            db.Users.SingleOrDefault(m => m.UserId == buyer.UserId).ContactInfo = buyerContactInfo;
            db.SaveChanges();
            return db.Users.SingleOrDefault(m => m.UserId == buyer.UserId);
        }
        public bool ChangePassword(Users buyer , string newPassword,string oldPassword) {
            try
            {
                if (db.Users.SingleOrDefault(m => m.UserId == buyer.UserId).Password == oldPassword)
                {
                    db.Users.SingleOrDefault(m => m.UserId == buyer.UserId).Password = newPassword;
                    db.SaveChanges();
                    return true;
                }
                else {
                    return false;
                }
            }
            catch {
                return false;
            }
        }
        public bool DeleteReceivingInfo(int receivingInfoId) {
            try
            {
                db.ReceivingInfos.Remove(db.ReceivingInfos.SingleOrDefault(m => m.ReceivingInfoId == receivingInfoId));
                db.SaveChanges();
                return true;
            }
            catch {
                return false;
            }
        }
        public bool EditReceivingInfo(ReceivingInfos receivingInfo)
        {
            try
            {
                if (receivingInfo.IsDefault == true)
                {
                    foreach (ReceivingInfos item in db.ReceivingInfos.Where(m => m.UserId == receivingInfo.UserId))
                    {
                        item.IsDefault = false;
                    }
                }
                db.ReceivingInfos.SingleOrDefault(m => m.ReceivingInfoId == receivingInfo.ReceivingInfoId).ReceiptAddress = receivingInfo.ReceiptAddress;
                db.ReceivingInfos.SingleOrDefault(m => m.ReceivingInfoId == receivingInfo.ReceivingInfoId).Consignee = receivingInfo.Consignee;
                db.ReceivingInfos.SingleOrDefault(m => m.ReceivingInfoId == receivingInfo.ReceivingInfoId).ContactInfo = receivingInfo.ContactInfo;
                db.ReceivingInfos.SingleOrDefault(m => m.ReceivingInfoId == receivingInfo.ReceivingInfoId).IsDefault = receivingInfo.IsDefault;
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
