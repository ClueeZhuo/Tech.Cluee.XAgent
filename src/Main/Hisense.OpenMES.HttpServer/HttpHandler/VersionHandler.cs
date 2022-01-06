using NewLife.Http;
using NewLife.Log;
using NewLife.Serialization;

namespace Hisense.OpenMES.HttpServer
{
    internal class VersionHandler : IHttpHandler
    {
        public void ProcessRequest(IHttpContext context)
        {
            OperateResult operateResult = OperateResult.Failed();

            try
            {
                var salt = context.Parameters["Salt"]?.ToString();
                operateResult = AuthService.AuthSalt(salt);

                if (OperateStatusEnum.Success.Equals(operateResult.Status))
                {
                    var versionJson = context.Parameters["version"]?.ToString();

                    var version = versionJson?.ToJsonEntity<Version>();

                    BaseConfig.UpdateConfig(version);

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
            }
            catch (Exception exp)
            {
                operateResult.Status = OperateStatusEnum.Fail;
                operateResult.Message = exp.Message;

                XTrace.WriteException(exp);
            }

            context.Response.SetResult(operateResult);
        }
    }
}
