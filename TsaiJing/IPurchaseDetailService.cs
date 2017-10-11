using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsaiJing
{
    public interface IPurchaseDetailService
    {
        bool DestroyByParent(int parentKey);
        int GetTotalAmount(int purchaseId);
    }
}
