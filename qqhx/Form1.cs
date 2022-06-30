using HFrameWork.SystemDll;
using HFrameWork.SystemInput;
using HXmain;
using HXmain.HXAction;
using HXmain.HXInfo;
using HXmain.HXInfo.CacheData;
using HXmain.HXInfo.Map;
using qqhx.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Web.Security;
using System.Windows.Forms;
using Tool;
using WSTools.WSLog;
using WSTools.WSThread;
using u32 = WSTools.WSWinAPI.WinUser32;

namespace qqhx
{
    public partial class Form1 : Form
    {

        [DllImport("MapView.dll")]
        public static extern void dllLoadMapView(string path);

        private static Point startPoint = LocationCache.getPoint();
        WSTools.WSThread.WsSyn syn;

        WSTools.WSHook.KeyboardHook key = new WSTools.WSHook.KeyboardHook();
        public Form1()
        {
            key.KeyPressEvent += Key_KeyPressEvent;
            key.Start();
            //bool createNew;
            //using (System.Threading.Mutex m = new System.Threading.Mutex(true, Application.ProductName, out createNew))
            //{ 
            //    if (!createNew)
            //    {
            //        MessageBox.Show("Only one instance of this application is allowed!");
            //        return;
            //    }
            //}

            //bool initiallyOwned = true;
            //bool isCreated;
            //Mutex m = new Mutex(initiallyOwned, "LEDController", out isCreated);
            //if (!(initiallyOwned && isCreated))
            //{
            //    MessageBox.Show("已启动");
            //    Application.Exit();
            //}
            //string a = "Qk0OEQAAAAAAADYAAAAoAAAATQAAAA4AAAABACAAAAAAAAAAAADEDgAAxA4AAAAAAAAAAAAApaqtAJymrQClqq0ApaqtAJymrQClqq0AnKatAJymrQClqq0AnKatAKWqrQClqq0AnKatAKWqrQCcpq0ApaqtAKWqrQCcpq0ApaqtAJymrQClqq0AnKatAKWqrQCcpq0ApaqtAJymrQClqrUApaqtAKWqrQCcpq0AnKatAKWqrQClqq0ApaqtAJyqrQCcqq0AnKqtAJyqrQClqq0ApaqtAKWqtQClqrUApaqtAKWqtQClqrUApaq1AKWqrQClqq0ApaqtAKWqrQClqq0Apaq1AKWqrQClrrUApbK1AKWqrQClqq0Apaq1AKWqtQClqq0Apaq1AKWqtQClqrUApaqtAKWqrQClqq0ApaqtAKWqrQClqrUApaqtAKWutQClsrUApaqtAKWqrQClqrUApaq1AKWqrQAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAJT//wAIDAgACAwIAAgMCAAIDAgAlP//AJT//wAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgAlP//AJT//wCU//8ACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAJT//wAIDAgACAwIAAgMCAAIDAgAlP//AAgMCAAIDAgACAwIAAgMCACU//8ACAwIAAgMCAAIDAgACAwIAJT//wAIDAgACAwIAAgMCAAIDAgAlP//AAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAJT//wAIDAgACAwIAJT//wAIDAgACAwIAAgMCACU//8ACAwIAAgMCACU//8ACAwIAAgMCACU//8ACAwIAAgMCAAIDAgAlP//AAgMCAAIDAgACAwIAJT//wAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAJT//wAIDAgAlP//AAgMCAAIDAgACAwIAJT//wAIDAgACAwIAJT//wAIDAgACAwIAAgMCACU//8ACAwIAAgMCACU//8ACAwIAAgMCACU//8ACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgAlP//AAgMCACU//8ACAwIAAgMCAAIDAgAlP//AAgMCACU//8ACAwIAAgMCAAIDAgACAwIAJT//wAIDAgACAwIAJT//wAIDAgACAwIAJT//wAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAJT//wAIDAgACAwIAAgMCACU//8ACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAJT//wAIDAgAlP//AAgMCACU//8ACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgAlP//AJT//wCU//8AlP//AJT//wCU//8AlP//AJT//wCU//8AlP//AJT//wAIDAgACAwIAAgMCAAIDAgAlP//AAgMCACU//8ACAwIAJT//wAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAJT//wAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCACU//8ACAwIAJT//wCU//8ACAwIAJT//wAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgAlP//AAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCACU//8AlP//AJT//wAIDAgAlP//AJT//wAIDAgACAwIAJT//wAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAJT//wCU//8AlP//AJT//wCU//8AlP//AJT//wCU//8AlP//AAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCACU//8ACAwIAAgMCAAIDAgACAwIAJT//wAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAJT//wAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAJT//wAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgAlP//AAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgAlP//AAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAA==";
            //string b = "Qk0OEQAAAAAAADYAAAAoAAAATQAAAA4AAAABACAAAAAAAAAAAADEDgAAxA4AAAAAAAAAAAAApaqtAJymrQClqq0ApaqtAJymrQClqq0AnKatAJymrQClqq0AnKatAKWqrQClqq0AnKatAKWqrQCcpq0ApaqtAKWqrQCcpq0ApaqtAJymrQClqq0AnKatAKWqrQCcpq0ApaqtAJymrQClqrUApaqtAKWqrQCcpq0AnKatAKWqrQClqq0ApaqtAJyqrQCcqq0AnKqtAJyqrQClqq0ApaqtAKWqtQClqrUApaqtAKWqtQClqrUApaq1AKWqrQClqq0ApaqtAKWqrQClqq0Apaq1AKWqrQClrrUApbK1AKWqrQClqq0Apaq1AKWqtQClqq0Apaq1AKWqtQClqrUApaqtAKWqrQClqq0ApaqtAKWqrQClqrUApaqtAKWutQClsrUApaqtAKWqrQClqrUApaq1AKWqrQAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAJT//wAIDAgACAwIAAgMCAAIDAgAlP//AJT//wAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgAlP//AJT//wCU//8ACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAJT//wAIDAgACAwIAAgMCAAIDAgAlP//AAgMCAAIDAgACAwIAAgMCACU//8ACAwIAAgMCAAIDAgACAwIAJT//wAIDAgACAwIAAgMCAAIDAgAlP//AAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAJT//wAIDAgACAwIAJT//wAIDAgACAwIAAgMCACU//8ACAwIAAgMCACU//8ACAwIAAgMCACU//8ACAwIAAgMCAAIDAgAlP//AAgMCAAIDAgACAwIAJT//wAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAJT//wAIDAgAlP//AAgMCAAIDAgACAwIAJT//wAIDAgACAwIAJT//wAIDAgACAwIAAgMCACU//8ACAwIAAgMCACU//8ACAwIAAgMCACU//8ACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgAlP//AAgMCACU//8ACAwIAAgMCAAIDAgAlP//AAgMCACU//8ACAwIAAgMCAAIDAgACAwIAJT//wAIDAgACAwIAJT//wAIDAgACAwIAJT//wAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAJT//wAIDAgACAwIAAgMCACU//8ACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAJT//wAIDAgAlP//AAgMCACU//8ACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgAlP//AJT//wCU//8AlP//AJT//wCU//8AlP//AJT//wCU//8AlP//AJT//wAIDAgACAwIAAgMCAAIDAgAlP//AAgMCACU//8ACAwIAJT//wAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAJT//wAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCACU//8ACAwIAJT//wCU//8ACAwIAJT//wAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgAlP//AAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCACU//8AlP//AJT//wAIDAgAlP//AJT//wAIDAgACAwIAJT//wAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAJT//wCU//8AlP//AJT//wCU//8AlP//AJT//wCU//8AlP//AAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCACU//8ACAwIAAgMCAAIDAgACAwIAJT//wAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAJT//wAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAJT//wAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgAlP//AAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgAlP//AAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAAgMCAAIDAgACAwIAA==";

            //for (int i = 0; i < a.Length; i++) {
            //    if (a[i] != b[i]) {
            //        "".ToString();
            //    }
            //}

            InitializeComponent();
            syn = new WsSyn(this);
            Log.LogEnable = false;
            mm.onlog += Mm_onlog;

        }

