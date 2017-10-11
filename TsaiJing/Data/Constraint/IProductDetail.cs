using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsaiJing.Data
{
    public interface IProductDetail
    {
        ProductSpecification FirstSpecification { get; set; }
        ProductSpecification SecondSpecification { get; set; }
    }
}
