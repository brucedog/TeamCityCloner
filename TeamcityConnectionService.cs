using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using TeamCitySharp;
using TeamCitySharp.DomainEntities;

namespace TeamcityClonerService
{
    public class TeamcityConnectionService : ITeamcityConnectionService, IDisposable
    {
        private string password;
        private readonly string teamcityUrl;
        private readonly int portNumber;
        private string userName;
        private TeamCityClient client;
        private IObservable<long> observableTeamcityConnectionService;
        private IDisposable onlineSubscription;
        private double pingRate;

        public TeamcityConnectionService(string userName, string password, string teamcityUrl, int portNumber)
        {
            this.userName = userName;
            this.password = password;
            this.teamcityUrl = teamcityUrl;
            this.portNumber = portNumber;
        }

        public bool IsConnected { get; set; }
        public List<Project> Projects { get; set; }

        public bool Connect()
        {
            try
            {
                client = new TeamCityClient(teamcityUrl + portNumber);
                client.Connect(userName, password);                
                Projects = client.Projects.All();

                observableTeamcityConnectionService = Observable.Timer(TimeSpan.FromSeconds(5), TimeSpan.FromMinutes(pingRate));
                onlineSubscription = observableTeamcityConnectionService.Subscribe(CheckTeamCityApi);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                return false;
            }

            return true;
        }

        public void Setup()
        {
            if (!IsConnected)
                Connect();
        }

        public void Dispose()
        {
            onlineSubscription.Dispose();
            onlineSubscription = null;
        }

        private void CheckTeamCityApi(long notUsed)
        {
            
        }
    }
}