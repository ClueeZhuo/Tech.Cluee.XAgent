using NewLife.Log;

namespace Hisense.OpenMES.HttpServer
{
    public class HttpService
    {
        public HttpService()
        {

        }

        public void DoWork(Object state)
        {
            try
            {
                XTrace.WriteLine(state?.ToString());

                XTrace.UseConsole();

                var server = new NewLife.Http.HttpServer
                {
                    Port = ConfigService.ServerPort,
                    Log = XTrace.Log,
                    SessionLog = XTrace.Log
                };
                server.Map("/", () => $@"<h1>HttpServer is running ~ {DateTime.Now.ToFullString()}</h1>");
                server.Map("/autoupdate", new AutoUpdateHandler());
                server.Start();

                Console.ReadLine();
            }
            catch (Exception exp)
            {
                XTrace.WriteException(exp);
            }
        }
    }
}
