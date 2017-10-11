using Autofac;
using Shengtai.Web.Telerik.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TsaiJing.Data;
using TsaiJing.Data.Service;
using TsaiJing.WebApplication.Models;

namespace TsaiJing
{
    internal class DefaultModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // database
            builder.RegisterType<DefaultDbContext>().As<DefaultDbContext>().InstancePerRequest();

            var userService = new UserService();
            builder.Register(c => userService).As<IUserService>().InstancePerRequest();
            builder.RegisterType<ManufacturerService>().As<IApiService<ManufacturerViewModel, int>>().As<IManufacturerService>().InstancePerRequest();

            var specificationService = new SpecificationService();
            builder.Register(c => specificationService).As<IApiService<SpecificationViewModel, int>>().As<ISpecificationService>().InstancePerRequest();
            builder.RegisterType<CategoryService>().As<IApiService<CategoryViewModel, int>>().As<ICategoryService>()
                .WithParameter("specificationService", specificationService)
                .InstancePerRequest();

            var productDetailService = new ProductDetailService();
            builder.Register(c => productDetailService).As<IApiService<ProductDetailViewModel, int>>().As<IProductDetailService>().InstancePerRequest();
            builder.RegisterType<ProductService>().As<IApiService<ProductViewModel, int>>().As<IProductService>()
                .WithParameter("productDetailService", productDetailService)
                .InstancePerRequest();

            builder.RegisterType<CustomerService>().As<IApiService<CustomerViewModel, int>>().As<ICustomerService>()
                .WithParameter("userService", userService)
                .InstancePerRequest();

            builder.RegisterType<BodyService>().As<IApiService<BodyViewModel, int>>().As<IBodyService>().InstancePerRequest();
            builder.RegisterType<TrackingRecordService>().As<IApiService<TrackingRecordViewModel, int>>().As<ITrackingRecordService>().InstancePerRequest();
            builder.RegisterType<SkinService>().As<IApiService<SkinViewModel, int>>().As<ISkinService>().InstancePerRequest();

            var purchaseDetailService = new PurchaseDetailService();
            builder.Register(c => purchaseDetailService).As<IApiService<PurchaseDetailViewModel, int>>().As<IPurchaseDetailService>().InstancePerRequest();
            builder.RegisterType<PurchaseService>().As<IApiService<PurchaseViewModel, int>>()
                .WithParameter("purchaseDetailService", purchaseDetailService)
                .InstancePerRequest();

            var shipmentDetailService = new ShipmentDetailService(userService);
            builder.Register(c => shipmentDetailService).As<IApiService<ShipmentDetailViewModel, int>>().As<IShipmentDetailService>().InstancePerRequest();
            builder.RegisterType<ShipmentService>().As<IApiService<ShipmentViewModel, int>>()
                .WithParameter("shipmentDetailService", shipmentDetailService)
                .WithParameter("userService", userService)
                .InstancePerRequest();

            builder.RegisterType<QueryService>().As<IQueryService>()
                .WithParameter("userService", userService)
                .InstancePerRequest();

            base.Load(builder);
        }
    }
}
