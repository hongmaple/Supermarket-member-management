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
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        DBTools db = new DBTools();
        public string zha = "";
        private void Form3_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string mm = textBox1.Text.ToString();

            string sql = @"update LoginInfo set pass = '"+mm+"'where login='"+zha+"'";
            int rs = db.DonIntsdf(sql);
            if (rs == 0)
            {
                MessageBox.Show("密码修改失败！");
            }
            else
            {
                MessageBox.Show("密码修改成功！");
                this.Close();
            }
        }
    }
}
