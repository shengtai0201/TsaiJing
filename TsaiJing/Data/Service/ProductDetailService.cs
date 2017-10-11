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
using System.Data.SqlClient;

namespace TsaiJing.Data.Service
{
    public class ProductDetailService : SqlRepository<DefaultDbContext>, IApiService<ProductDetailViewModel, int>, IProductDetailService
    {
        public ProductDetailService() : base("DefaultConnection") { }

        public bool Create(ProductDetailViewModel model, IDataSourceError error)
        {
            var productDetail = new ProductDetail
            {
                ProductId = model.ProductId,
                FirstSpecificationId = model.FirstSpecification.Value,
                Price = model.Price,
                SafeStock = model.SafeStock
            };
            if (model.SecondSpecification != null && model.SecondSpecification.Value.HasValue)
                productDetail.SecondSpecificationId = model.SecondSpecification.Value.Value;

            this.DbContext.ProductDetails.Add(productDetail);
            var result = false;
            try
            {
                this.DbContext.SaveChanges();
                model.ProductDetailId = productDetail.ProductDetailId;
                result = true;
            }
            catch { }

            return result;
        }

        private ProductDetail SingleOrDefault(int key)
        {
            return this.DbContext.ProductDetails.Where(d => d.ProductDetailId == key).SingleOrDefault();
        }

        public bool? Destroy(int key)
        {
            var productDetail = this.SingleOrDefault(key);
            if (productDetail == null)
                return null;

            this.DbContext.ProductDetails.Remove(productDetail);
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
            var productDetails = this.DbContext.ProductDetails.Where(d => d.ProductId == parentKey);
            this.DbContext.ProductDetails.RemoveRange(productDetails);

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

        public DataSourceResponse<ProductDetailViewModel> Read(DataSourceRequest request)
        {
            var responseData = this.DbContext.ProductDetails.Select(d => d);
            if (request.ServerFiltering != null)
            {
                ServerFilterInfo filterInfo = request.ServerFiltering.FilterCollection.Where(f => f.Field == "ProductId").SingleOrDefault();
                int productId = Convert.ToInt32(filterInfo.Value);
                responseData = responseData.Where(d => d.ProductId == productId);
            }

            var response = new DataSourceResponse<ProductDetailViewModel> { TotalRowCount = responseData.Count() };

            if (request.ServerPaging != null)
            {
                int skip = Math.Max(request.ServerPaging.PageSize * (request.ServerPaging.Page - 1), val2: 0);
                responseData = responseData.OrderBy(d => d.ProductDetailId).Skip(skip).Take(request.ServerPaging.PageSize);
            }

            var collection = responseData.Select(d => new
            {
                ProductDetailId = d.ProductDetailId,
                ProductId = d.ProductId,
                Price = d.Price,
                SafeStock = d.SafeStock,
                FirstSpecificationProductCategoryName = d.FirstSpecification.ProductCategory.Name,
                FirstSpecificationProductCategoryProductCategoryId = d.FirstSpecification.ProductCategory.ProductCategoryId,
                FirstSpecificationName = d.FirstSpecification.Name,
                FirstSpecificationProductSpecificationId = d.FirstSpecification.ProductSpecificationId,
                FirstSpecificationProductCategoryId = d.FirstSpecification.ProductCategoryId,
                SecondSpecificationId = d.SecondSpecificationId,
                SecondSpecificationProductCategoryName = d.SecondSpecification.ProductCategory.Name,
                SecondSpecificationProductCategoryProductCategoryId = d.SecondSpecification.ProductCategory.ProductCategoryId,
                SecondSpecificationName = d.SecondSpecification.Name,
                SecondSpecificationProductSpecificationId = d.SecondSpecification.ProductSpecificationId,
                SecondSpecificationProductCategoryId = d.SecondSpecification.ProductCategoryId
            }).ToList();
            foreach (var data in collection)
            {
                var viewModel = new ProductDetailViewModel
                {
                    ProductDetailId = data.ProductDetailId,
                    ProductId = data.ProductId,
                    Price = data.Price,
                    SafeStock = data.SafeStock
                };

                var firstCategory = new SelectListItem
                {
                    Text = data.FirstSpecificationProductCategoryName,
                    Value = data.FirstSpecificationProductCategoryProductCategoryId.ToString()
                };
                viewModel.FirstCategory = firstCategory;

                var firstSpecification = new FirstSpecificationViewModel
                {
                    Text = data.FirstSpecificationName,
                    Value = data.FirstSpecificationProductSpecificationId,
                    Parent = data.FirstSpecificationProductCategoryId
                };
                viewModel.FirstSpecification = firstSpecification;

                if (data.SecondSpecificationId.HasValue)
                {
                    var secondCategory = new SelectListItem
                    {
                        Text = data.SecondSpecificationProductCategoryName,
                        Value = data.SecondSpecificationProductCategoryProductCategoryId.ToString()
                    };
                    viewModel.SecondCategory = secondCategory;

                    var secondSpecification = new SpecificationViewModel
                    {
                        Text = data.SecondSpecificationName,
                        Value = data.SecondSpecificationProductSpecificationId,
                        Parent = data.SecondSpecificationProductCategoryId
                    };
                    viewModel.SecondSpecification = secondSpecification;
                }

                response.DataCollection.Add(viewModel);
            }

            return response;
        }

        public bool? Update(int key, ProductDetailViewModel model, IDataSourceError error)
        {
            var productDetail = this.SingleOrDefault(key);
            if (productDetail == null)
                return null;

            productDetail.FirstSpecificationId = model.FirstSpecification.Value;

            if (model.SecondSpecification != null && model.SecondSpecification.Value.HasValue)
                productDetail.SecondSpecificationId = model.SecondSpecification.Value.Value;

            productDetail.Price = model.Price;
            productDetail.SafeStock = model.SafeStock;

            bool result = false;
            try
            {
                this.DbContext.SaveChanges();
                result = true;
            }
            catch { }

            return result;
        }

        public IEnumerable<TextValueViewModel<int>> ReadProductDetails(DataSourceRequest request)
        {
            IList<TextValueViewModel<int>> result = new List<TextValueViewModel<int>>();
            ServerFilterInfo filterInfo = request.ServerFiltering.FilterCollection.Where(f => f.Field == "ProductId").SingleOrDefault();
            if (string.IsNullOrEmpty(filterInfo.Value))
                return result;

            int productId = Convert.ToInt32(filterInfo.Value);

            string cmdText = @"
                SELECT d.ProductDetailId [Value],
                       d.ProductId Parent,
                       s1.[Name]+' '+s2.[Name] [Text]
                FROM ProductDetail d
                     LEFT JOIN ProductSpecification s1 ON d.FirstSpecificationId = s1.ProductSpecificationId
                     LEFT JOIN ProductSpecification s2 ON d.SecondSpecificationId = s2.ProductSpecificationId
                WHERE d.ProductId = @ProductId";

            this.ReadData(dataReader =>
            {
                result.Add(new TextValueViewModel<int>
                {
                    Text = dataReader["Text"].ToString(),
                    Value = Convert.ToInt32(dataReader["Value"]),
                    Parent = Convert.ToInt32(dataReader["Parent"])
                });
            }, cmdText, new SqlParameter("ProductId", productId));

            return result;
        }
    }
}
