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

namespace TsaiJing.Data.Service
{
    public class BodyService : SqlRepository<DefaultDbContext>, IApiService<BodyViewModel, int>, IBodyService
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

        public BodyService() : base("DefaultConnection") { }

        public bool Create(BodyViewModel model, IDataSourceError error)
        {
            if (this.CurrentUser.IsInRole("Company"))
            {
                error.Error.AppendLine("公司端不可新增");
                return false;
            }

            var body = new Body
            {
                CustomerId = model.CustomerId,
                HealthSpine = model.HealthSpine,
                HealthBackPain = model.HealthBackPain,
                HealthOther = model.HealthOther,
                CurveChest = model.CurveChest,
                CurveArm = model.CurveArm,
                CurveButtock = model.CurveButtock,
                CurveStomachWaistAbdomen = model.CurveStomachWaistAbdomen,
                CurveThigh = model.CurveThigh,
                CurveCalf = model.CurveCalf,
                CurveFatSoft = model.CurveFatSoft,
                CurveFatHard = model.CurveFatHard,
                CurveFatOrange = model.CurveFatOrange,
                CurveFatTangled = model.CurveFatTangled,
                CurveFatOther = model.CurveFatOther,
                Diagnosis = model.Diagnosis
            };

            this.DbContext.Bodies.Add(body);
            bool result = false;
            try
            {
                this.DbContext.SaveChanges();
                result = true;
            }
            catch { }

            return result;
        }

        public bool? Destroy(int key)
        {
            throw new NotImplementedException();
        }

        public void Dispose() { }

        public DataSourceResponse<BodyViewModel> Read(DataSourceRequest request)
        {
            // todo: 區分 query data source
            if (this.CurrentUser.IsInRole("Company"))
            {
                // 公司端
            }
            else
            {
                // 督導、技術指導
            }

            string sqlTotalRowCount = @"
                SELECT COUNT(*)
                FROM
                (
                    {0}
                ) t";

            string cmdText = @"
                SELECT c.CustomerId, c.[Name], 
                    b.HealthSpine, b.HealthBackPain, b.HealthOther,
                    b.CurveChest, b.CurveArm, b.CurveButtock, b.CurveStomachWaistAbdomen, b.CurveThigh, b.CurveCalf,
                    b.CurveFatSoft, b.CurveFatHard, b.CurveFatOrange, b.CurveFatTangled, b.CurveFatOther, 
                    b.Diagnosis
                FROM Customer c
                     LEFT JOIN Body b ON c.CustomerId = b.CustomerId
                WHERE c.ConsultantId = '{0}'
                      OR c.UserId = '{0}'";
            cmdText = string.Format(cmdText, this.CurrentUserId);

            sqlTotalRowCount = string.Format(sqlTotalRowCount, cmdText);
            var response = new DataSourceResponse<BodyViewModel> { TotalRowCount = Convert.ToInt32(this.ExecuteScalar(sqlTotalRowCount)) };
            if (request.ServerPaging != null)
            {
                string sqlTake = @"
                    SELECT *
                    FROM
                    (
                        {0}
                    ) t
                    ORDER BY t.CustomerId ASC
                    OFFSET {1} ROWS FETCH NEXT {2} ROWS ONLY";

                int skip = Math.Max(request.ServerPaging.PageSize * (request.ServerPaging.Page - 1), val2: 0);
                cmdText = string.Format(sqlTake, cmdText, skip, request.ServerPaging.PageSize);
            }

            this.ReadData(dataReader =>
            {
                response.DataCollection.Add(new BodyViewModel
                {
                    CustomerId = Convert.ToInt32(dataReader["CustomerId"]),
                    CustomerName = dataReader["Name"].ToString(),
                    HealthSpine = this.ConvertToBoolean(dataReader["HealthSpine"]),
                    HealthBackPain = this.ConvertToBoolean(dataReader["HealthBackPain"]),
                    HealthOther = this.DbToString(dataReader["HealthOther"]),
                    CurveChest = this.ConvertToBoolean(dataReader["CurveChest"]),
                    CurveArm = this.ConvertToBoolean(dataReader["CurveArm"]),
                    CurveButtock = this.ConvertToBoolean(dataReader["CurveButtock"]),
                    CurveStomachWaistAbdomen = this.ConvertToBoolean(dataReader["CurveStomachWaistAbdomen"]),
                    CurveThigh = this.ConvertToBoolean(dataReader["CurveThigh"]),
                    CurveCalf = this.ConvertToBoolean(dataReader["CurveCalf"]),
                    CurveFatSoft = this.ConvertToBoolean(dataReader["CurveFatSoft"]),
                    CurveFatHard = this.ConvertToBoolean(dataReader["CurveFatHard"]),
                    CurveFatOrange = this.ConvertToBoolean(dataReader["CurveFatOrange"]),
                    CurveFatTangled = this.ConvertToBoolean(dataReader["CurveFatTangled"]),
                    CurveFatOther = dataReader["CurveFatOther"].ToString(),
                    Diagnosis = dataReader["Diagnosis"].ToString()
                });
            }, cmdText);

            return response;
        }

        private Body SingleOrDefault(int key)
        {
            return this.DbContext.Bodies.Where(b => b.CustomerId == key).SingleOrDefault();
        }

        public bool? Update(int key, BodyViewModel model, IDataSourceError error)
        {
            var body = this.SingleOrDefault(key);
            if (body == null)
                return this.Create(model, error);
            else
            {
                body.HealthSpine = model.HealthSpine;
                body.HealthBackPain = model.HealthBackPain;
                body.HealthOther = model.HealthOther;
                body.CurveChest = model.CurveChest;
                body.CurveArm = model.CurveArm;
                body.CurveButtock = model.CurveButtock;
                body.CurveStomachWaistAbdomen = model.CurveStomachWaistAbdomen;
                body.CurveThigh = model.CurveThigh;
                body.CurveCalf = model.CurveCalf;
                body.CurveFatSoft = model.CurveFatSoft;
                body.CurveFatHard = model.CurveFatHard;
                body.CurveFatOrange = model.CurveFatOrange;
                body.CurveFatTangled = model.CurveFatTangled;
                body.CurveFatOther = model.CurveFatOther;
                body.Diagnosis = model.Diagnosis;
            }

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
            var bodies = this.DbContext.Bodies.Where(b => b.CustomerId == parentKey);
            this.DbContext.Bodies.RemoveRange(bodies);

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