        private void Key_KeyPressEvent(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 45)
            {
                Environment.Exit(0);
            }
        }

        private static PtrGame getActivePtr()
        {

            PtrGame pg = new PtrGame();
            IntPtr ptr = User32.GetForegroundWindow();
            int rst = 0;
            User32.GetWindowThreadProcessId(ptr, out rst);
            Process p = System.Diagnostics.Process.GetProcessById(rst);
            pg.isGame = p.ProcessName == "QQhxgame";
            pg.Process = p;
            pg.ptr = ptr;
            return pg;
        }

        private delegate void onFindGame();
        private event onFindGame onFindGameEvent;


        private void setV(int v)
        {
            User32.ShowWindow(Process.GetCurrentProcess().MainWindowHandle, v);
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            //WSTools.WSThread.WsThread.Run(() =>
            //{
            //    System.Threading.Thread.Sleep(5000);
            //   // setV(0);
            //    setV(1);
            //});
            WuPingData.getWuping(null);//初始化物品信息
            startPoint.X = Math.Max(startPoint.X, 0);
            startPoint.X = Math.Min(startPoint.X, Screen.PrimaryScreen.Bounds.Width);

            startPoint.Y = Math.Max(startPoint.Y, 0);
            startPoint.Y = Math.Min(startPoint.Y, Screen.PrimaryScreen.Bounds.Height);

            Location = startPoint;

            // string text = BaiDuOcr.getImgText("D:/qqhximg/寄售_搜索.bmp");

            var os = Environment.OSVersion;
            // Process.Start("explorer", "d:\\ff.bmp");

            onFindGameEvent += Form1_onFindGameEvent;
            if (!findGame())
            {
                this.timer1.Enabled = true;
                this.Text = "未检测到qq华夏进程";
            }
        }

        private void KeyBoard_OnKeyDownEvent(object sender, KeyEventArgs e)
        {

        }


        Process[] ps;

        private bool findGame()
        {

            ps = Process.GetProcessesByName("QQhxgame");

            if (ps.Length == 0)
            {
                return false;
            }
            comboGameIndex.Items.Clear();
            checkedListBox1.Items.Clear();
            for (int i = 0; i < ps.Length; i++)
            {
                comboGameIndex.Items.Add(ps[i].Id);
                checkedListBox1.Items.Add(new GameItem(ps[i]));
                mm.addGageProcee(ps[i]);
            }
            comboGameIndex.SelectedIndex = 0;
            this.Text = "Form1";
            return true;
        }



        MainManager mm = new MainManager();
        private void Form1_onFindGameEvent()
        {
            // int key = Kernel32.OpenProcess(Kernel32.OpenProcess_VM_READ | Kernel32.OpenProcess_VM_WRITE, false, p.Id);
            //Kernel32.WriteProcessMemory(key,
            if (Process.GetProcessesByName("QQhxgame").Length > 1)
            {
                mm.IsFromScreen = true;
            }
        }


        private void Mm_onlog(object sender, EventArgs e)
        {
            this.richTextBox1.Text += sender.ToString() + "\r\n";
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            if (findGame())
            {
                return;
            }
            timer1.Enabled = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //HSearchPoint hp = ImageTool.findImageFromScreen(new Bitmap(@"D:\Users\Administrator\Desktop\s1.png"), 90);
            //if (hp.Success)
            //{
            //    MessageBox.Show(hp.Point.ToString());
            //}
            //else
            //{
            //    MessageBox.Show(hp.Times + "");
            //}
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (mm != null)
            {
                mm.寄售Search(text_寄售SearchName.Text);
            }
        }

        public void MoNiMouseClick()
        {
            //User32.SetForegroundWindow(p.MainWindowHandle);
            //int x = 344; // X coordinate of the click 
            //int y = 367; // Y coordinate of the click 
            //IntPtr handle = p.MainWindowHandle;
            //StringBuilder className = new StringBuilder(100);

            //// handle = GetWindow(handle, 5); // Get a handle to the child window  
            //IntPtr lParam = (IntPtr)((y << 16) | x); // The coordinates 
            //IntPtr wParam = IntPtr.Zero; // Additional parameters for the click (e.g. Ctrl) 
            //const uint downCode = 0x201; // Left click down code 
            //const uint upCode = 0x202; // Left click up code 
            //SendMessage(handle, downCode, wParam, lParam); // Mouse button down 
            //SendMessage(handle, upCode, wParam, lParam); // Mouse button up 
            //SendMessage(handle, upCode, wParam, lParam); // Mouse button up 

            IntPtr pt = FindWindow(null, "QQ华夏");


            //PostMessage(pt, 0x0100, 13, 1); //回车

            SendMessageN(pt, 0x0100, 65, 1); //a
            KeyBoard.sleep(500);
            SendMessageN(pt, 0x0100, 66, 1); //a
            KeyBoard.sleep(500);
            SendMessageN(pt, 0x0100, 67, 1); //a
            KeyBoard.sleep(500);
            // PostMessage(pt, 0x0100, 13, 1); //回车

            // PostMessage(pt, 0x0100, 13, 1);
            // PostMessage(pt, 0x0101, 13, 1);

            //IntPtr a1 = SendMessage(pt, 0x0100, new IntPtr(0x0D), IntPtr.Zero);
            //IntPtr a2 = SendMessage(pt, 0x0101, new IntPtr(0x0D), IntPtr.Zero);

            //IntPtr a5 = SendMessageN(pt, 515, 1, 32 * 65535 + 32);

            //IntPtr b1 = PostMessage(pt, 0x0100, 13, 1);
            //IntPtr b2 = PostMessage(pt, 0x0101, 13, 1);


        }

        [DllImport("user32.dll ", CharSet = CharSet.Auto)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr GetWindow(IntPtr hWnd, uint uCmd);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]
        static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", EntryPoint = "SendMessage", CharSet = CharSet.Auto, SetLastError = false)]
        static extern IntPtr SendMessageN(IntPtr hWnd, uint Msg, int wParam, int lParam);

        [DllImport("user32.dll", EntryPoint = "PostMessage", CharSet = CharSet.Auto, SetLastError = false)]
        static extern IntPtr PostMessage(IntPtr hWnd, uint Msg, int wParam, int lParam);

        private void txtH_TextChanged(object sender, EventArgs e)
        {
            TextBox t = (TextBox)sender;
            int v = 0;
            try
            {
                v = int.Parse(t.Text);
            }
            catch
            {
                t.Text = "1";
            }

        }

        Bitmap btemp = null;
        private void button4_Click(object sender, EventArgs e)
        {
            int x = int.Parse(txtX.Text);
            int y = int.Parse(txtY.Text);
            int w = int.Parse(txtW.Text);
            int h = int.Parse(txtH.Text);

            this.pictureBox1.Width = w;
            this.pictureBox1.Height = h;
            Bitmap bmp = null;
            if (mm.MainGame != null)
            {
                ScreenCapture sc = new ScreenCapture();
                sc.ImageModle = ScreenCapture.CaptureImageModle.MemeryCopy;
                sc.Handle = mm.MainGame.MainProcess.MainWindowHandle;
                bmp = sc.HImage;
                png_temp = sc.getImg(new Rectangle(x, y, w, h));
            }
            else
            {
                if (btemp != null)
                {
                    bmp = btemp;
                }
                else
                {
                    btemp = (Bitmap)pictureBox1.Image;
                }
            }

            if (bmp == null)
            {
                return;
            }
            int minw = x + w > bmp.Width ? bmp.Width - x : w;
            int minh = y + h > bmp.Height ? bmp.Height - y : h;

            Bitmap te = bmp.Clone(new Rectangle(x, y, w, h), bmp.PixelFormat);
            int vx = 480 / te.Width;
            int vy = 280 / te.Height;

            int min = Math.Min(vx, vy);
            if (min > 1)
            {
                te = ImageTool.ZoomBmp(te, min);
            }
            this.pictureBox1.Image = te;
            this.pictureBox1.Width = te.Width;
            this.pictureBox1.Height = te.Height;
            // button5_Click(null, null);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string path = "d:\\qqhximg\\" + this.text_save.Text + Environment.TickCount + ".bmp";
            if (check_isPng.Checked && png_temp != null)
            {
                png_temp.Save(path, ImageFormat.Bmp);//PNG格式,findIMG才成功
                isPNG = false;
                png_temp = null;
                //png_temp.Dispose();
            }
            else
            {
                this.pictureBox1.Image.Save(path, ImageFormat.Bmp);
            }
            HYL.ExpolorerPath(path);
        }

