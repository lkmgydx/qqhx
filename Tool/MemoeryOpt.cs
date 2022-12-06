using HFrameWork.SystemDll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Tool
{
    public sealed class MemoeryOpt
    {
        public static string ReadMemoryValueStr(int baseAddress, int processID, int len = 1000)
        {
            try
            {
                byte[] buffer = new byte[len];
                //获取缓冲区地址
                IntPtr byteAddress = Marshal.UnsafeAddrOfPinnedArrayElement(buffer, 0);

                byte[] btone = new byte[1];
                //获取缓冲区地址
                IntPtr byoneAddress = Marshal.UnsafeAddrOfPinnedArrayElement(btone, 0);

                //打开一个已存在的进程对象  0x1F0FFF 最高权限
                IntPtr hProcess = Kernel32.OpenProcess(0x1F0FFF, false, processID);

                int start = baseAddress;
                int end = baseAddress;
                Kernel32.ReadProcessMemory(hProcess, baseAddress, byoneAddress, 1, IntPtr.Zero);

                if (btone[0] == 0)
                {
                    return "";
                }

                //查找前段
                do
                {
                    start--;
                    Kernel32.ReadProcessMemory(hProcess, start, byoneAddress, 1, IntPtr.Zero);
                }
                while (btone[0] != 0);
                start++;
                //查找后段
                do
                {
                    end++;
                    Kernel32.ReadProcessMemory(hProcess, end, byoneAddress, 1, IntPtr.Zero);
                } while (btone[0] != 0);


                byte[] brstBuff = new byte[end - start];
                var ptr = Marshal.UnsafeAddrOfPinnedArrayElement(brstBuff, 0);

                Kernel32.ReadProcessMemory(hProcess, start , ptr, brstBuff.Length, IntPtr.Zero);

                //将制定内存中的值读入缓冲区 
                Kernel32.ReadProcessMemory(hProcess, baseAddress, byteAddress, 100, IntPtr.Zero);

                //关闭操作
                Kernel32.CloseHandle(hProcess);


                ////从非托管内存中读取一个 32 位带符号整数。
                //int max = 0;
                //for (int i = 0; i < buffer.Length; i++)
                //{
                //    if (buffer[i] == 0)
                //    {
                //        max = i;
                //        break;
                //    }
                //}
                //if (max == 0)
                //{
                //    return "";
                //}
                //byte[] bt = new byte[max];
                //Array.Copy(buffer, bt, max);
                return Encoding.GetEncoding("GB2312").GetString(brstBuff);
            }
            catch 
            {
                return "";
            }
            //00193070
        }
        public static string ReadMemoryValueStr(int baseAddress, int processID)
        {
            try
            {
                byte[] buffer = new byte[4];
                //获取缓冲区地址
                IntPtr byteAddress = Marshal.UnsafeAddrOfPinnedArrayElement(buffer, 0);
                //打开一个已存在的进程对象  0x1F0FFF 最高权限
                IntPtr hProcess = Kernel32.OpenProcess(0x1F0FFF, false, processID);
                //将制定内存中的值读入缓冲区
                Kernel32.ReadProcessMemory(hProcess, baseAddress, byteAddress, 4, IntPtr.Zero);
                //关闭操作
                Kernel32.CloseHandle(hProcess);
                //从非托管内存中读取一个 32 位带符号整数。
                Marshal.ReadByte(byteAddress, 0);
                IntPtr ptr = Marshal.ReadIntPtr(byteAddress);
                Marshal.PtrToStringAnsi(byteAddress);
                return GB2312ToUnicode(ptr, Marshal.PtrToStringAnsi(ptr).Length * 2);
            }
            catch
            {
                return "";
            }
            //00193070
        }

        public static string GB2312ToUnicode(IntPtr strNameGb2312, int nStrLen)
        {
            byte[] managedArray = new byte[nStrLen];
            Marshal.Copy(strNameGb2312, managedArray, 0, nStrLen);
            return Encoding.GetEncoding("gb2312").GetString(cutEnd(managedArray));
        }

        private static byte[] cutEnd(byte[] data)
        {
            int i = 0;
            List<byte> li = new List<byte>();
            while (i < data.Length && data[i] != '\0')
            {
                li.Add(data[i]);
                i++;
            }
            return li.ToArray();
        }

        public static int ReadMemoryValue(int baseAddress, int processID)
        {
            try
            {
                byte[] buffer = new byte[4];
                //获取缓冲区地址
                IntPtr byteAddress = Marshal.UnsafeAddrOfPinnedArrayElement(buffer, 0);
                //打开一个已存在的进程对象  0x1F0FFF 最高权限
                IntPtr hProcess = Kernel32.OpenProcess(0x1F0FFF, false, processID);
                //将制定内存中的值读入缓冲区
                Kernel32.ReadProcessMemory(hProcess, baseAddress, byteAddress, 4, IntPtr.Zero);
                //关闭操作
                Kernel32.CloseHandle(hProcess);
                //从非托管内存中读取一个 32 位带符号整数。
                return Marshal.ReadInt32(byteAddress);
            }
            catch
            {
                return 0;
            }
        }

        public static string ReadMemoryValue(int baseAddress, int length, int processID)
        {
            try
            {
                byte[] buffer = new byte[length];
                //获取缓冲区地址
                IntPtr byteAddress = Marshal.UnsafeAddrOfPinnedArrayElement(buffer, 0);
                //打开一个已存在的进程对象  0x1F0FFF 最高权限
                IntPtr hProcess = Kernel32.OpenProcess(0x1F0FFF, false, processID);
                //将制定内存中的值读入缓冲区
                Kernel32.ReadProcessMemory(hProcess, baseAddress, byteAddress, length, IntPtr.Zero);
                //关闭操作
                Kernel32.CloseHandle(hProcess);
                //从非托管内存中读取一个 32 位带符号整数。 
                string rst = Marshal.PtrToStringAnsi(byteAddress);
                return rst;
            }
            catch
            {
                return "";
            }
        }



        //将值写入指定内存地址中
        public static void WriteMemoryValue(int baseAddress, int processID, int value)
        {
            try
            {
                //打开一个已存在的进程对象  0x1F0FFF 最高权限
                IntPtr hProcess = Kernel32.OpenProcess(0x1F0FFF, false, processID);
                //从指定内存中写入字节集数据
                Kernel32.WriteProcessMemory(hProcess, (IntPtr)baseAddress, new int[] { value }, 4, IntPtr.Zero);
                //关闭操作
                Kernel32.CloseHandle(hProcess);
            }
            catch { }
        }
    }
}
