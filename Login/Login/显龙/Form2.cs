using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using Login;

namespace 神一样的登录界面
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        DBTools db = new DBTools();
        private void button1_Click(object sender, EventArgs e)
        {
            string zh = textBox1.Text;
            if (zh.Trim() == "")
            {
                MessageBox.Show("账号不能为空！");
            }
            else
            {
                string sql = @"select question from LoginInfo
                                where login='" + zh + "'";
                SqlDataReader re = db.reader(sql);
                if (re.Read())
                {
                    textBox2.Text = re["question"].ToString();
                }
                re.Close();
            }
        }

        private void label5_Click(object sender, EventArgs e)
        {
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string zh = textBox1.Text.ToString();
            string hd = textBox3.Text.ToString();
            string sql = @"select count(*) from LoginInfo 
                            where login='" + zh + "' and answer='" + hd + "'";
           int rs = db.Login(sql);
             if (rs == 0)
            {
                 MessageBox.Show("回答错误！");
            }
            else
            {
                 Form3 x = new Form3();
                 x.Show();
                 x.zha = zh;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
