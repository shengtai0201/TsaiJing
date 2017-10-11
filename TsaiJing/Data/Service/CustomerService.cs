using Shengtai;
using Shengtai.Web.Telerik.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TsaiJing.WebApplication.Models;
using Shengtai.Web.Telerik;
using Microsoft.AspNet.Identity;
using System.Data.SqlClient;
using TsaiJing.WebApplication.Models.Role;
using System.Web.Mvc;
using System.Security.Principal;
using System.Transactions;

namespace TsaiJing.Data.Service
{
    public class CustomerService : SqlRepository<DefaultDbContext>, IApiService<CustomerViewModel, int>, ICustomerService
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

        private IUserService userService;
        private IBodyService bodyService;
        private ITrackingRecordService trackingRecordService;
        private ISkinService skinService;
        public CustomerService(IUserService userService,
            IBodyService bodyService, ITrackingRecordService trackingRecordService, ISkinService skinService) : base("DefaultConnection")
        {
            this.userService = userService;
            this.bodyService = bodyService;
            this.trackingRecordService = trackingRecordService;
            this.skinService = skinService;
        }

        public bool Create(CustomerViewModel model, IDataSourceError error)
        {
            if (this.CurrentUser.IsInRole("Company"))
            {
                error.Error.AppendLine("公司端不可新增");
                return false;
            }

            var roleCustomer = this.userService.GetRoles().Where(r => r.Value.GetRoleType() == RoleType.Customer).SingleOrDefault();
            var customer = new Customer
            {
                Name = model.Name,
                Phone = model.Phone,
                Birthday = model.Birthday,
                Address = model.Address,
                Introducer = model.Introducer,
                ConsultantId = this.CurrentUserId,
                Height = model.Height,
                Weight = model.Weight,
                MemberRoleId = roleCustomer.Key
            };

            if (!string.IsNullOrEmpty(model.Career))
                customer.Career = model.Career;
            if (!string.IsNullOrEmpty(model.IdCardNumber))
                customer.IdCardNumber = model.IdCardNumber;
            if (!string.IsNullOrEmpty(model.Email))
                customer.Email = model.Email;
            if (!string.IsNullOrEmpty(model.LineId))
                customer.LineId = model.LineId;
            if (!string.IsNullOrEmpty(model.Remark))
                customer.Remark = model.Remark;

            this.DbContext.Customers.Add(customer);
            var result = false;
            try
            {
                this.DbContext.SaveChanges();
                model.CustomerId = customer.CustomerId;
                result = true;
            }
            catch { }

            return result;
        }

        private Customer SingleOrDefault(int key)
        {
            return this.DbContext.Customers.Where(c => c.CustomerId == key).SingleOrDefault();
        }

        public bool? Destroy(int key)
        {
            if (this.CurrentUser.IsInRole("Company"))
                return false;

            var customer = this.SingleOrDefault(key);
            if (customer == null)
                return null;

            TransactionScope transaction = new TransactionScope();
            bool result = this.bodyService.DestroyByParent(key);
            if (result)
            {
                result = this.trackingRecordService.DestroyByParent(key);
                if (result)
                {
                    result = this.skinService.DestroyByParent(key);
                    if (result)
                    {
                        this.DbContext.Customers.Remove(customer);
                        try
                        {
                            this.DbContext.SaveChanges();
                            result = true;
                        }
                        catch { }
                    }
                }
            }

            if (result)
                transaction.Complete();

            transaction.Dispose();
            return result;
        }

        public void Dispose() { }

