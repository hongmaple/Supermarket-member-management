using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using 网吧系统;
using 会员管理系统;
using EmployeeManager;

namespace Login
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void panel1_MouseEnter(object sender, EventArgs e)
        {
            panel1.BorderStyle = BorderStyle.Fixed3D;
        }
        private void panel1_MouseLeave(object sender, EventArgs e)
        {
            panel1.BorderStyle = BorderStyle.None;
        }
        private void panel3_MouseEnter(object sender, EventArgs e)
        {
            panel3.BorderStyle = BorderStyle.Fixed3D;
        }

        private void panel3_MouseLeave(object sender, EventArgs e)
        {
            panel3.BorderStyle = BorderStyle.None;
        }

        private void panel4_MouseEnter(object sender, EventArgs e)
        {
            panel4.BorderStyle = BorderStyle.Fixed3D;
        }

        private void panel4_MouseLeave(object sender, EventArgs e)
        {
            panel4.BorderStyle = BorderStyle.None;
        }

        private void panel5_MouseEnter(object sender, EventArgs e)
        {
            panel5.BorderStyle = BorderStyle.Fixed3D;
        }

        private void panel5_MouseLeave(object sender, EventArgs e)
        {
            panel5.BorderStyle = BorderStyle.None;
        }
        private void 在线帮助ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //OpenUrl("http://www.baidu.com", true);
            //System.Diagnostics.Process.Start("explorer.exe", "http://www.hnu.edu.cn/");

            System.Diagnostics.Process.Start("http://www.hnu.edu.cn/");
            //调用IE浏览器  
            //System.Diagnostics.Process.Start("iexplore.exe", "http://blog.csdn.net/testcs_dn");
        }

        private void panel5_Click(object sender, EventArgs e)//退出程序
        {
            DialogResult rs= MessageBox.Show("确定要退出程序吗？","提示",MessageBoxButtons.YesNo,MessageBoxIcon.Asterisk);
            if (rs==DialogResult.Yes)
            {
                Application.Exit();
            }
        }
        private void 锁定屏幕ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            suopin sc = new suopin();
            sc.ShowDialog(this);
        }

        private void panel1_Click(object sender, EventArgs e)//会员消费
        {
            panel6.Controls.Clear();
            Form2 cc = new Form2();
            cc.FormBorderStyle = FormBorderStyle.None;
            cc.MdiParent = this;
            cc.MdiParent = this;
            cc.TopLevel = false;
            this.panel6.Controls.Add(cc);
            cc.Show();
        }

        private void panel3_Click(object sender, EventArgs e)//会员管理
        {
            panel6.Controls.Clear();
            Management cc = new Management();
            cc.FormBorderStyle = FormBorderStyle.None;
            cc.MdiParent = this;
            cc.MdiParent = this;
            cc.TopLevel = false;
            this.panel6.Controls.Add(cc);
            cc.Show();
        }

        private void panel4_Click(object sender, EventArgs e)//系统设置
        {
            panel6.Controls.Clear();
            系统设置 cc = new 系统设置();
            cc.FormBorderStyle = FormBorderStyle.None;
            cc.MdiParent = this;
            cc.TopLevel = false;
            cc.Parent = panel6;
            cc.Show();
        }
        
        protected override CreateParams CreateParams
        {
            get
            {
 
                CreateParams cp = base.CreateParams;
 
                cp.ExStyle |= 0x02000000;  // Turn on WS_EX_COMPOSITED  
 
                if (this.IsXpOr2003 == true)
                {
                    cp.ExStyle |= 0x00080000;  // Turn on WS_EX_LAYERED
                    this.Opacity = 1;
                }
 
                return cp;
 
            }
 
        }  //防止闪烁

        private Boolean IsXpOr2003
        {
            get
            {
                OperatingSystem os = Environment.OSVersion;
                Version vs = os.Version;

                if (os.Platform == PlatformID.Win32NT)
                    if ((vs.Major == 5) && (vs.Minor != 0))
                        return true;
                    else
                        return false;
                else
                    return false;
            }
        }
        private void Form1_Load(object sender, EventArgs e)
            //窗体加载时,把所有窗体绘制在选项卡中，以达到，切换子窗体时不会影响之前操作的记录，但依旧有是冲突，无法达到完美的效果
        {
            Form2 cc = new Form2();
            cc.MdiParent = this;
            cc.Parent = panel6;
            cc.Show();

            //tabControl1.SelectedIndex = 1;
            //Management cc = new Management();
            //cc.FormBorderStyle = FormBorderStyle.None;
            ////cc.MdiParent = this;
            //cc.TopLevel = false;
            //this.panel7.Controls.Add(cc);
            //cc.Show();
            //tabControl1.SelectedIndex = 2;
            //系统设置 hui = new 系统设置();
            //hui.FormBorderStyle = FormBorderStyle.None;
            //// hui.MdiParent = this;
            //hui.TopLevel = false;
            //this.panel8.Controls.Add(hui);
            //hui.Show();
            //tabControl1.SelectedIndex = 0;
            //Form2 nn = new Form2();
            ////nn.MdiParent = this;
            ////nn.Parent = panel1;
            //nn.TopLevel = false;
            //this.panel6.Controls.Add(nn);
            //nn.Show();
            // panel6.Controls.Clear();

            //TabPage tab = new TabPage();
            //tab.Name = "bomo";
            //tab.Text = "选项卡1";

            //Form2 form = new Form2();

            //form.TopLevel = false;     //设置为非顶级控件

            //tab.Controls.Add(form);

            //tabControl1.TabPages.Add(tab);
            //TabPage tab2= new TabPage();
            //form.Show();
            //tab2.Name = "bomo1";
            //tab2.Text = "选项卡2";
            //Management cc = new Management();

            //cc.TopLevel = false;     //设置为非顶级控件

            //tab.Controls.Add(form);

            //tabControl1.TabPages.Add(tab2);

            //cc.Show();   

            
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void 修改密码YToolStripMenuItem_Click(object sender, EventArgs e)
        {
            神一样的登录界面.Form3 nn = new 神一样的登录界面.Form3();
            nn.zha = StaticPropertiesOfClasses.Id;
            nn.ShowDialog();
        }

        private void 系统设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.panel4_Click(sender, e);
        }

        private void 会员消费ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panel1_Click(sender, e);
        }

        private void 会员管理ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panel3_Click(sender, e);
        }

        private void 关于我们ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            lunbo jj = new lunbo();
            jj.ShowDialog();
        }
    }
}
