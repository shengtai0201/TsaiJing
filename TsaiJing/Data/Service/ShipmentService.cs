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
using System.Transactions;
using System.Web.Mvc;
using System.Data.SqlClient;

namespace TsaiJing.Data.Service
{
    public class ShipmentService : SqlRepository<DefaultDbContext>, IApiService<ShipmentViewModel, int>//, IShipmentService
    {
        private string currentUserId = string.Empty;
        private string CurrentUserId
        {
            get
            {
                if (string.IsNullOrEmpty(this.currentUserId))
                    this.currentUserId = this.CurrentUser.Identity.GetUserId();

                return this.currentUserId;
            }
        }

        private IShipmentDetailService shipmentDetailService;
        private IUserService userService;
        public ShipmentService(IShipmentDetailService shipmentDetailService, IUserService userService) : base("DefaultConnection")
        {
            //shipmentDetailService.ShipmentService = this;
            this.shipmentDetailService = shipmentDetailService;
            this.userService = userService;
        }

        public bool Create(ShipmentViewModel model, IDataSourceError error)
        {
            if (this.CurrentUser.IsInRole("Company"))
            {
                error.Error.AppendLine("公司端不可新增");
                return false;
            }

            var shipment = new Shipment
            {
                UserId = this.CurrentUserId,
                UserRoleId = this.userService.GetRoleByUser(this.CurrentUserId),
                Date = model.Date
            };
            if (model.Customer != null && !string.IsNullOrEmpty(model.Customer.Value))
            {
                shipment.CustomerId = Convert.ToInt32(model.Customer.Value);
                shipment.CustomerRoleId = this.userService.GetRoleByCustomer(shipment.CustomerId.Value);
            }

            this.DbContext.Shipments.Add(shipment);
            var result = false;
            try
            {
                this.DbContext.SaveChanges();
                model.ShipmentId = shipment.ShipmentId;
                result = true;
            }
            catch { }

            return result;
        }

        private Shipment SingleOrDefault(int key)
        {
            return this.DbContext.Shipments.Where(s => s.ShipmentId == key).SingleOrDefault();
        }

        public bool? Destroy(int key)
        {
            if (this.CurrentUser.IsInRole("Company"))
                return false;

            var shipment = this.SingleOrDefault(key);
            if (shipment == null)
                return null;

            TransactionScope transaction = new TransactionScope();
            bool result = this.shipmentDetailService.DestroyByParent(key);
            if (result)
            {
                this.DbContext.Shipments.Remove(shipment);

                try
                {
                    this.DbContext.SaveChanges();
                    result = true;
                }
                catch { }
            }

            if (result)
                transaction.Complete();

            transaction.Dispose();
            return result;
        }

        public void Dispose() { }

        public DataSourceResponse<ShipmentViewModel> Read(DataSourceRequest request)
        {
            IQueryable<Shipment> responseData;
            if (this.CurrentUser.IsInRole("Company"))
            {
                // 公司端
                responseData = this.DbContext.Shipments.Select(s => s);
            }
            else
            {
                // 督導、技術指導
                responseData = this.DbContext.Shipments.Where(s => s.UserId == this.CurrentUserId);
            }
            var response = new DataSourceResponse<ShipmentViewModel> { TotalRowCount = responseData.Count() };

            if (request.ServerPaging != null)
            {
                int skip = Math.Max(request.ServerPaging.PageSize * (request.ServerPaging.Page - 1), val2: 0);
                responseData = responseData.OrderByDescending(s => s.ShipmentId).Skip(skip).Take(request.ServerPaging.PageSize);
            }

            var roles = this.userService.GetRoles();
            var collection = responseData.ToList();
            foreach (var data in collection)
            {
                var shipment = new ShipmentViewModel
                {
                    ShipmentId = data.ShipmentId,
                    Date = data.Date,
                    TotalAmount = 0
                };

                // 計算
                var currentRole = roles[data.UserRoleId];
                var discount = currentRole.GetDiscount();
                if (data.CustomerId.HasValue)
                {
                    var customer = this.DbContext.Customers.Where(c => c.CustomerId == data.CustomerId).SingleOrDefault();
                    if (customer != null)
                        shipment.Customer = new SelectListItem { Text = customer.Name, Value = customer.CustomerId.ToString() };

                    var customerRole = roles[data.CustomerRoleId];
                    double customerDiscount = customerRole.GetDiscount();
                    discount -= customerDiscount;
                }

                // 總計金額
                var shipmentDetails = this.DbContext.ShipmentDetails.Where(d => d.ShipmentId == data.ShipmentId).ToList();
                foreach (var shipmentDetail in shipmentDetails)
                    shipment.TotalAmount += shipmentDetail.SubtotalAmount;

                // 消費回饋
                shipment.ConsumptionRebate = Convert.ToInt32(Math.Round(shipment.TotalAmount * discount, 0, MidpointRounding.AwayFromZero));

                response.DataCollection.Add(shipment);
            }

            return response;
        }

        public bool? Update(int key, ShipmentViewModel model, IDataSourceError error)
        {
            var shipment = this.SingleOrDefault(key);
            if (shipment == null)
                return null;

            if (model.Customer != null && !string.IsNullOrEmpty(model.Customer.Value))
            {
                shipment.CustomerId = Convert.ToInt32(model.Customer.Value);
                shipment.CustomerRoleId = this.userService.GetRoleByCustomer(shipment.CustomerId.Value);
            }
            else
                shipment.CustomerId = null;

            shipment.Date = model.Date;

            //this.shipmentDetailService.RefreshShipment(shipment);
            bool result = false;
            try
            {
                this.DbContext.SaveChanges();
                result = true;
            }
            catch { }

            return result;
        }

        //public bool? RefreshShipment(int shipmentId)
        //{
        //    var shipment = this.SingleOrDefault(shipmentId);
        //    if (shipment == null)
        //        return null;

        //    this.shipmentDetailService.RefreshShipment(shipment);
        //    bool result = false;
        //    try
        //    {
        //        this.DbContext.SaveChanges();
        //        result = true;
        //    }
        //    catch { }

        //    return result;
        //}
    }
}
