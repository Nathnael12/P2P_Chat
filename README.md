## **Peer to Peer (P2P) Network**

>In its simplest form, a peer-to-peer (P2P) network is created when two or more PCs are connected and share resources without going through a separate server computer 
>
><cite>Definition according to [Computer World](https://www.computerworld.com/article/2588287/networking-peer-to-peer-network.html)</cite>

## P2P Chat
In this project I tried to implement peer to peer connection to create a simple Chat app. You will have a Discovery server and peer clients in this project.
Each peer will have to connect with the discovery server first. Then the discovery server will register the address of each connected peers.
<br/>
In this Solution, you will find three projects 
- P2P Discovery Server,
- P2P Client, and
- Infrastructure
<br/>

The P2P Discovery Server should run first before any P2P Client is started. To do so, you can run the P2P Discovery Server project only by writing this command in CLI 
```bash
 dotnet run --project P2PDiscoveryServer
```
Now you are ready to run the peer clients. You can do this by running the following command.
```bash
dotnet run --project P2PClient
```
*Remember:* You should have at least two P2PClient instances running to have a chat.<br/>
Once the discovery server is started and the clients are started, You will see available commands in the clients side.
### How does it work?
Simply put, the discovery server has only one job. To listen to coming connections from any peer client. However, peers act as both a server and a client.
The server of one peer will be listening to connections from another peer (acting as a server). Also, a peer will connect to another peer's server (acting as a client).
## Contributing
Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.

