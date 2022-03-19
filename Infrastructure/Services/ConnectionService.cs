using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Infrastructure.Services
{
    public class ConnectionService : IConnectionService
    {
        private static IPAddress _address { get; set; }
        private static Int32 _port { get; set; }

        public string GetAddress()
        {
            return _address.ToString() + ":" + _port;
        }
        
        //Connect a peer to a given address
        public void Connect(string address)
        {
            try
            {
                // Create a TcpClient.
                // Note, for this client to work you need to have a TcpServer
                // connected to the same address as specified by the server, port
                // combination.
                string server = address.Split(':')[0];
                Int32 port = int.Parse(address.Split(':')[1]);

                if (string.IsNullOrEmpty(server))
                    throw new Exception();

                TcpClient client;

                Console.WriteLine("Connected to: {0} Please Write your message", address);

                while (true)
                {
                    client = new TcpClient(server, port);
                    Console.Write(">");
                    string message = Console.ReadLine();
                    if (message.Trim().ToLower() == "q")
                        break;
                    message = GetAddress() + "*" + message;
                    // Translate the passed message into ASCII and store it as a Byte array.
                    Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);

                    // Get a client stream for reading and writing.
                    NetworkStream stream = client.GetStream();

                    // Send the message to the connected TcpServer.
                    stream.Write(data, 0, data.Length);


                    // Receive the TcpServer.response.

                    // Buffer to store the response bytes.
                    data = new Byte[256];

                    // String to store the response ASCII representation.
                    String responseData = String.Empty;

                    
                    // Close everything.
                    stream.Close();
                };
                client.Close();
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException: {0}", e);
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }


        }

        //start a server for the peers
        public void Host()
        {
            TcpListener server = null;
            try
            {
                Int32 port = Configuration.ClientPort;
                _port = port;
                IPAddress localAddr = IPAddress.Parse(Configuration.ClientHost);
                _address = localAddr;

                // TcpListener server = new TcpListener(port);
                server = new TcpListener(localAddr, port);

                // Start listening for client requests.
                server.Start();
                Console.WriteLine("Peer address {0}:{1}", localAddr, port);
                // Buffer for reading data
                Byte[] bytes = new Byte[256];
                String data = null;

                // Enter the listening loop.
                while (true)
                {

                    // Perform a blocking call to accept requests.
                    // You could also use server.AcceptSocket() here.

                    TcpClient client = server.AcceptTcpClient();

                    data = null;

                    // Get a stream object for reading and writing
                    NetworkStream stream = client.GetStream();

                    int i;

                    // Loop to receive all the data sent by the client.
                    while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        // Translate data bytes to a ASCII string.
                        data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                        var remoteData = data.Split('*');
                        var remoteAddress = remoteData[0];
                        string message = "";
                        for (int k = 1; k < remoteData.Length; k++)
                        {
                            message += remoteData[k];
                        }
                        Console.WriteLine("{0} Says {1}", remoteAddress, message);

                    }

                    // Shutdown and end connection
                    client.Close();
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            finally
            {
                // Stop listening for new clients.
                server.Stop();
            }

        }

        //start a server for the discovery server
        public void Listen()
        {
            TcpListener server = null;
            try
            {
                // Set the TcpListener on port 13000.
                Int32 port = Configuration.ServerPort;
                IPAddress localAddr = IPAddress.Parse(Configuration.ServerHost);

                // TcpListener server = new TcpListener(port);
                server = new TcpListener(localAddr, port);

                // Start listening for client requests.
                server.Start();
                // Buffer for reading data
                Byte[] bytes = new Byte[256];
                String data = null;

                // Enter the listening loop.
                while (true)
                {

                    // Perform a blocking call to accept requests.
                    // You could also use server.AcceptSocket() here.

                    Console.WriteLine("Waiting for peers to connect");
                    TcpClient client = server.AcceptTcpClient();

                    data = null;

                    // Get a stream object for reading and writing
                    NetworkStream stream = client.GetStream();
                    int i;

                    bool cont = true;
                    // Loop to receive all the data sent by the client.
                    while (cont && (i = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        // Translate data bytes to a ASCII string.
                        data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);

                        if (data == "1")
                        {
                            string available = "Available peers\n";
                            if (Peers.Clients.Count < 2)
                                available = "No Available peers\n";
                            foreach (var myClient in Peers.Clients)
                            {
                                available += myClient + "\n";

                            }
                            byte[] message = System.Text.Encoding.ASCII.GetBytes(available);
                            cont = false;
                            stream.Write(message, 0, message.Length);
                        }
                        else if (data[0] == 'r')
                        {
                            Peers.Clients.Remove(data.Split('-')[1].Trim());
                        }
                        else if (data.Trim().ToLower() == "q")
                        {
                            Peers.Clients.Remove(GetAddress());
                        }
                        else
                        {
                            Peers.Clients.Add(data);

                        }

                    }

                    // Shutdown and end connection
                    client.Close();
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
           

        }

        //Connects a peer client to the discovery server
        //at this stage the discovery server will register clients' address
        public void ConnectDiscovery(string command = "")
        {
            try
            {
                // Create a TcpClient.
                // Note, for this client to work you need to have a TcpServer
                // connected to the same address as specified by the server, port
                // combination.
                string server = Configuration.ServerHost;
                Int32 port = Configuration.ServerPort;

                if (string.IsNullOrEmpty(server))
                    throw new Exception();

                TcpClient client = new TcpClient(server, port);

                if (string.IsNullOrEmpty(command))
                {
                    Console.WriteLine("Registered to Discovery Server");
                    Console.WriteLine();
                    NetworkStream address = client.GetStream();
                    Byte[] addressData = System.Text.Encoding.ASCII.GetBytes(_address.ToString() + ":" + _port);

                    // Send the message to the connected TcpServer.
                    address.Write(addressData, 0, addressData.Length);
                    address.Close();
                    client.Close();
                    return;
                }

                else if (command == "1")
                {
                    NetworkStream stream = client.GetStream();
                    Byte[] data = System.Text.Encoding.ASCII.GetBytes(command.ToString());

                    // Send the message to the connected TcpServer.
                    stream.Write(data, 0, data.Length);

                    // Receive the TcpServer.response.

                    // Buffer to store the response bytes.
                    data = new Byte[256];

                    // String to store the response ASCII representation.
                    String responseData = String.Empty;

                    // Read the first batch of the TcpServer response bytes.
                    Int32 bytes = stream.Read(data, 0, data.Length);
                    responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                    Console.WriteLine(responseData.Replace(_address + ":" + _port + "\n", String.Empty));

                }
                else if (command == "r")
                {
                    NetworkStream stream = client.GetStream();
                    Byte[] data = System.Text.Encoding.ASCII.GetBytes("r-" + GetAddress());
                    stream.Write(data, 0, data.Length);
                    client.Close();
                }
                else
                {
                    client.Close();
                    Connect(command);
                }

                client.Close();
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException: {0}", e);
            }
            catch (SocketException)
            {
                Console.WriteLine("Discovery Server Not Started: You can't get any functionality from the discovery server");
                //Console.WriteLine("SocketException: {0}", e);
            }


        }
    }
}
