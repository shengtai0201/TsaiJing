using Shengtai;
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
    public class CategoriesController : ApiController<CategoryViewModel, int>
    {
        public CategoriesController(IApiService<CategoryViewModel, int> service) : base(service) { }
    }
}
