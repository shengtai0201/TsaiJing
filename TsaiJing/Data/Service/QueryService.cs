using Shengtai;
using Shengtai.Web.Telerik.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TsaiJing.WebApplication.Models;
using Shengtai.Web.Telerik;
using Microsoft.AspNet.Identity;
using System.Data.SqlClient;
using System.Security.Principal;

namespace TsaiJing.Data.Service
{
    public class QueryService : SqlRepository<DefaultDbContext>, IQueryService
    {
        private IUserService userService;
        public QueryService(IUserService userService) : base("DefaultConnection")
        {
            this.userService = userService;
        }

        public DataSourceResponse<QueryViewModel> ReadQueries(DataSourceRequest request, IIdentity identity)
        {
            var currentUserId = identity.GetUserId();
            var roles = this.userService.GetRoles();

            var shipments = this.DbContext.Shipments.Where(s => s.UserId == currentUserId).ToList();
            IDictionary<string, QueryViewModel> total = new Dictionary<string, QueryViewModel>();
            foreach (var shipment in shipments)
            {
                string key = shipment.Date.ToString("yyyyMM");
                QueryViewModel viewModel;
                if (total.ContainsKey(key))
                    viewModel = total[key];
                else
                {
                    viewModel = new QueryViewModel
                    {
                        Year = shipment.Date.Year,
                        Month = shipment.Date.Month,
                        Performance = 0
                    };
                    total.Add(key, viewModel);
                }

                // 計算
                var currentRole = roles[shipment.UserRoleId];
                var discount = currentRole.GetDiscount();
                if (shipment.CustomerId.HasValue)
                {
                    var customerRole = roles[shipment.CustomerRoleId];
                    double customerDiscount = customerRole.GetDiscount();
                    discount -= customerDiscount;
                }

                var shipmentDetails = this.DbContext.ShipmentDetails.Where(d => d.ShipmentId == shipment.ShipmentId).ToList();
                foreach (var shipmentDetail in shipmentDetails)
                    viewModel.Performance += shipmentDetail.SubtotalAmount;

                viewModel.Bonus = Convert.ToInt32(Math.Round(viewModel.Performance * discount, 0, MidpointRounding.AwayFromZero));
            }

            var response = new DataSourceResponse<QueryViewModel> { TotalRowCount = total.Count };

            if (request.ServerPaging != null)
            {
                int skip = Math.Max(request.ServerPaging.PageSize * (request.ServerPaging.Page - 1), val2: 0);
                response.DataCollection = total.Values.OrderBy(v => new { v.Year, v.Month }).Skip(skip).Take(request.ServerPaging.PageSize).ToList();
            }

            return response;
        }

        public DataSourceResponse<QueryDetailViewModel> ReadQueryDetails(DataSourceRequest request, IIdentity identity)
        {
            string cmdText = @"
                SELECT c.[Name], ur.RoleId
                FROM Customer c JOIN AspNetUserRoles ur ON c.UserId = ur.UserId
                WHERE c.UserId = @UserId";

            var currentUserId = identity.GetUserId();
            var roles = this.userService.GetRoles();
            WebApplication.Models.Role.Role currentRole;
            string currentName = string.Empty;
            this.ReadDataSingle(dataReader =>
            {
                string currentRoleId = dataReader["RoleId"].ToString();
                currentRole = roles[currentRoleId];
                currentName = dataReader["Name"].ToString();
            }, cmdText, new SqlParameter("UserId", currentUserId));

            ServerFilterInfo yearFilterInfo = request.ServerFiltering.FilterCollection.Where(f => f.Field == "Year").SingleOrDefault();
            int year = Convert.ToInt32(yearFilterInfo.Value);
            ServerFilterInfo monthFilterInfo = request.ServerFiltering.FilterCollection.Where(f => f.Field == "Month").SingleOrDefault();
            int month = Convert.ToInt32(monthFilterInfo.Value);
            var shipments = this.DbContext.Shipments.Where(s => s.UserId == currentUserId && s.Date.Year == year && s.Date.Month == month).ToList();

            IDictionary<int, string> customers = new Dictionary<int, string>();
            IList<QueryDetailViewModel> responseData = new List<QueryDetailViewModel>();
            foreach (var shipment in shipments)
            {
                var viewModel = new QueryDetailViewModel
                {
                    PurchaseId = shipment.ShipmentId,
                    Year = year,
                    Month = month,
                    ShipmentDate = shipment.Date
                };

                if (shipment.CustomerId.HasValue)
                {
                    if (customers.ContainsKey(shipment.CustomerId.Value))
                        viewModel.CustomerName = customers[shipment.CustomerId.Value];
                    else
                    {
                        var customer = this.DbContext.Customers.Where(c => c.CustomerId == shipment.CustomerId.Value).SingleOrDefault();
                        if (customer != null)
                        {
                            customers.Add(shipment.CustomerId.Value, customer.Name);
                            viewModel.CustomerName = customer.Name;
                        }
                    }
                }
                else
                    viewModel.CustomerName = currentName;

                var shipmentDetails = this.DbContext.ShipmentDetails.Where(d => d.ShipmentId == shipment.ShipmentId).ToList();
                foreach (var shipmentDetail in shipmentDetails)
                    viewModel.TotalAmount += shipmentDetail.SubtotalAmount;

                responseData.Add(viewModel);
            }

            var response = new DataSourceResponse<QueryDetailViewModel> { TotalRowCount = responseData.Count };

            if (request.ServerPaging != null)
            {
                int skip = Math.Max(request.ServerPaging.PageSize * (request.ServerPaging.Page - 1), val2: 0);
                response.DataCollection = responseData.OrderBy(v => v.PurchaseId).Skip(skip).Take(request.ServerPaging.PageSize).ToList();
            }

            return response;
        }
    }
}
