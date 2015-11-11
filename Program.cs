using System;
using System.Configuration;
using System.IO;
using log4net;
using log4net.Config;
using Topshelf;

namespace TeamcityClonerService
{
    class Program
    {
        static ILog log = LogManager.GetLogger(typeof(Program));
        static void Main()
        {
            var fileInfo = new FileInfo(".\\log4net.config");
            XmlConfigurator.ConfigureAndWatch(fileInfo);
            string userName = string.Empty;
            string password = string.Empty;
            string teamcityUrl = string.Empty;
            int portNumber = 0;

            try
            {
                log.Info("service is starting up.");
                HostFactory.New(
                    service =>
                    {
                        userName = ConfigurationManager.AppSettings["userName"];
                        password = ConfigurationManager.AppSettings["password"];
                        teamcityUrl = ConfigurationManager.AppSettings["teamcityUrl"];
                        portNumber = Int32.Parse(ConfigurationManager.AppSettings["portNumber"]);

                        service.Service<TeamCityApiCloner>(
                            setupService =>
                            {
                                setupService.ConstructUsing(() => new TeamCityApiCloner(new TeamcityConnectionService(userName, password, teamcityUrl, portNumber)));
                            });

                        service.StartAutomaticallyDelayed();
                        service.StartAutomatically();
                        service.EnableServiceRecovery(e => e.RestartService(1));
                        service.SetDescription("Service clones TeamCity API results to public facing server.");
                        service.SetDisplayName("TeamCityApiCloner");
                        service.SetServiceName("TeamCityApiCloner");
                    });
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                log.Error(exception.Message);
            }
        }
    }
}
