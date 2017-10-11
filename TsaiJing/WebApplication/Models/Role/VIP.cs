using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsaiJing.WebApplication.Models.Role
{
    public class VIP : Role
    {
        private static VIP instance = new VIP();

        public static VIP GetInstance()
        {
            return instance;
        }

        private VIP() : base(RoleType.VIP, "VIP", .08) { }
    }
}
