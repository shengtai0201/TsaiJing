namespace TsaiJing.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TrackingRecord")]
    public partial class TrackingRecord
    {
        public int TrackingRecordId { get; set; }

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

        [StringLength(256)]
        public string Design { get; set; }

        [StringLength(256)]
        public string Buy { get; set; }

        public virtual Customer Customer { get; set; }
    }
}
