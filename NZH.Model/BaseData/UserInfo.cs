using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NZH.Model.BaseData
{
    public class UserInfo
    {
        /// <summary>
        /// 用户ID
        /// </summary>	
        public int UserID { get; set; }

        /// <summary>
        /// 用户名称
        /// </summary>	
        public string UserName { get; set; }

        /// <summary>
        /// 用户真实姓名
        /// </summary>	
        public string TrueName { get; set; }

        /// <summary>
        /// 用户密码
        /// </summary>		
        public string UserPassword { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>		
        public int UserUsable { get; set; }

        /// <summary>
        /// 注册时间
        /// </summary>		
        public DateTime? CreateDate { get; set; }

        /// <summary>
        /// 用户角色
        /// </summary>		
        public string RoleID { get; set; }

        /// <summary>
        /// 角色名
        /// </summary>		
        public string RoleName { get; set; }

        /// <summary>
        /// 用户备注
        /// </summary>		
        public string UserNote { get; set; }

        /// <summary>
        /// 当前级别
        /// </summary>		
        public string CurGradeName { get; set; }

        /// <summary>
        /// 建议级别
        /// </summary>		
        public string GradeName { get; set; }

        /// <summary>
        /// 等级代码
        /// </summary>		
        public string GradeCode { get; set; }

        /// <summary>
        /// 机种代码
        /// </summary>		
        public string MachineCode { get; set; }

        /// <summary>
        /// 机种名称
        /// </summary>		
        public string MachineName { get; set; }

        /// <summary>
        /// 线体
        /// </summary>		
        public string LineNo { get; set; }

        /// <summary>
        /// 分数
        /// </summary>	
        public decimal Score { get; set; }

        /// <summary>
        /// 分数
        /// </summary>	
        public decimal ScoreOld { get; set; }

        /// <summary>
        /// 用户信息集合
        /// </summary>

        public List<UserInfo> user { get; set; }

        /// <summary>
        /// 角色信息
        /// </summary>

        public List<RoleInfo> role { get; set; }

        /// <summary>
        /// 权限信息
        /// </summary>

        public List<AuthorityInfo> authority { get; set; }
    }
}
