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
    public class ManufacturerService : SqlRepository<DefaultDbContext>, IApiService<ManufacturerViewModel, int>, IManufacturerService
    {
        public ManufacturerService() : base("DefaultConnection") { }

        public bool Create(ManufacturerViewModel model, IDataSourceError error)
        {
            var manufacturer = new Manufacturer
            {
                Name = model.Name,
                Address = model.Address,
                ContactPerson = model.ContactPerson,
                ContactPersonPhone = model.ContactPersonPhone,
                Phone = model.Phone,
                Fax = model.Fax
            };

            this.DbContext.Manufacturers.Add(manufacturer);
            var result = false;
            try
            {
                this.DbContext.SaveChanges();
                model.ManufacturerId = manufacturer.ManufacturerId;
                result = true;
            }
            catch { }

            return result;
        }

        private Manufacturer SingleOrDefault(int key)
        {
            return this.DbContext.Manufacturers.Where(m => m.ManufacturerId == key).SingleOrDefault();
        }

        public bool? Destroy(int key)
        {
            var manufacturer = this.SingleOrDefault(key);
            if (manufacturer == null)
                return null;

            this.DbContext.Manufacturers.Remove(manufacturer);

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

        public DataSourceResponse<ManufacturerViewModel> Read(DataSourceRequest request)
        {
            var responseData = this.DbContext.Manufacturers.Select(m => m);
            if (request.ServerFiltering != null)
            {

            }

            var response = new DataSourceResponse<ManufacturerViewModel>
            {
                TotalRowCount = responseData.Count()
            };

            if (request.ServerPaging != null)
            {
                int skip = Math.Max(request.ServerPaging.PageSize * (request.ServerPaging.Page - 1), val2: 0);
                responseData = responseData.OrderBy(m => m.ManufacturerId).Skip(skip).Take(request.ServerPaging.PageSize);
            }

            response.DataCollection = responseData.Select(m => new ManufacturerViewModel
            {
                ManufacturerId = m.ManufacturerId,
                Name = m.Name,
                Address = m.Address,
                ContactPerson = m.ContactPerson,
                ContactPersonPhone = m.ContactPersonPhone,
                Phone = m.Phone,
                Fax = m.Fax
            }).ToList();

            return response;
        }

        public bool? Update(int key, ManufacturerViewModel model, IDataSourceError error)
        {
            var manufacturer = this.SingleOrDefault(key);
            if (manufacturer == null)
                return null;

            manufacturer.Name = model.Name;
            manufacturer.Address = model.Address;
            manufacturer.ContactPerson = model.ContactPerson;
            manufacturer.ContactPersonPhone = model.ContactPersonPhone;
            manufacturer.Phone = model.Phone;
            manufacturer.Fax = model.Fax;

            bool result = false;
            try
            {
                this.DbContext.SaveChanges();
                result = true;
            }
            catch { }

            return result;
        }

        public IEnumerable<SelectListItem> ReadManufacturers()
        {
            var result = this.DbContext.Manufacturers.Select(m => new SelectListItem
            {
                Text = m.Name,
                Value = m.ManufacturerId.ToString()
            });

            return result;
        }
    }
}
