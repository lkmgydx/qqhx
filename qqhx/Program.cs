using HXmain.HXInfo;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace qqhx
{
    static class Program
    {
        class f
        {
            public string a { get; set; }
            public int k { get; set; }
        }

        private static void saveConfig()
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoaming);
            config.Save(ConfigurationSaveMode.Full);
            ConfigurationManager.RefreshSection("appSettings");
        }

        private static Rectangle getRec(Point WantToPoint, Point WantToPointNext)
        {
            int left = Math.Min(WantToPoint.X, WantToPointNext.X);
            int top = Math.Min(WantToPoint.Y, WantToPointNext.Y);
            int right = Math.Max(WantToPoint.X, WantToPointNext.X);
            int bottom = Math.Max(WantToPoint.Y, WantToPointNext.Y);
            return new Rectangle(left, top, right - left, bottom - top);
        }

        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            //bool createNew;
            //using (System.Threading.Mutex m = new System.Threading.Mutex(true, Application.ProductName, out createNew))
            //{
            //    if (!createNew)
            //    {
            //        MessageBox.Show("Only one instance of this application is allowed!");
            //        return;
            //    }
            //}

            //bool createNew;
            //using (System.Threading.Mutex m = new System.Threading.Mutex(true, Application.ProductName, out createNew))
            //{
            //    if (createNew)
            //    {
            //        Application.EnableVisualStyles();
            //        Application.SetCompatibleTextRenderingDefault(false);
            //        Application.Run(new Form1());
            //    }
            //    else
            //    {
            //        MessageBox.Show("Only one instance of this application is allowed!");
            //        return;
            //    }
            //}

            bool isRun = IsApplicationOnRun("ApplicationName");
            if (isRun)
            {
                WSTools.WSWinFn.ShowError("程序已经启动!");
                return;
            }


            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            try
            {
                Application.Run(new Form1());
            }
            catch (Exception ex)
            {
                System.IO.File.WriteAllText("c:\\aa.txt", ex.StackTrace);
                System.IO.File.AppendAllText("C:\\aa.txt", "\r\n" + "存在未处理异常!!");
                System.IO.File.AppendAllText("C:\\aa.txt", "\r\n" + ex.Message);
            }

            //  Application.Run(new Form2());
            // Application.Run(new Form3());
        }


        [DllImport("kernel32.dll", EntryPoint = "CreateMutex", SetLastError = true)]
        public static extern IntPtr CreateMutex(IntPtr lpMutexAttributes, bool bInitialOwner, string lpName);

        [DllImport("kernel32.dll", EntryPoint = "ReleaseMutex", SetLastError = true)]
        public static extern bool ReleaseMutex(IntPtr hMutex);

        private const int ERROR_ALREADY_EXISTS = 0183;

        /// <summary>
        /// 判断程序是否在运行
        /// </summary>
        /// <returns></returns>
        public static bool IsApplicationOnRun()
        {
            return IsApplicationOnRun(System.Reflection.Assembly.GetExecutingAssembly());
        }
        /// <summary>
        /// 判断程序是否已经运行
        /// <param name="assembly">程序集实例</param>
        /// </summary>
        /// <returns>
        /// true: 程序已运行
        /// false: 程序未运行
        /// </returns>
        public static bool IsApplicationOnRun(System.Reflection.Assembly assembly)
        {
            string strAppName = assembly.GetName().Name;
            return IsApplicationOnRun(strAppName);
        }

        /// <summary>
        /// 判断程序是否已经运行
        /// <param name="assemblyName">程序名称</param>
        /// </summary>
        /// <returns>
        /// true: 程序已运行
        /// false: 程序未运行
        /// </returns>
        public static bool IsApplicationOnRun(string assemblyName)
        {
            IntPtr hMutex = CreateMutex(IntPtr.Zero, true, assemblyName);
            if (hMutex == IntPtr.Zero)
            {
                throw new ApplicationException("Failure creating mutex: " + Marshal.GetLastWin32Error().ToString("X"));
            }
            if (Marshal.GetLastWin32Error() == ERROR_ALREADY_EXISTS)
            {
                ReleaseMutex(hMutex);
                return true;
            }
            return false;
        }
    }
}
