using Shengtai;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using TsaiJing.WebApplication.Models;
using System.Security.Principal;
using Microsoft.AspNet.Identity;
using Shengtai.Web.Telerik;
using System.Data.SqlClient;
using TsaiJing.WebApplication.Models.Role;

namespace TsaiJing.Data.Service
{
    public class UserService : SqlRepository<DefaultDbContext>, IUserService
    {
        public UserService() : base("DefaultConnection") { }

        public bool CreateCustomer(string userId, ExternalLoginConfirmationViewModel model)
        {
            var customer = new Customer
            {
                UserId = userId,
                Name = model.Name,
                Phone = model.Phone,
                Birthday = model.Birthday,
                Address = model.Address,
                Introducer = model.Introducer
            };
            if (!string.IsNullOrEmpty(model.Career))
                customer.Career = model.Career;
            if (!string.IsNullOrEmpty(model.IdCardNumber))
                customer.IdCardNumber = model.IdCardNumber;
            if (model.Height.HasValue)
                customer.Height = model.Height.Value;
            if (model.Weight.HasValue)
                customer.Weight = model.Weight.Value;
            if (!string.IsNullOrEmpty(model.Email))
                customer.Email = model.Email;
            if (!string.IsNullOrEmpty(model.LineId))
                customer.LineId = model.LineId;

            this.DbContext.Customers.Add(customer);

            bool result = false;
            try
            {
                this.DbContext.SaveChanges();
                result = true;
            }
            catch { }

            return result;
        }

        public bool CreateCustomer(string userId, RegisterViewModel model)
        {
            var customer = new Customer
            {
                UserId = userId,
                Name = model.Name,
                Phone = model.Phone,
                Birthday = model.Birthday,
                Address = model.Address,
                Introducer = model.Introducer
            };
            if (!string.IsNullOrEmpty(model.Career))
                customer.Career = model.Career;
            if (!string.IsNullOrEmpty(model.IdCardNumber))
                customer.IdCardNumber = model.IdCardNumber;
            if (model.Height.HasValue)
                customer.Height = model.Height.Value;
            if (model.Weight.HasValue)
                customer.Weight = model.Weight.Value;
            if (!string.IsNullOrEmpty(model.Email))
                customer.Email = model.Email;
            if (!string.IsNullOrEmpty(model.LineId))
                customer.LineId = model.LineId;

            this.DbContext.Customers.Add(customer);

            bool result = false;
            try
            {
                this.DbContext.SaveChanges();
                result = true;
            }
            catch { }

            return result;
        }

        public string GetRoleByCustomer(int customerId)
        {
            string cmdText = @"SELECT r.Id FROM Customer c JOIN AspNetRoles r ON c.MemberRoleId = r.Id WHERE c.CustomerId = @CustomerId";
            var id = this.ExecuteScalar(cmdText, new SqlParameter("CustomerId", customerId));

            return id.ToString();
        }

        public string GetRoleByUser(string userId)
        {
            string cmdText = @"SELECT r.Id FROM AspNetUserRoles ur JOIN AspNetRoles r ON ur.RoleId = r.Id WHERE ur.UserId = @UserId";
            var id = this.ExecuteScalar(cmdText, new SqlParameter("UserId", userId));

            return id.ToString();
        }

        public IDictionary<string, Role> GetRoles()
        {
            IDictionary<string, Role> roles = new Dictionary<string, Role>();

            string cmdText = @"select * from AspNetRoles";
            this.ReadData(dataReader =>
            {
                string key = dataReader["Id"].ToString();
                string name = dataReader["Name"].ToString();

                if (Enum.TryParse(name, out RoleType result))
                {
                    switch (result)
                    {
                        case RoleType.Administrator:
                            roles.Add(key, Administrator.GetInstance());
                            break;
                        case RoleType.Customer:
                            roles.Add(key, WebApplication.Models.Role.Customer.GetInstance());
                            break;
                        case RoleType.DirectStore:
                            roles.Add(key, DirectStore.GetInstance());
                            break;
                        case RoleType.Guidance:
                            roles.Add(key, Guidance.GetInstance());
                            break;
                        case RoleType.Supervise:
                            roles.Add(key, Supervise.GetInstance());
                            break;
                        case RoleType.VIP:
                            roles.Add(key, VIP.GetInstance());
                            break;
                        case RoleType.VVIP:
                            roles.Add(key, VVIP.GetInstance());
                            break;
                    }
                }
            }, cmdText);

            return roles;
        }

        //public Role GetRoleByCustomer(int customerId)
        //{
        //    throw new NotImplementedException();
        //}

        //public Role GetRoleByUser(string userId)
        //{
        //    throw new NotImplementedException();
        //}

        public string GetUserName(IIdentity identity)
        {
            if (identity.Name == RoleType.Administrator.GetEnumDescription())
                return identity.Name;

            string cmdText = @"
                SELECT c.[Name]
                FROM AspNetUsers u
                     JOIN Customer c ON u.Id = c.UserId
                WHERE u.UserName = @UserName";

            var name = this.ExecuteScalar(cmdText, new SqlParameter("UserName", identity.Name));
            return name.ToString();
        }

