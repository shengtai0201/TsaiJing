using Microsoft.AspNet.Identity.Owin;
using Shengtai;
using Shengtai.Web.Telerik;
using Shengtai.Web.Telerik.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TsaiJing.WebApplication.Models.Role;

namespace TsaiJing.WebApplication.Controllers
{
    public class HomeController : Controller
    {
        private IUserService userService;
        private ICategoryService categoryService;
        private ISpecificationService specificationService;
        private IManufacturerService manufacturerService;
        private IProductService productService;
        private IProductDetailService productDetailService;
        private ICustomerService customerService;
        private IQueryService queryService;

        public HomeController(IUserService userService, ICategoryService categoryService, ISpecificationService specificationService,
            IManufacturerService manufacturerService, IProductService productService, IProductDetailService productDetailService, 
            ICustomerService customerService, IQueryService queryService)
        {
            this.userService = userService;
            this.categoryService = categoryService;
            this.specificationService = specificationService;
            this.manufacturerService = manufacturerService;
            this.productService = productService;
            this.productDetailService = productDetailService;
            this.customerService = customerService;
            this.queryService = queryService;
        }

        public ActionResult Index()
        {
            return View();
        }

        #region 使用者管理
        [Authorize(Roles = "Administrator")]
        public ActionResult Users()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ReadConsultants()
        {
            var result = this.userService.ReadConsultants();
            return this.Json(result);
        }

        [HttpPost]
        public ActionResult ReadCSGRoles()
        {
            var result = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Text = RoleType.Company.GetEnumDescription(),
                    Value = RoleType.Company.ToString()
                },
                new SelectListItem
                {
                    Text = RoleType.Supervise.GetEnumDescription(),
                    Value = RoleType.Supervise.ToString()
                },
                new SelectListItem
                {
                    Text = RoleType.Guidance.GetEnumDescription(),
                    Value = RoleType.Guidance.ToString()
                }
            };

            return this.Json(result);
        }
        #endregion

        #region 廠商設定
        [Authorize(Roles = "Company")]
        public ActionResult Manufacturers()
        {
            return View();
        }
        #endregion

        #region 商品規格設定
        [Authorize(Roles = "Company")]
        public ActionResult Specifications()
        {
            return View();
        }
        #endregion

        #region 商品設定
        [Authorize(Roles = "Company")]
        public ActionResult Products()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ReadCategories()
        {
            var result = this.categoryService.ReadCategories();
            return this.Json(result);
        }

        [HttpPost]
        public ActionResult ReadSpecifications([ModelBinder(typeof(DataSourceRequestModelBinder))]DataSourceRequest request)
        {
            var result = this.specificationService.ReadSpecifications(request);
            return this.Json(result);
        }
        #endregion

        #region 客戶資料卡
        [Authorize(Roles = "Supervise, Guidance, Company")]
        public ActionResult Customers()
        {
            return View();
        }

        [Authorize(Roles = "Supervise, Guidance, Company")]
        public ActionResult Bodies()
        {
            return View();
        }

        [Authorize(Roles = "Supervise, Guidance, Company")]
        public ActionResult TrackingRecords()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ReadCustomersResponse([ModelBinder(typeof(DataSourceRequestModelBinder))]DataSourceRequest request)
        {
            var result = this.customerService.ReadCustomers(request, this.User.Identity);
            return this.Json(result);
        }

        [Authorize(Roles = "Supervise, Guidance, Company")]
        public ActionResult Skins()
        {
            return View();
        }
        #endregion

        #region 會員申請
        [Authorize(Roles = "Supervise, Guidance, Company")]
        public ActionResult Members()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ReadCVWRoles()
        {
            var result = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Text = RoleType.Customer.GetEnumDescription(),
                    Value = RoleType.Customer.ToString()
                },
                new SelectListItem
                {
                    Text = RoleType.VIP.GetEnumDescription(),
                    Value = RoleType.VIP.ToString()
                },
                new SelectListItem
                {
                    Text = RoleType.VVIP.GetEnumDescription(),
                    Value = RoleType.VVIP.ToString()
                }
            };

            return this.Json(result);
        }
        #endregion

        #region 進貨
        [Authorize(Roles = "Company")]
        public ActionResult Purchases()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ReadManufacturers()
        {
            var result = this.manufacturerService.ReadManufacturers();
            return this.Json(result);
        }

        [HttpPost]
        public ActionResult ReadProducts()
        {
            var result = this.productService.ReadProducts();
            return this.Json(result);
        }

        [HttpPost]
        public ActionResult ReadProductDetails([ModelBinder(typeof(DataSourceRequestModelBinder))]DataSourceRequest request)
        {
            var result = this.productDetailService.ReadProductDetails(request);
            return this.Json(result);
        }
        #endregion

        #region 出貨
        [Authorize(Roles = "Supervise, Guidance, Company")]
        public ActionResult Shipments()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ReadCustomers()
        {
            var result = this.customerService.ReadCustomers(this.User);
            return this.Json(result);
        }
        #endregion

        #region 查詢業績與獎金
        [Authorize(Roles = "Supervise, Guidance")]
        public ActionResult Queries()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ReadQueries([ModelBinder(typeof(DataSourceRequestModelBinder))]DataSourceRequest request)
        {
            var result = this.queryService.ReadQueries(request, this.User.Identity);
            return this.Json(result);
        }

        [HttpPost]
        public ActionResult ReadQueryDetails([ModelBinder(typeof(DataSourceRequestModelBinder))]DataSourceRequest request)
        {
            var result = this.queryService.ReadQueryDetails(request, this.User.Identity);
            return this.Json(result);
        }
        #endregion
    }
}