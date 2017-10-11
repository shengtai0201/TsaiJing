using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsaiJing.WebApplication.Models.Role
{
    public class Customer : Role
    {
        private static Customer instance = new Customer();

        public static Customer GetInstance()
        {
            return instance;
        }

        private Customer() : base(RoleType.Customer, "顧客", 0) { }
    }
}
