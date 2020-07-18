using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace UDPServer
{
	static class Program
	{
		static void Main(string[] args)
		{
			double topBorder = 0;
			double lowerBorder = 0;

			XmlDocument xDoc = new XmlDocument();
			xDoc.Load("Config.xml");

			foreach (XmlNode node in xDoc.DocumentElement)
			{
				string name = node.Attributes[0].Value;
				double value = double.Parse(node["Value"].InnerText);	

				if(name == "lower")
				{
					lowerBorder = value;
				}

				if (name == "upper")
				{
					topBorder = value;
				}
			}


			Console.WriteLine("Enter the IP address:");
			
			string ip = Console.ReadLine();

			const int port = 8081;

			var udpEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);

			var udpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

			udpSocket.Bind(udpEndPoint);

			Random rnd = new Random();

			while (true)
			{				

				double parcel = rnd.NextDouble(lowerBorder, topBorder);

				var serverEndPoint = new IPEndPoint(IPAddress.Parse(ip), 8082); 

				udpSocket.SendTo(Encoding.UTF8.GetBytes(parcel.ToString()), serverEndPoint);

			}

		}

		public static double NextDouble(this Random RandGenerator, double MinValue, double MaxValue)
		{
			return RandGenerator.NextDouble() * (MaxValue - MinValue) + MinValue;
		}
	}
}
