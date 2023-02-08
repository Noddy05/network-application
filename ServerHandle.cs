using System;
namespace NetworkApplication
{
    class ServerHandle
    {
        public static void WelcomeReceived(int _fromClient, Packet _packet)
        {
            int clientId = _packet.ReadInt();
            string username = _packet.ReadString();

            Console.WriteLine($"{Server.clients[_fromClient].tcp.socket.Client.RemoteEndPoint} connected and is now player {clientId}, nickname: {username}.");
            if (_fromClient != clientId)
            {
                Console.WriteLine($"Player \"{username}\" (ID: {_fromClient}) has assumed the wrong ID, {clientId}.");
            }
        }
        public static void UDPTestReceived(int _fromClient, Packet _packet)
        {
            string message = _packet.ReadString();

            Console.WriteLine($"Message received via UDP from client {_fromClient}, {message}.");
        }
        public static void KickClientReceived(int _fromClient, Packet _packet)
        {
            Console.WriteLine($"Succesfully kicked client with ID {_fromClient}.");
        }
    }
}
