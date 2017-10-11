namespace TsaiJing.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ShipmentDetail")]
    public partial class ShipmentDetail
    {
        public int ShipmentDetailId { get; set; }

        public int ShipmentId { get; set; }

        public int? ProductId { get; set; }

        public int? ProductDetailId { get; set; }

        public int Quantity { get; set; }

        public int SubtotalAmount { get; set; }

        public virtual Product Product { get; set; }

        public virtual ProductDetail ProductDetail { get; set; }

        public virtual Shipment Shipment { get; set; }
    }
}
