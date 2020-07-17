using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace UDPServer
{
	class Program
	{
		static void Main(string[] args)
		{

			const string ip = "176.120.56.169";
			const int port = 8081;

			var udpEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);

			var udpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

			udpSocket.Bind(udpEndPoint);

			Random rnd = new Random();

			while (true)
			{				

				int value = rnd.Next();

				var serverEndPoint = new IPEndPoint(IPAddress.Parse(ip), 8082); 

				udpSocket.SendTo(Encoding.UTF8.GetBytes(value.ToString()), serverEndPoint);

			}

		}
	}
}
