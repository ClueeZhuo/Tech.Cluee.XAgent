namespace Hisense.OpenMES.HttpServer
{
    /// <summary>
    /// 处理相应业务返回结果
    /// </summary>
    [Serializable]
    internal class OperateResult
    {
        /// <summary>
        /// 操作结果构造函数
        /// </summary>
        internal OperateResult()
        {

        }

        /// <summary>
        /// 操作结果构造函数
        /// </summary>
        /// <param name="status">操作状态</param>
        /// <param name="Message">提示信息</param>
        internal OperateResult(OperateStatusEnum status, string Message)
        {
            this.Status = status;
            this.Message = Message;
        }

        /// <summary>
        /// 操作结果构造函数
        /// </summary>
        /// <param name="status">操作状态</param>
        /// <param name="Message">提示信息</param>
        /// <param name="businessDetail">业务信息</param>
        internal OperateResult(OperateStatusEnum status, string Message, string businessDetail)
        {
            this.Status = status;
            this.Message = Message;
            this.BusinessDetail = businessDetail;
        }

        /// <summary>
        /// 处理结果
        /// </summary>
        internal OperateStatusEnum Status { get; set; }

        /// <summary>
        /// 处理业务信息/错误信息
        /// </summary>
        internal string Message { get; set; }

        /// <summary>
        /// 处理业务结果
        /// </summary>
        internal object Result { get; set; }

        /// <summary>
        /// 操作结果返回信息
        /// </summary>
        internal string BusinessDetail { get; set; }

        #region 操作结果

        /// <summary>
        /// 无数据操作
        /// </summary>
        /// <param name="message">提示信息</param>
        /// <returns></returns>
        internal static OperateResult None(string message = "无数据")
        {
            return new OperateResult()
            {
                Status = OperateStatusEnum.None,
                Message = message
            };
        }

        /// <summary>
        /// 操作成功
        /// </summary>
        /// <param name="message">提示信息</param>
        /// <returns></returns>
        internal static OperateResult Success(string message = "操作成功")
        {
            return new OperateResult()
            {
                Status = OperateStatusEnum.Success,
                Message = message
            };
        }

        /// <summary>
        /// 操作成功
        /// </summary>
        /// <param name="result">业务结果</param>
        /// <returns></returns>
        internal static OperateResult Success<T>(T result)
        {
            return new OperateResult()
            {
                Status = OperateStatusEnum.Success,
                Result = result
            };
        }

        /// <summary>
        /// 操作失败
        /// </summary>
        /// <param name="message">提示信息</param>
        /// <returns></returns>
        internal static OperateResult Failed(string message = "操作失败")
        {
            return new OperateResult()
            {
                Status = OperateStatusEnum.Fail,
                Message = message
            };
        }

        /// <summary>
        /// 操作结果未知
        /// </summary>
        /// <param name="message">提示信息</param>
        /// <returns></returns>
        internal static OperateResult Unknown(string message = "操作结果未知")
        {
            return new OperateResult()
            {
                Status = OperateStatusEnum.Unknown,
                Message = message
            };
        }

        #endregion

    }
}
