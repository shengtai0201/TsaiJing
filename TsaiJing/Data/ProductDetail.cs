namespace TsaiJing.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ProductDetail")]
    public partial class ProductDetail
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ProductDetail()
        {
            ShipmentDetails = new HashSet<ShipmentDetail>();
            PurchaseDetails = new HashSet<PurchaseDetail>();
        }

        public int ProductDetailId { get; set; }

        public int ProductId { get; set; }

        public int FirstSpecificationId { get; set; }

        public int? SecondSpecificationId { get; set; }

        public int Price { get; set; }

        public int SafeStock { get; set; }

        public virtual Product Product { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ShipmentDetail> ShipmentDetails { get; set; }

        public virtual ProductSpecification FirstSpecification { get; set; }

        public virtual ProductSpecification SecondSpecification { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PurchaseDetail> PurchaseDetails { get; set; }
    }
}
