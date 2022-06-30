using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace qqhx
{
    class MultiCombox
    {
        private DataGridView dgv = new DataGridView();
        private Panel panel = new Panel();
        private DataGridViewCheckBoxColumn colCheckbox = new DataGridViewCheckBoxColumn();
        private DataGridViewTextBoxColumn colText = new DataGridViewTextBoxColumn();
        private DataGridViewTextBoxColumn colValue = new DataGridViewTextBoxColumn();
        private Form form;
        private Control targetBox;
        public List<string> displayMember;
        public List<string> valueMember;
        private bool isOpen
        {
            get
            {
                return this.panel.Visible;
            }
        }

        public delegate void onMsgHandler(string msg);
        public event onMsgHandler onMsgEvent;

        private string splitChar = ",";

        /// <summary>
        /// 根据传入值初始化
        /// </summary>
        /// <param name="form">父窗体</param>
        /// <param name="textBox">目标文本框</param>
        /// <param name="displayMember">显示的文本集合</param>
        /// <param name="valueMember">实际值文本集合</param>
        public MultiCombox(Form form, Control targetBox, List<string> displayMember, List<string> valueMember)
        {
            this.form = form;
            this.targetBox = targetBox;
            this.displayMember = displayMember;
            this.valueMember = valueMember;

            this.maxHeigth = 300;

            this.targetBox.LostFocus += TargetBox_LostFocus;
            this.targetBox.Click += TargetBox_Click;
            this.targetBox.TextChanged += TargetBox_TextChanged;

            if (displayMember.Count != valueMember.Count)
            {
                throw new Exception("displayMember和valueMember项数不一致");
            }
            InitControl();
        }

        private void TargetBox_TextChanged(object sender, EventArgs e)
        {
            addText("text change...");
            if (this.targetBox.Text != ShowValue)
            {
                targetBox.Text = ShowValue;
            }
        }

        private void addText(string text)
        {
            //onMsgEvent?.Invoke(text);
        }

        private void TargetBox_Click(object sender, EventArgs e)
        {
            //addText("TargetBox_Click ->" + form.ActiveControl.Name);
            Togglen();
        }

        private void TargetBox_LostFocus(object sender, EventArgs e)
        {
            //addText("TargetBox_LostFocus ->" + form.ActiveControl.Name);
            if (form.ActiveControl != this.targetBox)
            {
               // addText("TargetBox_LostFocus no active close ->" + form.ActiveControl.Name);
                Close();
            }
        }

        private void TargetBox_GotFocus(object sender, EventArgs e)
        {
            //addText("TargetBox_GotFocus ->" + form.ActiveControl.Name);
            Open();
            //Togglen();
        }

        void Togglen()
        {
            if (!isOpen)
                Open();
            else
                Close();
        }

        public int maxHeigth
        {
            get
            {
                return this.panel.Height;
            }
            set { this.panel.Height = value; }
        }

        /// <summary>
        /// 初始化控件
        /// </summary>
        private void InitControl()
        {

            panel.BackColor = form.BackColor;
            panel.AutoScroll = true;
            panel.AutoSize = true;
            panel.BorderStyle = BorderStyle.Fixed3D;
            //panel.Size = new System.Drawing.Size(300, maxHeigth);

            // 
            // colCheckbox
            // 
            colCheckbox.HeaderText = "checkBox";
            colCheckbox.Name = "colCheckbox";
            colCheckbox.ReadOnly = true;
            colCheckbox.Width = 50;
            // 
            // colText
            // 
            colText.HeaderText = "text";
            colText.Name = "colText";
            colText.ReadOnly = true;
            // 
            // colValue
            // 
            colValue.HeaderText = "value";
            colValue.Name = "colValue";
            colValue.ReadOnly = true;
            colValue.Visible = false;

            dgv.AllowUserToResizeColumns = false;
            dgv.AllowUserToResizeRows = false;
            //dgv.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
            dgv.BackgroundColor = System.Drawing.SystemColors.HighlightText;
            dgv.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dgv.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgv.ColumnHeadersVisible = false;
            dgv.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            colCheckbox,
            colText,
            colValue});
            dgv.Dock = System.Windows.Forms.DockStyle.Fill;
            dgv.Location = new System.Drawing.Point(0, 0);
            dgv.MultiSelect = false;
            dgv.Name = "dgv";
            dgv.RowHeadersVisible = false;
            dgv.RowTemplate.Height = 23;
            dgv.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            dgv.Size = new System.Drawing.Size(300, 561);
            dgv.TabIndex = 0;
            dgv.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(dgv_CellClick);

            dgv.Dock = DockStyle.Fill;
            panel.Controls.Add(dgv);
            form.Controls.Add(panel);
            panel.BringToFront();
            panel.Width = targetBox.Width;

            panel.Location = new Point(targetBox.Location.X, targetBox.Location.Y + targetBox.Height);

            dgv.Rows.Clear();

            for (int i = 0; i < displayMember.Count; i++)
            {
                dgv.Rows.Add(false, displayMember[i], valueMember[i]);
            }
            panel.Visible = false;
            panel.Leave += panel_Leave;

        }

        /// <summary>
        /// 失去焦点时关闭窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void panel_Leave(object sender, EventArgs e)
        {
            addText("panel_Leave ->" + form.ActiveControl.Name);
            if (form.ActiveControl != this.targetBox)
            {
                addText("panel_Leave no active close ->" + form.ActiveControl.Name);
                Close();
            }
        }

        private void dgv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            var dgv = sender as DataGridView;
            dgv.Rows[e.RowIndex].Selected = true;
            dgv.Rows[e.RowIndex].Cells[0].Value = !Convert.ToBoolean(dgv.Rows[e.RowIndex].Cells[0].Value);
        }

        /// <summary>
        /// 设置分割的标签
        /// </summary>
        /// <param name="splitChar"></param>
        public void SetSplitChar(string splitChar)
        {
            this.splitChar = splitChar;
        }


        /// <summary>
        /// 获取分割的标签
        /// </summary>
        /// <returns></returns>
        public string GetSplitChar()
        {
            return splitChar;
        }


        /// <summary>
        /// 打开下拉框
        /// </summary>
        public void Open()
        {
            if (isOpen)
                return;
            panel.Visible = true;
        }

        /// <summary>
        /// 关闭下拉框
        /// </summary>
        public void Close()
        {
            if (!isOpen)
                return;

            panel.Visible = false;
            if (targetBox.Text != ShowValue)
            {
                targetBox.Text = ShowValue;
            }

        }

        private string ShowValue
        {
            get
            {
                string rst = "";
                foreach (DataGridViewRow row in dgv.Rows)
                {
                    if (Convert.ToBoolean(row.Cells[colCheckbox.Name].Value))
                    {
                        rst += row.Cells[colText.Name].Value + splitChar;
                    }
                }
                return rst.TrimEnd(splitChar.ToArray());
            }
        }

        private string RealValue
        {
            get
            {
                string rst = "";
                foreach (DataGridViewRow row in dgv.Rows)
                {
                    if (Convert.ToBoolean(row.Cells[colCheckbox.Name].Value))
                    {
                        rst += row.Cells[colValue.Name].Value + splitChar;
                    }
                }
                return rst.TrimEnd(splitChar.ToArray());
            }
        }
    }
}
