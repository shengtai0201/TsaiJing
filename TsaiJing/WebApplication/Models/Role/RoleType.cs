using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsaiJing.WebApplication.Models.Role
{
    public enum RoleType : int
    {
        [Description("系統管理者")]
        Administrator,

        [Description("督導")]
        Supervise,

        [Description("技術指導")]
        Guidance,

        [Description("VIP")]
        VIP,

        [Description("VVIP")]
        VVIP,

        [Description("顧客")]
        Customer,

        [Description("直營店")]
        DirectStore,

        [Description("公司端")]
        Company
    }
}
