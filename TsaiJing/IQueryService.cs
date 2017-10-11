using Shengtai;
using Shengtai.Web.Telerik;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using TsaiJing.WebApplication.Models;

namespace TsaiJing
{
    public interface IQueryService
    {
        DataSourceResponse<QueryViewModel> ReadQueries(DataSourceRequest request, IIdentity identity);
        DataSourceResponse<QueryDetailViewModel> ReadQueryDetails(DataSourceRequest request, IIdentity identity);
    }
}
