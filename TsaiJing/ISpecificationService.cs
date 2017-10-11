using Shengtai;
using Shengtai.Web.Telerik;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TsaiJing.WebApplication.Models;

namespace TsaiJing
{
    public interface ISpecificationService
    {
        bool DestroyByParent(int parentKey);

        IEnumerable<SpecificationViewModel> ReadSpecifications(DataSourceRequest request);
    }
}
