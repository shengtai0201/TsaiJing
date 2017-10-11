using Shengtai;
using Shengtai.Web.Telerik.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TsaiJing.WebApplication.Models;
using Shengtai.Web.Telerik;

namespace TsaiJing.Data.Service
{
    public class TrackingRecordService : SqlRepository<DefaultDbContext>, IApiService<TrackingRecordViewModel, int>, ITrackingRecordService
    {
        public TrackingRecordService() : base("DefaultConnection") { }

        public bool Create(TrackingRecordViewModel model, IDataSourceError error)
        {
            if (this.CurrentUser.IsInRole("Company"))
            {
                error.Error.AppendLine("公司端不可新增");
                return false;
            }

            var trackingRecord = new TrackingRecord
            {
                CustomerId = model.CustomerId,
                ReferralTime = model.ReferralTime,
                BustUp = model.BustUp,
                BustDown = model.BustDown,
                MilkCapacity = model.MilkCapacity,
                Abdominal = model.Abdominal,
                Waist = model.Waist,
                Hip = model.Hip,
                LegLeft = model.LegLeft,
                LegRight = model.LegRight,
                Design = model.Design,
                Buy = model.Buy
            };

            this.DbContext.TrackingRecords.Add(trackingRecord);
            var result = false;
            try
            {
                this.DbContext.SaveChanges();
                model.TrackingRecordId = trackingRecord.TrackingRecordId;
                result = true;
            }
            catch { }

            return result;
        }

        private TrackingRecord SingleOrDefault(int key)
        {
            return this.DbContext.TrackingRecords.Where(r => r.TrackingRecordId == key).SingleOrDefault();
        }

        public bool? Destroy(int key)
        {
            if (this.CurrentUser.IsInRole("Company"))
                return false;

            var trackingRecord = this.SingleOrDefault(key);
            if (trackingRecord == null)
                return null;

            this.DbContext.TrackingRecords.Remove(trackingRecord);

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

        public DataSourceResponse<TrackingRecordViewModel> Read(DataSourceRequest request)
        {
            // 公司端、督導、技術指導
            var responseData = this.DbContext.TrackingRecords.Select(r => r);

            if (request.ServerFiltering != null)
            {
                ServerFilterInfo filterInfo = request.ServerFiltering.FilterCollection.Where(f => f.Field == "CustomerId").SingleOrDefault();
                int customerId = Convert.ToInt32(filterInfo.Value);
                responseData = responseData.Where(r => r.CustomerId == customerId);
            }

            var response = new DataSourceResponse<TrackingRecordViewModel> { TotalRowCount = responseData.Count() };

            if (request.ServerPaging != null)
            {
                int skip = Math.Max(request.ServerPaging.PageSize * (request.ServerPaging.Page - 1), val2: 0);
                responseData = responseData.OrderBy(r => r.TrackingRecordId).Skip(skip).Take(request.ServerPaging.PageSize);
            }

            response.DataCollection = responseData.Select(r => new TrackingRecordViewModel
            {
                TrackingRecordId = r.TrackingRecordId,
                CustomerId = r.CustomerId,
                ReferralTime = r.ReferralTime,
                BustUp = r.BustUp,
                BustDown = r.BustDown,
                MilkCapacity = r.MilkCapacity,
                Abdominal = r.Abdominal,
                Waist = r.Waist,
                Hip = r.Hip,
                LegLeft = r.LegLeft,
                LegRight = r.LegRight,
                Design = r.Design,
                Buy = r.Buy
            }).ToList();

            return response;
        }

        public bool? Update(int key, TrackingRecordViewModel model, IDataSourceError error)
        {
            var trackingRecord = this.SingleOrDefault(key);
            if (trackingRecord == null)
                return null;

            trackingRecord.ReferralTime = model.ReferralTime;
            trackingRecord.BustUp = model.BustUp;
            trackingRecord.BustDown = model.BustDown;
            trackingRecord.MilkCapacity = model.MilkCapacity;
            trackingRecord.Abdominal = model.Abdominal;
            trackingRecord.Waist = model.Waist;
            trackingRecord.Hip = model.Hip;
            trackingRecord.LegLeft = model.LegLeft;
            trackingRecord.LegRight = model.LegRight;
            trackingRecord.Design = model.Design;
            trackingRecord.Buy = model.Buy;

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
            var trackingRecords = this.DbContext.TrackingRecords.Where(r => r.CustomerId == parentKey);
            this.DbContext.TrackingRecords.RemoveRange(trackingRecords);

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
