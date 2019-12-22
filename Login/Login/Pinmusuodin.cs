using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Login
{
    public partial class Pinmusuodin : Form
    {
        public Pinmusuodin()
        {
            InitializeComponent();
        }
        string suodin;
        string jiesuo;
        bool no_close = true;
        string zt="锁定";

        private void button1_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            if (zt == "锁定")
            {
                suodin = suodin + btn.Text;
                textBox1.Text = suodin;
            }
            else
            {
                jiesuo = jiesuo + btn.Text;
                textBox1.Text = jiesuo;
            }
        }
        private void button12_Click(object sender, EventArgs e)
        {
            Button btn12 = (Button)sender;
            if (btn12.Text== "锁 定")
            {
                if (textBox1.Text != "")
                {
                    no_close = true;
                    button12.Text = "解 锁";
                    zt = "解锁";
                    textBox1.Text = "";
                }
                else
                {
                    MessageBox.Show("请设置密码");
                }
            }
            else
            {
                if (textBox1.Text!="")
                {
                   if (suodin==jiesuo)
                   {
                       no_close = false;
                       this.Close();
                   }
                   else
                   {
                       MessageBox.Show("密码错误");
                       jiesuo = "";
                   }
                }
                else
                {
                    MessageBox.Show("请输入解锁密码");
                }
            }
        }
        private void Pinmusuodin_FormClosing(object sender, FormClosingEventArgs e)//窗体关闭前的事件
        {
            e.Cancel = no_close;
        }

    }
}
