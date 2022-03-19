using Infrastructure.Models;
using Infrastructure.Services;
using System;
using System.Threading.Tasks;

namespace P2PDiscoveryServer
{
    internal class P2PDiscoveryServer
    {
        static void Main(string[] args)
        {
            IConnectionService discovery = new ConnectionService();

            //discovery server will be listening once the program started
            discovery.Listen();
            
        }
    }
}
