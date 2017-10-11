using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsaiJing.WebApplication.Models
{
    public class TrackingRecordViewModel
    {
        public int? TrackingRecordId { get; set; }
        public int CustomerId { get; set; }
        public DateTime ReferralTime { get; set; }
        public int? BustUp { get; set; }
        public int? BustDown { get; set; }
        public int? MilkCapacity { get; set; }
        public int? Abdominal { get; set; }
        public int? Waist { get; set; }
        public int? Hip { get; set; }
        public int? LegLeft { get; set; }
        public int? LegRight { get; set; }
        public string Design { get; set; }
        public string Buy { get; set; }
    }
}
