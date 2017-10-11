using Shengtai;
using Shengtai.Web.Telerik.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shengtai.Web.Telerik;
using TsaiJing.WebApplication.Models;

namespace TsaiJing.Data.Service
{
    public class SpecificationService : SqlRepository<DefaultDbContext>, IApiService<SpecificationViewModel, int>, ISpecificationService
    {
        public SpecificationService() : base("DefaultConnection") { }

        public bool Create(SpecificationViewModel model, IDataSourceError error)
        {
            var specification = new ProductSpecification { Name = model.Text, ProductCategoryId = model.Parent };

            this.DbContext.ProductSpecifications.Add(specification);
            var result = false;
            try
            {
                this.DbContext.SaveChanges();
                model.Value = specification.ProductSpecificationId;
                result = true;
            }
            catch { }

            return result;
        }

        private ProductSpecification SingleOrDefault(int key)
        {
            return this.DbContext.ProductSpecifications.Where(s => s.ProductSpecificationId == key).SingleOrDefault();
        }

        public bool? Destroy(int key)
        {
            var specification = this.SingleOrDefault(key);
            if (specification == null)
                return null;

            this.DbContext.ProductSpecifications.Remove(specification);

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

        public DataSourceResponse<SpecificationViewModel> Read(DataSourceRequest request)
        {
            var responseData = this.DbContext.ProductSpecifications.Select(s => s);
            if (request.ServerFiltering != null)
            {
                ServerFilterInfo filterInfo = request.ServerFiltering.FilterCollection.Where(f => f.Field == "Parent").SingleOrDefault();
                int productCategoryId = Convert.ToInt32(filterInfo.Value);
                responseData = responseData.Where(s => s.ProductCategoryId == productCategoryId);
            }

            var response = new DataSourceResponse<SpecificationViewModel> { TotalRowCount = responseData.Count() };

            if (request.ServerPaging != null)
            {
                int skip = Math.Max(request.ServerPaging.PageSize * (request.ServerPaging.Page - 1), val2: 0);
                responseData = responseData.OrderBy(s => s.ProductSpecificationId).Skip(skip).Take(request.ServerPaging.PageSize);
            }

            response.DataCollection = responseData.Select(s => new SpecificationViewModel
            {
                Text = s.Name,
                Value = s.ProductSpecificationId,
                Parent = s.ProductCategoryId
            }).ToList();

            return response;
        }

        public bool? Update(int key, SpecificationViewModel model, IDataSourceError error)
        {
            var specification = this.SingleOrDefault(key);
            if (specification == null)
                return null;

            specification.Name = model.Text;
            specification.ProductCategoryId = model.Parent;

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
            var specifications = this.DbContext.ProductSpecifications.Where(s => s.ProductCategoryId == parentKey);
            this.DbContext.ProductSpecifications.RemoveRange(specifications);

            bool result = false;
            try
            {
                this.DbContext.SaveChanges();
                result = true;
            }
            catch { }

            return result;
        }

        public IEnumerable<SpecificationViewModel> ReadSpecifications(DataSourceRequest request)
        {
            var responseData = this.DbContext.ProductSpecifications.Select(s => s);
            if (request.ServerFiltering != null)
            {
                ServerFilterInfo filterInfo = request.ServerFiltering.FilterCollection.Where(f => f.Field == "Parent").SingleOrDefault();
                int productCategoryId = Convert.ToInt32(filterInfo.Value);
                responseData = responseData.Where(s => s.ProductCategoryId == productCategoryId);
            }

            var response = responseData.Select(s => new SpecificationViewModel
            {
                Text = s.Name,
                Value = s.ProductSpecificationId,
                Parent = s.ProductCategoryId
            });

            return response;
        }
    }
}
