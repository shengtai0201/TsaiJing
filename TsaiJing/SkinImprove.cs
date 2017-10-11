using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsaiJing
{
    /// <summary>
    /// 改善膚況
    /// </summary>
    public enum SkinImprove : int
    {
        None,

        /// <summary>
        /// 粉刺
        /// </summary>
        Acne,

        /// <summary>
        /// 敏感
        /// </summary>
        Sensitive,

        /// <summary>
        /// 皺紋
        /// </summary>
        Wrinkle,

        /// <summary>
        /// 毛孔粗大
        /// </summary>
        LargePores,

        /// <summary>
        /// 斑
        /// </summary>
        Spot,

        /// <summary>
        /// 暗沉
        /// </summary>
        Dull,

        /// <summary>
        /// 痘疤
        /// </summary>
        Pock,

        /// <summary>
        /// 其他
        /// </summary>
        Other
    }
}
