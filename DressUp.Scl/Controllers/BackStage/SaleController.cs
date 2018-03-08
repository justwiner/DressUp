using DressUp.Scl.Filter;
using DressUp.Scl.Model.ServiceModel;
using DressUp.Scl.Model.ViewModel;
using DressUp.Scl.Service;
using DressUp_Scl_Data.Data;
using DressUp_Scl_Service.Service.BackStageService;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DressUp.Scl.Controllers.BackStage
{
    [MyLoginAuthorize(Order = 0)]
    public class SaleController : Controller
    {
        private SaleService saleService = new SaleService();
        private DataTFService TFService = new DataTFService();
        static PagesSVM<GoodsVM> goodsPages = new PagesSVM<GoodsVM>();
        static PagesSVM<OrdersSVM> goodsPagesOrder = new PagesSVM<OrdersSVM>();
        static GoodsAndAtlasSVM goods = new GoodsAndAtlasSVM();
        #region 云商
        //*****************商品列表**********************//
        [MyJurisdictionAuthorize(Order = 1)]
        public ActionResult GoodsList()
        {
            goodsPages.setAllList(TFService.TFGoods(SaleService.GetAllGoods()));
            return View("~/Views/BackStage/Sale/GoodsList/GoodsList.cshtml");
        }
        [MyJurisdictionAuthorize(Order = 1)]
        public ActionResult SearchMode()
        {
            return PartialView("~/Views/BackStage/Sale/GoodsList/SearchMode/SearchMode.cshtml", SaleService.GetAllGoodsTypes());
        }
        [MyJurisdictionAuthorize(Order = 1)]
        public ActionResult ShowBox()
        {
            return PartialView("~/Views/BackStage/Sale/GoodsList/ShowBox/ShowBox.cshtml", goodsPages);
        }
        public ActionResult GetTotalPageNum()
        {
            return Json(goodsPages.pageTotal, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GoPage(string goPage)
        {
            List<GoodsTypes> types = SaleService.GetDataGoodsTypes();
            int page = int.Parse(goPage);
            goodsPages.setPageNow(page);
            string jsonResult = JsonConvert.SerializeObject(TFService.TFGoodsSVM(goodsPages.nowList, types),Formatting.Indented);
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
        public ActionResult Find(int typeId, int stockId, int statusId)
        {
            List<Goods> byTypeList = SaleService.GetListByType(typeId, SaleService.GetAllGoods());
            List<Goods> byStockList = SaleService.GetListByStock(stockId, SaleService.GetAllGoods());
            List<Goods> byStatusList = SaleService.GetListByStatus(statusId, SaleService.GetAllGoods());
            List<Goods> goodsList = SaleService.FindIntersectionByGoodsId(byTypeList, SaleService.FindIntersectionByGoodsId(byStockList, byStatusList));
            goodsPages.setAllList(TFService.TFGoods(goodsList));
            return GoPage("1");
        }
        public ActionResult VagueSearch(string goodsName)
        {
            List<Goods> goodsList = new List<Goods>();
            if (goodsName != "")
            {
                goodsList = SaleService.GetAllGoods().Where(m => m.GoodsName.Contains(goodsName)).ToList();
            }
            else
            {
                goodsList = SaleService.GetAllGoods();
            }
            goodsPages.setAllList(TFService.TFGoods(goodsList));
            return GoPage("1");
        }
        //********************商品信息***********************//
        [MyJurisdictionAuthorize(Order = 1)]
        public ActionResult GoodsInfo(string goodsId) {
            Guid goodGuid = new Guid(goodsId);
            Goods item = saleService.GetGoodsInfoById(goodGuid);
            goods.Goods = TFService.TFGoodSVM(TFService.TFGood(item), SaleService.GetDataGoodsTypes());
            goods.Atlas = TFService.TFAtlas(saleService.GetGoodsAtlas(item.GoodsId));
            return View("~/Views/BackStage/Sale/GoodsInfo/GoodsInfo.cshtml");
        }
        [MyJurisdictionAuthorize(Order = 1)]
        public ActionResult GoodsInfoBox(){
            return PartialView("~/Views/BackStage/Sale/GoodsInfo/GoodsInfoBox/GoodsInfoBox.cshtml", goods);
        }
        public ActionResult ChangeGoodsInfo(string goodsId,string goodsName, string goodsDetail,string goodsPrice, string goodsPromotionPrice,string goodsIfPromotion,string goodsIfOnShelf,string goodsShelfNum) {
            Guid id = new Guid(goodsId);
            decimal price = Decimal.Parse(goodsPrice);
            decimal promotionPrice = Decimal.Parse(goodsPromotionPrice);
            Boolean ifPromotion = saleService.GetIf(goodsIfPromotion);
            Boolean ifOnShelf = saleService.GetIf(goodsIfOnShelf);
            int shelfNum = Int32.Parse(goodsShelfNum);
            Users user = Session["User"] as Users;
            Goods item = saleService.ChangeGoodsInfo(user, id, goodsName, goodsDetail,price, promotionPrice, ifPromotion, ifOnShelf, shelfNum);
            goods.Goods = TFService.TFGoodSVM(TFService.TFGood(item), SaleService.GetDataGoodsTypes());
            string jsonResult = JsonConvert.SerializeObject(goods.Goods,Formatting.Indented);
            return LargeJson(jsonResult, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult ChangeSimpleGraph()
        {
            Users user = Session["User"] as Users;
            HttpPostedFileBase file = Request.Files["simpleGraph"];
            if (file != null)
            {
                string fileName = Path.GetFileName(file.FileName);
                string fileExt = Path.GetExtension(file.FileName);
                string fileNewName = "SimpleGraph" + fileExt;
                string path = saleService.SetGoodsSimpleGraphPath(TFService.DTFGoods(goods.Goods),fileNewName);
                string fileSaveDir = Server.MapPath(path);
                if (!Directory.Exists(fileSaveDir))
                {
                    Directory.CreateDirectory(fileSaveDir);
                }
                file.SaveAs(Path.Combine(fileSaveDir, fileNewName));
                saleService.RecordOperationRecords(user, "为商品" + goods.Goods.GoodsName + "修改了展示图", "修改商品展示图");
            }
            return GoodsInfo(goods.Goods.GoodsId.ToString());
        }
        public ActionResult ChangeGoodsAtlas(HttpPostedFileBase[] fileToUpload)
        {
            Users user = Session["User"] as Users;
            saleService.RemoveGoodsAtlas(goods.Goods.GoodsId);
            int i = 1;
            foreach (HttpPostedFileBase file in fileToUpload)
            {
                string fileName = Path.GetFileName(file.FileName);
                string fileExt = Path.GetExtension(file.FileName);
                string fileNewName = i + fileExt;
                string path = saleService.SetGoodsAtlasPath(TFService.DTFGoods(goods.Goods), fileNewName);
                string fileSaveDir = Server.MapPath(path);
                if (!Directory.Exists(fileSaveDir))
                {
                    Directory.CreateDirectory(fileSaveDir);
                }
                file.SaveAs(Path.Combine(fileSaveDir, fileNewName));
                i++;
            }
            saleService.RecordOperationRecords(user, "为商品" + goods.Goods.GoodsName + "设置了" + i + "张详情图", "修改商品详情图");
            return GoodsInfo(goods.Goods.GoodsId.ToString());
        }
        //*******************缺货预警************************//
        [MyJurisdictionAuthorize(Order = 1)]
        public ActionResult ShortageWarning() {
            goodsPages.setAllList(TFService.TFGoods(saleService.GetListByStock_noStatic(-1)));
            return View("~/Views/BackStage/Sale/GoodsShortageWarning/ShortageWarning.cshtml");
        }
        [MyJurisdictionAuthorize(Order = 1)]
        public ActionResult ShortageWarningSearch() {
            return PartialView("~/Views/BackStage/Sale/GoodsShortageWarning/SearchByType/SearchByType.cshtml", SaleService.GetAllGoodsTypes());
        }
        [MyJurisdictionAuthorize(Order = 1)]
        public ActionResult ShortageWarningShowBox() {
            return PartialView("~/Views/BackStage/Sale/GoodsShortageWarning/ShowBox/ShowBox.cshtml", goodsPages);
        }
        public ActionResult ShourageWarningFind(int typeId ,int statusId) {
            List<Goods> byTypeList = SaleService.GetListByType(typeId, saleService.GetListByStock_noStatic(-1));
            List<Goods> byStatusList = SaleService.GetListByStatus(statusId, saleService.GetListByStock_noStatic(-1));
            List<Goods> goodsList = SaleService.FindIntersectionByGoodsId(byTypeList, byStatusList);
            goodsPages.setAllList(TFService.TFGoods(goodsList));
            return GoPage("1");
        }
        public ActionResult ShourageWarningVagueSearch(string goodsName)
        {
            List<Goods> goodsList = new List<Goods>();
            if (goodsName != "")
            {
                goodsList = saleService.GetListByStock_noStatic(-1).Where(m => m.GoodsName.Contains(goodsName)).ToList();
            }
            else
            {
                goodsList = saleService.GetListByStock_noStatic(-1);
            }
            goodsPages.setAllList(TFService.TFGoods(goodsList));
            return GoPage("1");
        }
        public ActionResult CreatWarehousingOrder(int purchaseNum,string goodsId) {
            Guid id = Guid.Parse(goodsId);
            Users user = Session["User"] as Users;
            Boolean result = saleService.CreatWarehousingOrder(user,purchaseNum, id);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region 交易
        //***************************所有订单*****************************//
        [MyJurisdictionAuthorize(Order = 1)]
        public ActionResult AllOrder() {
            goodsPagesOrder.setAllList(TFService.TFOrdersSVM(saleService.GetAllOrders()));
            return View("~/Views/BackStage/Sale/AllOrderList/AllOrderList.cshtml");
        }
        [MyJurisdictionAuthorize(Order = 1)]
        public ActionResult AllOrderShowBox() {
            return PartialView("~/Views/BackStage/Sale/AllOrderList/AllOrderShowBox/AllOrderShowBox.cshtml", goodsPagesOrder);
        }
        public ActionResult GetAllOrderPageNum() {
            return Json(goodsPagesOrder.pageTotal, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AllOrderGoPage(string goPage)
        {
            int page = int.Parse(goPage);
            List<Goods> goodsList = saleService.GetAllGoods_noStatic();
            goodsPagesOrder.setPageNow(page);
            string jsonResult = JsonConvert.SerializeObject(goodsPagesOrder.nowList, Formatting.Indented);
            return LargeJson(jsonResult, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AllOrderFindByStatus(int statusId) {
            goodsPagesOrder.setAllList(TFService.TFOrdersSVM(saleService.GetOrdersByStatusId(statusId,saleService.GetAllOrders())));
            return AllOrderGoPage("1");
        }
        public ActionResult AllOrderFindByTime(string minTime,string maxTime) {
            DateTime minData = DateTime.Parse(minTime);
            DateTime maxData = DateTime.Parse(maxTime);
            goodsPagesOrder.setAllList(TFService.TFOrdersSVM(saleService.GetOrdersByTime(minData,maxData, saleService.GetAllOrders())));
            return AllOrderGoPage("1");
        }
        //*****************************新订单*****************************//
        [MyJurisdictionAuthorize(Order = 1)]
        public ActionResult NewOrder() {
            goodsPagesOrder.setAllList(TFService.TFOrdersSVM(saleService.GetNewOrders()));
            return View("~/Views/BackStage/Sale/NewOrderList/NewOrderList.cshtml");
        }
        public ActionResult NewOrderFindByStatus(int statusId)
        {
            goodsPagesOrder.setAllList(TFService.TFOrdersSVM(saleService.GetOrdersByStatusId(statusId, saleService.GetNewOrders())));
            return AllOrderGoPage("1");
        }
        public ActionResult NewOrderFindByTime(string minTime, string maxTime)
        {
            DateTime minData = DateTime.Parse(minTime);
            DateTime maxData = DateTime.Parse(maxTime);
            goodsPagesOrder.setAllList(TFService.TFOrdersSVM(saleService.GetOrdersByTime(minData, maxData, saleService.GetNewOrders())));
            return AllOrderGoPage("1");
        }
        [MyJurisdictionAuthorize(Order = 1)]
        public ActionResult NewOrderShowBox()
        {
            return PartialView("~/Views/BackStage/Sale/NewOrderList/NewOrderShowBox/NewOrderShowBox.cshtml", goodsPagesOrder);
        }
        public ActionResult Library(string orderId) {
            Guid id = new Guid(orderId);
            Users user = Session["User"] as Users;
            Boolean result = saleService.Library(id, user);
            goodsPagesOrder.setAllList(TFService.TFOrdersSVM(saleService.GetNewOrders()));
            return Json(result,JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}