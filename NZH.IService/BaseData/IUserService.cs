using NZH.Model.BaseData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NZH.IService.BaseData
{
    public interface IUserService
    {
        UserInfo Login(string UserName, string UserPassword);

        int UpdatePassWord(string UserName, string userpassword);

        int AddUser(UserInfo user);

        int UpdateUser(UserInfo user);

        int DeleteUser(int UserID);

        List<UserInfo> GetUserInfo(UserInfo user);
    }
}
