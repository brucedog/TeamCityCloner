using System;
using log4net;
using Topshelf;

namespace TeamcityClonerService
{
    public class TeamCityApiCloner : ServiceControl
    {
        private readonly ITeamcityConnectionService teamcityConnectionService;
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
                if (teamcityConnectionService.Connect())
                {
                    // TODO Add timer to pull teamcity results and publish them on public server.
                }
                else
                {
                    throw new Exception("Could not connect to the teamcity server.");
                }
            }
            catch (Exception exception)
            {
                log.Error(exception.Message);
                return false;
            }
            return true;
        }

        public bool Stop(HostControl hostControl)
        {
            try
            {
                log.Info("TeamCityApiCloner is stopping.");
                
            }
            catch (Exception exception)
            {
                log.Error(exception.Message);
                return false;
            }
            return true;
        }
    }
}