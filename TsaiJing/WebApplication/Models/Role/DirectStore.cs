using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsaiJing.WebApplication.Models.Role
{
    public class DirectStore : Role
    {
        private static DirectStore instance = new DirectStore();

        public static DirectStore GetInstance()
        {
            return instance;
        }

        private DirectStore() : base(RoleType.DirectStore, "直營店", .42) { }
    }
}
