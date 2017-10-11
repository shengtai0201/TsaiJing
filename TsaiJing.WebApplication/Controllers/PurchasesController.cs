using Shengtai.Web.Telerik.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TsaiJing.WebApplication.Models;

namespace TsaiJing.WebApplication.Controllers
{
    [Authorize(Roles = "Company")]
    public class PurchasesController : ApiController<PurchaseViewModel, int>
    {
        public PurchasesController(IApiService<PurchaseViewModel, int> service) : base(service) { }
    }
}
