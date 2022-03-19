using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Services
{
    public interface IConnectionService
    {
        void Connect(string server);
        void Host();
        void Listen();
        void ConnectDiscovery(string command="");
        string GetAddress();
    }
}
