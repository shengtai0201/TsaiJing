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
    [Authorize(Roles = "Supervise, Guidance, Company")]
    public class ShipmentDetailsController : ApiController<ShipmentDetailViewModel, int>
    {
        public ShipmentDetailsController(IApiService<ShipmentDetailViewModel, int> service) : base(service) { }
    }
}
