using Shengtai;
using Shengtai.Web.Telerik.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TsaiJing.WebApplication.Models;
using Shengtai.Web.Telerik;
using System.Transactions;
using System.Web.Mvc;
using System.Data.SqlClient;

namespace TsaiJing.Data.Service
{
    public class ShipmentDetailService : SqlRepository<DefaultDbContext>, IApiService<ShipmentDetailViewModel, int>, IShipmentDetailService
    {
        private IUserService userService;
        public ShipmentDetailService(IUserService userService) : base("DefaultConnection")
        {
            this.userService = userService;
        }

        //public IShipmentService ShipmentService { get; set; }

        public bool Create(ShipmentDetailViewModel model, IDataSourceError error)
        {
            var shipmentDetail = new ShipmentDetail
            {
                ShipmentId = model.ShipmentId,
                ProductId = Convert.ToInt32(model.Product.Value),
                Quantity = model.Quantity
            };
            if (model.ProductDetail != null && !string.IsNullOrEmpty(model.ProductDetail.Value))
                shipmentDetail.ProductDetailId = Convert.ToInt32(model.ProductDetail.Value);

            // 存量檢查
            int quantity = 0, inventory = 0, safeStock = 0;     // 已出貨量, 已進貨量, 安全存量
            if (model.ProductDetail != null && !string.IsNullOrEmpty(model.ProductDetail.Value))
            {
                int productDetailId = Convert.ToInt32(model.ProductDetail.Value);

                // 已出貨量
                foreach (var sd in this.DbContext.ShipmentDetails.Where(d => d.ProductDetailId == productDetailId))
                    quantity += sd.Quantity;

                // 已進貨量
                foreach (var pd in this.DbContext.PurchaseDetails.Where(d => d.ProductDetailId == productDetailId))
                    inventory += pd.Inventory;

                // 安全存量
                var productDetail = this.DbContext.ProductDetails.Where(d => d.ProductDetailId == productDetailId).SingleOrDefault();
                safeStock = productDetail.SafeStock;

                shipmentDetail.SubtotalAmount = model.Quantity * productDetail.Price;
            }
            else if (model.Product != null && !string.IsNullOrEmpty(model.Product.Value))
            {
                int productId = Convert.ToInt32(model.Product.Value);

                // 已出貨量
                foreach (var sd in this.DbContext.ShipmentDetails.Where(d => d.ProductId == productId))
                    quantity += sd.Quantity;

                // 已進貨量
                foreach (var pd in this.DbContext.PurchaseDetails.Where(d => d.ProductId == productId))
                    inventory += pd.Inventory;

                // 安全存量
                var product = this.DbContext.Products.Where(p => p.ProductId == productId).SingleOrDefault();
                safeStock = product.SafeStock.Value;

                shipmentDetail.SubtotalAmount = model.Quantity * product.Price.Value;
            }

            //TransactionScope transaction = new TransactionScope();
            this.DbContext.ShipmentDetails.Add(shipmentDetail);
            var result = false;
            if (inventory >= (quantity + model.Quantity))
            {
                if ((inventory - (quantity + model.Quantity)) < safeStock)
                    error.Error.AppendLine("存貨數量低於安全數量，但可出貨。");

                try
                {
                    this.DbContext.SaveChanges();
                    model.ShipmentDetailId = shipmentDetail.ShipmentDetailId;
                    model.SubtotalAmount = shipmentDetail.SubtotalAmount;
                    result = true;
                }
                catch { }

                //if (result)
                //    result = this.ShipmentService.RefreshShipment(shipmentDetail.ShipmentId) ?? false;
            }
            else
                error.Error.AppendLine("出貨超過存貨數量，無法出貨。");

            //if (result)
            //    transaction.Complete();

            //transaction.Dispose();
            return result;
        }

        private ShipmentDetail SingleOrDefault(int key)
        {
            return this.DbContext.ShipmentDetails.Where(d => d.ShipmentDetailId == key).SingleOrDefault();
        }

        public bool? Destroy(int key)
        {
            var shipmentDetail = this.SingleOrDefault(key);
            if (shipmentDetail == null)
                return null;

            //TransactionScope transaction = new TransactionScope();
            this.DbContext.ShipmentDetails.Remove(shipmentDetail);
            bool result = false;
            try
            {
                this.DbContext.SaveChanges();
                result = true;
                //result = this.ShipmentService.RefreshShipment(shipmentDetail.ShipmentId) ?? false;
            }
            catch { }

            //if (result)
            //    transaction.Complete();

            //transaction.Dispose();
            return result;
        }

        public bool DestroyByParent(int parentKey)
        {
            var shipmentDetails = this.DbContext.ShipmentDetails.Where(d => d.ShipmentId == parentKey);
            this.DbContext.ShipmentDetails.RemoveRange(shipmentDetails);

            bool result = false;
            try
            {
                this.DbContext.SaveChanges();
                result = true;
            }
            catch { }

            return result;
        }

        public void Dispose() { }

