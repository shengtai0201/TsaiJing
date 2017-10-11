using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace TsaiJing.WebApplication.Models
{
    public class ProductDetailViewModel
    {
        public int? ProductDetailId { get; set; }

        public int ProductId { get; set; }

        public int Price { get; set; }
        public int SafeStock { get; set; }

        public SelectListItem FirstCategory { get; set; }
        public FirstSpecificationViewModel FirstSpecification { get; set; }
        //public SpecificationViewModel FirstSpecification { get; set; }

        public SelectListItem SecondCategory { get; set; }
        public SpecificationViewModel SecondSpecification { get; set; }
    }
}
