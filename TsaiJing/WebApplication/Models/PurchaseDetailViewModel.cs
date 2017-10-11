using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace TsaiJing.WebApplication.Models
{
    public class PurchaseDetailViewModel
    {
        public int? PurchaseDetailId { get; set; }
        public int PurchaseId { get; set; }
        public SelectListItem Product { get; set; }
        public SelectListItem ProductDetail { get; set; }
        public int Price { get; set; }
        public int Inventory { get; set; }
        public int? SubtotalAmount { get; set; }
    }
}
