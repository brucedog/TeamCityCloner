using System;
using System.Collections.Generic;
using TeamCitySharp;
using TeamCitySharp.DomainEntities;

namespace TeamcityClonerService
{
    public class TeamcityConnectionService : ITeamcityConnectionService
    {
        private string password;
        private readonly string teamcityUrl;
        private readonly int portNumber;
        private string userName;
        private TeamCityClient client;

        public TeamcityConnectionService(string userName, string password, string teamcityUrl, int portNumber)
        {
            this.userName = userName;
            this.password = password;
            this.teamcityUrl = teamcityUrl;
            this.portNumber = portNumber;
        }

        public bool Connect()
        {
            try
            {
                client = new TeamCityClient(teamcityUrl + portNumber);
                client.Connect(userName, password);
                Projects = client.Projects.All();                
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                return false;
            }

            return true;
        }

        public List<Project> Projects { get; set; }
    }
}