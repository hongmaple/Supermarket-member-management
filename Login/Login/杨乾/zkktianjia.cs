using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Login;
namespace 会员管理系统
{
    public partial class zkktianjia : Form
    {
        DataSet ds = new DataSet();
        DBTools db = new DBTools();
        public int ID;
        public bool bl = false;
        public zkktianjia()
        {
            InitializeComponent();
        }

        public void tj() {
            string djmc = textBox2.Text.Trim();
            string zkl = textBox3.Text.Trim();
            string sql = @"INSERT INTO [Member].[dbo].[CardType]
           ([typeName]
           ,[sale])
     VALUES
           ('" + djmc + "','" + zkl + "')";
            int rs = db.DonIntsdf(sql);
            if (rs == 1)
            {
                MessageBox.Show("保存成功！");
                this.Close();
            }
            else
            {
                MessageBox.Show("保存失败！");
            }
        }

        public void xg() {
            string sql = @"UPDATE [Member].[dbo].[CardType]
                           SET [typeName] = '" + textBox2.Text + "',[sale] = '" + textBox3.Text + "' WHERE id='" + ID + "'";
            int ds = db.DonIntsdf(sql);
            if (ds == 1)
            {
                MessageBox.Show("修改成功！");
                this.Close();
            }
            else
            {
                MessageBox.Show("修改失败！");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (bl==true)
            {
                xg();
            }
            else
            {
                tj();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public void Loadnr() {
            string sql = "select * from CardType where id='" + ID + "'";
            SqlDataReader reader = db.reader(sql);
            if (reader.Read())
            {
                textBox1.Text = ID.ToString();
                textBox2.Text = reader["typeName"].ToString();
                textBox3.Text = reader["sale"].ToString();
            }
        }

        private void zkktianjia_Load(object sender, EventArgs e)
        {
            if (bl==true)
            {
                Loadnr();
            }
        }
    }
}
