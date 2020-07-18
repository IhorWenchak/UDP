﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Collections.Specialized;
using System.Xml.Linq;
using System.Xml;

namespace UDPClient
{
	class Program
	{
		static void Main(string[] args)
		{			
			Console.WriteLine("Enter the IP address:");

			string ip = Console.ReadLine();
			
			const int port = 8082;

			var udpEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);

			var udpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

			udpSocket.Bind(udpEndPoint);

			while(true)
			{
				var buffer = new byte[256];
				var size = 0;
				var data = new StringBuilder();
				EndPoint senderEndPoint = new IPEndPoint(IPAddress.Any, 0);

				do
				{
					size = udpSocket.ReceiveFrom(buffer, ref senderEndPoint);
					data.Append(Encoding.UTF8.GetString(buffer));
					data.Append("\n");
				}
				while (udpSocket.Available > 0);

				Console.WriteLine(data);

			}

		}
	}
}
