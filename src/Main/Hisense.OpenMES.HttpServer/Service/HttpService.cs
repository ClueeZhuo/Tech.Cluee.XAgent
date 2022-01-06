using NewLife.Log;

namespace Hisense.OpenMES.HttpServer
{
    internal class HttpService
    {
        internal HttpService()
        {

        }

        internal void DoWork(Object state)
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
                server.Map("/Version", new VersionHandler());
                server.Map("/AutoUpdate", new AutoUpdateHandler());
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
