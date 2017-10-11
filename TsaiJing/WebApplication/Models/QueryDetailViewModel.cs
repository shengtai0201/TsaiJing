using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsaiJing.WebApplication.Models
{
    public class QueryDetailViewModel
    {
        public int PurchaseId { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public DateTime ShipmentDate { get; set; }
        public string CustomerName { get; set; }
        public int TotalAmount { get; set; }
    }
}
