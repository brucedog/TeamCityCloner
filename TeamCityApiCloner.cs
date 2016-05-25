using System;
using log4net;
using Topshelf;

namespace TeamcityClonerService
{
    public class TeamCityApiCloner : ServiceControl
    {
        private ITeamcityConnectionService teamcityConnectionService;
        private readonly ILog log = LogManager.GetLogger(typeof(Program));

        public TeamCityApiCloner(ITeamcityConnectionService teamcityConnectionService)
        {
            this.teamcityConnectionService = teamcityConnectionService;
        }

        public bool Start(HostControl hostControl)
        {
            try
            {
                log.Info("TeamCityApiCloner is starting up.");

                teamcityConnectionService.IsConnected = teamcityConnectionService.Connect();
                if (!teamcityConnectionService.IsConnected)
                    throw new Exception("TeamCity service could not connect.");
            }
            catch (Exception exception)
            {
                log.Error(exception.Message);
            }
            return true;
        }

        public bool Stop(HostControl hostControl)
        {
            try
            {
                log.Info("TeamCityApiCloner is stopping.");
                teamcityConnectionService.Dispose();
                teamcityConnectionService = null;
            }
            catch (Exception exception)
            {
                log.Error(exception.Message);
            }
            return true;
        }

        public bool Pause(HostControl hostControl)
        {
            log.Info("TeamCityApiCloner service Paused");

            return true;
        }

        public bool Continue(HostControl hostControl)
        {
            log.Info("TeamCityApiCloner service Continued");

            return true;
        }
    }
}