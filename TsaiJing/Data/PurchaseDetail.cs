namespace TsaiJing.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PurchaseDetail")]
    public partial class PurchaseDetail
    {
        public int PurchaseDetailId { get; set; }

        public int PurchaseId { get; set; }

        public int? ProductId { get; set; }

        public int? ProductDetailId { get; set; }

        public int Price { get; set; }

        public int Inventory { get; set; }

        public virtual Product Product { get; set; }

        public virtual ProductDetail ProductDetail { get; set; }

        public virtual Purchase Purchase { get; set; }
    }
}
