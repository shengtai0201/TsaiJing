using Microsoft.AspNet.Identity.Owin;
using Shengtai.Web.Telerik;
using Shengtai.Web.Telerik.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using TsaiJing.WebApplication.Models;

namespace TsaiJing.WebApplication.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class UsersController : ApiController
    {
        private ApplicationUserManager userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return userManager ?? HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                userManager = value;
            }
        }

        private readonly IUserService service;
        public UsersController(IUserService service)
        {
            service.CurrentUser = this.User;
            this.service = service;
        }

        [HttpGet]
        public DataSourceResponse<UserViewModel> Get([ModelBinder(typeof(DataSourceRequestModelBinder))]DataSourceRequest request)
        {
            return this.service.Read(this.UserManager, request);
        }

        [HttpPut]
        public HttpResponseMessage Put(string key, UserViewModel model)
        {
            if (!ModelState.IsValid)
                return Request.CreateResponse<ModelStateDictionary>(HttpStatusCode.BadRequest, ModelState);

            var response = new DataSourceResponse<UserViewModel> { DataCollection = new List<UserViewModel> { model }, TotalRowCount = 1 };
            bool? result = this.service.Update(this.UserManager, key, model);

            if (result == null)
                return Request.CreateResponse<IDataSourceResponse<UserViewModel>>(HttpStatusCode.NotFound, response);
            else
            {
                if (result.Value)
                    return Request.CreateResponse<IDataSourceResponse<UserViewModel>>(HttpStatusCode.OK, response);
                else
                    return Request.CreateResponse<IDataSourceResponse<UserViewModel>>(HttpStatusCode.InternalServerError, response);
            }
        }
    }
}
