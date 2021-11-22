using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TCP_IP_Sockets
{
    public string Ip { get; set; }
    public IPEndPoint EndPoint { get; set; }
    //public int port { get; set; }
    class Program
    {

        private const int BACKLOG = 5;
        static void Main(string[] args)
        {
            string hostIP = null;
            int hostPort;
            IPHostEntry host = null;
            

            try
            {
                Console.WriteLine("Local Host:");
                string localHostName = Dns.GetHostName();
                Console.WriteLine("\tHost Name:    " + localHostName);

                PrintHostInfo(localHostName);
            }
            catch (Exception ex)
            {
                Console.WriteLine("\tUnable to resolve host. " + ex.Message + "\n");
                Console.ReadLine();
            }

            try
            {
                Console.WriteLine("Ping device. Enter parameters:");
                Console.Write("Device IP:    ");
                hostIP = Convert.ToString(Console.ReadLine());


                host = Dns.GetHostEntry(hostIP);
                Console.WriteLine("\tHost Name:        " + host.HostName);

                PrintHostInfo(hostIP);
            }
            catch (Exception ex)
            {
                Console.WriteLine("\tUnable to resolve host. " + ex.Message + "\n");
                Console.ReadLine();
            }
            Console.WriteLine("Select your connection method:\n\t\t1:  TCP Client NetStream\n\t\t2:  TCP Client Connect\n\t\t3:  TCP Listener\n\t\t4:  UDP Client");
            Console.WriteLine("_____________________________________________________________");
            Console.Write("\t\t:>  ");
            int selection = Convert.ToInt32(Console.ReadLine());
            switch (selection)
            {
                case 1:
                    Console.WriteLine("Connect TCP Client NetStream. Enter device parameters:");
                    Console.WriteLine("IP:    " + hostIP);
                    Console.Write("Port:    ");
                    hostPort = Convert.ToInt32(Console.ReadLine());
                    TCPClientNetStream(hostIP, hostPort);
                    break;
                case 2:
                    Console.WriteLine("Connect TCP Client Connect. Enter device parameters:");
                    Console.WriteLine("IP:    " + hostIP);
                    Console.Write("Port:    ");
                    hostPort = Convert.ToInt32(Console.ReadLine());
                    TCPClientConnect(hostIP, hostPort);
                    break;
                case 3:
                    Console.WriteLine("Connect TCP Listener. Enter device parameters:");
                    Console.WriteLine("IP:    " + hostIP);
                    Console.Write("Port:    ");
                    hostPort = Convert.ToInt32(Console.ReadLine());
                    TCPListener(hostIP, hostPort);
                    break;
                case 4:
                    Console.WriteLine("Connect UDP Client. Enter device parameters:");
                    Console.WriteLine("IP:    " + hostIP);
                    Console.Write("Port:    ");
                    hostPort = Convert.ToInt32(Console.ReadLine());
                    UDPClient(hostIP, hostPort);
                    break;
            }

        }
        static void PrintHostInfo(String host)
        {
            try
            {
                IPHostEntry hostInfo;
                hostInfo = Dns.GetHostEntry(host);

                Console.WriteLine("\tCanonical Name: " + hostInfo.HostName);
                Console.Write("\tAddresses:  ");
                foreach( IPAddress ipaddr in hostInfo.AddressList)
                {
                    Console.Write(ipaddr.ToString() + " ");
                }
                Console.WriteLine();

                Console.Write("\tAliases:        ");
                foreach(string alias in hostInfo.Aliases)
                {
                    Console.Write(alias + " ");
                }
                Console.WriteLine("\n");

            }
            catch (Exception ex)
            {
                Console.WriteLine("\tUnable to resolve host: {0}. ", host + "Message: " + ex.Message + "\n");
                Console.ReadLine();
            }
        }
        static void TCPClientNetStream(string ip, int port)
        {
            try
            {
                Console.WriteLine();
                Thread.Sleep(1000);
                Console.WriteLine("connecting to tcp client...");
                TcpClient client = new TcpClient(ip, port);

                Thread.Sleep(1000);
                string connection = (client.Connected) ? "connected." : "not connected...";
                Console.WriteLine(connection);

                Thread.Sleep(1000);
                Console.WriteLine("getting network stream...");
                NetworkStream netStream = client.GetStream();

                Thread.Sleep(1000);
                string response = (netStream.DataAvailable) ? "Data is available!" : "Data not available...";
                Console.WriteLine(response);

                Thread.Sleep(1000);
                Console.WriteLine("disposing...");
                netStream.Dispose();
                Console.ReadLine();
                
            }
            catch (Exception ex)
            {
                Console.WriteLine("\tException TCPClientNetStream(): " + ex.Message + "\n");
                Console.ReadLine();
            }
        }
        static void TCPClientConnect(string ip, int port)
        {
            try
            {
                Console.WriteLine();
                Thread.Sleep(1000);
                Console.WriteLine("connecting to tcp client...");
                TcpClient client = new TcpClient(ip, port);

                Thread.Sleep(1000);
                string connection = (client.Connected) ? "connected." : "not connected...";
                Console.WriteLine(connection);

                Thread.Sleep(1000);
                Console.WriteLine("disposing...");
                client.Dispose();
                Console.ReadLine();

            }
            catch (Exception ex)
            {
                Console.WriteLine("\tException TCPClientConnect(): " + ex.Message + "\n");
                Console.ReadLine();
            }
        }
        static void TCPListener(string ip, int port)
        {
            try
            {
                Console.WriteLine();
                Thread.Sleep(1000);
                Console.WriteLine("connecting to tcp Listener...");
                IPAddress ipAddress = IPAddress.Parse(ip);
                TcpListener lis = new TcpListener(ipAddress, port);

                Thread.Sleep(1000);
                Console.WriteLine("start listening");
                lis.Start();



                Console.WriteLine("stop listening? <press return>");
                Console.ReadLine();
                lis.Stop();
                Console.ReadLine();

            }
            catch (Exception ex)
            {
                Console.WriteLine("\tException TCPListener(): " + ex.Message + "\n");
                Console.ReadLine();
            }
        }
        static void UDPClient(string ip, int port)
        {
            try
            {
                Console.WriteLine();
                Thread.Sleep(1000);
                Console.WriteLine("creating an IPEndPoint...");
                IPAddress ipAddress = IPAddress.Parse(ip);
                IPEndPoint ipEnd = new IPEndPoint(ipAddress, port);

                Thread.Sleep(1000);
                Console.WriteLine("opening a UDP connection...");
                UdpClient client = new UdpClient(port);

                Thread.Sleep(1000);
                Console.WriteLine("reading incoming data...");
                byte[] byteData = client.Receive(ref ipEnd);

                string returnData = Encoding.ASCII.GetString(byteData);
                Console.WriteLine("This is the message you received:  " + returnData.ToString());

                Thread.Sleep(1000);
                Console.WriteLine("close connection? <press return>");
                client.Close();
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine("\tException UDPClient(): " + ex.Message + "\n");
                Console.ReadLine();
            }

        }
        static void SocketTCPClient(string ip, int port)
        {
            try
            {
                Console.WriteLine();
                Thread.Sleep(1000);
                Console.WriteLine("creating a TCP socket instance...");
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                Thread.Sleep(1000);
                Console.WriteLine("creating an IPEndPoint...");
                IPAddress ipAddress = IPAddress.Parse(ip);
                IPEndPoint ipEnd = new IPEndPoint(ipAddress, port);

                Thread.Sleep(1000);
                Console.WriteLine("connecting to socket on IPEndPoint...");
                socket.Connect(ipEnd);

                
            }
            catch (Exception ex)
            {
                Console.WriteLine("\tException SocketTCPClient(): " + ex.Message + "\n");
                Console.ReadLine();
            }
        }
        static void TCPClientAsync(string ip, int port)
        {
            Console.WriteLine();
            Thread.Sleep(1000);
            Console.WriteLine("connecting to tcp client...");
            TcpClient client = new TcpClient(ip, port);

            Thread.Sleep(1000);
            string connection = (client.Connected) ? "connected." : "not connected...";
            Console.WriteLine(connection);

            Thread.Sleep(1000);
            Console.WriteLine("getting network stream...");
            NetworkStream netStream = client.GetStream();
            

            Thread.Sleep(1000);
            Console.WriteLine("begin recieve...");
            
        }
        static void SocketTCPAsync(string ip, int port)
        {
            try
            {

                Thread.Sleep(1000);
                Console.WriteLine("creating a TCP socket instance...");
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                Thread.Sleep(1000);
                Console.WriteLine("creating an IPEndPoint...");
                IPAddress ipAddress = IPAddress.Parse(ip);
                IPEndPoint ipEnd = new IPEndPoint(ipAddress, port);

                Thread.Sleep(1000);
                Console.WriteLine("binding to local endpoint...");
                socket.Bind(ipEnd);

                Console.WriteLine("listening on socket...");
                socket.Listen(BACKLOG);

                for (; ; )
                {
                    Console.WriteLine("Thread {0} ({1}) - Main(): BeginAccept()", Thread.CurrentThread.GetHashCode(), Thread.CurrentThread.ThreadState);
                }

                IAsyncResult result = socket.BeginAccept(new AsyncCallback(AcceptCallback), socket);

                result.AsyncWaitHandle.WaitOne();
            }
            catch (Exception ex)
            {
                Console.WriteLine("\tException SocketTCPClient(): " + ex.Message + "\n");
                Console.ReadLine();
            }
        }
        public static void AcceptCallback(IAsyncResult ansyncResult)
        {
            Socket socket = (Socket) asyncResult.AsyncState;

        }
    }
}
