using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkApplication
{
    class ServerSend
    {
        private static void SendTCPData(int _toClient, Packet _packet)
        {
            _packet.WriteLength();
            Server.clients[_toClient].tcp.SendData(_packet);
        }
        private static void SendUDPData(int _toClient, Packet _packet)
        {
            _packet.WriteLength();
            Server.clients[_toClient].udp.SendData(_packet);
        }
        private static void SendTCPDataToAll(Packet _packet)
        {
            _packet.WriteLength();
            for (int i = 0; i < Server.maxPlayers; i++)
            {
                Server.clients[i].tcp.SendData(_packet);
            }
        }
        private static void SendTCPDataToAll(int _exceptClient, Packet _packet)
        {
            _packet.WriteLength();
            for (int i = 0; i < Server.maxPlayers; i++)
            {
                if (i == _exceptClient)
                    continue;
                Server.clients[i].tcp.SendData(_packet);
            }
        }
        private static void SendUDPDataToAll(Packet _packet)
        {
            _packet.WriteLength();
            for (int i = 0; i < Server.maxPlayers; i++)
            {
                Server.clients[i].udp.SendData(_packet);
            }
        }
        private static void SendUDPDataToAll(int _exceptClient, Packet _packet)
        {
            _packet.WriteLength();
            for (int i = 0; i < Server.maxPlayers; i++)
            {
                if (i == _exceptClient)
                    continue;
                Server.clients[i].udp.SendData(_packet);
            }
        }

        public static void WelcomePacket(int _toClient, string _message)
        {
            using (Packet packet = new Packet((int)ServerPackets.welcome))
            {
                packet.Write(_message);
                packet.Write(_toClient);

                SendTCPData(_toClient, packet);
            }
        }
        public static void UDPTest(int _toClient)
        {
            using (Packet packet = new Packet((int)ServerPackets.setPlayerPosition))
            {
                packet.Write("testing udp");

                SendUDPData(_toClient, packet);
            }
        }
        public static void KickClient(int _toClient)
        {
            using (Packet packet = new Packet((int)ServerPackets.kickClient))
            {
                SendTCPData(_toClient, packet);
            }
        }
    }
}
