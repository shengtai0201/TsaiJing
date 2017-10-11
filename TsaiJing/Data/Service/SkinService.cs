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
    public class SkinService : SqlRepository<DefaultDbContext>, IApiService<SkinViewModel, int>, ISkinService
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

        public SkinService() : base("DefaultConnection") { }

        public bool Create(SkinViewModel model, IDataSourceError error)
        {
            if (this.CurrentUser.IsInRole("Company"))
            {
                error.Error.AppendLine("公司端不可新增");
                return false;
            }

            var skin = new Skin
            {
                CustomerId = model.CustomerId,
                ConditionDry = model.ConditionDry,
                ConditionOily = model.ConditionOily,
                ConditionSensitivity = model.ConditionSensitivity,
                ConditionMixed = model.ConditionMixed,
                ImproveAcne = model.ImproveAcne,
                ImproveSensitive = model.ImproveSensitive,
                ImproveWrinkle = model.ImproveWrinkle,
                ImproveLargePores = model.ImproveLargePores,
                ImproveSpot = model.ImproveSpot,
                ImproveDull = model.ImproveDull,
                ImprovePock = model.ImprovePock,
                ImproveOther = model.ImproveOther,
                Advice = model.Advice,
                Detail = model.Detail
            };

            this.DbContext.Skins.Add(skin);
            var result = false;
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

        public DataSourceResponse<SkinViewModel> Read(DataSourceRequest request)
        {
            // todo: query
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
		            s.ConditionDry, s.ConditionOily, s.ConditionSensitivity, s.ConditionMixed,
                    s.ImproveAcne, s.ImproveSensitive, s.ImproveWrinkle, s.ImproveLargePores, s.ImproveSpot, s.ImproveDull, s.ImprovePock,
                    s.ImproveOther, s.Advice, s.Detail
                FROM Customer c
                     LEFT JOIN Skin s ON c.CustomerId = s.CustomerId
                WHERE c.ConsultantId = '{0}'
                      OR c.UserId = '{0}'";
            cmdText = string.Format(cmdText, this.CurrentUserId);

            sqlTotalRowCount = string.Format(sqlTotalRowCount, cmdText);
            var response = new DataSourceResponse<SkinViewModel> { TotalRowCount = Convert.ToInt32(this.ExecuteScalar(sqlTotalRowCount)) };
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
                response.DataCollection.Add(new SkinViewModel
                {
                    CustomerId = Convert.ToInt32(dataReader["CustomerId"]),
                    CustomerName = dataReader["Name"].ToString(),
                    ConditionDry = this.ConvertToBoolean(dataReader["ConditionDry"]),
                    ConditionOily = this.ConvertToBoolean(dataReader["ConditionOily"]),
                    ConditionSensitivity = this.ConvertToBoolean(dataReader["ConditionSensitivity"]),
                    ConditionMixed = this.ConvertToBoolean(dataReader["ConditionMixed"]),
                    ImproveAcne = this.ConvertToBoolean(dataReader["ImproveAcne"]),
                    ImproveSensitive = this.ConvertToBoolean(dataReader["ImproveSensitive"]),
                    ImproveWrinkle = this.ConvertToBoolean(dataReader["ImproveWrinkle"]),
                    ImproveLargePores = this.ConvertToBoolean(dataReader["ImproveLargePores"]),
                    ImproveSpot = this.ConvertToBoolean(dataReader["ImproveSpot"]),
                    ImproveDull = this.ConvertToBoolean(dataReader["ImproveDull"]),
                    ImprovePock = this.ConvertToBoolean(dataReader["ImprovePock"]),
                    ImproveOther = this.DbToString(dataReader["ImproveOther"]),
                    Advice = this.DbToString(dataReader["Advice"]),
                    Detail = this.DbToString(dataReader["Detail"])
                });
            }, cmdText);

            return response;
        }

        private Skin SingleOrDefault(int key)
        {
            return this.DbContext.Skins.Where(s => s.CustomerId == key).SingleOrDefault();
        }

        public bool? Update(int key, SkinViewModel model, IDataSourceError error)
        {
            var skin = this.SingleOrDefault(key);
            if (skin == null)
                return this.Create(model, error);
            else
            {
                skin.ConditionDry = model.ConditionDry;
                skin.ConditionOily = model.ConditionOily;
                skin.ConditionSensitivity = model.ConditionSensitivity;
                skin.ConditionMixed = model.ConditionMixed;
                skin.ImproveAcne = model.ImproveAcne;
                skin.ImproveSensitive = model.ImproveSensitive;
                skin.ImproveWrinkle = model.ImproveWrinkle;
                skin.ImproveLargePores = model.ImproveLargePores;
                skin.ImproveSpot = model.ImproveSpot;
                skin.ImproveDull = model.ImproveDull;
                skin.ImprovePock = model.ImprovePock;
                skin.ImproveOther = model.ImproveOther;
                skin.Advice = model.Advice;
                skin.Detail = model.Detail;
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
            var skins = this.DbContext.Skins.Where(s => s.CustomerId == parentKey);
            this.DbContext.Skins.RemoveRange(skins);

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
