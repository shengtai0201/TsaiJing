using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsaiJing.WebApplication.Models
{
    public abstract class CustomerBase
    {
        public int? CustomerId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime Birthday { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Introducer { get; set; }
    }
}
