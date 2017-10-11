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

namespace TsaiJing.Data.Service
{
    public class CategoryService : SqlRepository<DefaultDbContext>, IApiService<CategoryViewModel, int>, ICategoryService
    {
        private ISpecificationService specificationService;
        public CategoryService(ISpecificationService specificationService) : base("DefaultConnection")
        {
            this.specificationService = specificationService;
        }

        public bool Create(CategoryViewModel model, IDataSourceError error)
        {
            var category = new ProductCategory { Name = model.Text };

            this.DbContext.ProductCategories.Add(category);
            var result = false;
            try
            {
                this.DbContext.SaveChanges();
                model.Value = category.ProductCategoryId;
                result = true;
            }
            catch { }

            return result;
        }

        private ProductCategory SingleOrDefault(int key)
        {
            return this.DbContext.ProductCategories.Where(c => c.ProductCategoryId == key).SingleOrDefault();
        }

        public bool? Destroy(int key)
        {
            var category = this.SingleOrDefault(key);
            if (category == null)
                return null;

            TransactionScope transaction = new TransactionScope();
            bool result = this.specificationService.DestroyByParent(key);
            if (result)
            {
                this.DbContext.ProductCategories.Remove(category);

                try
                {
                    this.DbContext.SaveChanges();
                    transaction.Complete();
                    result = true;
                }
                catch { }
            }

            transaction.Dispose();
            return result;
        }

        public void Dispose() { }

        public DataSourceResponse<CategoryViewModel> Read(DataSourceRequest request)
        {
            var responseData = this.DbContext.ProductCategories.Select(c => c);
            if (request.ServerFiltering != null)
            {

            }

            var response = new DataSourceResponse<CategoryViewModel>
            {
                TotalRowCount = responseData.Count()
            };

            if (request.ServerPaging != null)
            {
                int skip = Math.Max(request.ServerPaging.PageSize * (request.ServerPaging.Page - 1), val2: 0);
                responseData = responseData.OrderBy(c => c.ProductCategoryId).Skip(skip).Take(request.ServerPaging.PageSize);
            }

            response.DataCollection = responseData.Select(s => new CategoryViewModel
            {
                Text = s.Name,
                Value = s.ProductCategoryId
            }).ToList();

            return response;
        }

        public bool? Update(int key, CategoryViewModel model, IDataSourceError error)
        {
            var category = this.SingleOrDefault(key);
            if (category == null)
                return null;

            category.Name = model.Text;

            bool result = false;
            try
            {
                this.DbContext.SaveChanges();
                result = true;
            }
            catch { }

            return result;
        }

        public IList<CategoryViewModel> ReadCategories()
        {
            IList<CategoryViewModel> result = this.DbContext.ProductCategories.Select(c => new CategoryViewModel
            {
                Text = c.Name,
                Value = c.ProductCategoryId
            }).ToList();

            return result;
        }
    }
}
