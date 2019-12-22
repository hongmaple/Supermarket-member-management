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
    public partial class sptj : Form
    {
        public int bh;
        public bool bl = false;
        DBTools db = new DBTools();
        public sptj()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public void tj()
        {
            string sql = string.Format(@"INSERT INTO [Member].[dbo].[Goods]
                           ([goodsName]
                           ,[goodsPoint]
                           ,[goodsCount]
                           ,[goodsTypeId]
                           ,[goodsprice])
                     VALUES
                           ('{0}','{1}','{2}','{3}','{4}')",
                        textBox2.Text,
                        textBox4.Text,
                        textBox3.Text,
                        comboBox1.SelectedValue.ToString(),
                        textBox6.Text);
            int hs = db.DonIntsdf(sql);
            if (hs == 1)
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
            string sql = string.Format(@"UPDATE [Member].[dbo].[Goods]
                       SET [goodsName] = '{0}',
                        [goodsPoint] = '{1}',
                        [goodsCount] = '{2}',
                        [goodsTypeId] = '{3}',
                        [goodsprice] = '{4}'
                     WHERE goodsId='{5}'",
                        textBox2.Text,
                        textBox4.Text, 
                        textBox3.Text,
                        comboBox1.SelectedValue.ToString(),
                        textBox6.Text, bh);
            int hs = db.DonIntsdf(sql);
            if (hs == 1)
            {
                MessageBox.Show("修改成功！");
                this.Close();
            }
            else
            {
                MessageBox.Show("修改失败！");
            }
        }
        DataSet ds;
        public void Loadnr() {
            string sql = "select * from GoodsType";
            ds = db.QuerByAdapter(sql, "leibie");
            comboBox1.DisplayMember = "GoodsTypeName";
            comboBox1.ValueMember = "GoodsTypeId";
            comboBox1.DataSource = ds.Tables["leibie"];
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (bl == true)
            {
                xg();
            }
            else
            {
                tj();
            }
        }

        private void sptj_Load(object sender, EventArgs e)
        {
            Loadnr();
            if (bl == true)
            {
                string sql = "SELECT * FROM [Member].[dbo].[Goods] where goodsId='" + bh + "'";
                SqlDataReader reader=db.reader(sql);
                textBox1.Text = bh.ToString();
                if (reader.Read()) {
                    textBox2.Text = reader["goodsName"].ToString();
                    textBox4.Text = reader["goodsPoint"].ToString();
                    textBox3.Text = reader["goodsCount"].ToString();
                    textBox6.Text = reader["goodsprice"].ToString();
                    comboBox1.SelectedValue = reader["GoodsTypeId"];
                }
                reader.Close();
            }
            string sql2 = @"SELECT max([goodsId])
                            FROM [Member].[dbo].[Goods]";
            SqlDataReader reader2 = db.reader(sql2);
            if (reader2.Read())
            {
                textBox1.Text = int.Parse(reader2[0].ToString()) + 1 + "";

            }
            reader2.Close();
        }

    }
}
