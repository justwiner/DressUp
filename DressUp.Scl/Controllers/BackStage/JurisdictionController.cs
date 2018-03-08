using DressUp.Scl.Filter;
using DressUp.Scl.Model;
using DressUp.Scl.Model.ServiceModel;
using DressUp.Scl.Model.ViewModel;
using DressUp.Scl.Service;
using DressUp_Scl_Data.Data;
using DressUp_Scl_Service.Model;
using DressUp_Scl_Service.Service.BackStageService;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace DressUp.Scl.Controllers.BackStage
{
    [MyLoginAuthorize(Order = 0)]
    public class JurisdictionController : Controller
    {
        static PagesSVM<RolesVM> rolesPages = new PagesSVM<RolesVM>();
        static PagesSVM<UsersVM> usersPages = new PagesSVM<UsersVM>();
        private PermissionsService permissionsService = new PermissionsService();
        private JurisdictionService jurisdictionService = new JurisdictionService();
        private DataTFService TFService = new DataTFService();
        // GET: Jurisdiction
        #region 角色权限的修改
        [MyJurisdictionAuthorize(Order = 1)]
        public ActionResult JurisdictionManagement()
        {
            rolesPages.setAllList(TFService.TFRoles(permissionsService.GetAllRoles()));
            return View("~/Views/BackStage/Jurisdiction/JurisdictionManagement/JurisdictionManagement.cshtml");
        }
        [MyJurisdictionAuthorize(Order = 1)]
        public ActionResult ShowRolesTable() {
            return PartialView("~/Views/BackStage/Jurisdiction/JurisdictionManagement/RolesTable.cshtml", rolesPages);
        }
        public ActionResult GoPage(string goPage)
        {
            int page = int.Parse(goPage);
            rolesPages.setPageNow(page);
            string jsonResult = JsonConvert.SerializeObject(rolesPages.getNowList(), Formatting.Indented);
            return LargeJson(jsonResult, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetTotalPage()
        {
            return Json(rolesPages.getPageTotal(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetRolePermissions(string roleId)
        {
            Guid id = new Guid(roleId);
            List<JurisdictionTree> list = permissionsService.GetJurisdictionTree(id);
            string jsonResult = JsonConvert.SerializeObject(list, Formatting.Indented);
            return LargeJson(jsonResult, JsonRequestBehavior.AllowGet);
        }
        public ActionResult IfCanModifyRoles(string roleId) {
            Guid id = new Guid(roleId);
            return Json(permissionsService.IfCanModifyRoles(id), JsonRequestBehavior.AllowGet);
        }
        public ActionResult ModifyRoles(string menuIds,string roleID)
        {
            Guid roleId = new Guid(roleID);
            Users user = Session["User"] as Users;
            bool result = false;
            if (menuIds == "") {
                result = jurisdictionService.ModifyRoles(user, roleId);
            }
            else
            {
                string[] menuIdList = menuIds.Split(',');
                int[] menus = new int[menuIdList.Length];
                for (int i = 0; i < menuIdList.Length; i++)
                {
                    menus[i] = Convert.ToInt32(menuIdList[i]);
                }
                List<int> menuId = new List<int>(menus);
                result = jurisdictionService.ModifyRoles(user, menuId, roleId);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
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
        public ActionResult AddRole(string roleName) {
            bool result = jurisdictionService.AddRole(Session["User"] as Users, roleName);
            rolesPages.setAllList(TFService.TFRoles(permissionsService.GetAllRoles()));
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region 员工管理
        [MyJurisdictionAuthorize(Order = 1)]
        public ActionResult StaffManagement() {
            usersPages.setAllList(TFService.TFUsers(jurisdictionService.GetAllBackStageUsers()));
            return View("~/Views/BackStage/Jurisdiction/StaffManagement/StaffManagement.cshtml");
        }
        [MyJurisdictionAuthorize(Order = 1)]
        public ActionResult ShowUsersTable()
        {
            return PartialView("~/Views/BackStage/Jurisdiction/StaffManagement/UsersTable.cshtml", usersPages);
        }
        public ActionResult GoUsersPage(string goPage)
        {
            int page = int.Parse(goPage);
            usersPages.setPageNow(page);
            string jsonResult = JsonConvert.SerializeObject(usersPages.getNowList(), Formatting.Indented);
            return LargeJson(jsonResult, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetUsersTotalPage()
        {
            return Json(usersPages.getPageTotal(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult AddUser(string userName, string userAccount, string userPassword, string userContactInfo) {
            Users user = Session["User"] as Users;
            bool result = jurisdictionService.AddUser(user,userName,userAccount,userPassword,userContactInfo);
            usersPages.setAllList(TFService.TFUsers(jurisdictionService.GetAllBackStageUsers()));
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetUserPermissions(string userId) {
            Guid id = new Guid(userId);
            List<JurisdictionTree> list = permissionsService.GetJurisdictionTreeByUserId(id);
            string jsonResult = JsonConvert.SerializeObject(list, Formatting.Indented);
            return Json(jsonResult, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ModifyUsersPermission(string menuIds, string userId) {
            Guid modifyId = new Guid(userId);
            Users user = Session["User"] as Users;
            bool result = false;
            if (menuIds == "")
                {
                    result = jurisdictionService.ModifyUsersPermission(user, modifyId);
                }
            else
                {
                    string[] menuIdList = menuIds.Split(',');
                    int[] menus = new int[menuIdList.Length];
                    for (int i = 0; i < menuIdList.Length; i++)
                    {
                        menus[i] = Convert.ToInt32(menuIdList[i]);
                    }
                    List<int> menuId = new List<int>(menus);
                    result = jurisdictionService.ModifyUsersPermission(user, menuId, modifyId);
                }
            return Json(result, JsonRequestBehavior.AllowGet);
         }
    }
    #endregion
}