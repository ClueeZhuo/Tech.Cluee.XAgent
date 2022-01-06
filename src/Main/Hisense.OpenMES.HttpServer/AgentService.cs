using NewLife.Agent;
using NewLife.Log;
using NewLife.Threading;

internal class AgentService : ServiceBase
{
    #region 属性
    #endregion

    #region 构造函数
    /// <summary>
    /// 
    /// </summary>
    internal AgentService()
    {
        ServiceName = "OpenMES.HttpServer-Agent";

        DisplayName = "OpenMES.HttpServer服务代理";
        Description = "用于承载各种服务的服务代理！";
    }
    #endregion

    #region 核心
    private TimerX _timer;
    /// <summary>开始工作</summary>
    /// <param name="reason"></param>
    protected override void StartWork(String reason)
    {
        WriteLog("业务开始……");

        _timer = new TimerX(DoWork, reason, 0, 24 * 3600 * 1000) { Async = true };

        base.StartWork(reason);
    }

    private void DoWork(Object state)
    {
        XTrace.WriteLine("HttpService-START");
        new HttpService().DoWork(state);
        XTrace.WriteLine("HttpService-END");
    }

    /// <summary>停止服务</summary>
    /// <param name="reason"></param>
    protected override void StopWork(String reason)
    {
        WriteLog("业务结束！");

        _timer?.Dispose();

        base.StopWork(reason);
    }
    #endregion
}