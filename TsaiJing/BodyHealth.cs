using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsaiJing
{
    /// <summary>
    /// 健康
    /// </summary>
    public enum BodyHealth : int
    {
        None,

        /// <summary>
        /// 脊椎
        /// </summary>
        Spine,

        /// <summary>
        /// 腰酸背痛
        /// </summary>
        BackPain,

        /// <summary>
        /// 其他
        /// </summary>
        Other
    }
}
