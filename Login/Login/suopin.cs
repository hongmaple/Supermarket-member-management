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
    public partial class suopin : Form
    {
        public suopin()
        {
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            textBox1.Text = textBox1.Text + btn.Text;
        }
        string suoping;
        string jiesuo;
        private void button1_MouseEnter(object sender, EventArgs e)
        {
            //Button btn = (Button)sender;
            //textBox1.Text = textBox1.Text+btn.Text;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
             e.Cancel = bl;
        }
        bool bl = false;
        private void button12_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            if (textBox1.Text == "" && btn.Text == "锁屏")
            {
                MessageBox.Show("请设置密码", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else  if (btn.Text == "锁屏")
            {
                toolStrip1.Visible = false;
                textBox1.Visible = false;
                button1.Visible = false;
                button2.Visible = false;
                button3.Visible = false;
                button4.Visible = false;
                button5.Visible = false;
                button6.Visible = false;
                button7.Visible = false;
                button8.Visible = false;
                button9.Visible = false;
                button10.Visible = false;
                button11.Visible = false;
                button12.Visible = false;
                pictureBox1.Visible = true;
                bl = true;
                suoping = textBox1.Text;
                textBox1.Text = "";
                btn.Text = "解锁";
                label1.Visible = false;
               // MessageBox.Show("窗体已锁定", "提示");
            }
            else
            {
                jiesuo = textBox1.Text;
                if (suoping == jiesuo)
                {
                    bl = false;
                    //MessageBox.Show("窗体关闭中.......", "提示");
                    this.Close();
                }
                else if(textBox1.Text=="")
                {
                    MessageBox.Show("请输入密码", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
                else
                {
                    MessageBox.Show("密码不正确", "错误", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
            }
        }
        private void label1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label1_MouseEnter(object sender, EventArgs e)
        {
            label2.Text = "关闭";
            label1.BackColor = Color.White;
            label1.ForeColor = Color.Blue;
        }

        private void label1_MouseLeave(object sender, EventArgs e)
        {
            label2.Text = "";
            label1.BackColor = Color.Transparent;
            label1.ForeColor = Color.White;
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            string wb = textBox1.Text;
            int cd = wb.Length;
            if (textBox1.Text != "")
            {
                string sc = wb.Substring(0, cd - 1);
                textBox1.Text = sc;
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            pictureBox1.Visible = false;
            toolStrip1.Visible = true;
            textBox1.Visible = true;
            button1.Visible = true;
            button2.Visible = true;
            button3.Visible = true;
            button4.Visible = true;
            button5.Visible = true;
            button6.Visible = true;
            button7.Visible = true;
            button8.Visible = true;
            button9.Visible = true;
            button10.Visible = true;
            button11.Visible = true;
            button12.Visible = true;
        }
    }
}
