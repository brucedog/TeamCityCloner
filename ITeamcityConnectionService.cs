using System;

namespace TeamcityClonerService
{
    public interface ITeamcityConnectionService: IDisposable
    {
        bool Connect();

        void Setup();

        bool IsConnected { get; set; }
    }
}