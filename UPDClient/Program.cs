using System;
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
using System.Threading;

namespace UDPClient
{
	static class Program
	{
		static void Main(string[] args)
		{			
			Console.WriteLine("Enter the IP address:");

			string ip = Console.ReadLine();
			
			const int port = 8082;

			var udpEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);

			var udpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

			udpSocket.Bind(udpEndPoint);

			try
			{
				while (true)
				{
					Receiv receiv = new Receiv();
					receiv.Socket = udpSocket;
					receiv.Str = new StringBuilder();

					Thread tRec = new Thread(new ParameterizedThreadStart(Reciver));
					tRec.Start(receiv);


					Thread tOut = new Thread(new ParameterizedThreadStart(Output));
					tOut.Start(receiv.Str);
					
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("An exception was thrown: " + ex.ToString() + "\n  " + ex.Message);
			}
			finally
			{
				udpSocket.Close();
			}
		}

		public static double Average(double[] arr)
		{
			
			double sum = 0;
			foreach (double elem in arr)
			{
				sum += elem;
			}

			return sum / arr.Length;
		}

		public static double Median(double[] arr)
		{
		
			double[] copyArr = (double[])arr.Clone();
			Array.Sort(copyArr);
			return copyArr[copyArr.Length / 2];
		}

		public static double Mode(double[] arr)
		{
		
			Dictionary<double, int> dict = new Dictionary<double, int>();
			foreach (double elem in arr)
			{
				if (dict.ContainsKey(elem))
					dict[elem]++;
				else
					dict[elem] = 1;
			}

			int maxCount = 0;
			double mode = Double.NaN;
			foreach (double elem in dict.Keys)
			{
				if (dict[elem] > maxCount)
				{
					maxCount = dict[elem];
					mode = elem;
				}
			}

			return mode;
		}

		public class Receiv
		{
			public Socket Socket { get; set; }

			public StringBuilder Str { get; set; }
		}

		public static void Reciver(object receiv)
		{
			var buffer = new byte[256];
			var size = 0;

			Receiv myReceiv = (Receiv)receiv;
			Socket mySocket = (Socket)myReceiv.Socket;

			EndPoint senderEndPoint = new IPEndPoint(IPAddress.Any, 0);

			do
			{
				size = mySocket.ReceiveFrom(buffer, ref senderEndPoint);
				myReceiv.Str.Append(Encoding.UTF8.GetString(buffer));
				myReceiv.Str.Append("\n");
			}
			while (mySocket.Available > 0);
		}

		public static void Output(object putout)
		{
			StringBuilder myOut = (StringBuilder)putout;
			Console.WriteLine(myOut);
		}
	}
}
