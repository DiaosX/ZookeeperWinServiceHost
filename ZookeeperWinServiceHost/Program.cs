using System;
using Topshelf;

namespace ZookeeperWinServiceHost
{
    class Program
    {
        static Program()
        {
            AppDomain.CurrentDomain.AssemblyResolve += Startup.CurrentDomain_AssemblyResolve;
        }
        static void Main(string[] args)
        {
            HostFactory.Run(x =>
            {
                x.Service<Startup>(c =>
                {
                    c.ConstructUsing(name => new Startup());
                    c.WhenStarted(s => s.Start());
                    c.WhenStopped(s => s.Stop());
                });
                x.RunAsLocalSystem();
                x.SetServiceName(Startup.ServiceName);
                x.SetDisplayName(Startup.ServiceDisplayName);
                x.SetDescription(Startup.ServiceDesc);
            });
        }
    }
}
