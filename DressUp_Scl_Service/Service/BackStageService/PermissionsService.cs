using DressUp.Scl.Model;
using DressUp_Scl_Data.Data;
using DressUp_Scl_Service.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DressUp_Scl_Service.Service.BackStageService
{
    public class PermissionsService
    {
        private DressUpWebDbEntities db = new DressUpWebDbEntities();
        #region 权限分配
        //获取所有菜单信息
        public List<Menu_> GetAllMenuData()
        {
            List<Menu_> menuList = new List<Menu_>();
            foreach (Menus item in db.Menus.ToList())
            {
                menuList.Add(new Menu_()
                {
                    Name = item.MenuName,
                    Id = item.MenuId,
                    FatherID = item.FatherMenuId,
                    PermissionCode = item.PermissionCode,
                    Icon = item.MenuIcon,
                    Url = item.MenuUrl,
                });
            }
            return menuList;
        }
        //获取需要展示给用户的菜单
        public List<Menu_> GetMenu(Users user)
        {
            List<Menu_> userMenus = new List<Menu_>();
            List<Menu_> allMenuList = GetAllMenuData();
            List<Permissions> userPermission = GetPermissions(user);
            foreach (Permissions permission in userPermission)
            {
                foreach (Menu_ menu in allMenuList)
                {
                    if (permission.PermissionsCode.StartsWith(menu.PermissionCode))
                    {
                        if (!userMenus.Exists(m => m.PermissionCode == menu.PermissionCode))
                        {
                            userMenus.Add(menu);
                        }
                    }
                }
            }
            return userMenus;
        }
        //获取角色所需要的菜单
        public List<Menu_> GetRoleMenu(Guid roleId)
        {
            List<Menu_> userMenus = new List<Menu_>();
            List<Menu_> allMenuList = GetAllMenuData();
            List<Permissions> userPermission = GetRolePermissions(roleId);
            foreach (Permissions permission in userPermission)
            {
                foreach (Menu_ menu in allMenuList)
                {
                    if (permission.PermissionsCode.StartsWith(menu.PermissionCode))
                    {
                        if (!userMenus.Exists(m => m.PermissionCode == menu.PermissionCode))
                        {
                            userMenus.Add(menu);
                        }
                    }
                }
            }
            return userMenus;
        }
        //获取用户的所有权限
        public List<Permissions> GetPermissions(Users user)
        {
            try
            {
                List<Permissions> usersPermissions = new List<Permissions>();
                ICollection<Roles> userRoles = user.Roles;
                foreach (Roles role in userRoles)
                {
                    ICollection<Permissions> rolePermissions = role.Permissions;
                    foreach (Permissions permission in rolePermissions)
                    {
                        usersPermissions.Add(permission);
                    }
                    ICollection<Permissions> UserPermissions = user.Permissions;
                    foreach (Permissions permission in UserPermissions)
                    {
                        usersPermissions.Add(permission);
                    }
                }
                return usersPermissions;
            }
            catch {
                return null;
            }
        }
        //获取角色的所有权限
        public List<Permissions> GetRolePermissions(Guid roleId)
        {
            List<Permissions> rolePermissions = db.Roles.SingleOrDefault(m => m.RoleId == roleId).Permissions.ToList();
            return rolePermissions;
        }
        //获取角色的权限树
        public List<JurisdictionTree> GetJurisdictionTree(Guid roleId) {
            List<JurisdictionTree> jurisdictionTree = new List<JurisdictionTree>();
            List<Menu_> menus = GetRoleMenu(roleId);
            List<Menus> allMenus = db.Menus.ToList();
            foreach (Menus item in allMenus) {
                JurisdictionTree treeNode = new JurisdictionTree();
                treeNode.name = item.MenuName;
                treeNode.id = item.MenuId.ToString();
                treeNode.pId = item.FatherMenuId.ToString();
                foreach (Menu_ m in menus) {
                    if (m.Id == item.MenuId)
                    {
                        treeNode.@checked = true;
                        break;
                    }
                    else
                    {
                        treeNode.@checked = false;
                    }
                }
                jurisdictionTree.Add(treeNode);
            }
            return jurisdictionTree;
        }
        //获取用户的权限树
        public List<JurisdictionTree> GetJurisdictionTreeByUserId(Guid userId)
        {
            Users user = db.Users.SingleOrDefault(m => m.UserId == userId);
            List<JurisdictionTree> jurisdictionTree = new List<JurisdictionTree>();
            List<Menu_> menus = GetMenu(user);
            List<Menus> allMenus = db.Menus.ToList();
            foreach (Menus item in allMenus)
            {
                JurisdictionTree treeNode = new JurisdictionTree();
                treeNode.name = item.MenuName;
                treeNode.id = item.MenuId.ToString();
                treeNode.pId = item.FatherMenuId.ToString();
                foreach (Menu_ m in menus)
                {
                    if (m.Id == item.MenuId)
                    {
                        treeNode.@checked = true;
                        break;
                    }
                    else
                    {
                        treeNode.@checked = false;
                    }
                }
                jurisdictionTree.Add(treeNode);
            }
            return jurisdictionTree;
        }

        //判断是否能够修改角色信息
        public bool IfCanModifyRoles(Guid id)
        {
            Roles role = db.Roles.SingleOrDefault(m => m.RoleId == id);
            return role.IsDefault;
        }
        //从数据库获取所有角色
        public List<Roles> GetAllRoles() {
            return db.Roles.ToList();
        }
        
        //判断此菜单是否有子菜单
        public bool MenuIfHasChildren(Menus menu) {
            foreach (Menus item in db.Menus.ToList()) {
                if (item.FatherMenuId == menu.MenuId) {
                    return true;
                }
            }
            return false;
        }
        //通过处理好的菜单获取权限
        public List<Permissions> GetPermissionsByMenu(List<Menus> menus) {
            List<Permissions> permissionsList = new List<Permissions>();
            foreach (Menus item in menus)
            {
                string menuPermissionCode = item.PermissionCode;
                Permissions permission = db.Permissions.SingleOrDefault(m => m.PermissionsCode == menuPermissionCode);
                if (permission != null)
                {
                    permissionsList.Add(permission);
                }
            }
            return permissionsList;
        }
        #endregion
    }
}
