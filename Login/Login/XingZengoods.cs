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

namespace Login
{
    public partial class XingZengoods : Form
    {
        public XingZengoods()
        {
            InitializeComponent();
        }
        DBTools dbtools = new DBTools();
        DataSet linshi = new DataSet();
        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void XingZengoods_Load(object sender, EventArgs e)//窗体加载
        {
            string sql = @"SELECT [GoodsTypeId]
                          ,[GoodsTypeName]
                      FROM [Member].[dbo].[GoodsType]";
            linshi = dbtools.QuerByAdapter(sql, "abc");
            comboBox1.DisplayMember = "GoodsTypeName";
            comboBox1.ValueMember = "GoodsTypeId";
            comboBox1.DataSource = linshi.Tables["abc"];
            string sql2 = @"SELECT max([goodsId])
                            FROM [Member].[dbo].[Goods]";
            SqlDataReader reader = dbtools.reader(sql2);
            if (reader.Read())
            {
                textBox1.Text=int.Parse(reader[0].ToString())+1+"";
            }
        }
        public bool IsNumber(string str_number)//判断价格框是否为数字
        {
            //return System.Text.RegularExpressions.Regex.IsMatch(str_number, @"^[0-9]*$");
            return System.Text.RegularExpressions.Regex.IsMatch(str_number, @"[1-9]\d*.\d*|0.\d*[1-9]\d*");//只能允许价格框输入小数和整数
        }
        private void button2_Click(object sender, EventArgs e)
        {
            string goodsName = textBox2.Text;
            string id = comboBox1.SelectedValue.ToString();
            string jifen = numericUpDown1.Value.ToString();
            string count = numericUpDown2.Value.ToString();
            float price = float.Parse(textBox3.Text.Trim());
            string sql = string.Format(@"INSERT INTO [Member].[dbo].[Goods]
                                       ([goodsName]
                                       ,[goodsPoint]
                                       ,[goodsCount]
                                       ,[goodsTypeId]
                                       ,[goodsprice])
                                 VALUES('{0}','{1}','{2}','{3}','{4}')", goodsName, jifen, count,id,price);
            int jieguo = dbtools.DonIntsdf(sql);
            if (jieguo>0)
            {
                MessageBox.Show("增加成功");
            }
            else
            {
                MessageBox.Show("增加失败");
            }
        }

        private void textBox3_Click(object sender, EventArgs e)
        {
            if (IsNumber(textBox4.Text))
            {

            }
            else
            {
                //string.Format("{0,8:C2}",1);
                textBox4.Text = "1.00";
            }
        }

        private void textBox4_Click(object sender, EventArgs e)
        {
            if (IsNumber(textBox3.Text))
            {

            }
            else
            {
                textBox3.Text = "1.00";
            }
        }
        Point mouseOff;//鼠标移动的坐标
        bool leftFalg;//标记为是否为左键选中
        private void 神一样的登录界面_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                mouseOff = new Point(-e.X, -e.Y);//得到变量的值
                leftFalg = true;//点击左键，按下鼠标时标记为true
            }
        }

        private void 神一样的登录界面_MouseMove(object sender, MouseEventArgs e)
        {
            if (leftFalg)
            {
                Point mouseSet = Control.MousePosition;
                mouseSet.Offset(mouseOff.X, mouseOff.Y);//设置移动后的坐标
                Location = mouseSet;
            }
        }

        private void 神一样的登录界面_MouseUp(object sender, MouseEventArgs e)
        {
            if (leftFalg)
            {

                leftFalg = false;//释放鼠标后标记为false

            }
        }
    }
}
