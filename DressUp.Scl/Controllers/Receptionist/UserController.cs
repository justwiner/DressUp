using DressUp.Scl.Model.ServiceModel;
using DressUp.Scl.Model.ViewModel;
using DressUp.Scl.Service;
using DressUp_Scl_Data.Data;
using DressUp_Scl_Service.Service.ReceptionistService;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace DressUp.Scl.Controllers.Receptionist
{
    public class UserController : Controller
    {
        private UserService userService = new UserService();
        private DataTFService TFService = new DataTFService();
        private SimpleOrdersServcie orderService = new SimpleOrdersServcie();
        private static PagesSVM<OrdersSVM> orderSVMPages = new PagesSVM<OrdersSVM>();
        private static PagesSVM<ReceivingInfosVM> receivingInfosVMPages = new PagesSVM<ReceivingInfosVM>();
        // GET: User
        //************************************注册****************************************//
        public ActionResult BuyerRegister() {
            return View("~/Views/Receptionist/Register/Register.cshtml");
        }
        public ActionResult NameIfExist(string name) {
            bool result = userService.NameIfExist(name);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult RegisterNewBuer(string nickname, string account, string password) {
            bool result = userService.RegisterNewBuer(nickname, account, password);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        //************************************登录****************************************//
        public ActionResult BuyerLogin(string account, string password) {
            Users user = userService.BuyerLogin(account, password);
            if (user == null)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            else {
                Session["buyer"] = user;
                String json = "";
                StringBuilder JsonString = new StringBuilder();
                JsonString.Append("{");
                JsonString.Append("\"BuyerName\":\"" + user.Name +
                           "\",\"ShoppingCartsCount\":\"" + user.ShoppingCarts.Count +
                           "\"");
                JsonString.Append("}");
                json = JsonString.ToString();
                return Json(json, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult IfBuyerLogin() {
            Users user = Session["buyer"] as Users;
            if (user == null)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            else {
                String json = "";
                StringBuilder JsonString = new StringBuilder();
                JsonString.Append("{");
                JsonString.Append("\"BuyerName\":\"" + user.Name +
                           "\",\"ShoppingCartsCount\":\"" + userService.GetBuyersShoppingCarts(user).Count +
                           "\"");
                JsonString.Append("}");
                json = JsonString.ToString();
                return Json(json, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult BuyerLogOut() {
            try
            {
                Session["buyer"] = null;
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex) {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }
        //*************************************购物车*************************************//
        public ActionResult AddShopCartsGoods(string GoodsId, int goodsNum) {
            Users buyer = Session["buyer"] as Users;
            if (buyer == null)
            {
                return Json("null", JsonRequestBehavior.AllowGet);
            }
            bool result = userService.AddShopCartsGoods(new Guid(GoodsId), goodsNum, buyer.UserId);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DeleteShopCartsGoods(int shoppingCartsId) {
            userService.DeleteShopCartsGoods(shoppingCartsId);
            Users buyer = Session["buyer"] as Users;
            List<ShopCartsSVM> showList = TFService.TFShopCartsSVM(userService.GetBuyersShoppingCarts(buyer));
            string jsonResult = RenderPartialView(ControllerContext, ViewData, TempData, "~/Views/Receptionist/Index/Sidebar/ShopCartsHaveGoods.cshtml", showList);
            return Json(jsonResult, JsonRequestBehavior.AllowGet);
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
        //************************************订单********************************//
        public ActionResult GetReceivingInfo() {
            Users buyer = Session["buyer"] as Users;
            List<ReceivingInfosVM> list = TFService.TFReceivingInfos(userService.GetReceivingInfo(buyer.UserId));
            if (list != null)
            {
                string jsonResult = RenderPartialView(ControllerContext, ViewData, TempData, "~/Views/Receptionist/OrderInfo/AllReceivingBox.cshtml", list);
                return Json(jsonResult, JsonRequestBehavior.AllowGet);
            }
            else {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult GetDefaultReceivingInfo() {
            Users buyer = Session["buyer"] as Users;
            ReceivingInfosVM receivingInfosVM = TFService.TFReceivingInfo(userService.GetDefaultReceivingInfo(buyer.UserId));
            return PartialView("~/Views/Receptionist/OrderInfo/ReceivingInfo.cshtml", receivingInfosVM);
        }
        public ActionResult ShowOrders(List<ConciseOrder> conciseOrders) {
            List<SimpleOrdersSVM> list = orderService.CreatSimpleOrders(conciseOrders);
            return PartialView("~/Views/Receptionist/OrderInfo/OrderInfo.cshtml", list);
        }
        public ActionResult SaveReceivingInfo(string newConsignee,string newReceiptAddress,string newContactInfo) {
            Users buyer = Session["buyer"] as Users;
            ReceivingInfos receivingInfo = userService.SaveReceivingInfo(buyer.UserId,newConsignee, newReceiptAddress, newContactInfo);
            if (receivingInfosVMPages != null) {
                List<ReceivingInfos> dataReceivingInfos = userService.GetUserReceivingInfo(buyer.UserId);
                List<ReceivingInfosVM> receivingInfos = TFService.TFReceivingInfos(dataReceivingInfos);
                receivingInfosVMPages.setAllList(receivingInfos);
            }
            string jsonResult = JsonConvert.SerializeObject(TFService.TFReceivingInfo(receivingInfo), Formatting.Indented);
            return Json(jsonResult, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetReceivingInfoByIdInOrderPage(int receivingInfoId)
        {
            ReceivingInfos receivingInfo = userService.GetReceivingInfoById(receivingInfoId);
            ReceivingInfosVM receivingInfoVM = TFService.TFReceivingInfo(receivingInfo);
            string jsonResult = JsonConvert.SerializeObject(receivingInfoVM);
            return Json(jsonResult, JsonRequestBehavior.AllowGet);
        }
        public ActionResult CreatOrders(string minimalistOrders) {
            Users buyer = Session["buyer"] as Users;
            List<MiniOrders> miniOrders = JsonConvert.DeserializeObject<List<MiniOrders>>(minimalistOrders);
            List<int> shopCartsIds = new List<int>();
            foreach (MiniOrders item in miniOrders) {
                shopCartsIds.Add(item.ShopCartsId);
            }
            bool result = userService.SaveOrders(shopCartsIds,TFService.DTFOrders(buyer.UserId,miniOrders));
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        //************************************个人中心******************************//
        public ActionResult GetOrderById(string orderId) {
            Guid id = new Guid(orderId);
            OrdersSVM order = TFService.TFOrderSVM(userService.GetOrderById(id));
            string jsonResult = RenderPartialView(ControllerContext, ViewData, TempData, "~/Views/Receptionist/BuyerInfo/OrderDetail.cshtml", order);
            return Json(jsonResult, JsonRequestBehavior.AllowGet);
        }
        //主页
        public ActionResult GetNearlyOrder()
        {
            Users buyer = Session["buyer"] as Users;
            List<OrdersSVM> list = TFService.TFOrdersSVM(userService.GetNearlyOrder(buyer.UserId));
            return PartialView("~/Views/Receptionist/BuyerInfo/BuyerInfoHomePage/BuyerInfoPageTable.cshtml", list);
        }
        public ActionResult GetUserInfo() {
            Users buyer = Session["buyer"] as Users;
            int waitReceivingOrdersNum = userService.WaitReceivingOrdersNum(buyer.UserId);
            UsersInfoSVM userInfo = TFService.TFUsersInfoSVM(buyer, waitReceivingOrdersNum);
            return PartialView("~/Views/Receptionist/BuyerInfo/BuyerInfoHomePage/BuyerInfoHomePage.cshtml", userInfo);
        }
        public ActionResult GetUserInfoToString()
        {
            Users buyer = Session["buyer"] as Users;
            int waitReceivingOrdersNum = userService.WaitReceivingOrdersNum(buyer.UserId);
            UsersInfoSVM userInfo = TFService.TFUsersInfoSVM(buyer, waitReceivingOrdersNum);
            string jsonResult = RenderPartialView(ControllerContext, ViewData, TempData, "~/Views/Receptionist/BuyerInfo/BuyerInfoHomePage/BuyerInfoHomePage.cshtml", userInfo);
            return Json(jsonResult, JsonRequestBehavior.AllowGet);
        }
        //订单
        public ActionResult GetAllOrderPage(int orderStatusId)
        {
            Users buyer = Session["buyer"] as Users;
            List<OrdersSVM> orders = TFService.TFOrdersSVM(userService.GetOrderById(buyer.UserId, orderStatusId));
            orderSVMPages.setAllList(orders);
            int allOrdersNum = orders.Count;
            int waitReceivingOrdersNum = userService.WaitReceivingOrdersNum(buyer.UserId);
            SimpleOrderInfoSVM simpleOrderInfo = TFService.TFSimpleOrderInfo(allOrdersNum, waitReceivingOrdersNum);
            string jsonResult = RenderPartialView(ControllerContext, ViewData, TempData, "~/Views/Receptionist/BuyerInfo/BuyerOrders/BuyerOrders.cshtml", simpleOrderInfo);
            return Json(jsonResult, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SetOrdersPage(int orderStatusId)
        {
            Users buyer = Session["buyer"] as Users;
            List<OrdersSVM> orders = TFService.TFOrdersSVM(userService.GetOrderById(buyer.UserId, orderStatusId));
            orderSVMPages.setAllList(orders);
            return Json(orderSVMPages.pageTotal, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetAllOrder(int page)
        {
            orderSVMPages.setPageNow(page);
            return PartialView("~/Views/Receptionist/BuyerInfo/BuyerOrders/BuyerOrdersTable.cshtml", orderSVMPages.nowList);
        }
        public ActionResult GetOrdersByStatusId(int orderStatusId) {
            Users buyer = Session["buyer"] as Users;
            List<OrdersSVM> orders = TFService.TFOrdersSVM(userService.GetOrderById(buyer.UserId, orderStatusId));
            orderSVMPages.setAllList(orders);
            orderSVMPages.setPageNow(1);
            string jsonResult = RenderPartialView(ControllerContext, ViewData, TempData, "~/Views/Receptionist/BuyerInfo/BuyerOrders/BuyerOrdersTable.cshtml", orderSVMPages.nowList);
            return Json(jsonResult, JsonRequestBehavior.AllowGet);
        }
        public ActionResult OrdersPageGoNum(int page)
        {
            orderSVMPages.setPageNow(page);
            string jsonResult = RenderPartialView(ControllerContext, ViewData, TempData, "~/Views/Receptionist/BuyerInfo/BuyerOrders/BuyerOrdersTable.cshtml", orderSVMPages.nowList);
            return Json(jsonResult, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ConfirmReceipt(string orderId) {
            Users buyer = Session["buyer"] as Users;
            Guid id = new Guid(orderId);
            bool result = userService.ModifyOrderState(id, "已完成");
            List<OrdersSVM> orders = TFService.TFOrdersSVM(userService.GetOrderById(buyer.UserId, 1));
            orderSVMPages.setAllList(orders);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ApplyRefund(string orderId)
        {
            Users buyer = Session["buyer"] as Users;
            Guid id = new Guid(orderId);
            bool result = userService.ModifyOrderState(id, "退款中");
            List<OrdersSVM> orders = TFService.TFOrdersSVM(userService.GetOrderById(buyer.UserId, 1));
            orderSVMPages.setAllList(orders);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        // 修改查看个人信息
        public ActionResult UserInfoPage() {
            Users buyer = Session["buyer"] as Users;
            UsersVM buyerVm = TFService.TFUser(buyer);
            string jsonResult = RenderPartialView(ControllerContext, ViewData, TempData, "~/Views/Receptionist/BuyerInfo/BuyerInfo/BuyerInfo.cshtml", buyerVm);
            return Json(jsonResult, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ChangeBuyerInfo(string buyerName,string buyerContactInfo) {
            Users buyer = userService.ChangeBuyerInfo(Session["buyer"] as Users,buyerName, buyerContactInfo);
            string jsonResult = JsonConvert.SerializeObject(TFService.TFUserSVM(buyer), Formatting.Indented);
            return Json(jsonResult, JsonRequestBehavior.AllowGet);
        }
        //收货地址
        public ActionResult ReceivingAddress() {
            Users buyer = Session["buyer"] as Users;
            UsersVM buyerVm = TFService.TFUser(buyer);
            string jsonResult = RenderPartialView(ControllerContext, ViewData, TempData, "~/Views/Receptionist/BuyerInfo/ReceivingAddress/ReceivingAddress.cshtml", buyerVm);
            return Json(jsonResult, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetUserReceivingInfo() {
            Users buyer = Session["buyer"] as Users;
            List<ReceivingInfos> dataReceivingInfos = userService.GetUserReceivingInfo(buyer.UserId);
            List<ReceivingInfosVM> receivingInfos = TFService.TFReceivingInfos(dataReceivingInfos);
            receivingInfosVMPages.setAllList(receivingInfos);
            return PartialView("~/Views/Receptionist/BuyerInfo/ReceivingAddress/ReceivingAddressTable.cshtml", receivingInfos);
        }
        public ActionResult ReceivingAddressPageGoNum(int page)
        {
            receivingInfosVMPages.setPageNow(page);
            string jsonResult = RenderPartialView(ControllerContext, ViewData, TempData, "~/Views/Receptionist/BuyerInfo/ReceivingAddress/ReceivingAddressTable.cshtml", receivingInfosVMPages.nowList);
            return Json(jsonResult, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SetReceivingAddress()
        {
            Users buyer = Session["buyer"] as Users;
            List<ReceivingInfos> dataReceivingInfos = userService.GetUserReceivingInfo(buyer.UserId);
            List<ReceivingInfosVM> receivingInfos = TFService.TFReceivingInfos(dataReceivingInfos);
            receivingInfosVMPages.setAllList(receivingInfos);
            return Json(receivingInfosVMPages.pageTotal, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DeleteReceivingInfo(int receivingInfoId)
        {
            Users buyer = Session["buyer"] as Users;
            bool result = userService.DeleteReceivingInfo(receivingInfoId);
            List<ReceivingInfos> dataReceivingInfos = userService.GetUserReceivingInfo(buyer.UserId);
            List<ReceivingInfosVM> receivingInfos = TFService.TFReceivingInfos(dataReceivingInfos);
            receivingInfosVMPages.setAllList(receivingInfos);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult EditReceivingInfo(string receivingObjJson)
        {
            Users buyer = Session["buyer"] as Users;
            ReceivingInfosVM receivingInfoVM = JsonConvert.DeserializeObject<ReceivingInfosVM>(receivingObjJson);
            receivingInfoVM.UserId = buyer.UserId;
            ReceivingInfos receivingInfo = TFService.DTFReceivingInfosVM(receivingInfoVM);
            bool result = userService.EditReceivingInfo(receivingInfo);
            List<ReceivingInfos> dataReceivingInfos = userService.GetUserReceivingInfo(buyer.UserId);
            List<ReceivingInfosVM> receivingInfos = TFService.TFReceivingInfos(dataReceivingInfos);
            receivingInfosVMPages.setAllList(receivingInfos);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetReceivingInfoById(int receivingInfoId)
        {
            ReceivingInfos receivingInfo = userService.GetReceivingInfoById(receivingInfoId);
            string jsonResult = RenderPartialView(ControllerContext, ViewData, TempData, "~/Views/Receptionist/BuyerInfo/EditReceivingInfoBox.cshtml", TFService.TFReceivingInfo(receivingInfo));
            return Json(jsonResult, JsonRequestBehavior.AllowGet);
        }
        //修改密码
        public ActionResult ChangePasswordPage() {
            string jsonResult = RenderPartialView(ControllerContext, ViewData, TempData, "~/Views/Receptionist/BuyerInfo/ChangePassword/ChangePassword.cshtml", null);
            return Json(jsonResult, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ChangePassword(string newPassword,string oldPassword) {
            bool result = userService.ChangePassword(Session["buyer"] as Users, newPassword, oldPassword);
            Session["buyer"] = null;
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}