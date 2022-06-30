using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace qqhx
{
    public partial class Form5 : Form
    {
        public Form5()
        {
            InitializeComponent();
            //Form1 f = new Form1();
            //f.Show();
        }

        private Bitmap mainBMP = null;

        private void btn_import_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                mainBMP = Image.FromFile(openFileDialog1.FileName) as Bitmap;

                btn_refresh.PerformClick();
            }
        }

        private void btn_refresh_Click(object sender, EventArgs e)
        {
            //101,164,162
            if (mainBMP == null)
            {
                return;
            }
            lab_width.Text = mainBMP.Width.ToString();
            lab_height.Text = mainBMP.Height.ToString();
            panel1.Height = mainBMP.Height;
            var rec = new Rectangle(new Point(getStr(lab_x), getStr(lab_y)), new Size(getStr(lab_width), getStr(lab_height)));
            if (rec.X + rec.Width > mainBMP.Width)
            {
                rec.Width = mainBMP.Width - rec.X;
            }
            if (rec.Y + rec.Height > mainBMP.Height)
            {
                rec.Height = mainBMP.Height - rec.Y;
            }
            Bitmap bmp = mainBMP.Clone(rec, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

            this.pictureBox1.Image = bmp;
            this.pictureBox1.Width = mainBMP.Width;
            this.pictureBox1.Height = mainBMP.Height;
        }

        private int getStr(TextBox tex)
        {
            int v = 0;
            try
            {
                v = Int32.Parse(tex.Text);
            }
            catch
            {

            }
            tex.Text = v.ToString();
            return v;
        }

        private void Form5_Load(object sender, EventArgs e)
        {
            string file1 = @"11_00_47 - 3298031.bmp";
            string file = @"10_59_18-2758472.bmp";

            initImg("03_11_41-17901317");//115.195.181

            string dir = @"D:\Program Files\hproject\qqhx\qqhx\bin\Debug\img\";
            string[] fs = System.IO.Directory.GetFiles(@"D:\Program Files\hproject\qqhx\qqhx\bin\Debug\img\");

            for (int i = 0; i < fs.Length; i++)
            {
                this.listBox1.Items.Add(fs[i].Replace(dir, "").Replace(".bmp", ""));
            }
        }

        private void initImg(string name)
        {
            string file = @"D:\Program Files\hproject\qqhx\qqhx\bin\Debug\img\" + name + ".bmp";

            this.mainBMP = (Bitmap)Image.FromFile(file);
            btn_refresh.PerformClick();
        }

        bool fill = false;

        private void btn_drawLine_Click(object sender, EventArgs e)
        {
            //  //101,164,162 // 82.130.123  //115.195.181
            int sizeX = 30;
            int sizeY = 30;
            var g = this.pictureBox1.CreateGraphics();
            var ffont = new Font("宋体", 12);

            var p = new System.Drawing.Drawing2D.GraphicsPath(System.Drawing.Drawing2D.FillMode.Winding);
            for (int i = 295; i < 900; i += sizeX) //竖线
                for (int j = 90; j < 600; j += sizeY)
                {
                    Rectangle r = new Rectangle(i, j, sizeX, sizeY);
                    g.DrawRectangle(Pens.Red, r);
                    g.DrawString(i / 50 + "-" + j / 50, ffont, Brushes.Yellow, new Point(i, j));
                    using (Bitmap bmp = mainBMP.Clone(r, mainBMP.PixelFormat))
                    {
                        //hasSomeObj ho = new hasSomeObj(bmp);

                        ////包 中间颜色
                        //ho.addColor(148, 247, 247);
                        //ho.addColor(148, 251, 247);

                        //ho.addColor(198, 227, 214);
                        //// ho.addColor(214, 255, 255);
                        //ho.addColor(231, 251, 247);
                        //// ho.addColor(189, 227, 214);  冲突了
                        //// ho.addColor(173, 231, 231);
                        //ho.addColor(189, 227, 231);
                        //ho.addColor(181, 243, 247);
                        //ho.addColor(198, 227, 231);
                        //ho.addColor(231, 247, 247);


                        ////包  绿色
                        ////  ho.addColor(189 ,243 ,247);
                        //ho.addColor(140, 251, 247);
                        //ho.addColor(148, 227, 231);


                        ////长箱子 中间颜色
                        //ho.addColor(198, 247, 247);
                        //ho.addColor(231, 243, 247);
                        //// ho.addColor(231, 255, 255);
                        ////ho.addColor(181, 255, 255);
                        //ho.addColor(206, 231, 231);
                        //ho.addColor(214, 235, 231);

                        ////长箱子  边上颜色
                        //ho.addColor(140, 227, 206);
                        //ho.addColor(181, 239, 247);
                        ////ho.addColor(173, 231, 231);
                        //ho.addColor(90, 207, 198);
                        //ho.addColor(156, 235, 222);

                        ////ho.addColor(140, 235, 231);
                        ////ho.addColor(148, 251, 247);
                        ////ho.addColor(140, 235, 231);
                        ////ho.addColor(156, 231, 231);
                        ////ho.addColor(189, 239, 247);
                        ////ho.addColor(181, 251, 247);
                        ////ho.addColor(140, 231, 231);
                        ////ho.addColor(115, 215, 198);
                        ////ho.addColor(198, 239, 239);
                        ////ho.addColor(198, 255, 247);



                        ////盒子颜色
                        ////ho.addColor(148, 227, 231);
                        ////ho.addColor(214, 255, 255);
                        ////ho.addColor(173, 243, 247);
                        ////ho.addColor(132, 207, 198);
                        ////ho.addColor(247 ,255 ,255);
                        ////ho.addColor(206 ,239 ,247);
                        ////ho.addColor(132, 207 ,189);

                        ////ho.addColor(222 ,251, 247);//中心
                        ////ho.addColor(189 ,239 ,239);

                        //if (i / 50 == 16 && j / 50 == 8)
                        //{
                        //    "".ToString(); //140,235,231    148,251,247   140,235,231   156,231,231  265,231,231
                        //    //bmp.Save("D://ss.bmp");
                        //} //140,235,231    148,251,247   140,235,231   156,231,231  265,231,231
                        if (fill && Tool.ImageTool.hasSomeOne(bmp))
                        {
                            g.FillRectangle(Brushes.Blue, r);
                        }
                    }
                }
            //for (int i = 0; i < pictureBox1.Width; i += 50) //竖线
            //{
            //   g.DrawLine(Pens.Red, new PointF(i, 0), new PointF( i, pictureBox1.Height));
            //}

            //for (int j = 0; j < pictureBox1.Height; j += 50) //横线
            //{
            //    g.DrawLine(Pens.Red, new PointF(0, j), new PointF(pictureBox1.Width, j));
            //}
            g.DrawPath(Pens.Red, p);
        }

        class hasSomeObj
        {

            private Bitmap bmp;
            List<int[]> ar = new List<int[]>();
            public hasSomeObj(Bitmap bmp)
            {
                this.bmp = bmp;
            }
            public void addColor(int r, int g, int b)
            {
                ar.Add(new int
                    [] { r, g, b });
            }

            public bool hasSome()
            {
                for (int i = 0; i < ar.Count; i++)
                {
                    if (hasSome2(bmp, ar[i][0], ar[i][1], ar[i][2]))
                    {
                        string rst = ar[i][0] + "," + ar[i][1] + "," + ar[i][2];
                        Clipboard.SetText(rst);
                        WSTools.WSWinFn.ShowInfo(rst);
                        return true;
                    }
                }
                return false;
            }

            private bool hasSome2(Bitmap bmp, int r, int g, int b)
            {
                for (int i = 0; i < bmp.Width; i++)
                    for (int j = 0; j < bmp.Height; j++)
                    {
                        var f = bmp.GetPixel(i, j);
                        if (f.R == r && f.G == g && f.B == b)
                        {
                            return true;
                        }
                    }

                return false;
            }


        }


        private bool hasSome(Bitmap bmp)
        {
            for (int i = 0; i < bmp.Width; i++)
                for (int j = 0; j < bmp.Height; j++)
                {
                    //101,164,162 // 82.130.123
                    var f = bmp.GetPixel(i, j);
                    if (f.R == 82 && f.G == 130 && f.B == 123)
                    {
                        return true;
                    }
                }

            return false;
        }

        private bool hasSome2(Bitmap bmp, int r, int g, int b)
        {
            for (int i = 0; i < bmp.Width; i++)
                for (int j = 0; j < bmp.Height; j++)
                {
                    //101,164,162 // 82.130.123
                    var f = bmp.GetPixel(i, j);
                    if (f.R == r && f.G == g && f.B == b)
                    {
                        return true;
                    }
                }

            return false;
        }

        private void check_wuping_CheckedChanged(object sender, EventArgs e)
        {
            this.fill = check_wuping.Checked;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                initImg(listBox1.SelectedItem.ToString());
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            initImg("wp");
            Bitmap bmp = (Bitmap) this.pictureBox1.Image;
            for (int i = 5; i < bmp.Width; i++) {
            }
        }
    }
}
