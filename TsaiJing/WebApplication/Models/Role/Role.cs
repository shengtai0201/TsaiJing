using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsaiJing.WebApplication.Models.Role
{
    // 角色
    public abstract class Role
    {
        private RoleType type;
        // 名稱
        private String name;
        // 折扣
        private double discount;

        protected Role(RoleType type, String name, double discount)
        {
            this.type = type;
            this.name = name;
            this.discount = discount;
        }

        public RoleType GetRoleType()
        {
            return type;
        }

        public String GetName()
        {
            return name;
        }

        public double GetDiscount()
        {
            return discount;
        }
    }
}
