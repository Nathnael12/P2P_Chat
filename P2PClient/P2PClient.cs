using Infrastructure.Models;
using Infrastructure.Services;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace P2PClient
{
    public class P2PClient
    {
        static void Main(string[] args)
        {
            IConnectionService peer = new ConnectionService();
            
            //Create a host server using another thread
            Task.Factory.StartNew(() => peer.Host());
            
            //connect to discovery server to get registered
            peer.ConnectDiscovery();

            //Show commands and accept user input
            Console.WriteLine("\nAVAILABLE COMMANDS");
            Console.WriteLine(" LIST \t\t \t view all peer addresses\n");
            Console.WriteLine(" Q \t\t \t Quit\n");
            Console.WriteLine(" PEER - [Peer Address] \t connect to peer client\n");
            while (true)
            {
                Console.WriteLine("ENTER YOUR COMMAND\n");
                string command = Console.ReadLine();
                switch (command.Trim().Split('-')[0].Trim().ToLower())
                {
                    case "list":
                        peer.ConnectDiscovery("1");
                        break;
                    case "q":
                        //Peers Clients = new Peers();
                        peer.ConnectDiscovery("r");
                        return;
                    case "peer":
                        string peerAddress = command.Trim().Split('-')[1].Trim();
                        peer.ConnectDiscovery(peerAddress);
                        break;
                    default:
                        return;
                }
            }
            
        }
        
        
    }
    
}