        public DataSourceResponse<CustomerViewModel> Read(DataSourceRequest request)
        {
            IQueryable<Customer> responseData;
            if (this.CurrentUser.IsInRole("Company"))
            {
                // 公司端
                responseData = this.DbContext.Customers.Select(c => c);
            }
            else
            {
                // 督導、技術指導
                responseData = this.DbContext.Customers.Where(c => c.ConsultantId == this.CurrentUserId || c.UserId == this.CurrentUserId);
            }

            var response = new DataSourceResponse<CustomerViewModel>
            {
                TotalRowCount = responseData.Count()
            };

            if (request.ServerPaging != null)
            {
                int skip = Math.Max(request.ServerPaging.PageSize * (request.ServerPaging.Page - 1), val2: 0);
                responseData = responseData.OrderBy(c => c.CustomerId).Skip(skip).Take(request.ServerPaging.PageSize);
            }

            var collection = responseData.Select(c => new
            {
                CustomerId = c.CustomerId,
                Name = c.Name,
                Phone = c.Phone,
                Birthday = c.Birthday,
                Career = c.Career,
                Address = c.Address,
                IdCardNumber = c.IdCardNumber,
                Introducer = c.Introducer,
                ConsultantId = c.ConsultantId,
                Height = c.Height,
                Weight = c.Weight,
                Email = c.Email,
                LineId = c.LineId,
                Remark = c.Remark
            }).ToList();

            IDictionary<string, string> consultants = new Dictionary<string, string>();
            foreach (var data in collection)
            {
                var customer = new CustomerViewModel
                {
                    CustomerId = data.CustomerId,
                    Name = data.Name,
                    Phone = data.Phone,
                    Birthday = data.Birthday,
                    Career = data.Career,
                    Address = data.Address,
                    IdCardNumber = data.IdCardNumber,
                    Introducer = data.Introducer,
                    Height = data.Height,
                    Weight = data.Weight,
                    Email = data.Email,
                    LineId = data.LineId,
                    Remark = data.Remark
                };

                if (!string.IsNullOrEmpty(data.ConsultantId))
                {
                    if (consultants.ContainsKey(data.ConsultantId))
                        customer.ConsultantName = consultants[data.ConsultantId];
                    else
                    {
                        var consultant = this.DbContext.Customers.Where(c => c.UserId == data.ConsultantId).SingleOrDefault();
                        if (consultant != null)
                        {
                            customer.ConsultantName = consultant.Name;
                            consultants.Add(data.ConsultantId, consultant.Name);
                        }
                    }
                }

                response.DataCollection.Add(customer);
            }

            return response;
        }

        public bool? Update(int key, CustomerViewModel model, IDataSourceError error)
        {
            var customer = this.SingleOrDefault(key);
            if (customer == null)
                return null;

            customer.Name = model.Name;
            customer.Phone = model.Phone;
            customer.Birthday = model.Birthday;
            customer.Address = model.Address;
            customer.Introducer = model.Introducer;
            customer.Height = model.Height;
            customer.Weight = model.Weight;

            if (!string.IsNullOrEmpty(model.Career))
                customer.Career = model.Career;
            if (!string.IsNullOrEmpty(model.IdCardNumber))
                customer.IdCardNumber = model.IdCardNumber;
            if (!string.IsNullOrEmpty(model.Email))
                customer.Email = model.Email;
            if (!string.IsNullOrEmpty(model.LineId))
                customer.LineId = model.LineId;
            if (!string.IsNullOrEmpty(model.Remark))
                customer.Remark = model.Remark;

            bool result = false;
            try
            {
                this.DbContext.SaveChanges();
                result = true;
            }
            catch { }

            return result;
        }

