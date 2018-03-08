using DressUp.Scl.Model.ViewModel;
using DressUp.Scl.Model.ServiceModel;
using DressUp.Scl.Service;
using DressUp_Scl_Data.Data;
using DressUp_Scl_Service.Model;
using DressUp_Scl_Service.Service.BackStageService;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using DressUp.Scl.Filter;

namespace DressUp.Scl.Controllers.BackStage
{
    [MyLoginAuthorize(Order = 0)]
    public class StoreManagementController : Controller
    {
        StoreService storeService = new StoreService();
        NewMessageService newPageService = new NewMessageService();
        SaleService saleService = new SaleService();
        DataTFService TFService = new DataTFService();
        static PagesSVM<GoodsVM> goodsPages = new PagesSVM<GoodsVM>();
        static PagesSVM<StoreNewMessageSVM> newMessages = new PagesSVM<StoreNewMessageSVM>();
        // GET: StoreManagement
        //************************************库存***********************************//
        [MyJurisdictionAuthorize(Order = 1)]
        public ActionResult Stock()
        {
            goodsPages.setAllList(TFService.TFGoods(storeService.GetAllGoods()));
            return View("~/Views/BackStage/StoreManagement/StoreCheck/_Stock.cshtml");
        }
        [MyJurisdictionAuthorize(Order = 1)]
        public ActionResult GoodsTypes() {
            return PartialView("~/Views/BackStage/StoreManagement/StoreCheck/GoodsType/GoodsTypes.cshtml", storeService.GetAllGoodsTypes());
        }
        [MyJurisdictionAuthorize(Order = 1)]
        public ActionResult CheckBox()
        {
            return PartialView("~/Views/BackStage/StoreManagement/StoreCheck/CheckBox/CheckBox.cshtml", goodsPages);
        }
        public ActionResult GetTotalPageNum() {
            return Json(goodsPages.pageTotal, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GoPage(string goPage)
        {
            List<GoodsTypes> types = storeService.GetDataGoodsTypes();
            int page = int.Parse(goPage);
            goodsPages.setPageNow(page);
            string jsonResult = JsonConvert.SerializeObject(TFService.TFGoodsSVM(goodsPages.nowList, types), Formatting.Indented);
            return LargeJson(jsonResult, JsonRequestBehavior.AllowGet);
        }
        public JsonResult LargeJson(object data)
        {
            return new System.Web.Mvc.JsonResult()
            {
                Data = data,
                MaxJsonLength = Int32.MaxValue,
            };
        }
        public JsonResult LargeJson(object data, JsonRequestBehavior behavior)
        {
            return new System.Web.Mvc.JsonResult()
            {
                Data = data,
                JsonRequestBehavior = behavior,
                MaxJsonLength = Int32.MaxValue
            };
        }
        public ActionResult Find(int typeId, int stockId, int statusId) {
            List<Goods> byTypeList = storeService.GetListByType(typeId);
            List<Goods> byStockList = storeService.GetListByStock(stockId);
            List<Goods> byStatusList = storeService.GetListByStatus(statusId);
            List<Goods> goodsList = storeService.FindIntersectionByGoodsId(byTypeList, storeService.FindIntersectionByGoodsId(byStockList, byStatusList));
            goodsPages.setAllList(TFService.TFGoods(goodsList));
            return GoPage("1");
        }
        public ActionResult VagueSearch(string goodsName) {
            List<Goods> goodsList = new List<Goods>();
            if (goodsName != "")
            {
                goodsList = storeService.GetAllGoods().Where(m => m.GoodsName.Contains(goodsName)).ToList();
            }
            else {
                goodsList = storeService.GetAllGoods();
            }
            goodsPages.setAllList(TFService.TFGoods(goodsList));
            return GoPage("1");
        }
        //************************************入库***********************************//
        [MyJurisdictionAuthorize(Order = 1)]
        public ActionResult Storage() {
            return View("~/Views/BackStage/StoreManagement/Storage/Storage.cshtml");
        }
        [MyJurisdictionAuthorize(Order = 1)]
        public ActionResult StorageGoodsBox() {
            List<GoodsTypes_> list = new List<GoodsTypes_>();
            List<GoodsTypes_> allList = storeService.GetAllGoodsTypes();
            foreach (GoodsTypes_ item in allList) {
                list.AddRange(item.Children);
            }
            return PartialView("~/Views/BackStage/StoreManagement/Storage/StorageGoodsBox.cshtml", list);
        }
        public ActionResult NewStorage(string goodsName, string goodsType, int goodsNum, int purchasePrice) {
            Users user = System.Web.HttpContext.Current.Session["User"] as Users;
            Boolean result = storeService.NewStorage(goodsName, goodsType, goodsNum, purchasePrice, user);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        //************************************出入库明细***********************************//-------未实现
        [MyJurisdictionAuthorize(Order = 1)]
        public ActionResult InventoryDetails() {
            return View("~/Views/BackStage/StoreManagement/InventoryDetails/_InventoryDetails.cshtml");
        }
        //************************************新消息处理**********************************//
        [MyJurisdictionAuthorize(Order = 1)]
        public ActionResult StoreNewProcessMessage() {
            newMessages.setAllList(newPageService.TogetherNewMessage());
            return View("~/Views/BackStage/StoreManagement/NewMessage/NewMessage.cshtml");
        }
        [MyJurisdictionAuthorize(Order = 1)]
        public ActionResult NewMessageShowBox() {
            return PartialView("~/Views/BackStage/StoreManagement/NewMessage/NewMessageShowBox/NewMessageShowBox.cshtml", newMessages);
        }
        public ActionResult NewMessageGetTotalPageNum() {
            return Json(newMessages.pageTotal, JsonRequestBehavior.AllowGet);
        }
        public ActionResult NewMessageGoPage(string goPage) {
            int page = int.Parse(goPage);
            newMessages.setPageNow(page);
            string jsonResult = JsonConvert.SerializeObject(newMessages.nowList, Formatting.Indented);
            return LargeJson(jsonResult, JsonRequestBehavior.AllowGet);
        }
        public ActionResult FindByMessageType(int typeId) {
            newMessages.setAllList(newPageService.FindByMessageType(typeId, newPageService.TogetherNewMessage()));
            return NewMessageGoPage("1");
        }
        public ActionResult NewMessageInfo(string messageId) {
            Guid id = new Guid(messageId);
            string JsonString="";
            StoreNewMessageSVM message = newPageService.GetMessageInfoById(id);
            if (message.Type == "入库")
            {
                Goods goods = storeService.GetGoodsById(message.GoodsId);
                string goodsType = storeService.GetDataGoodsTypes().SingleOrDefault(m => m.TypeId == goods.ClassificationId).TypeName;
                JsonString = JsonConvert.SerializeObject(TFService.TFStorageOrder(message,goods,goodsType),Formatting.Indented);
            }
            else {
                ShipmentList shipment = storeService.GetAllShipmentList().SingleOrDefault(m => m.OrderId == id);
                Orders order = saleService.GetAllOrders().SingleOrDefault(m => m.OrderNum == shipment.OrderNum);
                Goods goods = storeService.GetGoodsById(order.GoodsId);
                string goodsType = storeService.GetDataGoodsTypes().SingleOrDefault(m => m.TypeId == goods.ClassificationId).TypeName;
                JsonString = JsonConvert.SerializeObject(TFService.TFLibraryOrder(message, goods, order, goodsType), Formatting.Indented);
            }
            return LargeJson(JsonString, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GoodsPurchase(string messageId,string goodsId,string goodsName,string goodsType,int goodsNum, decimal purchasePrice){
            Users user = Session["User"] as Users;
            Guid messageID = new Guid(messageId);
            Boolean result = false;
            if (goodsId == "")
            {
                result = storeService.GoodsPurchase(messageID,user, goodsName, goodsType, goodsNum, purchasePrice);
            }
            else {
                Guid id = new Guid(goodsId);
                result = storeService.GoodsPurchase(messageID,user, id,goodsNum);
            }
            newMessages.setAllList(newPageService.TogetherNewMessage());
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GoodsShipment(string orderId , string messageId, string goodsId, string goodsName, string contactInfo,int goodsNum,string consignee,string receiptAddress){
            Users user = Session["User"] as Users;
            Guid orderID = new Guid(orderId);
            Guid id = new Guid(goodsId);
            Guid messageID = new Guid(messageId);
            Boolean result = storeService.GoodsShipment(orderID,messageID, user, id,goodsName, contactInfo, goodsNum, consignee, receiptAddress);
            newMessages.setAllList(newPageService.TogetherNewMessage());
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}