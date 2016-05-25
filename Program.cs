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
            
            try
            {
                string userName = GetTeamCityUsername();
                string password = GetTeamCityPassword();
                string teamcityUrl = GetTeamcityUrl();
                int portNumber = GetPortNumber();
                double scanRate = GetScanRate();

                log.Info("service is starting up.");

                HostFactory.Run(
                    service =>
                    {
                        service.Service(settings => new TeamCityApiCloner(new TeamcityConnectionService(userName, password, teamcityUrl, portNumber, scanRate)), s =>
                        {
                            s.BeforeStartingService(_ => Console.WriteLine("BeforeStart"));
                            s.BeforeStoppingService(_ => Console.WriteLine("BeforeStop"));
                        });

                        service.SetStartTimeout(TimeSpan.FromSeconds(10));
                        service.SetStopTimeout(TimeSpan.FromSeconds(10));

                        service.EnableServiceRecovery(esr =>
                        {
                            esr.RestartService(3);
                            esr.RunProgram(7, "ping google.com");
                            esr.RestartComputer(5, "message");

                            esr.OnCrashOnly();
                            esr.SetResetPeriod(2);
                        });

                        service.RunAsLocalSystem();
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

        private static string GetTeamCityUsername()
        {
            return ConfigurationManager.AppSettings["userName"];
        }

        private static string GetTeamCityPassword()
        {
            return ConfigurationManager.AppSettings["password"];
        }

        private static string GetTeamcityUrl()
        {
            return ConfigurationManager.AppSettings["teamcityUrl"];
        }

        private static double GetScanRate()
        {
            double scanRate = 0;
            return double.TryParse(ConfigurationManager.AppSettings["scanRate"], out scanRate) ? scanRate : scanRate;
        }

        private static int GetPortNumber()
        {
            int portNumber = 0;
            return int.TryParse(ConfigurationManager.AppSettings["portNumber"], out portNumber) ? portNumber : portNumber;
        }
    }
}
