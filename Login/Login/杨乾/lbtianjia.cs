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
namespace 会员管理系统
{
    public partial class lbtianjia : Form
    {
        public bool bl = false;
        public int bh;
        DBTools db = new DBTools();
        public lbtianjia()
        {
            InitializeComponent();
        }
        public void tj() {

            string sql = @"INSERT INTO [Member].[dbo].[GoodsType]
                           ([GoodsTypeName])
                     VALUES
                           ('"+textBox2.Text+"')";
            int hs = db.DonIntsdf(sql);
            if (hs==1)
            {
                MessageBox.Show("添加成功！");
                this.Close();
            }
            else
            {
                MessageBox.Show("添加失败！");
            }
        }

        public void xg()
        {
            string sql = @"UPDATE [Member].[dbo].[GoodsType]
                    SET [GoodsTypeName] = '"+textBox2.Text+"' WHERE GoodsTypeId='"+bh+"'";
            int hs=db.DonIntsdf(sql);
            if (hs==1)
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
            if (bl!=true)
            {
                tj();
            }
            else
            {
                xg();
            }
        }

        private void lbtianjia_Load(object sender, EventArgs e)
        {
            if (bl==true)
            {
                string sql = @"SELECT [GoodsTypeId]
                              ,[GoodsTypeName]
                          FROM [Member].[dbo].[GoodsType]
                            WHERE GoodsTypeId="+bh;
                SqlDataReader reader=db.reader(sql);
                textBox1.Text = bh.ToString();
                if (reader.Read())
                {
                    textBox2.Text = reader["GoodsTypeName"].ToString();
                }
                reader.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
