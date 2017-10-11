using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsaiJing.Data
{
    public partial class DefaultDbContext : DbContext
    {
        public DefaultDbContext() : base("name=DefaultConnection") { }
    }
}
