
using DressUp.Scl.Filter;
using DressUp.Scl.Model.ServiceModel;
using DressUp.Scl.Service;
using DressUp_Scl_Data.Data;
using DressUp_Scl_Service.Model;
using DressUp_Scl_Service.Service.BackStageService;
using Newtonsoft.Json;
using System;
using System.Web.Mvc;

namespace DressUp.Scl.Controllers.BackStage
{
    [MyLoginAuthorize(Order = 0)]
    public class GeneralOperationController : Controller
    {
        private GeneralOperationService generalOperationService = new GeneralOperationService();
        static PagesSVM<OperationRecords_> pages = new PagesSVM<OperationRecords_>();
        static DataTFService TFService = new DataTFService();
        // GET: GeneralOperation
        [MyJurisdictionAuthorize(Order = 1)]
        public ActionResult UserInfo()
        {
            return View("~/Views/BackStage/GeneralOperation/PersonInfo.cshtml");
        }
        [MyJurisdictionAuthorize(Order = 1)]
        public ActionResult ChangePassword()
        {
            return View("~/Views/BackStage/GeneralOperation/ChangePassword.cshtml");
        }
        public ActionResult CheckPassword(String oldPassword ,String newPassword)
        {
            Users user = System.Web.HttpContext.Current.Session["User"] as Users;
            Boolean result = generalOperationService.CheckPassword(user, oldPassword, newPassword);
            return Json(result);
        }
        [MyJurisdictionAuthorize(Order = 1)]
        public ActionResult WorkLog() {
            Users user = System.Web.HttpContext.Current.Session["User"] as Users;
            bool ifHasAllRecords = false;
            foreach (Roles item in user.Roles) {
                if (item.RoleId.Equals("7282a016-989d-4222-8dfa-ab022d2c6502"))
                    ifHasAllRecords = true;
            }
            if (ifHasAllRecords)
            {
                pages.setAllList(generalOperationService.GetAllRecords());
            }
            else {
                pages.setAllList(generalOperationService.GetMyRecords(user));
            }
            return View("~/Views/BackStage/GeneralOperation/WorkLog/WorkLog.cshtml");
        }
        public ActionResult GetTotalPageNum() {
            return Json(pages.pageTotal, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GoPage(string goPage)
        {
            int page = int.Parse(goPage);
            pages.setPageNow(page);
            string jsonResult = JsonConvert.SerializeObject(pages.getNowList(), Formatting.Indented);
            return Json(jsonResult,JsonRequestBehavior.AllowGet);
        }
        [MyJurisdictionAuthorize(Order = 1)]
        public ActionResult ShowPage() {
            return PartialView("~/Views/BackStage/GeneralOperation/WorkLog/RecordPage/RecordPage.cshtml", pages);
        }
        public ActionResult FindByTime(int timeId) {
            Users user = System.Web.HttpContext.Current.Session["User"] as Users;
            Boolean ifHasAllRecords = false;
            foreach (Roles item in user.Roles)
            {
                if (item.RoleId.Equals("7282a016-989d-4222-8dfa-ab022d2c6502"))
                    ifHasAllRecords = true;
            }
            if (ifHasAllRecords)
            {
                pages.setAllList(generalOperationService.FindByTime(timeId, generalOperationService.GetAllRecords()));
            }
            else
            {
                pages.setAllList(generalOperationService.FindByTime(timeId, generalOperationService.GetMyRecords(user)));
            }
            return GoPage("1");
        }
    }
}