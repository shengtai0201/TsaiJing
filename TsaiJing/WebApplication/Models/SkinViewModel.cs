using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsaiJing.WebApplication.Models
{
    public class SkinViewModel
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
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
        public string ImproveOther { get; set; }
        public string Advice { get; set; }
        public string Detail { get; set; }
    }
}
