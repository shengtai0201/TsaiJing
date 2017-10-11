using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsaiJing.WebApplication.Models.Role
{
    public class Guidance : Role
    {
        private static Guidance instance = new Guidance();

        public static Guidance GetInstance()
        {
            return instance;
        }

        private Guidance() : base(RoleType.Guidance, "技術指導", .29) { }
    }
}
