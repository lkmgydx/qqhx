using HFrameWork.SystemDll;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HXmain
{

    public class InsertWindow
    {
        Process _mainProcess;
        Timer _tick;
        User32.RECT _pri_rec;
        Point _location;

        Form _form;
        /// <summary>  
        /// 将程序嵌入窗体  
        /// </summary>  
        /// <param name="pW">容器</param>  
        /// <param name="appname">程序名</param>  
        public InsertWindow(Form form, Process process, System.Drawing.Point location)
        {
            _tick = new Timer();
            _pri_rec = new User32.RECT();
            _tick.Tick += T_Tick;
            _tick.Interval = 100;
            _mainProcess = process;
            process.Exited += Process_Exited;
            _location = location;
            _form = form;
            _tick.Start();
        }

        private void Process_Exited(object sender, EventArgs e)
        {
            _tick.Stop();
        }

        private void T_Tick(object sender, EventArgs e)
        {
            User32.RECT rec = getRec();
            if (_pri_rec.left != rec.left || _pri_rec.right != rec.right || _pri_rec.top != rec.top || _pri_rec.bottom != rec.bottom)
            {
                _pri_rec = rec;
                _form.Location = new Point(rec.left + _location.X, rec.top + _location.Y);
            }

        }

        /// <summary>
        /// 获取坐标
        /// </summary>
        /// <returns></returns>
        public User32.RECT getRec()
        {
            User32.RECT rec = new User32.RECT();
            User32.GetWindowRect(_mainProcess.MainWindowHandle, ref rec);
            return rec;
        }
    }
}
