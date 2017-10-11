namespace TsaiJing.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Body")]
    public partial class Body
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CustomerId { get; set; }

        public bool HealthSpine { get; set; }

        public bool HealthBackPain { get; set; }

        [StringLength(64)]
        public string HealthOther { get; set; }

        public bool CurveChest { get; set; }

        public bool CurveArm { get; set; }

        public bool CurveButtock { get; set; }

        public bool CurveStomachWaistAbdomen { get; set; }

        public bool CurveThigh { get; set; }

        public bool CurveCalf { get; set; }

        public bool CurveFatSoft { get; set; }

        public bool CurveFatHard { get; set; }

        public bool CurveFatOrange { get; set; }

        public bool CurveFatTangled { get; set; }

        [StringLength(64)]
        public string CurveFatOther { get; set; }

        [StringLength(128)]
        public string Diagnosis { get; set; }

        public virtual Customer Customer { get; set; }
    }
}