        public DataSourceResponse<ShipmentDetailViewModel> Read(DataSourceRequest request)
        {
            var responseData = this.DbContext.ShipmentDetails.Select(d => d);
            if (request.ServerFiltering != null)
            {
                ServerFilterInfo filterInfo = request.ServerFiltering.FilterCollection.Where(f => f.Field == "ShipmentId").SingleOrDefault();
                int shipmentId = Convert.ToInt32(filterInfo.Value);
                responseData = responseData.Where(d => d.ShipmentId == shipmentId);
            }

            var response = new DataSourceResponse<ShipmentDetailViewModel> { TotalRowCount = responseData.Count() };

            if (request.ServerPaging != null)
            {
                int skip = Math.Max(request.ServerPaging.PageSize * (request.ServerPaging.Page - 1), val2: 0);
                responseData = responseData.OrderBy(d => d.ShipmentDetailId).Skip(skip).Take(request.ServerPaging.PageSize);
            }

            var collection = responseData.Select(d => new
            {
                ShipmentDetailId = d.ShipmentDetailId,
                ShipmentId = d.ShipmentId,
                Quantity = d.Quantity,
                SubtotalAmount = d.SubtotalAmount
            }).ToList();
            foreach (var data in collection)
            {
                var viewModel = new ShipmentDetailViewModel
                {
                    ShipmentDetailId = data.ShipmentDetailId,
                    ShipmentId = data.ShipmentId,
                    Product = new SelectListItem(),
                    ProductDetail = new SelectListItem(),
                    Quantity = data.Quantity,
                    SubtotalAmount = data.SubtotalAmount
                };

                var product =
                    (from d in this.DbContext.ShipmentDetails
                     join p in this.DbContext.Products on d.ProductId equals p.ProductId
                     where d.ShipmentDetailId == data.ShipmentDetailId
                     select new { p.ProductId, p.Name }).SingleOrDefault();
                if (product != null)
                {
                    viewModel.Product.Text = product.Name;
                    viewModel.Product.Value = product.ProductId.ToString();
                }

                var productDetail =
                    (from d in this.DbContext.ShipmentDetails
                     join pd in this.DbContext.ProductDetails on d.ProductDetailId equals pd.ProductDetailId
                     join ps1 in this.DbContext.ProductSpecifications on pd.FirstSpecificationId equals ps1.ProductSpecificationId
                     join ps2 in this.DbContext.ProductSpecifications on pd.SecondSpecificationId equals ps2.ProductSpecificationId into ps2Group
                     where d.ShipmentDetailId == data.ShipmentDetailId
                     from ps2New in ps2Group.DefaultIfEmpty()
                     select new
                     {
                         Name = ps1.Name + (ps2New == null ? "" : " " + ps2New.Name),
                         ProductDetailId = pd.ProductDetailId
                     }).SingleOrDefault();
                if (productDetail != null)
                {
                    viewModel.ProductDetail.Text = productDetail.Name;
                    viewModel.ProductDetail.Value = productDetail.ProductDetailId.ToString();
                }

                response.DataCollection.Add(viewModel);
            }

            return response;
        }

        //public void RefreshShipment(Shipment shipment)
        //{
        //    // 總計金額
        //    int totalAmount = 0;
        //    var shipmentDetails = this.DbContext.ShipmentDetails.Where(d => d.ShipmentId == shipment.ShipmentId).ToList();
        //    foreach (var shipmentDetail in shipmentDetails)
        //        totalAmount += shipmentDetail.SubtotalAmount;

        //    // 角色
        //    var roles = this.userService.GetRoles();
        //    WebApplication.Models.Role.Role role = WebApplication.Models.Role.Customer.GetInstance();
        //    if (shipment.CustomerId.HasValue)
        //    {
        //        var customer = this.DbContext.Customers.Where(c => c.CustomerId == shipment.CustomerId.Value).SingleOrDefault();
        //        if (customer != null && !string.IsNullOrEmpty(customer.MemberRoleId))
        //            role = roles[customer.MemberRoleId];
        //    }
        //    else
        //    {
        //        string cmdText = @"select ur.RoleId from AspNetUserRoles ur where ur.UserId = @UserId";
        //        string roleId = this.ExecuteScalar(cmdText, new SqlParameter("UserId", shipment.UserId)).ToString();
        //        role = roles[roleId];
        //    }

        //    // 消費回饋
        //    shipment.ConsumptionRebate = Convert.ToInt32(Math.Round(totalAmount * role.GetDiscount(), 0, MidpointRounding.AwayFromZero));
        //}

        public bool? Update(int key, ShipmentDetailViewModel model, IDataSourceError error)
        {
            var shipmentDetail = this.SingleOrDefault(key);
            if (shipmentDetail == null)
                return null;

            int? price = null;
            if (model.Product != null && !string.IsNullOrEmpty(model.Product.Value))
            {
                shipmentDetail.ProductId = Convert.ToInt32(model.Product.Value);

                var product = this.DbContext.Products.Where(p => p.ProductId == shipmentDetail.ProductId).SingleOrDefault();
                if (product != null)
                    price = product.Price;
            }
            if (model.ProductDetail != null && !string.IsNullOrEmpty(model.ProductDetail.Value))
            {
                shipmentDetail.ProductDetailId = Convert.ToInt32(model.ProductDetail.Value);

                var productDetail = this.DbContext.ProductDetails.Where(d => d.ProductDetailId == shipmentDetail.ProductDetailId).SingleOrDefault();
                if (productDetail != null)
                    price = productDetail.Price;
            }

            if (!price.HasValue)
                return null;

            shipmentDetail.Quantity = model.Quantity;
            shipmentDetail.SubtotalAmount = price.Value * model.Quantity;

            bool result = false;
            try
            {
                this.DbContext.SaveChanges();
                model.SubtotalAmount = shipmentDetail.SubtotalAmount;
                result = true;
            }
            catch { }

            return result;
        }
    }
}
