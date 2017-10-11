using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TsaiJing.Data;

namespace TsaiJing
{
    public interface IShipmentDetailService
    {
        bool DestroyByParent(int parentKey);
    }
}