        public DataSourceResponse<MemberViewModel> ReadMembers(DataSourceRequest request)
        {
            IQueryable<Customer> responseData;
            if (this.CurrentUser.IsInRole("Company"))
            {
                // 公司端
                responseData = this.DbContext.Customers.Where(c => c.ConsultantId != null);
            }
            else
            {
                // 督導、技術指導
                responseData = this.DbContext.Customers.Where(c => c.ConsultantId == this.CurrentUserId);
            }

            var response = new DataSourceResponse<MemberViewModel> { TotalRowCount = responseData.Count() };

            if (request.ServerPaging != null)
            {
                int skip = Math.Max(request.ServerPaging.PageSize * (request.ServerPaging.Page - 1), val2: 0);
                responseData = responseData.OrderBy(c => c.CustomerId).Skip(skip).Take(request.ServerPaging.PageSize);
            }

            var collection = responseData.Select(c => new
            {
                CustomerId = c.CustomerId,
                Name = c.Name,
                Email = c.Email,
                Birthday = c.Birthday,
                Phone = c.Phone,
                Mobile = c.Mobile,
                Address = c.Address,
                Introducer = c.Introducer,
                ConsultantId = c.ConsultantId,
                MemberDate = c.MemberDate,
                MemberRoleId = c.MemberRoleId
            }).ToList();

            var roles = this.userService.GetRoles();
            string cmdText = @"
                SELECT r.[Name] RoleName,
                       c.[Name] GuidanceName,
                       sc.[Name] SuperviseName
                FROM Customer c
                     JOIN AspNetUserRoles ur ON c.UserId = ur.UserId
                     JOIN AspNetRoles r ON ur.RoleId = r.Id
                     LEFT JOIN Customer sc ON c.ConsultantId = sc.UserId
                WHERE c.UserId = @ConsultantId";
            foreach (var data in collection)
            {
                var memberRole = roles[data.MemberRoleId];
                var member = new MemberViewModel
                {
                    CustomerId = data.CustomerId,
                    Name = data.Name,
                    Email = data.Email,
                    Birthday = data.Birthday,
                    Phone = data.Phone,
                    Mobile = data.Mobile,
                    Address = data.Address,
                    Introducer = data.Introducer,
                    MemberRole = new SelectListItem { Text = memberRole.GetName(), Value = data.MemberRoleId },
                    JoinDate = data.MemberDate
                };

                if (!string.IsNullOrEmpty(data.ConsultantId))
                {
                    // 取得該顧問的角色
                    // 如果是督導，則 GuidanceName, SuperviseName 都填入該姓名
                    // 如果是技術指導，填入其姓名於 GuidanceName，又取得其督導姓名填入 SuperviseName
                    this.ReadDataSingle(dataReader =>
                    {
                        if (Enum.TryParse(dataReader["RoleName"].ToString(), out RoleType role))
                        {
                            member.GuidanceName = member.SuperviseName = dataReader["GuidanceName"].ToString();

                            if (role == RoleType.Guidance)
                                member.SuperviseName = dataReader["SuperviseName"].ToString();
                        }

                    }, cmdText, new SqlParameter("ConsultantId", data.ConsultantId));
                }

                response.DataCollection.Add(member);
            }

            return response;
        }

        public bool? UpdateMember(int key, MemberViewModel model, IDataSourceError error)
        {
            var member = this.SingleOrDefault(key);
            if (member == null)
                return null;

            member.Name = model.Name;
            member.Email = model.Email;
            member.Birthday = model.Birthday;
            member.Phone = model.Phone;
            member.Mobile = model.Mobile;
            member.Address = model.Address;
            member.Introducer = model.Introducer;

            if (!member.MemberDate.HasValue && member.MemberRoleId != model.MemberRole.Value)
                member.MemberDate = DateTime.Now;

            member.MemberRoleId = model.MemberRole.Value;

            bool result = false;
            try
            {
                this.DbContext.SaveChanges();

                model.JoinDate = member.MemberDate;
                result = true;
            }
            catch { }

            return result;
        }

        public DataSourceResponse<SelectListItem> ReadCustomers(DataSourceRequest request, IIdentity identity)
        {
            var currentUserId = identity.GetUserId();
            IQueryable<Customer> responseData;
            if (this.CurrentUser.IsInRole("Company"))
            {
                // 公司端
                responseData = this.DbContext.Customers.Select(c => c);
            }
            else
            {
                // 督導、技術指導
                responseData = this.DbContext.Customers.Where(c => c.ConsultantId == currentUserId || c.UserId == currentUserId);
            }

            var response = new DataSourceResponse<SelectListItem> { TotalRowCount = responseData.Count() };

            if (request.ServerPaging != null)
            {
                int skip = Math.Max(request.ServerPaging.PageSize * (request.ServerPaging.Page - 1), val2: 0);
                responseData = responseData.OrderBy(c => c.CustomerId).Skip(skip).Take(request.ServerPaging.PageSize);
            }

            response.DataCollection = responseData.Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.CustomerId.ToString()
            }).ToList();

            return response;
        }

        public IEnumerable<SelectListItem> ReadCustomers(IPrincipal currentUser)
        {
            var currentUserId = currentUser.Identity.GetUserId();
            IQueryable<Customer> responseData;
            if (currentUser.IsInRole("Company"))
            {
                // 公司端
                responseData = this.DbContext.Customers.Select(c => c);
            }
            else
            {
                // 督導、技術指導
                responseData = this.DbContext.Customers.Where(c => c.ConsultantId == currentUserId);
            }

            return responseData.Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.CustomerId.ToString()
            });
        }
    }
}
