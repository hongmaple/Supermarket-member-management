using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Login;

namespace 神一样的登录界面
{
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
        }

        DBTools db = new DBTools();
        private void button1_Click(object sender, EventArgs e)
        {
            string z = textBox1.Text.ToString();
            string m = textBox2.Text.ToString();
            string w = textBox3.Text.ToString();
            string h = textBox4.Text.ToString();
            string sql = @"insert into LoginInfo
                           (login,pass,question,answer,status,unlockTime)
                           values('" + z + "','" + m + "','" + w + "','" + h + "','1','')";
            int rs= db.DonIntsdf(sql);
            if (rs == 0)
            {
                MessageBox.Show("注册失败！");
            }
            else
            {
                MessageBox.Show("注册成功！");
            }
        }
    }
}
