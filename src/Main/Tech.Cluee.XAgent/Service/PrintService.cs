
using Amib.Threading;

using NewLife.Agent;
using NewLife.Log;
using NewLife.Threading;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Tech.Cluee.Blazor.XAgent
{
    public class PrintService : ServiceBase
    {
        #region 属性/字段

        private readonly string[] _processAddress;

        #endregion

        #region 构造函数

        /// <summary>
        /// 实例化一个代理服务
        /// </summary>
        public PrintService()
        {
            // 一般在构造函数里面指定服务名
            ServiceName = "PrintAgent";

            DisplayName = "新生命服务代理";
            Description = "用于承载各种服务的服务代理！";

            Setting set = Setting.Current;

            _processAddress = set.WatchDog.Split(','); ;
        }
        #endregion

        #region 核心

        private TimerX _DaemonTimer;

        /// <summary>
        /// 开始服务
        /// </summary>
        /// <param name="reason"></param>
        protected override void StartWork(string reason)
        {
            WriteLog("业务开始……");

            _DaemonTimer = new TimerX(DoScannWork, reason, "0/10 * * * * ?") { Async = true };

            //_DaemonTimer = new TimerX(DoScannWork, reason, 10_000, 24 * 3600 * 1000) { Async = true };

            base.StartWork(reason);
        }

        /// <summary>
        /// 扫描业务循环工作
        /// </summary>
        /// <param name="state"></param>
        private void DoScannWork(object state)
        {
            WriteLog($"*********{state}**********");

            try
            {
                if (_processAddress != null && _processAddress.Length > 0)
                {
                    foreach (string address in _processAddress)
                    {
                        string fullPathAddress = address.GetFullPath().ToLower();
                        if (File.Exists(fullPathAddress))
                        {
                            Process[] arrayProcess = Process.GetProcesses();
                            foreach (Process process in arrayProcess)
                            {
                                //System、Idle进程会拒绝访问其全路径
                                if (process.ProcessName != "System" && process.ProcessName != "Idle")
                                {
                                    try
                                    {
                                        if (fullPathAddress.Equals(process.MainModule?.FileName?.ToLower()?.Trim()))
                                        {
                                            WriteLog("进程已启动");

                                            //DaemonProcess(process, address);

                                            DaemonProcessActor(process, fullPathAddress);

                                            return;
                                        }
                                    }
                                    catch (Exception)
                                    {
                                        //XTrace.WriteException(exp);
                                    }
                                }
                            }

                            WriteLog("进程未启动");

                            //1.Process
                            //Process startProcess = new Process();
                            //startProcess.StartInfo.WorkingDirectory = @"D:\";
                            //startProcess.StartInfo.FileName = fullPathAddress;
                            //startProcess.StartInfo.CreateNoWindow = false;

                            //startProcess.Start();

                            //2. Cjwdev.WindowsApi.dll

                            //3.

                            string directory = fullPathAddress.AsDirectory().Parent.FullName;

                            bool createProcessResult = DLLImport.CreateProcess(fullPathAddress, "d:\\", out int appProcessId);



                            if (createProcessResult)
                            {
                                Process startProcess = Process.GetProcessById(appProcessId);

                                //DaemonProcess(startProcess, fullPathAddress);

                                DaemonProcessActor(startProcess, fullPathAddress);
                            }
                            else
                            {
                                XTrace.WriteException(new Exception("启动进程失败"));
                            }
                        }
                        else
                        {
                            XTrace.WriteException(new Exception($"文件不存在-{address}"));
                        }
                    }
                }
                else
                {
                    XTrace.WriteException(new Exception("未配置-WatchDog"));
                }

            }
            catch (Exception exp)
            {
                XTrace.WriteException(exp);
            }


        }

        /// <summary>
        /// 守护进程-线程池版
        /// </summary>
        /// <param name="process"></param>
        /// <param name="address"></param>
        private void DaemonProcess(Process process, string address)
        {
            ProcessHandler processHandler = new ProcessHandler(process, address);

            List<string> resultList = new List<string>();

            try
            {
                //using ()
                //{
                SmartThreadPool smartThreadPool = new SmartThreadPool(new STPStartInfo
                {
                    MinWorkerThreads = 1, //最小线程数
                    MaxWorkerThreads = 10, //最大线程数
                    AreThreadsBackground = true, //设置为后台线程
                });
                List<IWorkItemResult> workItemResults = new List<IWorkItemResult>();

                object[] restartProcessPlan = new object[] { "RESTART-HELLO" };
                object[] hangProcessPlan = new object[] { "HANG-HELLO" };
                object[] werFaultProcessPlan = new object[] { "WERFAULT-HELLO" };

                //workItemResults.Add(smartThreadPool.QueueWorkItem(new WorkItemCallback(processHandler.BeTouchRestartProcess), restartProcessPlan));
                workItemResults.Add(smartThreadPool.QueueWorkItem(new WorkItemCallback(processHandler.BeTouchHangProcess), hangProcessPlan));
                workItemResults.Add(smartThreadPool.QueueWorkItem(new WorkItemCallback(processHandler.BeTouchWerFaultProcess), werFaultProcessPlan));

                //smartThreadPool.Start();

                if (SmartThreadPool.WaitAll(workItemResults.ToArray()))
                {
                    foreach (IWorkItemResult t in workItemResults)
                    {
                        resultList.Add((string)t.Result);
                    }
                }
                //}
            }
            catch (Exception)
            {

            }

            foreach (string item in resultList)
            {
                XTrace.WriteLine(item);
            }

            //new Thread(new ThreadStart(processHandler.BeTouchRestartProcess)).Start();
        }

        /// <summary>
        /// 守护进程-模型版
        /// </summary>
        /// <param name="process"></param>
        /// <param name="address"></param>
        private void DaemonProcessActor(Process process, string address)
        {
            XTrace.WriteLine("HANG-ACOTR-INIT");

            HangProcessActor actor = new HangProcessActor(process, address);

            XTrace.WriteLine("HANG-ACOTR-START");

            actor.Tell("HANG-ACOTR-HELLO");

            XTrace.WriteLine("HANG-ACOTR-WAIT");

            actor.Stop();

            XTrace.WriteLine("HANG-ACOTR-END");
        }

        /// <summary>
        /// 停止服务
        /// </summary>
        /// <param name="reason"></param>
        protected override void StopWork(string reason)
        {
            WriteLog("业务结束！");

            _DaemonTimer.Dispose();

            base.StopWork(reason);
        }

        #endregion
    }

}
