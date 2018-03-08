using System.Web.Mvc;
using System.Collections.Generic;
using DressUp_Scl_Service.Model;
using DressUp_Scl_Service.Service.BackStageService;
using DressUp_Scl_Data.Data;
using DressUp.Scl.Model.ServiceModel;
using System.Linq;
using DressUp.Scl.Model.ViewModel;
using DressUp.Scl.Service;
using DressUp.Scl.Filter;

namespace DressUp.Scl.Controllers
{
    [MyLoginAuthorize(Order = 0)]
    public class HomePageController : Controller
    {
        PermissionsService permissionsService = new PermissionsService();
        LogService logService = new LogService();
        GeneralOperationService generalOperationService = new GeneralOperationService();
        PermissionsService permissionService = new PermissionsService();
        DataTFService TFService = new DataTFService();
        // GET: HomePage

        //登录页
        [AllowAnonymous]
        public ActionResult BackLogPage()
        {
            return View("~/Views/BackStage/HomePage/BackLogPage.cshtml");
        }
        //登录信息判断
        [AllowAnonymous]
        public ActionResult LogJudge(string userAccount, string userPassword)
        {
            Users user = logService.JudgeLog(userAccount, userPassword);
            if (user == null)
                return Json(false,JsonRequestBehavior.AllowGet);
            else
            {
                Session["User"] = user;
                return Json(true, JsonRequestBehavior.AllowGet);
            }
        }
        //退出登录
        public ActionResult SignOut() {
            Session.Remove("User");
            return View("~/Views/BackStage/HomePage/BackLogPage.cshtml");
        }
        //获取Session保存的用户的角色表
        public ActionResult GetUserRoles()
        {
            Users user = System.Web.HttpContext.Current.Session["User"] as Users;
            List<string> permissionCode = new List<string>();
            foreach (Permissions item in permissionService.GetPermissions(user))
            {
                permissionCode.Add(item.PermissionsCode);
            }
            return Json(permissionCode, JsonRequestBehavior.AllowGet);
        }
        #region 后台主页
        //后台主页入口--BackHome
        public ActionResult BackHome()
        {
            return View("~/Views/BackStage/HomePage/BackHome.cshtml");
        }
        //后台设置框分布页
        public ActionResult SetBox()
        {
            return PartialView("~/Views/BackStage/HomePage/SetBox/SetBox.cshtml");
        }
        //账号信息分布页
        public ActionResult AccountInfo()
        {
            return PartialView("~/Views/BackStage/HomePage/AccountInfo/AccountInfo.cshtml");
        }
        //财务信息分布页
        public ActionResult FinancialInfo()
        {
            return PartialView("~/Views/BackStage/HomePage/FinancialInfo/FinancialInfo.cshtml");
        }
        //销量排行分布页
        public ActionResult SalesRanking()
        {
            //List<GoodsVM> goodsList = TFService.TFGoods(generalOperationService.GetSalesRanking());
            return PartialView("~/Views/BackStage/HomePage/SalesRanking/SalesRanking.cshtml");
        }
        //缺货小计分布页
        public ActionResult StockSubtotal()
        {
            List<GoodsVM> goodsList = TFService.TFGoods(generalOperationService.GetStockSubtotalList());
            return PartialView("~/Views/BackStage/HomePage/StockSubtotal/StockSubtotal.cshtml", goodsList);
        }
        //工作日志分布页
        public ActionResult JobLog()
        {
            List<OperationRecordsVM> list = TFService.TFOperationRecords(generalOperationService.GetMyOperationRecords(Session["User"] as Users));
            return PartialView("~/Views/BackStage/HomePage/JobLog/JobLog.cshtml",list);
        }
        //销售统计图分布页
        public ActionResult SalesStatistics()
        {
            return PartialView("~/Views/BackStage/HomePage/SalesStatistics/SalesStatistics.cshtml");
        }
        //进出库统计分布页
        public ActionResult ImportExport()
        {
            return PartialView("~/Views/BackStage/HomePage/ImportExport/ImportExport.cshtml");
        }
        #endregion
        #region 后台布局页
        //后台布局页--侧边栏菜单分布页
        public ActionResult SideBarMenu()
        {
            Users user = System.Web.HttpContext.Current.Session["User"] as Users;
            List<Menu_> list = permissionsService.GetMenu(user);
            List<MenusSVM> menuList = list.Select(p => new MenusSVM()
            {
                Id = p.Id,
                Name = p.Name,
                FatherID = p.FatherID,
                PermissionCode = p.PermissionCode,
                Icon = p.Icon,
                Url = p.Url
            }).ToList();
            return PartialView("~/Views/BackStage/HomePage/SideBarMenu/SideBarMenu.cshtml", menuList);
        }
        //后台布局页--导航栏分布页
        public ActionResult NavigationBar()
        {
            return PartialView("~/Views/BackStage/HomePage/NavigationBar/NavigationBar.cshtml");
        }
        #endregion
        public ActionResult GetNewMessageNum() {
            Users user = Session["User"] as Users;
            int newMessageNum = generalOperationService.GetNewMessageNum(user);
            return Json(newMessageNum, JsonRequestBehavior.AllowGet);
        }
        public ActionResult NotFound() {
            return View("~/Views/Shared/Error-404.cshtml");
        }
    }
}