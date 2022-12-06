using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TestWin
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        static string root = @"D:\qqhximg\test\";
        static string path = root + "x.bmp";

        private void Form1_Load(object sender, EventArgs e)
        {
            string[] pathf = System.IO.Directory.GetFiles(@"D:\qqhximg\test\vv");

            this.comboBox1.Items.AddRange(pathf);
            //foreach (var item in pathf)
            //{
            //    Filter(item);
            //}
        }

        private Image FilterImg(string pp)
        {
            Bitmap im = (Bitmap)System.Drawing.Image.FromFile(pp);
            for (int i = 0; i < im.Width; i++)
            {
                for (int j = 0; j < im.Height; j++)
                {
                    Color c = im.GetPixel(i, j);
                    //&& inRange(c.G) && inRange(c.B)
                    // r  50 - 255
                    if (inRange((int)numericUpDown1.Value, (int)numericUpDown2.Value, c.R) &&
                        inRange((int)numericUpDown3.Value, (int)numericUpDown4.Value, c.G) &&
                        inRange((int)numericUpDown5.Value, (int)numericUpDown6.Value, c.B)
                        )
                    {
                        im.SetPixel(i, j, Color.FromArgb(c.R, c.G, c.B));

                    }
                    else
                    {
                        im.SetPixel(i, j, Color.FromArgb(0, 0, 0));
                    }
                }
            }
            //string svae = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(pp), DateTime.Now.Ticks + "_XXX.bmp");
            return im;
        }

        private Image Filter(string pp)
        {
            Bitmap im = (Bitmap)System.Drawing.Image.FromFile(pp);
            for (int i = 0; i < im.Width; i++)
            {
                for (int j = 0; j < im.Height; j++)
                {
                    Color c = im.GetPixel(i, j);
                    //&& inRange(c.G) && inRange(c.B)
                    // r  50 - 255
                    if (inRange(150, 255, c.G))
                    {
                        im.SetPixel(i, j, Color.FromArgb(c.R, c.G, c.B));

                    }
                    else
                    {
                        im.SetPixel(i, j, Color.FromArgb(0, 0, 0));
                    }
                }
            }
            //string svae = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(pp), DateTime.Now.Ticks + "_XXX.bmp");
            return im;
        }

        private bool inRange(int v)
        {
            return inRange(200, 254, v);
        }

        private bool inRange(int start, int end, int v)
        {
            return v > start && v < end;
        }

        private void button1_Click(object sender, EventArgs e)
        {


        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            refreshPP();
        }

        private string filePath = "";
        private void refreshPP()
        {
            string pp = @"D:\qqhximg\test\vv\637923780732780825.bmp";
            pp = filePath;
            Image im = Image.FromFile(pp);
            Size s = new Size(im.Width * 5, im.Height * 5);
            pictureBox1.Size = s;
            pictureBox2.Size = s;
            this.pictureBox1.Image = im;
            this.pictureBox2.Image = FilterImg(pp);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            filePath = comboBox1.SelectedItem.ToString();
            refreshPP();
        }

        private void numericUpDown5_ValueChanged(object sender, EventArgs e)
        {
            refreshPP();
        }
    }
}