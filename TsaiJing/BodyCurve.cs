using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsaiJing
{
    /// <summary>
    /// 曲線
    /// </summary>
    public enum BodyCurve : int
    {
        None,

        /// <summary>
        /// 胸部(豐、減、維持)
        /// </summary>
        Chest,

        /// <summary>
        /// 手臂
        /// </summary>
        Arm,

        /// <summary>
        /// 臀部
        /// </summary>
        Buttock,

        /// <summary>
        /// 胃、腰、腹部
        /// </summary>
        StomachWaistAbdomen,

        /// <summary>
        /// 大腿
        /// </summary>
        Thigh,

        /// <summary>
        /// 小腿
        /// </summary>
        Calf,

        /// <summary>
        /// 脂肪狀況
        /// </summary>
        FatCondition
    }
}
