using System.ComponentModel;

namespace Hisense.OpenMES.HttpServer
{
    /// <summary>
    /// 逻辑业务处理状态
    /// </summary>
    public enum OperateStatusEnum
    {
        /// <summary>
        /// 不作任何业务处理=0
        /// </summary>
        [Description("不作任何业务处理")]
        None = 0,

        /// <summary>
        /// 业务处理成功=1
        /// </summary>
        [Description("业务处理成功")]
        Success = 1,

        /// <summary>
        /// 业务处理失败=2
        /// </summary>
        [Description("业务处理失败")]
        Fail = 2,

        /// <summary>
        /// 业务结果未知=3
        /// </summary>
        [Description("业务结果未知")]
        Unknown = 3
    }
}
