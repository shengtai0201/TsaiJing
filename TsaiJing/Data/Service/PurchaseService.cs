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
using Microsoft.AspNet.Identity;
using System.Transactions;

namespace TsaiJing.Data.Service
{
    public class PurchaseService : SqlRepository<DefaultDbContext>, IApiService<PurchaseViewModel, int>
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

        private IPurchaseDetailService purchaseDetailService;
        public PurchaseService(IPurchaseDetailService purchaseDetailService) : base("DefaultConnection")
        {
            this.purchaseDetailService = purchaseDetailService;
        }

        public bool Create(PurchaseViewModel model, IDataSourceError error)
        {
            var purchase = new Purchase
            {
                ManufacturerId = Convert.ToInt32(model.Manufacturer.Value),
                UserId = this.CurrentUserId,
                Date = model.Date,
                Remark = model.Remark
            };

            this.DbContext.Purchases.Add(purchase);
            var result = false;
            try
            {
                this.DbContext.SaveChanges();
                model.PurchaseId = purchase.PurchaseId;
                model.TotalAmount = 0;
                result = true;
            }
            catch { }

            return result;
        }

        private Purchase SingleOrDefault(int key)
        {
            return this.DbContext.Purchases.Where(p => p.PurchaseId == key).SingleOrDefault();
        }

        public bool? Destroy(int key)
        {
            var purchase = this.SingleOrDefault(key);
            if (purchase == null)
                return null;

            TransactionScope transaction = new TransactionScope();
            bool result = this.purchaseDetailService.DestroyByParent(key);
            if (result)
            {
                this.DbContext.Purchases.Remove(purchase);

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

        public DataSourceResponse<PurchaseViewModel> Read(DataSourceRequest request)
        {
            var responseData = this.DbContext.Purchases.Select(p => p);
            var response = new DataSourceResponse<PurchaseViewModel> { TotalRowCount = responseData.Count() };

            if (request.ServerPaging != null)
            {
                int skip = Math.Max(request.ServerPaging.PageSize * (request.ServerPaging.Page - 1), val2: 0);
                responseData = responseData.OrderByDescending(p => p.PurchaseId).Skip(skip).Take(request.ServerPaging.PageSize);
            }

            var collection =
                (from p in responseData
                 join m in this.DbContext.Manufacturers on p.ManufacturerId equals m.ManufacturerId
                 select new
                 {
                     PurchaseId = p.PurchaseId,
                     ManufacturerName = m.Name,
                     ManufacturerId = p.ManufacturerId,
                     Date = p.Date,
                     Remark = p.Remark
                 }).ToList();
            foreach (var data in collection)
            {
                var purchase = new PurchaseViewModel
                {
                    PurchaseId = data.PurchaseId,
                    Manufacturer = new SelectListItem { Text = data.ManufacturerName, Value = data.ManufacturerId.ToString() },
                    Date = data.Date,
                    TotalAmount = this.purchaseDetailService.GetTotalAmount(data.PurchaseId),
                    Remark = data.Remark
                };

                response.DataCollection.Add(purchase);
            }

            return response;
        }

        public bool? Update(int key, PurchaseViewModel model, IDataSourceError error)
        {
            var purchase = this.SingleOrDefault(key);
            if (purchase == null)
                return null;

            purchase.ManufacturerId = Convert.ToInt32(model.Manufacturer.Value);
            purchase.Date = model.Date;
            purchase.Remark = model.Remark;

            bool result = false;
            try
            {
                this.DbContext.SaveChanges();
                result = true;
            }
            catch { }

            return result;
        }
    }
}
