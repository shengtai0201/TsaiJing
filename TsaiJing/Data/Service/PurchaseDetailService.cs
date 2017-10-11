using Shengtai;
using Shengtai.Web.Telerik.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TsaiJing.WebApplication.Models;
using Shengtai.Web.Telerik;
using System.Web.Mvc;

namespace TsaiJing.Data.Service
{
    public class PurchaseDetailService : SqlRepository<DefaultDbContext>, IApiService<PurchaseDetailViewModel, int>, IPurchaseDetailService
    {
        public PurchaseDetailService() : base("DefaultConnection") { }

        public bool Create(PurchaseDetailViewModel model, IDataSourceError error)
        {
            var purchaseDetail = new PurchaseDetail
            {
                PurchaseId = model.PurchaseId,
                Price = model.Price,
                Inventory = model.Inventory
            };
            if (model.Product != null && !string.IsNullOrEmpty(model.Product.Value))
                purchaseDetail.ProductId = Convert.ToInt32(model.Product.Value);
            if (model.ProductDetail != null && !string.IsNullOrEmpty(model.ProductDetail.Value))
                purchaseDetail.ProductDetailId = Convert.ToInt32(model.ProductDetail.Value);

            this.DbContext.PurchaseDetails.Add(purchaseDetail);
            var result = false;
            try
            {
                this.DbContext.SaveChanges();
                model.PurchaseDetailId = purchaseDetail.PurchaseDetailId;
                model.SubtotalAmount = model.Price * model.Inventory;
                result = true;
            }
            catch { }

            return result;
        }

        private PurchaseDetail SingleOrDefault(int key)
        {
            return this.DbContext.PurchaseDetails.Where(d => d.PurchaseDetailId == key).SingleOrDefault();
        }

        public bool? Destroy(int key)
        {
            var purchaseDetail = this.SingleOrDefault(key);
            if (purchaseDetail == null)
                return null;

            this.DbContext.PurchaseDetails.Remove(purchaseDetail);
            bool result = false;
            try
            {
                this.DbContext.SaveChanges();
                result = true;
            }
            catch { }

            return result;
        }

        public bool DestroyByParent(int parentKey)
        {
            var purchaseDetails = this.DbContext.PurchaseDetails.Where(d => d.PurchaseId == parentKey);
            this.DbContext.PurchaseDetails.RemoveRange(purchaseDetails);

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

        public int GetTotalAmount(int purchaseId)
        {
            int totalAmount = 0;

            var purchaseDetails = this.DbContext.PurchaseDetails.Where(d => d.PurchaseId == purchaseId).ToList();
            foreach (var purchaseDetail in purchaseDetails)
                totalAmount += purchaseDetail.Price * purchaseDetail.Inventory;

            return totalAmount;
        }

        public DataSourceResponse<PurchaseDetailViewModel> Read(DataSourceRequest request)
        {
            var responseData = this.DbContext.PurchaseDetails.Select(d => d);
            if (request.ServerFiltering != null)
            {
                ServerFilterInfo filterInfo = request.ServerFiltering.FilterCollection.Where(f => f.Field == "PurchaseId").SingleOrDefault();
                int purchaseId = Convert.ToInt32(filterInfo.Value);
                responseData = responseData.Where(d => d.PurchaseId == purchaseId);
            }

            var response = new DataSourceResponse<PurchaseDetailViewModel> { TotalRowCount = responseData.Count() };

            if (request.ServerPaging != null)
            {
                int skip = Math.Max(request.ServerPaging.PageSize * (request.ServerPaging.Page - 1), val2: 0);
                responseData = responseData.OrderBy(d => d.PurchaseDetailId).Skip(skip).Take(request.ServerPaging.PageSize);
            }

            var collection = responseData.Select(d => new
            {
                PurchaseDetailId = d.PurchaseDetailId,
                PurchaseId = d.PurchaseId,
                ProductId = d.ProductId,
                Price = d.Price,
                Inventory = d.Inventory,
                SubtotalAmount = d.Price * d.Inventory
            }).ToList();
            foreach (var data in collection)
            {
                var viewModel = new PurchaseDetailViewModel
                {
                    PurchaseDetailId = data.PurchaseDetailId,
                    PurchaseId = data.PurchaseId,
                    Product = new SelectListItem(),
                    ProductDetail = new SelectListItem(),
                    Price = data.Price,
                    Inventory = data.Inventory,
                    SubtotalAmount = data.SubtotalAmount
                };

                var product =
                    (from d in this.DbContext.PurchaseDetails
                     join p in this.DbContext.Products on d.ProductId equals p.ProductId
                     where d.PurchaseDetailId == data.PurchaseDetailId
                     select new { p.ProductId, p.Name }).SingleOrDefault();
                if (product != null)
                {
                    viewModel.Product.Text = product.Name;
                    viewModel.Product.Value = product.ProductId.ToString();
                }

                var productDetail =
                    (from d in this.DbContext.PurchaseDetails
                     join pd in this.DbContext.ProductDetails on d.ProductDetailId equals pd.ProductDetailId
                     join ps1 in this.DbContext.ProductSpecifications on pd.FirstSpecificationId equals ps1.ProductSpecificationId
                     join ps2 in this.DbContext.ProductSpecifications on pd.SecondSpecificationId equals ps2.ProductSpecificationId into ps2Group
                     where d.PurchaseDetailId == data.PurchaseDetailId
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

        public bool? Update(int key, PurchaseDetailViewModel model, IDataSourceError error)
        {
            var purchaseDetail = this.SingleOrDefault(key);
            if (purchaseDetail == null)
                return null;

            if (model.Product != null && !string.IsNullOrEmpty(model.Product.Value))
                purchaseDetail.ProductId = Convert.ToInt32(model.Product.Value);
            if (model.ProductDetail != null && !string.IsNullOrEmpty(model.ProductDetail.Value))
                purchaseDetail.ProductDetailId = Convert.ToInt32(model.ProductDetail.Value);

            purchaseDetail.Price = model.Price;
            purchaseDetail.Inventory = model.Inventory;

            bool result = false;
            try
            {
                this.DbContext.SaveChanges();
                model.SubtotalAmount = model.Price * model.Inventory;
                result = true;
            }
            catch { }

            return result;
        }
    }
}
