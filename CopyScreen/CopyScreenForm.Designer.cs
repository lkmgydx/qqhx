namespace qqhx
{
    partial class CopyScreenForm
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
            this.components = new System.ComponentModel.Container();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.保存sToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.退出EToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.保存sToolStripMenuItem,
            this.退出EToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(116, 48);
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            // 
            // 保存sToolStripMenuItem
            // 
            this.保存sToolStripMenuItem.Name = "保存sToolStripMenuItem";
            this.保存sToolStripMenuItem.Size = new System.Drawing.Size(115, 22);
            this.保存sToolStripMenuItem.Text = "保存(&S)";
            this.保存sToolStripMenuItem.Click += new System.EventHandler(this.保存sToolStripMenuItem_Click);
            // 
            // 退出EToolStripMenuItem
            // 
            this.退出EToolStripMenuItem.Name = "退出EToolStripMenuItem";
            this.退出EToolStripMenuItem.Size = new System.Drawing.Size(115, 22);
            this.退出EToolStripMenuItem.Text = "退出(&E)";
            this.退出EToolStripMenuItem.Click += new System.EventHandler(this.退出EToolStripMenuItem_Click);
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.Filter = "*.bmp|*.bmp";
            // 
            // CopyScreenForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(539, 392);
            this.ContextMenuStrip = this.contextMenuStrip1;
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "CopyScreenForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CopyScreenForm";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.CopyScreenForm_FormClosed);
            this.Load += new System.EventHandler(this.CopyScreenForm_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.CopyScreenForm_Paint);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CopyScreenForm_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.CopyScreenForm_KeyUp);
            this.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.CopyScreenForm_MouseDoubleClick);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.CopyScreenForm_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.CopyScreenForm_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.CopyScreenForm_MouseUp);
            this.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.CopyScreenForm_PreviewKeyDown);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 保存sToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 退出EToolStripMenuItem;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
    }
}