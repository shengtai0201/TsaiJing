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

namespace TsaiJing.Data.Service
{
    public class ProductService : SqlRepository<DefaultDbContext>, IApiService<ProductViewModel, int>, IProductService
    {
        private IProductDetailService productDetailService;
        public ProductService(IProductDetailService productDetailService) : base("DefaultConnection")
        {
            this.productDetailService = productDetailService;
        }

        public bool Create(ProductViewModel model, IDataSourceError error)
        {
            var product = new Product
            {
                Name = model.Name,
                Price = model.Price,
                SafeStock = model.SafeStock
            };

            this.DbContext.Products.Add(product);
            var result = false;
            try
            {
                this.DbContext.SaveChanges();
                model.ProductId = product.ProductId;
                result = true;
            }
            catch { }

            return result;
        }

        private Product SingleOrDefault(int key)
        {
            return this.DbContext.Products.Where(p => p.ProductId == key).SingleOrDefault();
        }

        public bool? Destroy(int key)
        {
            var product = this.SingleOrDefault(key);
            if (product == null)
                return null;

            TransactionScope transaction = new TransactionScope();
            bool result = this.productDetailService.DestroyByParent(key);
            if (result)
            {
                this.DbContext.Products.Remove(product);

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

        public DataSourceResponse<ProductViewModel> Read(DataSourceRequest request)
        {
            var responseData = this.DbContext.Products.Select(p => p);
            if (request.ServerFiltering != null)
            {

            }

            var response = new DataSourceResponse<ProductViewModel>
            {
                TotalRowCount = responseData.Count()
            };

            if (request.ServerPaging != null)
            {
                int skip = Math.Max(request.ServerPaging.PageSize * (request.ServerPaging.Page - 1), val2: 0);
                responseData = responseData.OrderBy(p => p.ProductId).Skip(skip).Take(request.ServerPaging.PageSize);
            }

            response.DataCollection = responseData.Select(p => new ProductViewModel
            {
                ProductId = p.ProductId,
                Name = p.Name,
                Price = p.Price,
                SafeStock = p.SafeStock
            }).ToList();

            return response;
        }

        public bool? Update(int key, ProductViewModel model, IDataSourceError error)
        {
            var product = this.SingleOrDefault(key);
            if (product == null)
                return null;

            product.Name = model.Name;
            product.Price = model.Price;
            product.SafeStock = model.SafeStock;

            bool result = false;
            try
            {
                this.DbContext.SaveChanges();
                result = true;
            }
            catch { }

            return result;
        }

        public IEnumerable<SelectListItem> ReadProducts()
        {
            var result = this.DbContext.Products.Select(p => new SelectListItem
            {
                Text = p.Name,
                Value = p.ProductId.ToString()
            });

            return result;
        }
    }
}
