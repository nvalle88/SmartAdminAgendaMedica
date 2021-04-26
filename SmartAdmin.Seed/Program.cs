#region Using

using log4net;
using log4net.Config;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using SmartAdminSaludsa;
using System.IO;
using System.Reflection;
using System.Xml;

#endregion

namespace SmartAdmin.Seed
{
    internal class Program
    {
        internal static void Main(string[] args) 
        {

            XmlDocument log4netConfig = new XmlDocument();
            log4netConfig.Load(File.OpenRead("log4net.config"));
            var repo = log4net.LogManager.CreateRepository(Assembly.GetEntryAssembly(),
                       typeof(log4net.Repository.Hierarchy.Hierarchy));
            log4net.Config.XmlConfigurator.Configure(repo, log4netConfig["log4net"]);


            BuildWebHost(args).Run();



        }

        private static IWebHost BuildWebHost(string[] args) => WebHost.CreateDefaultBuilder(args).UseStartup<Startup>().Build();
    }
}
