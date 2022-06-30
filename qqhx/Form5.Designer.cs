namespace qqhx
{
    partial class Form5
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.radio_move = new System.Windows.Forms.RadioButton();
            this.radio = new System.Windows.Forms.RadioButton();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.lab_x = new System.Windows.Forms.TextBox();
            this.lab_y = new System.Windows.Forms.TextBox();
            this.lab_width = new System.Windows.Forms.TextBox();
            this.lab_height = new System.Windows.Forms.TextBox();
            this.btn_refresh = new System.Windows.Forms.Button();
            this.btn_import = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btn_drawLine = new System.Windows.Forms.Button();
            this.check_wuping = new System.Windows.Forms.CheckBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.button5 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(86, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "上";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(86, 51);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 1;
            this.button2.Text = "下";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(13, 32);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 2;
            this.button3.Text = "左";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(158, 32);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 23);
            this.button4.TabIndex = 3;
            this.button4.Text = "右";
            this.button4.UseVisualStyleBackColor = true;
            // 
            // radio_move
            // 
            this.radio_move.AutoSize = true;
            this.radio_move.Checked = true;
            this.radio_move.Location = new System.Drawing.Point(292, 32);
            this.radio_move.Name = "radio_move";
            this.radio_move.Size = new System.Drawing.Size(47, 16);
            this.radio_move.TabIndex = 4;
            this.radio_move.TabStop = true;
            this.radio_move.Text = "移动";
            this.radio_move.UseVisualStyleBackColor = true;
            // 
            // radio
            // 
            this.radio.AutoSize = true;
            this.radio.Location = new System.Drawing.Point(346, 31);
            this.radio.Name = "radio";
            this.radio.Size = new System.Drawing.Size(47, 16);
            this.radio.TabIndex = 5;
            this.radio.Text = "剪切";
            this.radio.UseVisualStyleBackColor = true;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(68, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(342, 240);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 6;
            this.pictureBox1.TabStop = false;
            // 
            // lab_x
            // 
            this.lab_x.Location = new System.Drawing.Point(86, 102);
            this.lab_x.Name = "lab_x";
            this.lab_x.Size = new System.Drawing.Size(100, 21);
            this.lab_x.TabIndex = 7;
            // 
            // lab_y
            // 
            this.lab_y.Location = new System.Drawing.Point(86, 129);
            this.lab_y.Name = "lab_y";
            this.lab_y.Size = new System.Drawing.Size(100, 21);
            this.lab_y.TabIndex = 8;
            // 
            // lab_width
            // 
            this.lab_width.Location = new System.Drawing.Point(202, 102);
            this.lab_width.Name = "lab_width";
            this.lab_width.Size = new System.Drawing.Size(100, 21);
            this.lab_width.TabIndex = 9;
            // 
            // lab_height
            // 
            this.lab_height.Location = new System.Drawing.Point(202, 129);
            this.lab_height.Name = "lab_height";
            this.lab_height.Size = new System.Drawing.Size(100, 21);
            this.lab_height.TabIndex = 10;
            // 
            // btn_refresh
            // 
            this.btn_refresh.Location = new System.Drawing.Point(318, 100);
            this.btn_refresh.Name = "btn_refresh";
            this.btn_refresh.Size = new System.Drawing.Size(75, 23);
            this.btn_refresh.TabIndex = 11;
            this.btn_refresh.Text = "刷新";
            this.btn_refresh.UseVisualStyleBackColor = true;
            this.btn_refresh.Click += new System.EventHandler(this.btn_refresh_Click);
            // 
            // btn_import
            // 
            this.btn_import.Location = new System.Drawing.Point(318, 127);
            this.btn_import.Name = "btn_import";
            this.btn_import.Size = new System.Drawing.Size(75, 23);
            this.btn_import.TabIndex = 12;
            this.btn_import.Text = "导入...";
            this.btn_import.UseVisualStyleBackColor = true;
            this.btn_import.Click += new System.EventHandler(this.btn_import_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 179);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(776, 271);
            this.panel1.TabIndex = 13;
            // 
            // btn_drawLine
            // 
            this.btn_drawLine.Location = new System.Drawing.Point(420, 72);
            this.btn_drawLine.Name = "btn_drawLine";
            this.btn_drawLine.Size = new System.Drawing.Size(75, 23);
            this.btn_drawLine.TabIndex = 14;
            this.btn_drawLine.Text = "画线";
            this.btn_drawLine.UseVisualStyleBackColor = true;
            this.btn_drawLine.Click += new System.EventHandler(this.btn_drawLine_Click);
            // 
            // check_wuping
            // 
            this.check_wuping.AutoSize = true;
            this.check_wuping.Location = new System.Drawing.Point(292, 57);
            this.check_wuping.Name = "check_wuping";
            this.check_wuping.Size = new System.Drawing.Size(72, 16);
            this.check_wuping.TabIndex = 15;
            this.check_wuping.Text = "选中物品";
            this.check_wuping.UseVisualStyleBackColor = true;
            this.check_wuping.CheckedChanged += new System.EventHandler(this.check_wuping_CheckedChanged);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.listBox1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(615, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(161, 179);
            this.panel2.TabIndex = 16;
            // 
            // listBox1
            // 
            this.listBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 12;
            this.listBox1.Location = new System.Drawing.Point(0, 0);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(161, 179);
            this.listBox1.TabIndex = 0;
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(451, 11);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(75, 23);
            this.button5.TabIndex = 17;
            this.button5.Text = "button5";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // Form5
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(776, 450);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.check_wuping);
            this.Controls.Add(this.btn_drawLine);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btn_import);
            this.Controls.Add(this.btn_refresh);
            this.Controls.Add(this.lab_height);
            this.Controls.Add(this.lab_width);
            this.Controls.Add(this.lab_y);
            this.Controls.Add(this.lab_x);
            this.Controls.Add(this.radio);
            this.Controls.Add(this.radio_move);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Name = "Form5";
            this.Text = "物品掉落查询";
            this.Load += new System.EventHandler(this.Form5_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.RadioButton radio_move;
        private System.Windows.Forms.RadioButton radio;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TextBox lab_x;
        private System.Windows.Forms.TextBox lab_y;
        private System.Windows.Forms.TextBox lab_width;
        private System.Windows.Forms.TextBox lab_height;
        private System.Windows.Forms.Button btn_refresh;
        private System.Windows.Forms.Button btn_import;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btn_drawLine;
        private System.Windows.Forms.CheckBox check_wuping;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Button button5;
    }
}