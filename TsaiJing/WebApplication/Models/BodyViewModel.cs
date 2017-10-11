using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsaiJing.WebApplication.Models
{
    public class BodyViewModel
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public bool HealthSpine { get; set; }
        public bool HealthBackPain { get; set; }
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
        public string CurveFatOther { get; set; }
        public string Diagnosis { get; set; }
    }
}
