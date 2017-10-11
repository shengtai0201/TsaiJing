namespace TsaiJing.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Customer")]
    public partial class Customer
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Customer()
        {
            Shipments = new HashSet<Shipment>();
            TrackingRecords = new HashSet<TrackingRecord>();
        }

        public int CustomerId { get; set; }

        [StringLength(128)]
        public string UserId { get; set; }

        [Required]
        [StringLength(64)]
        public string Name { get; set; }

        [Required]
        [StringLength(16)]
        public string Phone { get; set; }

        [StringLength(16)]
        public string Mobile { get; set; }

        public DateTime Birthday { get; set; }

        [StringLength(8)]
        public string Career { get; set; }

        [Required]
        [StringLength(128)]
        public string Address { get; set; }

        [StringLength(16)]
        public string IdCardNumber { get; set; }

        [Required]
        [StringLength(8)]
        public string Introducer { get; set; }

        [StringLength(128)]
        public string ConsultantId { get; set; }

        public int? Height { get; set; }

        public int? Weight { get; set; }

        [StringLength(32)]
        public string Email { get; set; }

        [StringLength(16)]
        public string LineId { get; set; }

        [StringLength(256)]
        public string Remark { get; set; }

        public DateTime? MemberDate { get; set; }

        [StringLength(128)]
        public string MemberRoleId { get; set; }

        public virtual Body Body { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Shipment> Shipments { get; set; }

        public virtual Skin Skin { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TrackingRecord> TrackingRecords { get; set; }
    }
}
