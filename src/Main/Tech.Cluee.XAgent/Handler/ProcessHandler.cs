using NewLife.Log;
using NewLife.Model;

using System;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace Tech.Cluee.Blazor.XAgent
{
    public class ProcessHandler
    {
        private readonly Process _process;
        private readonly string _address;

        public ProcessHandler(Process process, string address)
        {
            _process = process;
            _address = address;
        }

        /// <summary>
        /// 重启进程
        /// </summary>
        /// <param name="plan">计划任务</param>
        public object BeTouchRestartProcess(object plan)
        {
            XTrace.WriteLine("重启作业守护开始");

            string hello = (string)(plan as object[])[0];

            XTrace.WriteLine(hello);

            try
            {
                //1.获取鼠标位置
                //Point point = new Point(0, 0);
                //GetCursorPos(ref point);

                //2.获取窗口句柄
                //IntPtr formHandle = WindowFromPoint(point);//得到窗口句柄  p为当前位置（Point）

                //3.根据句柄获取窗口标题：    
                //StringBuilder title = new StringBuilder(256);
                //GetWindowText(formHandle, title, title.Capacity);//得到窗口的标题

                XTrace.WriteLine("RESTART-WAITFOREXIT");

                _process.WaitForExit();
                _process.Close();    //释放已退出进程的句柄
                _process.StartInfo.FileName = _address;
                _process.Start();

                XTrace.WriteLine("RESTART-NEXT");
            }
            catch (Exception exp)
            {
                XTrace.WriteException(exp);
            }

            return "RESTART-SUCCESS";
        }

        /// <summary>
        /// 挂起经常监控
        /// </summary>
        /// <param name="plan">计划任务</param>
        public object BeTouchHangProcess(object plan)
        {
            XTrace.WriteLine("挂起作业守护开始");

            string hello = (string)(plan as object[])[0];

            XTrace.WriteLine(hello);

            try
            {
                //1.获取鼠标位置
                //Point point = new Point(0, 0);
                //GetCursorPos(ref point);

                //2.获取窗口句柄
                //IntPtr formHandle = WindowFromPoint(point);//得到窗口句柄  p为当前位置（Point）

                //3.根据句柄获取窗口标题：    
                //StringBuilder title = new StringBuilder(256);
                //GetWindowText(formHandle, title, title.Capacity);//得到窗口的标题

                //Spy++获取Ghost窗体句柄
                IntPtr formHandle = DLLImport.FindWindow("Ghost", null);

                if (formHandle != IntPtr.Zero)
                {
                    StringBuilder title = new StringBuilder(256);
                    DLLImport.GetWindowText(formHandle, title, title.Capacity);

                    XTrace.WriteLine($"HANG-TITLE-{title}");

                    XTrace.WriteLine("HANG-GHOST");

                    if (title.ToString().Contains("未响应"))
                    {
                        _process.Kill();
                        _process.Close();
                    }
                }
                XTrace.WriteLine("HANG-NEXT");
            }
            catch (Exception exp)
            {
                XTrace.WriteException(exp);
            }

            return "HANG-SUCCESS";
        }

        /// <summary>
        /// 已停止工作进程监控
        /// </summary>
        public object BeTouchWerFaultProcess(object plan)
        {
            XTrace.WriteLine("故障作业守护开始");

            string hello = (string)(plan as object[])[0];

            XTrace.WriteLine(hello);

            try
            {
                Process[] arrayProcess = Process.GetProcesses();
                foreach (Process process in arrayProcess)
                {
                    //System、Idle进程会拒绝访问其全路径
                    if (process.ProcessName != "System" && process.ProcessName != "Idle")
                    {
                        try
                        {
                            string fileName = process.MainModule?.FileName?.ToLower() ?? "";

                            if (fileName.Contains("werfault.exe"))
                            {
                                process.Kill();
                                process.Close();

                                _process.Kill();
                                _process.Close();
                            }
                        }
                        catch (Exception)
                        {
                            //XTrace.WriteException(exp);
                        }
                    }
                }

                XTrace.WriteLine("WERFAULT-NEXT");
            }
            catch (Exception exp)
            {
                XTrace.WriteException(exp);
            }

            return "WERFAULT-SUCCESS";
        }
    }

    /// <summary>
    /// 挂起模型
    /// </summary>
    public class HangProcessActor : Actor
    {
        private readonly Process _process;
        private readonly string _address;

        public HangProcessActor(Process process, string address)
        {
            _process = process;
            _address = address;
        }

        protected override async Task ReceiveAsync(ActorContext context)
        {
            XTrace.WriteLine("挂起模型作业守护开始");

            XTrace.WriteLine(context.Message?.ToString());

            try
            {
                //1.获取鼠标位置
                //Point point = new Point(0, 0);
                //GetCursorPos(ref point);

                //2.获取窗口句柄
                //IntPtr formHandle = WindowFromPoint(point);//得到窗口句柄  p为当前位置（Point）

                //3.根据句柄获取窗口标题：    
                //StringBuilder title = new StringBuilder(256);
                //GetWindowText(formHandle, title, title.Capacity);//得到窗口的标题

                //Spy++获取Ghost窗体句柄
                IntPtr formHandle = DLLImport.FindWindow("Ghost", null);

                if (formHandle != IntPtr.Zero)
                {
                    StringBuilder title = new StringBuilder(256);
                    DLLImport.GetWindowText(formHandle, title, title.Capacity);

                    XTrace.WriteLine($"HANG-TITLE-{title}");

                    XTrace.WriteLine("HANG-GHOST");

                    if (title.ToString().Contains("未响应"))
                    {
                        _process.Kill();
                        _process.Close();
                    }
                }
                XTrace.WriteLine("HANG-NEXT");
            }
            catch (Exception exp)
            {
                XTrace.WriteException(exp);
            }

            await Task.Delay(3000);
        }
    }
}