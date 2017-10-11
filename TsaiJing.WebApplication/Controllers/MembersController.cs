using Shengtai.Web.Telerik;
using Shengtai.Web.Telerik.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using TsaiJing.WebApplication.Models;

namespace TsaiJing.WebApplication.Controllers
{
    [Authorize(Roles = "Supervise, Guidance, Company")]
    public class MembersController : ApiController
    {
        private ICustomerService service;
        public MembersController(ICustomerService service)
        {
            service.CurrentUser = this.User;
            this.service = service;
        }

        [HttpGet]
        public DataSourceResponse<MemberViewModel> Get([ModelBinder(typeof(DataSourceRequestModelBinder))]DataSourceRequest request)
        {
            return this.service.ReadMembers(request);
        }

        [HttpPut]
        public HttpResponseMessage Put(int key, MemberViewModel model)
        {
            if (!ModelState.IsValid)
                return Request.CreateResponse<ModelStateDictionary>(HttpStatusCode.BadRequest, ModelState);

            var response = new DataSourceResponse<MemberViewModel> { DataCollection = new List<MemberViewModel> { model }, TotalRowCount = 1 };
            bool? result = this.service.UpdateMember(key, model, response);

            if (result == null)
                return Request.CreateResponse<IDataSourceResponse<MemberViewModel>>(HttpStatusCode.NotFound, response);
            else
            {
                if (result.Value)
                    return Request.CreateResponse<IDataSourceResponse<MemberViewModel>>(HttpStatusCode.OK, response);
                else
                    return Request.CreateResponse<IDataSourceResponse<MemberViewModel>>(HttpStatusCode.InternalServerError, response);
            }
        }
    }
}
