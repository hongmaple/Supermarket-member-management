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

namespace 网吧系统
{
    public partial class Recharge : Form
    {
        public Recharge()
        {
            InitializeComponent();
        }
        public int zbh;
        GM gm = new GM();
        private void Recharge_Load(object sender, EventArgs e)
        {
            string sql = "select * from MemberInfo where id=" + zbh;
            SqlDataReader reader = gm.reader(sql);
            if (reader.Read())
            {
                label5.Text = reader["id"].ToString();
                label11.Text = reader["name"].ToString();
                label7.Text = reader["Balance"].ToString()+"元";
            }
            reader.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(textBox1.Text.Trim(), @"[0-9]\d*.\d*|\d*[0-9]\d*"))
            {
                string sql = string.Format(@"update MemberInfo
                               set Balance=Balance+({0}+{1}*0.1)
                               where id={2}", textBox1.Text.Trim(), textBox1.Text.Trim(),zbh);
                int hs = gm.Zsg(sql);
                if (hs==1)
                {
                    MessageBox.Show("充值成功！","提示");
                    this.Close();
                }
                else
                {
                    MessageBox.Show("充值失败！", "提示");
                }
                return;
            }
            MessageBox.Show("请输入有效值！", "警告",MessageBoxButtons.OK,MessageBoxIcon.Asterisk);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
                try
                {
                    if (System.Text.RegularExpressions.Regex.IsMatch(textBox1.Text.Trim(), @"[0-9]\d*.\d*|\d*[0-9]\d*"))
                    {
                        double js = double.Parse(textBox1.Text.Trim().ToString());
                        textBox3.Text = (js + js * 0.1).ToString();
                    }
                }
                catch (Exception)
                {
                    textBox1.Text = "0";
                    MessageBox.Show("请输入有效值！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
            }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close(); 
        }
    }
}
