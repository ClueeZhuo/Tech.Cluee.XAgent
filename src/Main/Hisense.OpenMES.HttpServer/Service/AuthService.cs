using NewLife.Log;

namespace Hisense.OpenMES.HttpServer
{
    internal static class AuthService
    {
        internal static OperateResult AuthSalt(string salt)
        {
            OperateResult operateResult = OperateResult.Failed();

            try
            {
                if (DateTime.Now.AddDays(1).ToString("yyyyMMdd").Equals(salt))
                {
                    operateResult = OperateResult.Success();
                }
                else
                {
                    operateResult.Status = OperateStatusEnum.Fail;
                    operateResult.Message = "验证失败";
                }
            }
            catch (Exception exp)
            {
                operateResult.Status = OperateStatusEnum.Fail;
                operateResult.Message = exp.Message;

                XTrace.WriteException(exp);
            }

            return operateResult;
        }
    }
}
