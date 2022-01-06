using NewLife.Http;
using NewLife.Log;

namespace Hisense.OpenMES.HttpServer
{
    public class AutoUpdateHandler : IHttpHandler
    {
        public void ProcessRequest(IHttpContext context)
        {
            var salt = context.Parameters["Salt"]?.ToString();
            OperateResult operateResult = AuthSalt(salt);

            if (OperateStatusEnum.Success.Equals(operateResult.Status))
            {
                var version = context.Parameters["version"];

                var files = context.Request.Files;
                if (files != null && files.Length > 0)
                {
                    foreach (var file in files)
                    {
                        file.SaveToFile(@$"D:\{file.FileName}");

                        operateResult = new OperateResult()
                        {
                            Status = OperateStatusEnum.Success,
                            Result = new
                            {
                                file.FileName,
                                file.Length,
                                file.ContentType
                            }
                        };
                    }
                }
            }

            context.Response.SetResult(operateResult);
        }

        private OperateResult AuthSalt(string salt)
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