        private void txtX_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button4.PerformClick();
            }
        }

        private void txtH_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up)
            {
                TextBox t = (TextBox)sender;
                t.Text = (int.Parse(t.Text) + 1) + "";
                button4.PerformClick();
            }
            else if (e.KeyCode == Keys.Down)
            {
                TextBox t = (TextBox)sender;
                t.Text = (int.Parse(t.Text) - 1) + "";
                button4.PerformClick();
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (mm != null)
            {
                // mm.寄售search北斗("北斗");
                // mm.寄售search无相("无相");
                mm.寄售search元如("元如");
                // mm.ss();
                // mm.寄售search北斗("北斗");
            }
        }


        private void sendKey(int key)
        {
            SendMessageN(mm.MainGame.MainProcess.MainWindowHandle, 0x0100, key, 1); //a
        }

        //private int baseAddress = 0x0015E944;

        // private int baseAddress = 0x25D20000;
        private void button6_Click(object sender, EventArgs e)
        {


        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (mm != null)
            {
                mm.寄售Search(text_寄售SearchName.Text);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (mm != null)
            {
                mm.寄售Search(this.comboBox1.SelectedItem.ToString());
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            EmailTool.SendToQQ("AAA", "TEST");
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            this.autoSetDown.Enabled = checkBox1.Checked;
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            mm.SetDown();
        }



        private Rectangle getRec()
        {
            int x = int.Parse(txtX.Text);
            int y = int.Parse(txtY.Text);
            int w = int.Parse(txtW.Text);
            int h = int.Parse(txtH.Text);
            return new Rectangle(x, y, w, h);
        }
        private void button8_Click(object sender, EventArgs e)
        {
            Bitmap bt = (Bitmap)Image.FromFile(@"D:\qqhximg\v2.bmp");
            btemp = bt;
            txtX.Text = "448";
            txtY.Text = "103";
            txtW.Text = "100";
            txtH.Text = "100";

            int x = int.Parse(txtX.Text);
            int y = int.Parse(txtY.Text);
            int w = int.Parse(txtW.Text);
            int h = int.Parse(txtH.Text);
            Bitmap btt = new Bitmap(w, h);
            for (int i = 0; i < w; i++)
            {
                int fx = i + x;
                if (fx > bt.Width)
                {
                    continue;
                }
                for (int j = 0; j < h; j++)
                {
                    int fy = j + y;
                    if (fy > bt.Height)
                    {
                        continue;
                    }
                    btt.SetPixel(i, j, bt.GetPixel(fx, fy));
                }
            }
            this.pictureBox1.Image = btt;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (mm != null)
            {
                mm.EnableYZM = check_YZM.Checked;
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            //EmailTool.SendToQQ();
            WSTools.WSThread.WsThread.Run(() =>
            {
                三海经.三第经日常(mm.MainGame);
            });
        }

        private void button10_Click(object sender, EventArgs e)
        {
            WSTools.WSThread.WsThread.Run(() =>
            {
                ToDaySlD.升龙殿钱善NPC任务3(mm.MainGame);
                ToDaySlD.升龙殿钱善NPC任务1_自动捐满(mm.MainGame);
            });
            //timer4.Enabled = true;
            // active();
            // SendMessageN(p.MainWindowHandle, 0x0100, 65, 1);
            // SendMessageN(p.MainWindowHandle, 0x0201, 0, 0X0180017A);
            // SendMessageN(p.MainWindowHandle, 0x0202, 0, 0X0180017A);
            // sendKey(Keys.B);
        }



        private void button11_Click(object sender, EventArgs e)
        {
            ToDaySlD.升龙殿投票NPC任务(mm.MainGame);
            // this.Text =   ImageTool.getNum(@"D:\qqhximg\_YZB_02");
            // this.richTextBox1.Text = "识别内容为：" + HOCR.ImageToText(pictureBox1.Image);
        }

        private void button12_Click(object sender, EventArgs e)
        {
            clickPWD();
            KeyBoard.sleep(1000);
            clicY();
            KeyBoard.sleep(1000);
            clickDl();
        }


        private void clickDl()
        {
            HSearchPoint hp = Tool.ImageTool.findImageFromScreen(Properties.Resources.dl, 70);
            if (hp.Success)
            {
                Mouse.move(hp.CenterPoint);
                Mouse.leftclick();
                //KeyBoard.down_up(new Keys[] { Keys.A, Keys.B, Keys.C, Keys.D });
            }
        }

        private void clicY()
        {
            HSearchPoint hp = Tool.ImageTool.findImageFromScreen(Properties.Resources.y, 70);
            if (hp.Success)
            {
                Mouse.move(hp.CenterPoint);
                Mouse.leftclick();
                //KeyBoard.down_up(new Keys[] { Keys.A, Keys.B, Keys.C, Keys.D });
            }
        }



        private void clickPWD()
        {
            HSearchPoint hp = Tool.ImageTool.findImageFromScreen(Properties.Resources.yzm, 70);
            if (hp.Success)
            {
                Mouse.move(hp.CenterPoint);
                Mouse.leftclick();
                //KeyBoard.down_up(new Keys[] { Keys.A, Keys.B, Keys.C, Keys.D });
            }
        }


        private void button12_Click_1(object sender, EventArgs e)
        {
            Form1_Load(sender, e);
        }

        private void comboGameIndex_SelectedIndexChanged(object sender, EventArgs e)
        {
            //p = ps[comboGameIndex.SelectedIndex];
            //onFindGameEvent();
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            timer3.Enabled = checkBox3.Enabled;
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            this.timer3.Enabled = false;
            if (mm != null)
            {
                mm.follow();
            }
            timer3.Enabled = checkBox3.Enabled;
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            richTextBox1.SelectionStart = richTextBox1.Text.Length;
            richTextBox1.ScrollToCaret();
        }

        private void button13_Click(object sender, EventArgs e)
        {
            //Bitmap bmp = new Bitmap("d:\\qq.bmp");
            this.BackgroundImage = toRed(new System.IO.FileStream("d:\\qq.bmp", System.IO.FileMode.Open));
        }

        public Bitmap toRed(System.IO.FileStream curImageStream)
        {
            try
            {
                Bitmap resultBitmap = new Bitmap(curImageStream);
                resultBitmap = resultBitmap.Clone(new Rectangle(0, 0, resultBitmap.Width, resultBitmap.Height), PixelFormat.Format24bppRgb);

                for (int i = 0; i < resultBitmap.Width; i++)
                {
                    for (int j = 0; j < resultBitmap.Height; j++)
                    {
                        Color cl = resultBitmap.GetPixel(i, j);
                        if (cl.Name != "ffffffff")
                        {
                            Color c = Color.FromArgb(255, 255, cl.G, cl.B);
                            resultBitmap.SetPixel(i, j, c);
                        }
                    }
                }
                return resultBitmap;
            }
            catch
            {
                return null;
            }
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (mm != null)
            {
                mm.AutoLive = checkBox4.Checked;
            }
        }


        static bool S(int i)
        {
            Kernel32.FreeConsole();
            return false;
        }
        static Kernel32.ConsoleCtrlDelegate cc = new Kernel32.ConsoleCtrlDelegate(S);
        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {

            if (checkBox5.Checked)
            {
                Kernel32.AllocConsole();
                Log.LogEnable = true;
                Log.LogToConsole = true;

            }
            else
            {
                Kernel32.FreeConsole();
                Log.LogEnable = false;
            }

            this.richTextBox1.Visible = checkBox5.Checked;
            if (richTextBox1.Visible)
            {
                this.Height += richTextBox1.Height;
            }
            else
            {
                this.Height -= richTextBox1.Height;
            }
        }

        private void button13_Click_1(object sender, EventArgs e)
        {
            this.pictureBox1.Image = new HandlerScreenCopy(mm.MainGame.MainProcess.MainWindowHandle).getImage();   //; ScreenCopy.Copy(getRec());   
            this.Size = pictureBox1.Image.Size;
            this.pictureBox1.Size = pictureBox1.Image.Size;
        }

        private void pictureBox1_LoadCompleted(object sender, AsyncCompletedEventArgs e)
        {
            pictureBox1.Width = pictureBox1.Image.Width;
            pictureBox1.Height = pictureBox1.Image.Height;
        }


        private void timer4_Tick(object sender, EventArgs e)
        {
            Point down = new Point(510, 430);
            Point up = new Point(490, 380);
            Point left = new Point(440, 400);
            Point right = new Point(600, 380);
            Point self = new Point(520, 380);
            right = new Point(630, 418);
            Point movePoint = right;

            if (mm.MainGame != null)
            {
                mm.MainGame.pointClick(movePoint);
            }
            // HandlerScreenCopy hs = new HandlerScreenCopy(p.MainWindowHandle);
            // hs.MouseMove(100, 100);
            //Mouse.leftclick();
            // SendMessageN(p.MainWindowHandle, 0x0201, 0, MAKELPARAM((ushort)movePoint.X, (ushort)movePoint.Y));
            // SendMessageN(p.MainWindowHandle, 0x0202, 0, MAKELPARAM((ushort)movePoint.X, (ushort)movePoint.Y));

            // this.richTextBox1.Text += SendMessageN(p.MainWindowHandle, 0x0201, 0, MAKELPARAM(300, 300)) + "\r\n";
            //Mouse.rightclick();
            //SendMessageN(p.MainWindowHandle, 0x0202, 0, 400);
        }

        private void leftClick(int x, int y)
        {
            SendMessageN(mm.MainGame.MainProcess.MainWindowHandle, 0x0202, 0, MAKELPARAM((ushort)x, (ushort)y));
        }


        private void rightClick(int x, int y)
        {
            return;
            //SendMessageN(p.MainWindowHandle, 0x0204, 0, MAKELPARAM((ushort)x, (ushort)y));
        }

        int MAKELPARAM(ushort wLow, ushort wHigh)
        {
            return wHigh * 0x10000 + wLow;
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            string msg = comboBox3.SelectedItem.ToString();
            string[] items = msg.Split(' ');
            string[] rst = new string[5];
            int i = 0;
            for (int j = 0; j < items.Length; j++)
            {
                if (!string.IsNullOrEmpty(items[j]))
                {
                    rst[i++] = items[j];
                    if (i >= 5)
                    {
                        break;
                    }
                }
            }
            this.txtX.Text = rst[1];
            this.txtY.Text = rst[2];
            this.txtW.Text = rst[3];
            this.txtH.Text = rst[4];
            button4.PerformClick();
        }

        private void button15_Click(object sender, EventArgs e)
        {
            ToDaySlD.升龙殿官职晋升NPC任务(mm.MainGame);
            //this.richTextBox1.Text = "识别内容为：" + HOCR.ImageToText(pictureBox1.Image, HOCR.OCRType.Chinese);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            LocationCache.setPoint(this.Location);
        }

        private void timer_one_jineng_Tick(object sender, EventArgs e)
        {
            if (mm != null)
            {
                sendKey(192);
                sendKey(49);
            }
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            //this.timer_one_jineng.Enabled = checkBox6.Checked;
            this.mm.MainGame.AutoOneKey = checkBox6.Checked;
        }


        private Point[] getResourcePoint(string name)
        {
            List<Point> pt = new List<Point>();
            using (Stream fs = typeof(Form1).Assembly.GetManifestResourceStream("qqhx." + name))
            {
                using (System.IO.StreamReader sr = new System.IO.StreamReader(fs))
                {
                    string[] str = sr.ReadToEnd().Split('\n');
                    for (int i = 0; i < str.Length; i++)
                    {
                        try
                        {
                            string[] t = str[i].Replace("\r", "").Split(' ');
                            pt.Add(new Point(Int32.Parse(t[0]), Int32.Parse(t[1])));
                        }
                        catch (Exception ex)
                        {
                            Console.Write("转换异常!" + str[i], ex);
                        }
                    }
                }
            }
            return pt.ToArray();
        }

        private void button28_Click(object sender, EventArgs e)
        {
            if (mm.MainGame != null)
            {
                mm.MainGame.runPath(getResourcePoint("Points.txt"));
            }

            //Point [] pt = new Point[] {new Point( 290 147) }
            // 290 147
            // 290 144
            // 288 138
            // 284 136
            // 278 132
            // 275 136
            // 273 139
            // 271 145
            // 271 150
            // 273 158
            //278 156
            // 287 153
        }

        private void button29_Click(object sender, EventArgs e)
        {
            if (mm.MainGame == null)
            {
                return;
            }
            Point p = mm.MainGame.CurrentLocation;
            this.richTextBox1.Text += p.X + " " + p.Y + "\r\n";
        }



        private void MainGame_onEndRun(object sender, EventArgs e)
        {
            Point[] pt = getResourcePoint("轩辕.txt");
            mm.MainGame.runPath(pt);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            // //baseAddress = 0x00192F54;
            ////               0x00122F6C
            //Convert.ToInt32(this.textBox1.Text, 16)
            //this.mm.BaseAddress = Convert.ToInt32(this.textBox1.Text, 16);
            //updateBaseAddress();
        }

        private void updateBaseAddress()
        {
            try
            {
                this.mm.BaseAddress = Convert.ToInt32(this.textBox1.Text, 16);
            }
            catch (Exception)
            {

            }

        }

        private void timer5_Tick(object sender, EventArgs e)
        {
            ScreenCapture cs = new ScreenCapture(mm.MainGame.MainProcess.MainWindowHandle);
            using (Bitmap bmp = cs.getImg(new Rectangle(300, 87, 6, 5)))
            {
                for (int i = 0; i < bmp.Width; i++)
                {
                    for (int j = 0; j < bmp.Height; j++)
                    {
                        Color c = bmp.GetPixel(i, j);
                        if (c.R != 0 || c.G != 0 || c.B != 0)
                        {
                            this.Text = "未选择";
                            return;
                        }
                    }
                }
                this.Text = "已选择";
            }

        }

        private void button33_Click(object sender, EventArgs e)
        {
            string dir = AppDomain.CurrentDomain.BaseDirectory + "runPath";
            Directory.CreateDirectory(dir);
            openFileDialog1.InitialDirectory = dir;
            if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if (mm.MainGame != null)
                {
                    if (mm.AutoTilian)
                    {
                        mm.MainGame.AUTOTiLianWuPing();
                    }
                    Point[] p = getFilePoints(openFileDialog1.FileName);
                    if (this.反向吗)
                    {
                        p = p.Reverse().ToArray();
                    }
                    mm.MainGame.runGamePath.onRoundEnd -= RunGamePath_onRoundEnd;
                    mm.MainGame.runGamePath.onRoundEnd += RunGamePath_onRoundEnd;
                    mm.MainGame.runPath(p);

                }
            }
        }

        private void RunGamePath_onRoundEnd(object sender, EventArgs e)
        {
            MainGame game = sender as MainGame;
            if (game == null)
            {
                return;
            }
            game.WuPingLanInfo.TilianAllForce();
            game.drawMouse();
        }

        private Point[] getFilePoints(string path)
        {
            List<Point> pt = new List<Point>();
            string[] str = File.ReadAllText(path).Split('\n');
            for (int i = 0; i < str.Length; i++)
            {
                try
                {
                    string[] t = str[i].Replace("\r", "").Split(' ');
                    pt.Add(new Point(Int32.Parse(t[0]), Int32.Parse(t[1])));
                }
                catch (Exception ex)
                {
                    Console.Write("转换异常!" + str[i], ex);
                }
            }

            return pt.ToArray();
        }

        private void checkBox8_CheckedChanged(object sender, EventArgs e)
        {
            if (mm.MainGame != null)
            {
                mm.MainGame.isXUNHUAN = checkBox8.Checked;
            }
        }

        private void button19_Click_1(object sender, EventArgs e)
        {
            if (mm.MainGame != null)
            {
                mm.MainGame.isXUNHUAN = true;
                mm.MainGame.runPath(getResourcePoint("Points.txt"));
            }
        }

        private void button20_Click_1(object sender, EventArgs e)
        {
            if (mm.MainGame != null)
            {
                mm.MainGame.isXUNHUAN = false;
                mm.MainGame.runPath(getResourcePoint("轩辕FB.txt"));
            }
        }

        private void button30_Click(object sender, EventArgs e)
        {
            if (mm.MainGame != null)
            {
                mm.MainGame.isXUNHUAN = true;
                Log.log("开始轩辕挂机!");
                Point[] pt = getResourcePoint("轩辕.txt");
                mm.MainGame.runPath(pt);
                Log.log("结束!");
            }
        }

        long lastCheckTime = DateTime.Now.Ticks;


        private void drawGame()
        {
            try
            {
                Graphics g = Graphics.FromHwnd(((ProcessItem)comboBox2.SelectedItem).Process.MainWindowHandle);
                g.DrawString("测试画一下，10，10", new Font("", 20), Brushes.Red, new Point(10, 10));
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        private void checkBox9_CheckedChanged(object sender, EventArgs e)
        {
            Log.LogEnable = checkBox9.Checked;
        }

        private void button23_Click_2(object sender, EventArgs e)
        {
            if (mm.MainGame != null)
            {
                mm.MainGame.isXUNHUAN = true;
                mm.MainGame.runPath(getResourcePoint("阪泉.txt"));
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (mm != null)
            {
                mm.EnableYZM = false;
                mm.Dispose();
            }
            Application.Exit();
        }

        private void button24_Click_1(object sender, EventArgs e)
        {
            if (mm.MainGame != null)
            {
                mm.MainGame.isXUNHUAN = false;
                long tickcount = Environment.TickCount;
                mm.MainGame.runPath(getResourcePoint("运镖.txt"), new Action(() =>
                {
                    Tool.EmailTool.SendToQQ("运镖完成!耗时:" + (Environment.TickCount - tickcount) + "毫秒", "YB");
                }));
            }
        }

        private void button25_Click_1(object sender, EventArgs e)
        {
            if (mm.MainGame != null)
            {
                mm.MainGame.isXUNHUAN = true;
                mm.MainGame.runPath(getResourcePoint("昆仑FB.txt"));
            }
        }

        private void button18_Click(object sender, EventArgs e)
        {
            if (mm.MainGame != null)
            {
                mm.MainGame.isXUNHUAN = true;
                mm.MainGame.runPath(getResourcePoint("Points.txt").Reverse().ToArray());
            }
        }

        private void button26_Click_1(object sender, EventArgs e)
        {

        }

        private void button26_Click_2(object sender, EventArgs e)
        {
            int time = 1000;
            try
            {
                time = Int32.Parse(txt_timerClickTime.Text);
            }
            catch
            {
            }
            if (time <= 100)
            {
                time = 100;
            }
            if (time > 100000)
            {
                time = 100000;
            }
            txt_timerClickTime.Text = time.ToString();
            timer_click.Interval = time;
            this.timer_click.Enabled = !this.timer_click.Enabled;
            if (timer_click.Enabled)
            {
                ((Button)sender).Text = "定时3技能停止";
            }
            else
            {
                ((Button)sender).Text = "定时3技能开始";
            }
        }

        private void timer_click_Tick(object sender, EventArgs e)
        {
            this.mm.SendKey(Keys.D3);
        }

        private void button27_Click_1(object sender, EventArgs e)
        {
            if (mm.MainGame != null)
            {
                mm.MainGame.isXUNHUAN = true;
                mm.MainGame.runPath(getResourcePoint("阪泉小范围.txt"));
            }
        }

        private void button28_Click_1(object sender, EventArgs e)
        {
            if (mm.MainGame != null)
            {
                MapBase map = mm.MainGame.CurrentMap;
                this.lab_map_name.Text = map.Name;
                this.richTextBox1.Text += "\r\n当前地图: [" + map.MapKey + "]";
            }
        }

        private void button34_Click(object sender, EventArgs e)
        {
            if (mm.MainGame != null)
            {
                this.lab_name.Text = mm.MainGame.getString(0x07DB0CDA);
            }
        }


        private void button35_Click(object sender, EventArgs e)
        {
            string rst = HYLOcr.getRst(this.pictureBox1.Image);
            if (string.IsNullOrEmpty(rst))
            {
                return;
            }
            WSTools.WSWinFn.ShowInfo("获取结果为:\r\n" + rst);
        }

        private void button36_Click(object sender, EventArgs e)
        {
            string rst = BaiDuOcr.getImgText(this.pictureBox1.Image);
            if (string.IsNullOrEmpty(rst))
            {
                return;
            }
            WSTools.WSWinFn.ShowInfo("获取结果为:\r\n" + rst);
        }

        private void checkBox11_CheckedChanged(object sender, EventArgs e)
        {
            mm.AutoZudui = ((CheckBox)sender).Checked;
        }

        private void Form1_LocationChanged(object sender, EventArgs e)
        {

        }

        private void button37_Click(object sender, EventArgs e)
        {
            CopyScreenForm cf = new CopyScreenForm();
            cf.onSucSelectEventHandler2 += Cf_onSucSelectEventHandler2;
            cf.Show();
        }

        private void Cf_onSucSelectEventHandler2(Point start, Point end)
        {
            if (mm.MainGame == null)
            {
                return;
            }
            Point pleft = mm.MainGame.ToClientPoint(start);
            Point right = mm.MainGame.ToClientPoint(end);

            syn.Run(() =>
            {
                txtX.Text = pleft.X.ToString();
                txtY.Text = pleft.Y.ToString();

                txtW.Text = (right.X - pleft.X) + "";
                txtH.Text = (right.Y - pleft.Y) + "";

                button4_Click(null, null);
            });
        }

        private void button38_Click(object sender, EventArgs e)
        {
            this.pictureBox1.Width = this.pictureBox1.Image.Width * 2;
            this.pictureBox1.Height = this.pictureBox1.Image.Width * 2;
            this.pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            this.pictureBox1.Refresh();
        }

        private void button39_Click(object sender, EventArgs e)
        {
            this.pictureBox1.Width = this.pictureBox1.Width / 2;
            this.pictureBox1.Height = this.pictureBox1.Width / 2;
        }

        private PtrGame ptrGame = getActivePtr();

        private bool isInitSelect = false;

        //bool RightClick = true;
        //long lastClick = 0;
        private void timer2_Tick_1(object sender, EventArgs e)
        {
            IntPtr pt = User32.GetForegroundWindow();
            if (pt == ptrGame.ptr && ptrGame.isGame && isInitSelect)
            {
                return;
            }
            //if (RightClick && Environment.TickCount - lastClick > 1000 * 10)
            //{
            //    Mouse.cacheLocation();
            //    //SendKey(Keys.R);
            //   // MouseMove(400, 300);
            //    Mouse.rightclick();
            //    Mouse.reventLocation();
            //    lastClick = Environment.TickCount;
            //}
            ptrGame = getActivePtr();
            if (ptrGame.isGame)
            {
                this.Text = "qqHX - " + ptrGame.Process.Id;
                this.mm.setMainProcess(ptrGame.Process);
                this.mm.MainGame.onLocationChange -= MainGame_onLocationChange2;
                this.mm.MainGame.onLocationChange += MainGame_onLocationChange2;
                MainGame_onLocationChange2(new Point(0, 0), mm.MainGame.CurrentLocation);

                this.mm.MainGame.onSelectChangeEvent -= MainGame_onSelectChangeEvent;
                this.mm.MainGame.onSelectChangeEvent += MainGame_onSelectChangeEvent;

                this.lab_map_name.Text = mm.MainGame.MapName;
                for (var i = 0; i < this.checkedListBox1.Items.Count; i++)
                {
                    if (checkedListBox1.Items[i].ToString() == ptrGame.Process.Id.ToString())
                    {
                        this.checkedListBox1.SetItemChecked(i, true);
                    }
                    else
                    {
                        this.checkedListBox1.SetItemChecked(i, false);
                    }
                }
                this.checkedListBox1.Items.ToString();
                isInitSelect = true;
            }
        }

        private void MainGame_onSelectChangeEvent(bool isSelect)
        {
            string rst = "-";
            if (isSelect)
            {
                rst = "-" + mm.MainGame.SelectName;
            }
            syn.Run(() =>
            {
                Console.WriteLine(rst);
                toolStrip_selectName.Text = rst;

            });
        }

        private void MainGame_onLocationChange2(Point before, Point now)
        {
            syn.Run(() =>
            {
                toolStrip_postion.Text = now.ToString();
                toolStrip_mapName.Text = mm.MainGame.CurrentMap.Name;
            });
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click_2(object sender, EventArgs e)
        {
            MainGame m = mm.MainGame;
            if (m == null)
            {
                if (findGame())
                {
                    m = new MainGame(ps[0]);
                }
            }
            if (m == null)
            {
                return;
            }
            Form3 f3 = new Form3();
            f3.game = m;
            f3.Show();
        }

        private const uint WS_EX_LAYERED = 0x80000;
        private const int WS_EX_TRANSPARENT = 0x20;
        private const int GWL_STYLE = (-16);
        private const int GWL_EXSTYLE = (-20);
        private const int LWA_ALPHA = 0;



        private void btn_dy2_Click(object sender, EventArgs e)
        {

        }


        private void button6_Click_1(object sender, EventArgs e)
        {
            new Form2().Show();
        }

        private void button14_Click(object sender, EventArgs e)
        {
            签到.开始签到(mm.MainGame);
        }

        private void button16_Click(object sender, EventArgs e)
        {
            ToDaySlD.Start升龙殿(mm.MainGame);
        }

        private void button17_Click(object sender, EventArgs e)
        {
            this.txtX.Text = MapBase.MapRectangle.X.ToString();
            txtY.Text = MapBase.MapRectangle.Y.ToString();
            txtW.Text = MapBase.MapRectangle.Width.ToString();
            txtH.Text = MapBase.MapRectangle.Height.ToString();
        }

        private void button21_Click(object sender, EventArgs e)
        {
            MapBase.MapRectangle = this.getRec();
        }

        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {
            t_polling_game.Enabled = checkBox7.Checked;
            check_YZM.Checked = checkBox7.Checked;
        }


        int pg = 0;
        private void t_polling_game_Tick(object sender, EventArgs e)
        {
            pg++;
            if (pg > mm.Count)
            {
                pg = 0;
            }
            try
            {
                mm[pg].Active();
            }
            catch (Exception)
            {

            }
        }

        private void button22_Click(object sender, EventArgs e)
        {
            AutoInXHZ.run(mm.MainGame);
        }

        private void button29_Click_1(object sender, EventArgs e)
        {
            mm.MainGame.AutoGet = !mm.MainGame.AutoGet;
            button29.Text = "拾取:" + mm.MainGame.AutoGet;
        }

        private void button31_Click(object sender, EventArgs e)
        {
            StringBuilder s = new StringBuilder("\r\n");
            for (int i = 0; i < mm.MainGame.MainProcess.Modules.Count; i++)
            {
                s.Append("基址列表：").Append("0X" + mm.MainGame.MainProcess.Modules[i].EntryPointAddress.ToString("X8")).Append("\r\n");
            }
            this.richTextBox1.Text += s.ToString();
        }

        private bool 反向吗 = false;
        private void checkBox10_CheckedChanged(object sender, EventArgs e)
        {
            反向吗 = checkBox10.Checked;
        }

        private void button32_Click(object sender, EventArgs e)
        {
            //Package pa = new Package();
            //pa.game = mm.MainGame;
            //pa.Show();
            if (mm.MainGame == null) return;
            var point = TiLian.show提练Win(mm.MainGame);
            WuPingLanInfo wi = new WuPingLanInfo(mm.MainGame);
            wi.WuPings();
        }

        private void button35_Click_1(object sender, EventArgs e)
        {
            WsThread.Run(() =>
            {
                new WuPingLanInfo(mm.MainGame).TilianAllForce();
                mm.MainGame.drawMouse();
            });
        }

        private void checkBox2_CheckedChanged_1(object sender, EventArgs e)
        {
            SystemConfig.setHidePerson(mm.MainGame, checkBox2.Checked);
        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        List<Point> runPoints = new List<Point>();
        private void button36_Click_1(object sender, EventArgs e)
        {
            btn_rec_run.Enabled = false;
            btn_rec_start.Enabled = false;
            btn_rec_stop.Enabled = true;
            runPoints = new List<Point>();
            mm.MainGame.onLocationChange += MainGame_onLocationChange;
        }

        private void MainGame_onLocationChange(Point before, Point now)
        {
            runPoints.Add(now);
            Log.log("rec---->" + now);
        }

        private void button40_Click(object sender, EventArgs e)
        {
            btn_rec_run.Enabled = true;
            btn_rec_start.Enabled = true;
            btn_rec_stop.Enabled = false;
            mm.MainGame.onLocationChange -= MainGame_onLocationChange;
            StringBuilder s = new StringBuilder();
            foreach (var item in runPoints)
            {
                s.Append(item.X).Append(" ").Append(item.Y).Append("\r\n");
            }
            string dir = AppDomain.CurrentDomain.BaseDirectory + "RunPath/";
            Directory.CreateDirectory(dir);
            File.WriteAllText(dir + mm.MainGame.CurrentMap.Name + Environment.TickCount + ".txt", s.ToString());

        }

        private void button41_Click(object sender, EventArgs e)
        {
            if (runPoints.Count == 0)
            {
                WSTools.WSWinFn.ShowWarn("无位置，跳过");
                return;
            }
            mm.MainGame.isXUNHUAN = this.checkBox8.Checked;
            mm.MainGame.runPath(runPoints.ToArray(), new Action(() =>
            {
                WSTools.WSWinFn.ShowWarn("执行完成!");
            }));
        }

        private void check_lock_CheckedChanged(object sender, EventArgs e)
        {
            if (check_lock.Checked)
            {
                int x = WinTool.getNumFromText(txt_lock_x);
                int y = WinTool.getNumFromText(txt_lock_y);
                if (x == 0 || y == 0)
                {
                    check_lock.Checked = false;
                    return;
                }
                txt_lock_x.Enabled = false;
                txt_lock_y.Enabled = false;
                this.mm.MainGame.onLocationChange += MainGame_onLocationChange1;
            }
            else
            {
                txt_lock_x.Enabled = true;
                txt_lock_y.Enabled = true;
                this.mm.MainGame.onLocationChange -= MainGame_onLocationChange1;

            }
        }

        private void MainGame_onLocationChange1(Point before, Point now)
        {

        }

        private void button36_Click_2(object sender, EventArgs e)
        {
            if (this.pictureBox1.Image == null)
            {
                this.txt_Pic_hash.Text = "";
                return;
            }

            this.txt_Pic_hash.Text = mm.MainGame.UUIDImageRec(this.getRec());
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.mm.MainGame.界面显示.显示人物世界发言();
            //this.Text = "当前攻击状态:" + mm.MainGame.isUseSkill.ToString();
        }

        /// <summary>
        /// 右击车夫
        /// </summary>
        private void doNPC()
        {
            Bitmap bmp = (Bitmap)Image.FromFile(@"D:\qqhximg\yz18229953.bmp");
            // bmp = (Bitmap)Image.FromFile(@"E:\bb.bmp");
            Bitmap bmp2 = (Bitmap)Image.FromFile(@"E:\bb.bmp");
            //mm.MainGame.fin
            using (Bitmap bmpt = mm.MainGame.getImg())
            {
                //var hs = ImageTool.findSigleImg(bmp2, bmpt, Color.FromArgb(255, 255, 138, 0));
                //if (hs.Success)
                //{
                //    mm.MainGame.MouseRigthClick(hs.CenterPoint);
                //}
            }
        }

        System.Timers.Timer t = new System.Timers.Timer();
        SelectInfo si;
        private void btn_dy1_Click(object sender, EventArgs e)
        {
            mm.MainGame.Active();
            ToDaySlD.升龙殿投票NPC任务_打劫(mm.MainGame);
            //if (mm.MainGame.CurrentMap.Name == "天圣原")
            //{
            //   // 天圣原Map.NPC_驿站车夫.MoveTo(mm.MainGame, "祭台");
            //}
            //DrawMapPoints dp = new DrawMapPoints(mm.MainGame);
            //dp.Start();
            //Bitmap bmp = (Bitmap)Image.FromFile(@"D:\qqhximg\ss24923781.bmp");

            //Bitmap br = ImageTool.cutSigleBmp(bmp, Color.FromArgb(255, 255, 138, 0));

            //br.Save("e:\\rr.bmp");

            //ImgStrCls ic = new ImgStrCls();
            //Bitmap bmp = ic.bmpCF;
            //bmp = (Bitmap)Image.FromFile(@"D:\qqhximg\yz18229953.bmp");
            //Dictionary<Color, int> dir = new Dictionary<Color, int>();
            //Color c = Color.FromArgb(255, 255, 138, 0);
            //Bitmap bt = new Bitmap(bmp.Width, bmp.Height);
            //List<Point> li = new List<Point>();

            //for (int i = 0; i < bmp.Width; i++)
            //{
            //    for (int j = 0; j < bmp.Height; j++)
            //    {
            //        Color cc = bmp.GetPixel(i, j);
            //        if(c==cc)
            //       // if (cc.A == 255 && cc.R == 255 && cc.G == 138 && cc.B == 0)
            //        {
            //            // bt.SetPixel(i, j, cc);
            //            li.Add(new Point(i, j));
            //        }
            //        if (dir.ContainsKey(cc))
            //        {
            //            dir[cc]++;
            //        }
            //        else
            //        {
            //            dir.Add(cc, 1);
            //        }
            //    }
            //}
            //bt.Save("E:\\bb.bmp");
            //"".ToString();
        }

        private void btn_dy(object sender, EventArgs e)
        {
            //AsyncPortScan.Maint(new string[] { "192.168.1.1" });
            Console.WriteLine("OK");
            //  mm.MainGame.movePersion.MovePoint(151, 134);
            //HXmain.HXInfo.Map.Map升龙殿.Map.NPC_宅院驿丞.MoveTo(mm.MainGame);
            //HXmain.HXInfo.NPC.升龙殿.NPC_宅院驿丞.排行榜任务(mm.MainGame);
            // 领取俸禄

            //HXmain.HXAction.ToDaySlD.升龙殿官职晋升NPC任务1(mm.MainGame);
            //Mouse.HideMouse();
            //AUTOTiLianWuPing aw = new AUTOTiLianWuPing(mm.MainGame);
            //aw.StartAutoTiLian();
            // AUTOTiLianWuPing aw = new AUTOTiLianWuPing(mm.MainGame);
            //aw.StartAutoTiLian();
            //var hs = mm.MainGame.findImg(Resources.ww52889578);
            // this.Text = hs.Success + hs.Point.ToString();
            //var bmp = mm.MainGame.getImg(new Rectangle(756, 298, 42, 9));
            //var suc = "";
            //var hs = mm.MainGame.findImg(bmp);
            //suc = hs.Success + "_" + hs.Point.ToString();

            //bmp.Save("d://cc.bmp", ImageFormat.Png);

            //hs = mm.MainGame.findImg((Bitmap)Image.FromFile("d://cc.bmp"));

            //hs = mm.MainGame.findImg(Resources.ww52889578);
            //suc = hs.Success + "_" + hs.Point.ToString();
            //this.Text = suc;
            //Mouse.move(500, 500);
            //var i = new WuPingLanInfo(mm.MainGame).getEmpty();

            //MessageBox.Show(i.Count+"");
            //var b = ImageTool.ZoomBmp(ImageTool.CopyPriScreen(new Rectangle(100, 100, 200, 200)), 20);


            //b.Save("f://vv.bmp");
            //mm.MainGame.Active();
            //mm.MainGame.SendKey(System.Windows.Forms.Keys.D0);
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            CopyScreenForm cf = new CopyScreenForm();
            cf.onSucSelectEventHandler2 += Cf_onSucSelectEventHandler21;
            cf.Show();
        }

        private bool isPNG = false;//是否存为PNG格式
        private Bitmap png_temp = null;

        private void Cf_onSucSelectEventHandler21(Point start, Point end)
        {
            if (mm.MainGame == null)
            {
                return;
            }
            Point pleft = mm.MainGame.ToClientPoint(start);
            Point right = mm.MainGame.ToClientPoint(end);

            syn.Run(() =>
            {
                txtX.Text = pleft.X.ToString();
                txtY.Text = pleft.Y.ToString();

                txtW.Text = (right.X - pleft.X) + "";
                txtH.Text = (right.Y - pleft.Y) + "";

                button4_Click(null, null);
                png_temp = ImageTool.cutSigleBmp((Bitmap)this.pictureBox1.Image, Color.FromArgb(255, 255, 138, 0));
                Point[] png_temps = ImageTool.cutSigleBmpPoints((Bitmap)this.pictureBox1.Image, Color.FromArgb(255, 255, 138, 0));

                NpcData nd = new NpcData();
                nd.MapName = mm.MainGame.CurrentMap.Name;
                nd.Name = "驿站车夫";
                nd.NPCPoint = png_temps;
                NpcData.addNpcInfo(nd);
                // WSTools.WSSerialize.BinSerialize.toFile("e:\\vv.dmp",png_temp);

                this.pictureBox1.Image = png_temp;
                isPNG = true;
            });
        }

        private void check_autoTilian_CheckedChanged(object sender, EventArgs e)
        {
            mm.AutoTilian = check_autoTilian.Enabled;
        }

        private void button40_Click_1(object sender, EventArgs e)
        {
            mm.MainGame.Active();
            mm.MainGame.AutoRun();
        }

        private void button41_Click_1(object sender, EventArgs e)
        {
            var pri = button41.Text;
            button41.Text = "运行中...";
            button41.Enabled = false;
            WsThread.Run(() =>
            {
                try
                {
                    for (int j = 0; j < 3; j++)
                    {
                        for (int i = 0; i < mm.Count; i++)
                        {
                            Log.logForce("三海经日常开始1:");
                            try
                            {
                                mm[i].Active();
                            }
                            catch (Exception ex)
                            {
                                Log.logErrorForce("激活异常!", ex);
                            }
                            Log.logForce("激活完开始任务:");
                            三海经.三第经日常(mm[i]);
                            if (j == 2)
                            {
                                签到.开始签到(mm[i]);
                            }
                        }
                        Log.logForce("三海经日休息中:");
                        WsThread.Run(() =>
                        {
                            mm.MainGame.Active();
                            mm.MainGame.AutoRun();
                        });
                        Thread.Sleep(5 * 60 * 1000 + 10);//休息5分10秒
                    }
                    Log.logForce("三海经日休结束!");
                }
                finally
                {
                    syn.Run(() =>
                    {
                        button41.Text = pri;
                        button41.Enabled = true;
                    });
                }

            });
        }


        private void printChild(IntPtr main, string child)
        {
            IntPtr winPtr = u32.GetWindow(main, u32.GetWindowCmd.GW_CHILD);
            do
            {
                //4、继续获取下一个子窗口
                StringBuilder f = new StringBuilder(1000);
                u32.GetClassName(winPtr, f, 1000);
                System.Diagnostics.Trace.Write(child + winPtr);
                System.Diagnostics.Trace.Write(" \t ");
                System.Diagnostics.Trace.WriteLine(f);
                IntPtr c = u32.GetWindow(winPtr, u32.GetWindowCmd.GW_CHILD);
                if (c != IntPtr.Zero)
                {
                    printChild(winPtr, child + "  -c- ");
                }

                if ("DSOFramerDocWnd" == f.ToString())
                {
                    u32.RECT re = new u32.RECT();
                    u32.GetWindowRect(winPtr, ref re);
                    System.Diagnostics.Trace.Write("before -- ");
                    System.Diagnostics.Trace.WriteLine(re);
                    u32.ShowWindow(winPtr, u32.nCMDShowModel.SW_HIDE);
                    SendMessage(winPtr, 0x10, IntPtr.Zero, IntPtr.Zero);

                    u32.GetWindowRect(winPtr, ref re);
                    System.Diagnostics.Trace.Write("after -- ");
                    System.Diagnostics.Trace.WriteLine(re);
                }
                winPtr = u32.GetWindow(winPtr, u32.GetWindowCmd.GW_HWNDNEXT);
            } while (winPtr != IntPtr.Zero);
        }

        private void button42_Click(object sender, EventArgs e)
        {
            IntPtr desktopPtr = u32.GetDesktopWindow();            //2、获得一个子窗口（这通常是一个顶层窗口，当前活动的窗口）

            IntPtr ff = new IntPtr(0x00270FE8);
            FindWindow("DSOFramerDocWnd", null);

            Process p = Process.GetProcessesByName("WsToolsTest")[0];
            IntPtr winPtr = p.MainWindowHandle;

            try
            {
                printChild(p.MainWindowHandle, "");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex);
            }

            //DSOFramerOCXWnd
            //winPtr = GetWindow(winPtr, GetWindowCmd.GW_CHILD);
            //// winPtr = desktopPtr;
            //while (winPtr != IntPtr.Zero)

            //{                //4、继续获取下一个子窗口

            //    winPtr = GetWindow(winPtr, GetWindowCmd.GW_HWNDNEXT);
            //    StringBuilder f = new StringBuilder(1000);
            //    u32.GetClassName(winPtr, f, 1000);
            //    System.Diagnostics.Trace.Write(winPtr);
            //    System.Diagnostics.Trace.Write(" \t ");
            //    System.Diagnostics.Trace.WriteLine(f);
            //    if (f.ToString() == "DSOFramerOCXWnd")
            //    {
            //        printChild(winPtr);
            //    }
            //    // u32.ShowWindow(winPtr, WSTools.WSWinAPI.nCMDShowModel.SW_MAXIMIZE);
            //}

            //WSTools.WSWinAPI.WinUser32.SetForegroundWindow(ff);
            WSTools.WSWinAPI.WinUser32.MoveWindow(ff, 100, 100, 200, 200, true);
            //WSTools.WSWinAPI.WinUser32.ShowWindow(ff, WSTools.WSWinAPI.nCMDShowModel.SW_HIDE);
            return;

            IntPtr pt = FindWindow("DSOFramerOCXWnd", null);
            if (pt == IntPtr.Zero)
            {
                pt = FindWindow("DSOFramerDocWnd", null);
            }
            if (pt != null)
            {
                WSTools.WSWinAPI.WinUser32.SetForegroundWindow(pt);
            }

            return;
            //IntPtr winPtr = GetWindow(desktopPtr, GetWindowCmd.GW_CHILD);            //3、循环取得桌面下的所有子窗口




            ////IntPtr pt = new IntPtr(0x002511B0);
            //// WSTools.WSWinAPI.WinUser32.ShowWindow(pt, WSTools.WSWinAPI.nCMDShowModel.SW_MAXIMIZE);

            //while (winPtr != IntPtr.Zero)

            //{                //4、继续获取下一个子窗口

            //    winPtr = GetWindow(winPtr, GetWindowCmd.GW_HWNDNEXT);

            //    //Trace.WriteLine(winPtr);
            //    var rec = new WSTools.WSWinAPI.WinUser32.RECT();
            //    WSTools.WSWinAPI.WinUser32.GetWindowRect(winPtr, ref rec);
            //    if (rec.Left > 100 && rec.Left < 150)
            //    {
            //        if (rec.Top > 100 && rec.Top < 150)
            //        {
            //            Trace.WriteLine(winPtr);
            //            WSTools.WSWinAPI.WinUser32.ShowWindow(winPtr, WSTools.WSWinAPI.nCMDShowModel.SW_HIDE);
            //            WSTools.WSWinAPI.WinUser32.MoveWindow(winPtr, 100, 100, 200, 200, true);
            //        }
            //    }
            //    //WSTools.WSWinAPI.WinUser32
            //    //"AXWIN Frame Window"
            //}
        }


        private void button43_Click(object sender, EventArgs e)
        {
            //Process p = Process.GetProcessesByName("WsToolsTest")[0];
            //IntPtr winPtr = p.MainWindowHandle;
            List<IntPtr> ptr = WindowTool.findWindow("DSOFramerDocWnd");
            Console.WriteLine("数量总计为:" + ptr.Count);
            if (ptr.Count <= 1)
            {
                return;
            }
            else
            {
                for (int i = 0; i < ptr.Count - 1; i++)
                {
                    Console.WriteLine("close ptr：" + ptr[i]);
                    WindowTool.CloseWin(ptr[i]);
                }
            }
        }

        private void button44_Click(object sender, EventArgs e)
        {
            List<IntPtr> ptr = WindowTool.findWindow("DSOFramerDocWnd");
            Console.WriteLine("数量总计为:" + ptr.Count);
            System.Diagnostics.Trace.WriteLine("数量总计为:" + ptr.Count);
        }

        private void checkBox12_CheckedChanged(object sender, EventArgs e)
        {
            mm.MainGame.RightClick = checkBox12.Checked;
        }

        private void button45_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Clipboard.SetText(txtX.Text + "," + txtY.Text);
        }

        private void button46_Click(object sender, EventArgs e)
        {
            mm.MainGame.MouseMove(getRec().Location);
        }
    }

    class PtrGame
    {
        public IntPtr ptr { get; set; }
        public Process Process { get; set; }
        public bool isGame { get; set; }
    }


    class ProcessItem
    {
        public Process Process { get; set; }
        public ProcessItem(Process p)
        {
            Process = p;
        }

        public override string ToString()
        {
            return Process.ProcessName;
        }
    }

    class GameItem
    {

        public Process Process { get; set; }
        public GameItem(Process p)
        {
            Process = p;
        }

        public override string ToString()
        {
            return Process.Id.ToString();
        }
    }
}
