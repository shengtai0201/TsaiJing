using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsaiJing.WebApplication.Models
{
    public class ProductViewModel
    {
        public int? ProductId { get; set; }

        public string Name { get; set; }

        public int? Price { get; set; }
        public int? SafeStock { get; set; }
    }
}
