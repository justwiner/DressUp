using DressUp_Scl_Data.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DressUp_Scl_Service.Service.BackStageService
{
    public class JurisdictionService
    {
        private DressUpWebDbEntities db = new DressUpWebDbEntities();
        private GeneralOperationService addRecords = new GeneralOperationService();
        private GeneralOperationService recordService = new GeneralOperationService();
        private PermissionsService permissionsService = new PermissionsService();
        public bool AddRole(Users user,string roleName) {
            try
            {
                Roles role = new Roles()
                {
                    RoleId = Guid.NewGuid(),
                    Name = roleName,
                    CreationTime = DateTime.Now,
                    IsDefault = false,
                    Code = roleName
                };
                db.Roles.Add(role);
                db.SaveChanges();

                string detail = "添加角色 ： " + roleName + " ";
                string type = "添加角色";
                addRecords.RecordOperationRecords(user, detail,type);
                return true;
            }
            catch {
                return false;
            }
        }
        //修改角色信息
        public bool ModifyRoles(Users user, List<int> menuIds, Guid roleID)
        {
            try
            {
                Roles role = db.Roles.SingleOrDefault(m => m.RoleId == roleID);
                    db.Roles.SingleOrDefault(m => m.RoleId == roleID).Permissions.Clear();
                    List<Menus> roleMenuList = new List<Menus>();
                    foreach (int item in menuIds)
                    {
                        Menus menu = db.Menus.SingleOrDefault(m => m.MenuId == item);
                        if (!permissionsService.MenuIfHasChildren(menu))
                        {
                            roleMenuList.Add(menu);
                        }
                    }
                    List<Permissions> permissionslist = permissionsService.GetPermissionsByMenu(roleMenuList);

                    foreach (Permissions item in permissionslist)
                    {
                      db.Roles.FirstOrDefault(m => m.RoleId == roleID).Permissions.Add(
                          db.Permissions.FirstOrDefault(p => p.PermissionId == item.PermissionId)
                      );
                    }

                    db.SaveChanges();
                    string detail = "为角色：" + role.Name + "修改了权限信息。";
                    string type = "修改角色信息";
                    recordService.RecordOperationRecords(user, detail, type);
                return true;
             }
            catch
            {
                return false;
            }
        }
        public bool ModifyRoles(Users user, Guid roleId)
        {
            try
            {
                Roles role = db.Roles.SingleOrDefault(m => m.RoleId == roleId);
                db.Roles.SingleOrDefault(m => m.RoleId == roleId).Permissions.Clear();
                db.SaveChanges();
                string detail = "为角色：" + role.Name + "修改了权限信息。";
                string type = "修改角色信息";
                recordService.RecordOperationRecords(user, detail, type);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public List<Users> GetAllBackStageUsers()
        {
            List<Users> allBackStageUsers = new List<Users>();
            foreach (Users item in db.Users.ToList())
            {
                if (!(item.Roles.Count == 1 && item.Roles.ElementAt(0).RoleId.ToString() == "299dc18e-4347-41cc-b8b4-477ce0d49699"))
                {
                    allBackStageUsers.Add(item);
                }
            }
            return allBackStageUsers;
        }
        public bool AddUser(Users user, string userName, string userAccount, string userPassword, string userContactInfo)
        {
            try
            {
                Users newUser = new Users()
                {
                    UserId = Guid.NewGuid(),
                    Name = userName,
                    Account = userAccount,
                    Password = userPassword,
                    CreationTime = DateTime.Now,
                    ContactInfo = userContactInfo,
                };
                db.Users.Add(newUser);
                db.SaveChanges();

                string detail = "添加用户 ： " + userName + " ";
                string type = "添加用户";
                addRecords.RecordOperationRecords(user, detail, type);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public List<Permissions> GetUserPermissions(Guid userId)
        {
            Users user = db.Users.SingleOrDefault(m => m.UserId == userId);
            List<Permissions> userPermissions = new List<Permissions>();
            foreach (Roles role in user.Roles) {
                userPermissions.AddRange(permissionsService.GetRolePermissions(role.RoleId));
            }
            userPermissions.AddRange(user.Permissions);
            return userPermissions;
        }
        public bool ModifyUsersPermission(Users user, Guid userId) {
            try
            {
                Users modifyUser = db.Users.SingleOrDefault(m => m.UserId == userId);
                db.Users.SingleOrDefault(m => m.UserId == userId).Permissions.Clear();
                db.Users.SingleOrDefault(m => m.UserId == userId).Roles.Clear();
                db.SaveChanges();
                string detail = "为用户：" + modifyUser.Name + "修改了权限信息。";
                string type = "修改用户信息";
                recordService.RecordOperationRecords(user, detail, type);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool ModifyUsersPermission(Users user, List<int> menuIds, Guid userId)
        {
            try
            {
                Users modifyUser = db.Users.SingleOrDefault(m => m.UserId == userId);
                db.Users.SingleOrDefault(m => m.UserId == userId).Permissions.Clear();
                db.Users.SingleOrDefault(m => m.UserId == userId).Roles.Clear();
                List<Menus> menuList = GetMenuList(menuIds);
                List<Permissions> allPermissionslist = permissionsService.GetPermissionsByMenu(menuList);
                List<Roles> rolesList = SetRolesList(allPermissionslist);
                List<Permissions> permissionsList = GetPermissions(allPermissionslist, rolesList);


                foreach (Permissions item in permissionsList)
                {
                    db.Users.SingleOrDefault(m => m.UserId == userId).Permissions.Add(
                        db.Permissions.FirstOrDefault(p => p.PermissionId == item.PermissionId)
                        );
                }
                foreach (Roles item in rolesList)
                {
                    db.Users.SingleOrDefault(m => m.UserId == userId).Roles.Add(item);
                }
                db.SaveChanges();

                string detail = "为用户：" + modifyUser.Name + "修改了权限信息。";
                string type = "修改用户信息";
                recordService.RecordOperationRecords(user, detail, type);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public List<Permissions> GetPermissions(List<Permissions> allPermissionslist, List<Roles> rolesList) {
            List<Permissions> intersectionList = new List<Permissions>();
            foreach (Roles item in rolesList) {
                foreach (Permissions p in item.Permissions) {
                    allPermissionslist.Remove(p);
                }
            }
            return allPermissionslist;
        }
        public List<Roles> SetRolesList(List<Permissions> allPermissionslist) {
            List<Roles> rolesList = new List<Roles>();
            List<Roles> allRoles = db.Roles.ToList();
            foreach (Roles item in allRoles)
            {
                bool ifThisRole = true;
                foreach (Permissions p in item.Permissions)
                {
                    if (allPermissionslist.Find(m => m.PermissionId == p.PermissionId) == null)
                    {
                        ifThisRole = false;
                        break;
                    }
                }
                if (ifThisRole)
                {
                    rolesList.Add(item);
                }
            }
            return rolesList;
        }
        public List<Menus> GetMenuList(List<int> menuIds) {
            List<Menus> menuList = new List<Menus>();
            foreach (int item in menuIds)
            {
                Menus menu = db.Menus.SingleOrDefault(m => m.MenuId == item);
                if (!permissionsService.MenuIfHasChildren(menu))
                {
                    menuList.Add(menu);
                }
            }
            return menuList;
        }

    }
}