        // http://www.byteblocks.com/Post/AspNet-Identity-Lock-and-Unlock-User-Accounts
        public DataSourceResponse<UserViewModel> Read(UserManager<ApplicationUser> userManager, DataSourceRequest request)
        {
            string cmdText = @"
                SELECT u.Id UserId,
                       c.[Name] CustomerName,
                       cuc.[Name] ConsultantText,
                       cu.Id ConsultantValue,
                       r.[Name] RoleValue
                FROM AspNetUsers u
                     LEFT JOIN AspNetUserRoles ur ON u.Id = ur.UserId
                     LEFT JOIN AspNetRoles r ON ur.RoleId = r.Id
                     LEFT JOIN Customer c ON c.UserId = u.Id
                     LEFT JOIN AspNetUsers cu ON c.ConsultantId = cu.Id
                     LEFT JOIN Customer cuc ON cu.Id = cuc.UserId
                WHERE r.[Name] = 'Supervise'
                      OR r.[Name] = 'Guidance'
                      OR r.[Name] = 'Company'
                      OR r.[Name] IS NULL
                ORDER BY u.Id";

            IList<UserViewModel> users = new List<UserViewModel>();
            this.ReadData(dataReader =>
            {
                var consultant = new SelectListItem();
                consultant.Text = this.DbToString(dataReader["ConsultantText"]);
                consultant.Value = this.DbToString(dataReader["ConsultantValue"]);

                var role = new SelectListItem();
                if (dataReader["RoleValue"] != DBNull.Value)
                {
                    role.Value = dataReader["RoleValue"].ToString();
                    if (Enum.TryParse<RoleType>(role.Value, out RoleType roleType))
                        role.Text = roleType.GetEnumDescription();
                }

                users.Add(new UserViewModel
                {
                    UserId = dataReader["UserId"].ToString(),
                    CustomerName = this.DbToString(dataReader["CustomerName"]),
                    Consultant = consultant,
                    Role = role
                });
            }, cmdText);

            foreach (var user in users)
                user.LockedOut = userManager.IsLockedOut(user.UserId);

            var response = new DataSourceResponse<UserViewModel>();
            response.TotalRowCount = users.Count;

            if (request.ServerPaging != null)
            {
                int skip = Math.Max(request.ServerPaging.PageSize * (request.ServerPaging.Page - 1), val2: 0);
                users = users.OrderBy(u => u.UserId).Skip(skip).Take(request.ServerPaging.PageSize).ToList();
            }

            response.DataCollection = users;
            return response;
        }

        public IEnumerable<SelectListItem> ReadConsultants()
        {
            IList<SelectListItem> result = new List<SelectListItem>
            {
                new SelectListItem { Text = "請選擇" }
            };

            string cmdText = @"
                SELECT u.Id,
                       CASE
                           WHEN c.[Name] IS NULL
                           THEN u.UserName
                           ELSE c.[Name]
                       END [Name]
                FROM AspNetUsers u
                     JOIN AspNetUserRoles ur ON u.Id = ur.UserId
                     JOIN AspNetRoles r ON ur.RoleId = r.Id
                     LEFT JOIN Customer c ON c.UserId = u.Id
                WHERE r.[Name] = 'Supervise'
                      OR r.[Name] = 'Guidance'";


            this.ReadData(dataReader =>
            {
                var item = new SelectListItem
                {
                    Text = dataReader["Name"].ToString(),
                    Value = dataReader["Id"].ToString()
                };
                result.Add(item);
            }, cmdText);

            return result;
        }

        //public IEnumerable<SelectListItem> ReadRoles()
        //{
        //    IList<SelectListItem> result = new List<SelectListItem>
        //    {
        //        new SelectListItem
        //        {
        //            Text = RoleType.Company.GetEnumDescription(),
        //            Value = RoleType.Company.ToString()
        //        },
        //        new SelectListItem
        //        {
        //            Text = RoleType.Supervise.GetEnumDescription(),
        //            Value = RoleType.Supervise.ToString()
        //        },
        //        new SelectListItem
        //        {
        //            Text = RoleType.Guidance.GetEnumDescription(),
        //            Value = RoleType.Guidance.ToString()
        //        }
        //    };

        //    return result;
        //}

        public bool? Update(UserManager<ApplicationUser> userManager, string key, UserViewModel model)
        {
            bool allSuccess = true;

            var user = userManager.Users.Where(u => u.Id == key).SingleOrDefault();
            if (user == null)
                return null;

            // Role
            if (!string.IsNullOrEmpty(model.Role.Value))
            {
                if (!userManager.IsInRole(key, model.Role.Value))
                {
                    var roles = userManager.GetRoles(key).ToArray();
                    IdentityResult result = userManager.RemoveFromRoles(key, roles);
                    if (result.Succeeded)
                        result = userManager.AddToRole(key, model.Role.Value);

                    allSuccess &= result.Succeeded;
                }
            }

            // Consultant
            //if (!string.IsNullOrEmpty(model.Consultant.Value))
            //{
            //    var customer = this.DbContext.Customers.Where(c => c.UserId == key).SingleOrDefault();
            //    if (customer == null)
            //        return null;

            //    customer.ConsultantId = model.Consultant.Value;
            //    try
            //    {
            //        this.DbContext.SaveChanges();
            //    }
            //    catch
            //    {
            //        allSuccess = false;
            //    }
            //}
            var customer = this.DbContext.Customers.Where(c => c.UserId == key).SingleOrDefault();
            if (customer == null)
                return null;

            customer.ConsultantId = model.Consultant.Value;
            try
            {
                this.DbContext.SaveChanges();
            }
            catch
            {
                allSuccess = false;
            }

            // LockedOut
            if (userManager.IsLockedOut(key) != model.LockedOut)
            {
                IdentityResult result;
                if (model.LockedOut)
                {
                    result = userManager.SetLockoutEnabled(key, true);
                    if (result.Succeeded)
                        result = userManager.SetLockoutEndDate(key, DateTimeOffset.MaxValue);
                }
                else
                {
                    result = userManager.SetLockoutEnabled(key, false);
                    if (result.Succeeded)
                        result = userManager.ResetAccessFailedCount(key);
                }

                allSuccess &= result.Succeeded;
            }

            return allSuccess;
        }
    }
}
