using Shengtai;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TsaiJing.WebApplication.Models;

namespace TsaiJing
{
    public interface ICategoryService
    {
        IList<CategoryViewModel> ReadCategories();
    }
}
