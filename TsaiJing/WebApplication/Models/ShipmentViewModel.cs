using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace TsaiJing.WebApplication.Models
{
    public class ShipmentViewModel
    {
        public int? ShipmentId { get; set; }
        public SelectListItem Customer { get; set; }
        public DateTime Date { get; set; }
        public int TotalAmount { get; set; }
        public int? ConsumptionRebate { get; set; }
    }
}
