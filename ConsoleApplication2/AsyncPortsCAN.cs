using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApplication2
{
    public class AsyncPortScan
    {
        private static string ip;
        public static void Maint(string[] args)
        {
            ip = args[0];
            int[] ports = new int[] { 21, 22, 23, 25, 80, 110, 135, 445, 587, 993, 995 };

            ports = new int[65535];
            for (int i = 0; i < ports.Length; i++)
            {
                ports[i] = i;
            }


            Console.WriteLine("开始扫描 . . . ");
            foreach (int port in ports)
            {
                new Thread(() =>
                {
                    Scan(ip, port);
                });

            }
            Console.WriteLine("扫描结束!");
        }

        public static void Scan(string m_host, int m_port)
        {
            TcpClient tc = new TcpClient();
            //设置超时时间
            tc.SendTimeout = tc.ReceiveTimeout = 200;

            try
            {
                //同步方法
                //IPAddress ip = IPAddress.Parse(host);
                //IPEndPoint IPendp = new IPEndPoint(ip, port);
                //tc.Connect(IPendp);

                //异步方法
                TcpClient tcp = new TcpClient(m_host, m_port);

                if (tc.Connected)
                {
                    Console.WriteLine(m_port + "_suc");
                    //如果连接上，证明此端口为开放状态
                    // UpdateListBox(listBox1, m_host + ":" + m_port.ToString());
                }
            }
            catch (System.Net.Sockets.SocketException e)
            {
                //容错处理
                //MessageBox.Show("Port {0} is closed", host.ToString());
                Console.WriteLine(m_port + "_false");
            }
            finally
            {
                tc.Close();
                tc = null;

            }
        }

        /// <summary>
        /// 从命令行参数中提取端口
        /// </summary>
        /// <param name="args">命令行参数</param>
        /// <param name="ip">输出 IP地址</param>
        /// <param name="startPort">输出 起始端口号</param>
        /// <param name="endPort">输出 终止端口号</param>
        /// <returns>提取成功返回true，否则返回false</returns>
        private static bool GetPortRange(string[] args, out IPAddress ip, out int startPort, out int endPort)
        {
            ip = null;
            startPort = endPort = 0;
            // 帮助命令
            if (args.Length != 0 && (args[0] == "/?" || args[0] == "/h" || args[0] == "/help"))
            {
                Console.WriteLine("扫面指定 ip 的主机的从 开始端口 到 结束端口 之间的端口.");
                Console.WriteLine("命令格式: PortScanCmd IPAddress startPort endPort");
                Console.WriteLine("例如: PortScanCmd 127.0.0.1 1 1280");
                return false;
            }

            if (args.Length == 3)
            {
                // 解析端口号成功
                if (IPAddress.TryParse(args[0], out ip) && int.TryParse(args[1], out startPort) && int.TryParse(args[2], out endPort))
                {
                    return true;
                }
                else
                {
                    Console.WriteLine("参数格式不正确！");
                    return false;
                }
            }
            else
            {
                Console.WriteLine("参数数目不正确！");
                return false;
            }
        }

        /// <summary>
        /// 端口 扫描
        /// </summary>
        /// <param name="ip">扫描的 IP地址</param>
        /// <param name="startPort">起始端口号</param>
        /// <param name="endPort">终止端口号</param>
        static void Scan(IPAddress ip, int startPort, int endPort)
        {
            Random rand = new Random((int)DateTime.Now.Ticks);
            Console.WriteLine("开始扫描 . . . ");
            for (int port = startPort; port < endPort; port++)
            {
                Socket scanSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
                //寻找一个未使用的端口进行绑定
                do
                {
                    try
                    {
                        scanSocket.Bind(new IPEndPoint(IPAddress.Any, rand.Next(65535)));
                        break;
                    }
                    catch
                    {
                        //绑定失败
                    }
                } while (true);

                try
                {
                    scanSocket.BeginConnect(new IPEndPoint(ip, port), ScanCallBack, new ArrayList() { scanSocket, port });

                }
                catch (Exception ex)
                {
                    Console.WriteLine("端口 {0,5}\t关闭.\n{1}", port, ex.Message);
                    continue;
                }

            }

            Console.WriteLine("扫描结束!");
        }

        /// <summary>
        /// BeginConnect的回调函数
        /// </summary>
        /// <param name="result">异步Connect的结果</param>
        static void ScanCallBack(IAsyncResult result)
        {
            // 解析 回调函数输入 参数
            ArrayList arrList = (ArrayList)result.AsyncState;
            Socket scanSocket = (Socket)arrList[0];
            int port = (int)arrList[1];
            // 判断端口是否开放
            if (result.IsCompleted && scanSocket.Connected)
            {
                Console.WriteLine("端口 {0,5}\t打开.", port);
            }
            else
            {
                //Console.WriteLine("端口 {0,5}\t关闭.", port); 
            }
            // 关闭套接字
            scanSocket.Close();
        }
    }
}
