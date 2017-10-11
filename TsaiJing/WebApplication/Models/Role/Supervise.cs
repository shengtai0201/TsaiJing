using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsaiJing.WebApplication.Models.Role
{
    public class Supervise : Role
    {
        private static Supervise instance = new Supervise();

        public static Supervise GetInstance()
        {
            return instance;
        }

        private Supervise() : base(RoleType.Supervise, "督導", .38) { }
    }
}
