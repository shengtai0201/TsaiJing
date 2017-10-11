using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace TsaiJing.WebApplication.Models
{
    public class ShipmentDetailViewModel
    {
        public int? ShipmentDetailId { get; set; }
        public int ShipmentId { get; set; }
        public SelectListItem Product { get; set; }
        public SelectListItem ProductDetail { get; set; }
        public int Quantity { get; set; }
        public int? SubtotalAmount { get; set; }
    }
}
