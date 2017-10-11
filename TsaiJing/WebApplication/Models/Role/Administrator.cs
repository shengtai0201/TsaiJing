using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsaiJing.WebApplication.Models.Role
{
    public class Administrator : Role
    {
        private static Administrator instance = new Administrator();

        public static Administrator GetInstance()
        {
            return instance;
        }

        private Administrator() : base(RoleType.Administrator, "系統管理者", 0) { }
    }
}
