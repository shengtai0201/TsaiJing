using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsaiJing.WebApplication.Models.Role
{
    public class VVIP : Role
    {
        private static VVIP instance = new VVIP();

        public static VVIP GetInstance()
        {
            return instance;
        }

        private VVIP() : base(RoleType.VVIP, "VVIP", .18) { }
    }
}
