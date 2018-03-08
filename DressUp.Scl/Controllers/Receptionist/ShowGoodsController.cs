using DressUp.Scl.Model.ServiceModel;
using DressUp.Scl.Model.ViewModel;
using DressUp.Scl.Service;
using DressUp_Scl_Data.Data;
using DressUp_Scl_Service.Model;
using DressUp_Scl_Service.Service.ReceptionistService;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DressUp.Scl.Controllers.Receptionist
{
    public class ShowGoodsController : Controller
    {
        ShowGoodsService showGoodsService = new ShowGoodsService();
        UserService userService = new UserService();
        DataTFService TFService = new DataTFService();
        static PagesSVM<GoodsVM> goodsPages = new PagesSVM<GoodsVM>();
        // GET: BuyGoods
        #region 前台布局页
        public ActionResult Index()
        {
            return View("~/Views/Receptionist/HomePage/_Index.cshtml");
        }
        public ActionResult GetGoodsTypeList() {
            List<GoodsTypes_> list = showGoodsService.GetAllGoodsTypes();
            return PartialView("~/Views/Receptionist/Index/Head/_HeadNavigation.cshtml", list);
        }
        public ActionResult Siderbar() {
            return PartialView("~/Views/Receptionist/Index/Sidebar/_Sidebar.cshtml");
        }
        public ActionResult HeadLogin() {
            return PartialView("~/Views/Receptionist/Index/Head/_HeadLogin.cshtml");
        }
        public ActionResult GetShoppingCartsList() {
            Users buyer = Session["buyer"] as Users;
            if (buyer == null) {
                string jsonResult = RenderPartialView(ControllerContext, ViewData, TempData, "~/Views/Receptionist/Index/Sidebar/NoGoods.cshtml", null);
                return Json(jsonResult, JsonRequestBehavior.AllowGet);
            }
            List<ShoppingCarts> list = showGoodsService.GetShoppingCarts(buyer);
            List<ShopCartsSVM> showList = TFService.TFShopCartsSVM(list);
            if (showList == null || showList.Count == 0)
            {
                string jsonResult = RenderPartialView(ControllerContext, ViewData, TempData, "~/Views/Receptionist/Index/Sidebar/NoGoods.cshtml",null);
                return Json(jsonResult, JsonRequestBehavior.AllowGet);
            }
            else {
                string jsonResult = RenderPartialView(ControllerContext, ViewData, TempData, "~/Views/Receptionist/Index/Sidebar/ShopCartsHaveGoods.cshtml", showList);
                return Json(jsonResult, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        #region 首页模块
        // 热门模块
        public ActionResult ShowHotBoxGoods() {
            return PartialView("~/Views/Receptionist/HomePage/HotGoodsBox/HotGoodsBox.cshtml", showGoodsService.GetHotGoods());
        }
        //  美味餐厨模块
        public ActionResult ShowCookGoods() {
            return PartialView("~/Views/Receptionist/HomePage/_GoodsList1.cshtml", showGoodsService.GetGoodsByTypeId(10));
        }
        //  家纺布艺模块
        public ActionResult HomeAndClothArt() {
            return PartialView("~/Views/Receptionist/HomePage/_GoodsList1.cshtml", showGoodsService.GetGoodsByTypeId(12));
        }
        //  收纳模块
        public ActionResult StorageBox()
        {
            return PartialView("~/Views/Receptionist/HomePage/_GoodsList1.cshtml", showGoodsService.GetGoodsByTypeId(9));
        }
        //  卫浴模块
        public ActionResult Bathroom() {
            return PartialView("~/Views/Receptionist/HomePage/_GoodsList1.cshtml", showGoodsService.GetGoodsByTypeId(7));
        }
        //  居家饰物模块
        public ActionResult HomeAccessories() {
            return PartialView("~/Views/Receptionist/HomePage/_GoodsList1.cshtml", showGoodsService.GetGoodsByTypeId(11));
        }
        //  客厅模块
        public ActionResult LivingRoom()
        {
            return PartialView("~/Views/Receptionist/HomePage/_GoodsList2.cshtml", showGoodsService.GetGoodsByTypeId(20));
        }
        //  卧房模块
        public ActionResult Bedroom()
        {
            return PartialView("~/Views/Receptionist/HomePage/_GoodsList2.cshtml", showGoodsService.GetGoodsByTypeId(19));
        }
        //  原创模块
        public ActionResult Original()
        {
            return PartialView("~/Views/Receptionist/HomePage/_GoodsList2.cshtml", showGoodsService.GetGoodsByTypeId(22));
        }
        //  北欧模块
        public ActionResult NorthernEurope()
        {
            return PartialView("~/Views/Receptionist/HomePage/_GoodsList2.cshtml", showGoodsService.GetGoodsByTypeId(21));
        }
        #endregion
        #region  同类商品页
        public ActionResult SetGoodsPageListById(int typeId) {
            List<Goods> list = showGoodsService.GetGoodsByTypeId(typeId);
            goodsPages.setAllList(TFService.TFGoods(list));
            TempData["TypeName"] = showGoodsService.GetTypeName(typeId);
            TempData["TypeEnglishName"] = "/" + showGoodsService.GetTypeEnglishName(typeId);
            return Category();
        }
        public ActionResult SetGoodsPageListbySearch(string keywords) {
            List<Goods> list = showGoodsService.GetGoodsBySearch(keywords);
            goodsPages.setAllList(TFService.TFGoods(list));
            TempData["TypeName"] = keywords;
            TempData["TypeEnglishName"] = "";
            return Json(true,JsonRequestBehavior.AllowGet);
        }
        public ActionResult Category(){
            goodsPages.setPageContent(6);
            return View("~/Views/Receptionist/Category/Category.cshtml");
        }
        public ActionResult ShowCategory() {
            return PartialView("~/Views/Receptionist/Category/GoodsBox.cshtml", goodsPages.nowList);
        }
        public ActionResult LoadMore(int pageNum)
        {
            if (pageNum > goodsPages.pageTotal)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            else {
                goodsPages.setPageNow(pageNum);
                string jsonResult = RenderPartialView(ControllerContext, ViewData, TempData, "~/Views/Receptionist/Category/GoodsBox.cshtml", goodsPages.nowList);
                return Json(jsonResult,JsonRequestBehavior.AllowGet);
            }
        }
        public static string RenderPartialView(ControllerContext controllerContext, ViewDataDictionary viewData,
            TempDataDictionary tempData, string viewName, object model)
        {
            if (string.IsNullOrEmpty(viewName))
            {
                viewName = controllerContext.RouteData.GetRequiredString("action");
            }
            viewData.Model = model;
            using (var stringWriter = new StringWriter())
            {
                ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(controllerContext, viewName);
                var viewContext = new ViewContext(controllerContext, viewResult.View, viewData, tempData, stringWriter);
                viewResult.View.Render(viewContext, stringWriter);
                var result = stringWriter.GetStringBuilder().ToString();
                return result;
            }
        }
        protected static string RenderPartialViewToString(Controller controller, IView view)
        {
            //IView view = ViewEngines.Engines.FindPartialView(controller.ControllerContext, partialViewName).View;
            using (StringWriter writer = new StringWriter())
            {
                ViewContext viewContext = new ViewContext(controller.ControllerContext, view, controller.ViewData, controller.TempData, writer);
                viewContext.View.Render(viewContext, writer);
                return writer.ToString();
            }
        }
        public ActionResult Page() {
            return PartialView("~/Views/Receptionist/Category/PageButton.cshtml", goodsPages);
        }
        #endregion
        #region 商品详情页
        public ActionResult GoodsDetails(string goodsId) {
            Guid id = new Guid(goodsId);
            GoodsAndAtlasSVM goods = new GoodsAndAtlasSVM();
            goods.Goods = TFService.TFGoodSVM(TFService.TFGood(showGoodsService.GetGoodsById(id)), showGoodsService.GetDataGoodsTypes());
            goods.Atlas = TFService.TFAtlas(showGoodsService.GetGoodsAtlas(id));
            TempData["GoodsName"] = goods.Goods.GoodsName;
            return View("~/Views/Receptionist/GoodsDetails/GoodsDetails.cshtml", goods);
        }
        #endregion
        #region 订单支付页
        public ActionResult OrdersInfo(List<ConciseOrder> conciseOrders) {
            return View("~/Views/Receptionist/OrderInfo/OrderInfoPage.cshtml", conciseOrders);
        }
        #endregion
        #region 个人信息页
        public ActionResult BuyerInfoPage() {
            return View("~/Views/Receptionist/BuyerInfo/BuyerInfoPage.cshtml");
        }
        #endregion
    }
}