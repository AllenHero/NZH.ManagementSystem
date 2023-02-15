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

        int UpdatePassWord(string UserName, string UserPassword);

        int AddUser(UserInfo User);

        int UpdateUser(UserInfo User);

        int DeleteUser(int UserID);

        List<UserInfo> GetUserInfo(UserInfo User);
    }
}
