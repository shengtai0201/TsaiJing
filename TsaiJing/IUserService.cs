using Shengtai;
using Shengtai.Web.Telerik;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using TsaiJing.WebApplication.Models;
using Microsoft.AspNet.Identity;
using TsaiJing.WebApplication.Models.Role;

namespace TsaiJing
{
    public interface IUserService : ICurrentUser
    {
        bool CreateCustomer(string userId, ExternalLoginConfirmationViewModel model);
        bool CreateCustomer(string userId, RegisterViewModel model);
        string GetUserName(IIdentity identity);

        DataSourceResponse<UserViewModel> Read(UserManager<ApplicationUser> userManager, DataSourceRequest request);
        bool? Update(UserManager<ApplicationUser> userManager, string key, UserViewModel model);

        IEnumerable<SelectListItem> ReadConsultants();
        //IEnumerable<SelectListItem> ReadRoles();

        //IList<KeyValuePair<string, Role>> GetRoles();
        IDictionary<string, Role> GetRoles();
        //Role GetRoleByUser(string userId);
        //Role GetRoleByCustomer(int customerId);
        string GetRoleByUser(string userId);
        string GetRoleByCustomer(int customerId);
    }
}
