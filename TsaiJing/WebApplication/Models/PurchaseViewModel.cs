using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace TsaiJing.WebApplication.Models
{
    public class PurchaseViewModel
    {
        public int? PurchaseId { get; set; }
        public SelectListItem Manufacturer { get; set; }
        public DateTime Date { get; set; }
        public int TotalAmount { get; set; }
        public string Remark { get; set; }
    }
}
