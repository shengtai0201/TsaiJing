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

namespace TsaiJing
{
    public interface ICustomerService : ICurrentUser
    {
        DataSourceResponse<MemberViewModel> ReadMembers(DataSourceRequest request);
        bool? UpdateMember(int key, MemberViewModel model, IDataSourceError error);

        DataSourceResponse<SelectListItem> ReadCustomers(DataSourceRequest request, IIdentity identity);

        IEnumerable<SelectListItem> ReadCustomers(IPrincipal currentUser);
    }
}
