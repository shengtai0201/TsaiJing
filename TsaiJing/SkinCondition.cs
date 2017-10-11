using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsaiJing
{
    /// <summary>
    /// 膚質狀況
    /// </summary>
    public enum SkinCondition : int
    {
        None,

        /// <summary>
        /// 乾性
        /// </summary>
        Dry,

        /// <summary>
        /// 油性
        /// </summary>
        Oily,

        /// <summary>
        /// 敏感性
        /// </summary>
        Sensitivity,

        /// <summary>
        /// 混合性
        /// </summary>
        Mixed
    }
}
