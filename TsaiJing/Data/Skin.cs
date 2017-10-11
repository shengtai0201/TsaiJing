namespace TsaiJing.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Skin")]
    public partial class Skin
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CustomerId { get; set; }

        public bool ConditionDry { get; set; }

        public bool ConditionOily { get; set; }

        public bool ConditionSensitivity { get; set; }

        public bool ConditionMixed { get; set; }

        public bool ImproveAcne { get; set; }

        public bool ImproveSensitive { get; set; }

        public bool ImproveWrinkle { get; set; }

        public bool ImproveLargePores { get; set; }

        public bool ImproveSpot { get; set; }

        public bool ImproveDull { get; set; }

        public bool ImprovePock { get; set; }

        [StringLength(64)]
        public string ImproveOther { get; set; }

        [StringLength(128)]
        public string Advice { get; set; }

        [StringLength(256)]
        public string Detail { get; set; }

        public virtual Customer Customer { get; set; }
    }
}
