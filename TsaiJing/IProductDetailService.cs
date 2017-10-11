using Shengtai.Web.Telerik;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using TsaiJing.WebApplication.Models;

namespace TsaiJing
{
    public interface IProductDetailService
    {
        bool DestroyByParent(int parentKey);
        IEnumerable<TextValueViewModel<int>> ReadProductDetails(DataSourceRequest request);
    }
}
