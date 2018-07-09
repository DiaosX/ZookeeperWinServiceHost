using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace ZookeeperWinServiceHost
{
    public class Startup
    {
        static int m_ProcessId;
        public Startup()
        {
        }
        private int StartProcess(string cmdScript)
        {
            var processInfo = new ProcessStartInfo("cmd.exe", "/c " + cmdScript)
            {
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                ErrorDialog = true
            };
            //processInfo.WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory;
            var process = Process.Start(processInfo);
            process.OutputDataReceived += (object sender, DataReceivedEventArgs e) =>
            {
                LogUtil.WriteInfo("output>>" + e.Data);
            };
            process.BeginOutputReadLine();

            process.ErrorDataReceived += (object sender, DataReceivedEventArgs e) =>
            {
                LogUtil.WriteError("error>>" + e.Data);
            };
            process.BeginErrorReadLine();
            return process.Id;
        }
        public void Start()
        {
            LogUtil.WriteInfo("output>>zookeeper server is starting....");
            m_ProcessId = StartProcess(GetZkServerStartCmd());
            LogUtil.WriteInfo("output>>zookeeper server is started,the cmd.exe pid is " + m_ProcessId);
        }
        public void Stop()
        {
            if (m_ProcessId != 0)
            {
                LogUtil.WriteInfo("output>>zookeeper server is stopping,the cmd.exe pid is" + m_ProcessId);
                StartProcess(GetZkServerStopCmd());
                LogUtil.WriteInfo("output>>zookeeper server is stopped.");
                m_ProcessId = 0;
            }
            else
            {
                LogUtil.WriteError("error>>zookeeper server process id is 0.");
            }
        }
        private string GetZkServerStartCmd()
        {
            var basePath = AppDomain.CurrentDomain.BaseDirectory;
            var cmdScript = Path.Combine(basePath, "zkServer.cmd");
            return cmdScript;
        }
        private string GetZkServerStopCmd()
        {
            var cmdScript = string.Format("taskkill /PID {0} /F /T ", m_ProcessId);
            return cmdScript;
        }
        public static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            var dllPath = "ZookeeperWinServiceHost.dll." + new AssemblyName(args.Name).Name + ".dll";
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(dllPath))
            {
                byte[] assemblyData = new byte[stream.Length];
                stream.Read(assemblyData, 0, assemblyData.Length);
                return Assembly.Load(assemblyData);
            }
        }
        internal static string ServiceName
        {
            get
            {
                var serviceName = ConfigUtil.GetConfigVal("ServiceName", _ => _, "");
                if (string.IsNullOrEmpty(serviceName))
                {
                    throw new ArgumentNullException("The service name is not allowed to be null.");
                }
                return serviceName;
            }
        }
        internal static string ServiceDesc
        {
            get
            {
                return ConfigUtil.GetConfigVal("ServiceDesc", _ => _, ServiceName);
            }
        }
        internal static string ServiceDisplayName
        {
            get
            {
                return ConfigUtil.GetConfigVal("serviceDisplayName", _ => _, ServiceName);
            }
        }
    }
}

