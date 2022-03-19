using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Models
{
    public static class Configuration
    {
        //the peer clients are going to create a server at 127.0.0.1:[random port]
        //the discovery server is listening to port 13000 at 127.0.0.1

        public static string ClientHost { get; set; } = "127.0.0.1";
        public static string ServerHost { get; set; } = "127.0.0.1";
        public static int ClientPort { get; set; } = new Random().Next(13001, 14000);
        public static int ServerPort { get; set; } = 13000;

    }

}
