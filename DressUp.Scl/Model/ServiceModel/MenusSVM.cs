using DressUp_Scl_Service.Service.BackStageService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DressUp.Scl.Model.ServiceModel
{
    public class MenusSVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int FatherID { get; set; }
        public string PermissionCode { get; set; }
        public string Icon { get; set; }
        public string Url { get; set; }

        PermissionsService service = new PermissionsService();
        //判断当前菜单是否有子菜单
        public Boolean IfHasChildren()
        {
            List<DressUp_Scl_Service.Model.Menu_> menus = service.GetAllMenuData();
            List<MenusSVM> menuList = menus.Select(p => new MenusSVM()
            {
                Id = p.Id,
                Name = p.Name,
                FatherID = p.FatherID,
                PermissionCode = p.PermissionCode,
                Icon = p.Icon,
                Url = p.Url
            }).ToList();
            foreach (MenusSVM menu in menuList)
            {
                if (menu.FatherID == this.Id)
                {
                    return true;
                }
            }
            return false;
        }
        //判断当前菜单是否有父级菜单
        public Boolean IfHasFather(List<MenusSVM> menuList)
        {
            foreach (MenusSVM menu in menuList)
            {
                if (menu.Id == this.FatherID)
                {
                    return true;
                }
            }
            return false;
        }
        //获取当前菜单的子菜单
        public List<MenusSVM> GetChildren(List<MenusSVM> menuList)
        {
            List<MenusSVM> menu_list = new List<MenusSVM>();
            foreach (MenusSVM menu in menuList)
            {
                if (menu.FatherID == this.Id)
                {
                    menu_list.Add(new MenusSVM()
                    {
                        Id = menu.Id,
                        FatherID = menu.FatherID,
                        Name = menu.Name,
                        PermissionCode = menu.PermissionCode,
                        Icon = menu.Icon,
                        Url = menu.Url
                    });
                }
            }
            return menu_list;
        }
    }
}