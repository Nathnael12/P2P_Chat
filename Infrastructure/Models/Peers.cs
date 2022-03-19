using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace Infrastructure.Models
{
    //This class will contain a list of Peer Clients that are connected to the discovery server
    // the lists are displayed to other peer clients up on request
    public static class Peers
    {
        public static List<string> Clients { get; set; } = new List<string>();
    }
}
