using DressUp_Scl_Data.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DressUp_Scl_Service.Service.BackStageService
{
    public class LogService
    {
        private DressUpWebDbEntities db = new DressUpWebDbEntities();
        //判断登录信息
        public Users JudgeLog(string account, string password)
        {
            List<Users> userList = db.Users.ToList();
            foreach (Users user in userList)
            {
                if (user.Account == account && user.Password == password)
                {
                    if (user.Roles.Count == 1)
                    {
                        if (user.Roles.ToList().Exists(m => m.Code == "Buyer"))
                            return null;
                        else
                        {
                            return user;
                        }
                    }
                    else
                    {
                        return user;
                    }
                }
            }
            return null;
        }
    }
}
