using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EmployeeManager
{
    public partial class lunbo : Form
    {
        public lunbo()
        {
            InitializeComponent();
        }
        private void lunbo_Load(object sender, EventArgs e)
        {
            //pictureBox1.Image = imageList1.Images.
            //if (index < this.ilAbout.Images.Count - 1)
            //{
            //    index++;
            //}
            //else  // 否则从第一个图片开始显示，索引从0开始
            //{
            //    index = 0;
            //}
            //// 设置图片框显示的图片   
            //this.pbAbout.Image = ilAbout.Images[index];
            timer1.Enabled = true;
        }
        int i = 3;
        private void timer1_Tick(object sender, EventArgs e)
        {
            i--;
            if (i == 0)
            {
                i = 3;
            }
            pictureBox2.Image = imageList1.Images[i];
        }

        private void linkLabel1_MouseEnter(object sender, EventArgs e)
        {
            label6.Visible = true;
            label6.Text = "单击加入QQ群";
        }

        private void linkLabel1_MouseLeave(object sender, EventArgs e)
        {
            label6.Visible = false;
        }

        private void label5_MouseEnter(object sender, EventArgs e)
        {
            label7.Visible = true;
            label7.Text = "关闭";
            label6.ForeColor = Color.Blue;
        }

        private void label5_MouseLeave(object sender, EventArgs e)
        {
            label7.Visible = false;
            label6.ForeColor = Color.White;
        }

        private void label5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://jq.qq.com/?_wv=1027&k=54EmKQz");
        }

    }
}
