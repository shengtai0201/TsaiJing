using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsaiJing.WebApplication.Models.Role
{
    public class Company : Role
    {
        private static Company instance = new Company();

        public static Company GetInstance()
        {
            return instance;
        }

        private Company() : base(RoleType.Company, "公司端", 0) { }
    }
}
