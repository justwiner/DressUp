using DressUp_Scl_Service.Service.BackStageService;
using System;
using System.Collections.Generic;

namespace DressUp_Scl_Service.Model
{
    public class Menu_
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
            List<Menu_> menuList = service.GetAllMenuData();
            foreach (Menu_ menu in menuList)
            {
                if (menu.FatherID == this.Id)
                {
                    return true;
                }
            }
            return false;
        }
        //判断当前菜单是否有父级菜单
        public Boolean IfHasFather(List<Menu_> menuList)
        {
            foreach (Menu_ menu in menuList)
            {
                if (menu.Id == this.FatherID)
                {
                    return true;
                }
            }
            return false;
        }
        //获取当前菜单的子菜单
        public List<Menu_> GetChildren(List<Menu_> menuList)
        {
            List<Menu_> menu_list = new List<Menu_>();
            foreach (Menu_ menu in menuList)
            {
                if (menu.FatherID == this.Id)
                {
                    menu_list.Add(new Menu_()
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
