using DressUp.Scl.Model.ServiceModel;
using DressUp.Scl.Model.ViewModel;
using DressUp_Scl_Data.Data;
using DressUp_Scl_Service.Service.ReceptionistService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DressUp.Scl.Service
{
    public class DataTFService
    {
        ShowGoodsService service = new ShowGoodsService();
        UserService userService = new UserService();
        public List<GoodsVM> TFGoods(List<Goods> dataGoods) {
            if (dataGoods == null) {
                return null;
            }
            List<GoodsVM> goodsList = dataGoods.Select(p => new GoodsVM()
            {
                GoodsId = p.GoodsId,
                GoodsName = p.GoodsName,
                ClassificationId = p.ClassificationId,
                GoodsSimpleGraph = p.GoodsSimpleGraph,
                GoodsDetail = p.GoodsDetail,
                PurchasePrice = p.PurchasePrice,
                Price = p.Price,
                PromotionPrice = p.PromotionPrice,
                GoodsStatus = p.GoodsStatus,
                Stock = p.Stock,
                ShelvesNum = p.ShelvesNum,
                IfNewGoods = p.IfNewGoods,
                IfHot = p.IfHot,
                OnShelfTime = p.OnShelfTime,
                OffShelfTime = p .OffShelfTime,
                GoodsSaleNum = p.GoodsSaleNum
            }).ToList();
            return goodsList;
        }
        public GoodsVM TFGood(Goods dataGood)
        {
            if (dataGood == null)
            {
                return null;
            }
            GoodsVM goods = new GoodsVM()
            {
                GoodsId = dataGood.GoodsId,
                GoodsName = dataGood.GoodsName,
                ClassificationId = dataGood.ClassificationId,
                GoodsSimpleGraph = dataGood.GoodsSimpleGraph,
                GoodsDetail = dataGood.GoodsDetail,
                PurchasePrice = dataGood.PurchasePrice,
                Price = dataGood.Price,
                PromotionPrice = dataGood.PromotionPrice,
                GoodsStatus = dataGood.GoodsStatus,
                Stock = dataGood.Stock,
                ShelvesNum = dataGood.ShelvesNum,
                IfNewGoods = dataGood.IfNewGoods,
                IfHot = dataGood.IfHot,
                OnShelfTime = dataGood.OnShelfTime,
                OffShelfTime = dataGood.OffShelfTime,
                GoodsSaleNum = dataGood.GoodsSaleNum
            };
            return goods;
        }
        public Goods DTFGoods(GoodsSVM goodsSVM) {
            if (goodsSVM == null)
            {
                return null;
            }
            DateTime? onShelfTime;
            DateTime? OffShelfTime;
            if (goodsSVM.OnShelfTime == "")
            {
                onShelfTime = null;
            }
            else {
                onShelfTime = DateTime.Parse(goodsSVM.OnShelfTime);
            }
            if (goodsSVM.OffShelfTime == "")
            {
                OffShelfTime = null;
            }
            else
            {
                OffShelfTime = DateTime.Parse(goodsSVM.OffShelfTime);
            }
            Goods goods = new Goods()
            {
                GoodsId = goodsSVM.GoodsId,
                GoodsName = goodsSVM.GoodsName,
                ClassificationId = goodsSVM.ClassificationId,
                GoodsSimpleGraph = goodsSVM.GoodsSimpleGraph,
                GoodsDetail = goodsSVM.GoodsDetail,
                PurchasePrice = goodsSVM.PurchasePrice,
                Price = goodsSVM.Price,
                PromotionPrice = goodsSVM.PromotionPrice,
                GoodsStatus = goodsSVM.GoodsStatus,
                Stock = goodsSVM.Stock,
                ShelvesNum = goodsSVM.ShelvesNum,
                IfNewGoods = goodsSVM.IfNewGoods,
                IfHot = goodsSVM.IfHot,
                OnShelfTime = onShelfTime,
                OffShelfTime = OffShelfTime,
                GoodsSaleNum = goodsSVM.GoodsSaleNum
            };
            return goods;
        }
        public List<GoodsSVM> TFGoodsSVM(List<GoodsVM> VMGoods, List<GoodsTypes> dataGoodsTypes) {
            if (VMGoods == null || dataGoodsTypes == null)
            {
                return null;
            }
            List<GoodsSVM> goodsList = new List<GoodsSVM>();
            foreach (GoodsVM p in VMGoods)
            {
                string goodsType = dataGoodsTypes.Find(m => m.TypeId == p.ClassificationId).TypeName;
                goodsList.Add(new GoodsSVM()
                {
                    GoodsId = p.GoodsId,
                    GoodsName = p.GoodsName,
                    ClassificationId = p.ClassificationId,
                    GoodsSimpleGraph = p.GoodsSimpleGraph,
                    GoodsDetail = p.GoodsDetail,
                    PurchasePrice = p.PurchasePrice,
                    Price = p.Price,
                    PromotionPrice = p.PromotionPrice,
                    GoodsStatus = p.GoodsStatus,
                    Stock = p.Stock,
                    ShelvesNum = p.ShelvesNum,
                    IfNewGoods = p.IfNewGoods,
                    IfHot = p.IfHot,
                    OnShelfTime = p.OnShelfTime.ToString(),
                    OffShelfTime = p.OffShelfTime.ToString(),
                    GoodsSaleNum = p.GoodsSaleNum,
                    GoodsTypeName = goodsType
                });
            }
            return goodsList;
        }
        public GoodsSVM TFGoodSVM(GoodsVM VMGood, List<GoodsTypes> dataGoodsTypes) {
            if (VMGood == null || dataGoodsTypes == null)
            {
                return null;
            }
            string goodsType = dataGoodsTypes.Find(m => m.TypeId == VMGood.ClassificationId).TypeName;
            return new GoodsSVM() {
                GoodsId = VMGood.GoodsId,
                GoodsName = VMGood.GoodsName,
                ClassificationId = VMGood.ClassificationId,
                GoodsSimpleGraph = VMGood.GoodsSimpleGraph,
                GoodsDetail = VMGood.GoodsDetail,
                PurchasePrice = VMGood.PurchasePrice,
                Price = VMGood.Price,
                PromotionPrice = VMGood.PromotionPrice,
                GoodsStatus = VMGood.GoodsStatus,
                Stock = VMGood.Stock,
                ShelvesNum = VMGood.ShelvesNum,
                IfNewGoods = VMGood.IfNewGoods,
                IfHot = VMGood.IfHot,
                OnShelfTime = VMGood.OnShelfTime.ToString(),
                OffShelfTime = VMGood.OffShelfTime.ToString(),
                GoodsSaleNum = VMGood.GoodsSaleNum,
                GoodsTypeName = goodsType
            };
        }
        public List<UsersVM> TFUsers(List<Users> dataUsers) {
            if (dataUsers == null)
            {
                return null;
            }
            List<UsersVM> usersList = dataUsers.Select(p => new UsersVM()
            {
                UserId = p.UserId,
                Name = p.Name,
                Account = p.Account,
                Password = p.Password,
                CreationTime = p.CreationTime,
                ContactInfo = p.ContactInfo
            }).ToList();
            return usersList;
        }
        public UsersVM TFUser(Users dataUser)
        {
            if (dataUser == null)
            {
                return null;
            }
            UsersVM user = new UsersVM()
                {
                    UserId = dataUser.UserId,
                    Name = dataUser.Name,
                    Account = dataUser.Account,
                    Password = dataUser.Password,
                    CreationTime = dataUser.CreationTime,
                    ContactInfo = dataUser.ContactInfo
                };
            return user;
        }
        public List<OperationRecordsVM> TFOperationRecords(List<OperationRecords> dataOperationRecords) {
            if (dataOperationRecords == null)
            {
                return null;
            }
            List<OperationRecordsVM> list = dataOperationRecords.Select(p => new OperationRecordsVM() {
                RecordId = p.RecordId,
                UserId = p.UserId,
                RecordingTime = p.RecordingTime.ToString(),
                Details = p.Details,
                OperationType = p.OperationType
            }).ToList();
            return list;
        }
        public List<RolesVM> TFRoles(List<Roles> dataRoles) {
            if (dataRoles == null)
            {
                return null;
            }
            List<RolesVM> list = dataRoles.Select(p => new RolesVM()
            {
                RoleId = p.RoleId,
                Name = p .Name,
                Code = p.Code,
                CreationTime = p.CreationTime,
                IsDefault = p.IsDefault
            }).ToList();
            return list;
        }
        public UserSVM TFUserSVM(Users dataUser) {
            if (dataUser == null)
            {
                return null;
            }
            UserSVM user = new UserSVM()
            {
                UserId = dataUser.UserId,
                BuyerName = dataUser.Name,
                Account = dataUser.Account,
                Password = dataUser.Password,
                CreationTime = dataUser.CreationTime,
                ContactInfo = dataUser.ContactInfo,
                ShoppingCartsCount = userService.GetBuyersShoppingCarts(dataUser).Count
            };
            return user;
        }
        public List<OrdersVM> TFOrders(List<Orders> dataOrders) {
            if (dataOrders == null)
            {
                return null;
            }
            List<OrdersVM> list = dataOrders.Select(p => new OrdersVM()
            {
                OrderNum = p.OrderNum,
                OrderStatus = p.OrderStatus,
                UserId = p.UserId,
                GoodsId = p.GoodsId,
                GoodsName = p.GoodsName,
                GoodsPrice = p.GoodsPrice,
                GoodsNum = p.GoodsNum,
                OrderGenerationTime = p.OrderGenerationTime.ToString(),
                ReceiptAddress = p.ReceiptAddress,
                Consignee = p.Consignee,
                ContactInfo = p . ContactInfo
            }).ToList();
            return list;
        }
        public OrdersSVM TFOrderSVM(Orders dataOrder) {
            if (dataOrder == null) {
                return null;
            }
            string goodsImg = service.GetAllGoods().SingleOrDefault(m => m.GoodsId == dataOrder.GoodsId).GoodsSimpleGraph;
            return new OrdersSVM() {
                OrderNum = dataOrder.OrderNum,
                OrderStatus = dataOrder.OrderStatus,
                GoodsImg = goodsImg,
                GoodsName = dataOrder.GoodsName,
                GoodsPrice = dataOrder.GoodsPrice,
                GoodsNum = dataOrder.GoodsNum,
                OrderGenerationTime = dataOrder.OrderGenerationTime.ToString(),
                ReceiptAddress = dataOrder.ReceiptAddress,
                Consignee = dataOrder.Consignee,
                ContactInfo = dataOrder.ContactInfo
            };
        }
        public List<OrdersSVM> TFOrdersSVM(List<Orders> dataOrders) {
            if (dataOrders == null)
            {
                return null;
            }
            List<OrdersSVM> list = new List<OrdersSVM>();
            foreach (Orders p in dataOrders) {
                string goodsImg = service.GetAllGoods().SingleOrDefault(m => m.GoodsId == p.GoodsId).GoodsSimpleGraph;
                list.Add(new OrdersSVM()
                {
                    OrderNum = p.OrderNum,
                    OrderStatus = p.OrderStatus,
                    GoodsImg = goodsImg,
                    GoodsName = p.GoodsName,
                    GoodsPrice = p.GoodsPrice,
                    GoodsNum = p.GoodsNum,
                    OrderGenerationTime = p.OrderGenerationTime.ToString(),
                    ReceiptAddress = p.ReceiptAddress,
                    Consignee = p.Consignee,
                    ContactInfo = p.ContactInfo
                });
            }
            return list;
        }
        public StorageOrderSVM TFStorageOrder(StoreNewMessageSVM message ,Goods goods,string goodsType) {
            if (message == null || goods == null || goodsType == "" || goodsType == null)
            {
                return null;
            }
            return new StorageOrderSVM()
            {
                GoodsName = message.GoodsName,
                MessageType = message.Type,
                Num = message.Num,
                MessageId = message.Id,
                GoodsType = goodsType,
                GoodsPurchasePrice = goods.PurchasePrice,
                GoodsId = goods.GoodsId
            };
        }
        public LibraryOrderSVM TFLibraryOrder(StoreNewMessageSVM message, Goods goods,Orders order,string goodsType)
        {
            if (message == null || goods == null || goodsType == "" || goodsType == null || order == null)
            {
                return null;
            }
            return new LibraryOrderSVM()
            {
                MessageId = message.Id,
                MessageType = message.Type,
                GoodsName = message.GoodsName ,
                GoodsType = goodsType ,
                ContactInfo = order.ContactInfo ,
                OrderId = order.OrderNum ,
                GoodsPurchasePrice = goods.PurchasePrice ,
                GoodsNum = order.GoodsNum ,
                Consignee = order.Consignee ,
                ReceiptAddress = order.ReceiptAddress ,
                GoodsId = goods.GoodsId 
            };
        }
        public List<GoodsAtlasVM> TFAtlas(List<GoodsAtlas> dataAtlas) {
            if (dataAtlas == null)
            {
                return null;
            }
            List<GoodsAtlasVM> list = dataAtlas.Select(p => new GoodsAtlasVM()
            {
                ImgId = p.ImgId,
                Img = p.Img,
                GoodsId = p.GoodsId,
            }).ToList();
            return list;
        }
        public List<ShopCartsSVM> TFShopCartsSVM(List<ShoppingCarts> dataList) {
            if (dataList == null) {
                return null;
            }
            int serialNumber = 0;
            List<ShopCartsSVM> list = new List<ShopCartsSVM>();
            foreach (ShoppingCarts item in dataList) {
                Goods goods = service.GetGoodsById(item.GoodsId);
                list.Add(new ShopCartsSVM() {
                    UserId = item.UserId,
                    GoodsId = item.GoodsId,
                    GoodsNum = item.GoodsNum,
                    SerialNumber = serialNumber,
                    ShoppingCartId = item.ShoppingCartId,
                    GoodsSimpleGraph = goods.GoodsSimpleGraph,
                    GoodsName = goods.GoodsName,
                    Price = goods.Price,
                    TotalPrice = (goods.Price * item.GoodsNum)
                });
                serialNumber++;
            }
            return list;
        }
        public List<ReceivingInfosVM> TFReceivingInfos(List<ReceivingInfos> dataList) {
            if (dataList == null)
            {
                return null;
            }
            List<ReceivingInfosVM> list = new List<ReceivingInfosVM>();
            foreach (ReceivingInfos item in dataList)
            {
                list.Add(new ReceivingInfosVM()
                {
                    UserId = item.UserId,
                    Consignee = item.Consignee,
                    ContactInfo = item.ContactInfo,
                    ReceiptAddress = item.ReceiptAddress,
                    IsDefault = item.IsDefault,
                    ReceivingInfoId = item.ReceivingInfoId,
                });
            }
            return list;
        }
        public ReceivingInfos DTFReceivingInfosVM(ReceivingInfosVM data) {
            if (data == null) {
                return null;
            }
            return new ReceivingInfos() {
                UserId = data.UserId,
                Consignee = data.Consignee,
                ContactInfo = data.ContactInfo,
                ReceiptAddress = data.ReceiptAddress,
                IsDefault = data.IsDefault,
                ReceivingInfoId = data.ReceivingInfoId,
            };
        }
        public ReceivingInfosVM TFReceivingInfo(ReceivingInfos data)
        {
            if (data == null)
            {
                return null;
            }
            ReceivingInfosVM receivingInfosVM = new ReceivingInfosVM()
            {
                UserId = data.UserId,
                Consignee = data.Consignee,
                ContactInfo = data.ContactInfo,
                ReceiptAddress = data.ReceiptAddress,
                IsDefault = data.IsDefault,
                ReceivingInfoId = data.ReceivingInfoId,
            };
            return receivingInfosVM;
        }
        public List<Orders> DTFOrders(Guid buyerId,List<MiniOrders> miniOrders) {
            List<Orders> orders = new List<Orders>();
            List<Goods> goodsList = service.GetAllGoods();
            List<ReceivingInfos> receivingInfoList = service.GetAllReceivingInfos();

            Goods goods = new Goods();
            ReceivingInfos receivingInfos = new ReceivingInfos();
            foreach (MiniOrders item in miniOrders) {
                goods = goodsList.SingleOrDefault(m => m.GoodsId == item.GoodsId);
                receivingInfos = receivingInfoList.SingleOrDefault(m => m.ReceivingInfoId == item.ReceivingInfoId);
                orders.Add(new Orders() {
                    OrderNum = Guid.NewGuid(),
                    OrderStatus = "已支付",
                    UserId = buyerId,
                    GoodsId = goods.GoodsId,
                    GoodsName = goods.GoodsName,
                    GoodsPrice = (decimal)goods.Price,
                    GoodsNum = item.GoodsNum,
                    OrderGenerationTime = DateTime.Now,
                    ReceiptAddress = receivingInfos.ReceiptAddress,
                    Consignee = receivingInfos.Consignee,
                    ContactInfo = receivingInfos.ContactInfo
                });
            }
            return orders;
        }
        public UsersInfoSVM TFUsersInfoSVM(Users buyer,int waitReceivingOrdersNum) {
            return new UsersInfoSVM()
            {
                UserId = buyer.UserId,
                Name = buyer.Name,
                WaitReceivingOrdersNum = waitReceivingOrdersNum
            };
        }
        public SimpleOrderInfoSVM TFSimpleOrderInfo(int allOrdersNum,int waitReceivingOrdersNum) {
            return new SimpleOrderInfoSVM()
            {
                AllOrdersNum = allOrdersNum,
                WaitReceivingNum = waitReceivingOrdersNum
            };
        }
    }
}