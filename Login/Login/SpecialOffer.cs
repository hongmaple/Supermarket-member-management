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
    public partial class SpecialOffer : Form
    {
        public SpecialOffer()
        {
            InitializeComponent();
        }
        public float Balance;//会员余额
        public int goodsId;
        public int huiyanbh;//卡号
        public float jine;//累计消费金额
        float danjia = 0;//折扣
        DBTools cha = new DBTools();
        public void chxun()
        {
        
            //1.查询会员信息，放入相应位置
            string sql = string.Format(@"SELECT 
                                       [name]
                                      ,CardType.typeName
                                      ,[point]
                                      ,CardType.sale
                                  FROM [Member].[dbo].[MemberInfo]
                                  inner join dbo.CardType
                                  on [MemberInfo].cardType=CardType.id
                                  where [MemberInfo].id={0}", huiyanbh);
            SqlDataReader reder = cha.reader(sql);
            label7.Text = huiyanbh.ToString();
            if (reder.Read())
            {
                label8.Text = reder["name"].ToString();
                label9.Text = reder["typeName"].ToString();
                label10.Text = reder["point"].ToString();
                label11.Text = reder["sale"].ToString() + "折";
                danjia = float.Parse(reder["sale"].ToString());//折扣
                label12.Text = jine.ToString();
            }
            reder.Close();
        }
        private void SpecialOffer_Load(object sender, EventArgs e)
        {
            chxun();
        }
       
        private void button3_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 0;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 1;
        }

        private void label29_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label29_MouseEnter(object sender, EventArgs e)
        {
            label15.Visible = true;
            label15.Text = "关闭";
            label29.ForeColor = Color.Blue;
        }

        private void label29_MouseLeave(object sender, EventArgs e)
        {
            label15.Visible = false;
            label29.ForeColor = Color.White;
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

        private void timer1_Tick(object sender, EventArgs e)//定时任务
        {
            coujian();
        }
        int count = 0;
        int jieguo = 0;
        public void coujian() 
        {
            Random random = new Random();
            if (int.Parse(label10.Text) >= 10)
            {
                string sql = string.Format(@"UPDATE [Member].[dbo].[MemberInfo]
                                       SET [point] = {0}
                                     WHERE id={1}", int.Parse(label10.Text) - 10, huiyanbh);
                int sf = cha.DonIntsdf(sql);
                if (sf > 0)
                {
                    jieguo = random.Next(100);
                    button1.Text = "";
                    button1.Text = jieguo.ToString();
                    count++;
                }
                if (sf > 0 && count==99)
                {
                    timer1.Enabled = false;
                    count = 0;
                    int num1 = random.Next(100);
                    int num2 = random.Next(100);
                    int num3 = random.Next(100);
                 
                    if (jieguo == num1)
                    {
                        textBox1.Text = "恭喜会员卡号为" + huiyanbh + "的用户\n\n获得一等奖，奖品为价值198元的真维斯牛仔裤一条";
                        chxun();
                    }
                    else if (jieguo > num2)
                    {
                        textBox1.Text = "恭喜会员卡号为" + huiyanbh + "的用户获得二等奖\n\n奖品为价值10元的QQ点卡一个";
                        chxun();
                    }
                    else if (jieguo < num3)
                    {
                        textBox1.Text = "恭喜会员卡号为" + huiyanbh + "的用户获得三等奖\n\n奖品为价值1元的棒棒糖一个";
                    }
                    else
                    {
                        textBox1.Text = "谢谢参与";
                        chxun();
                    }
                }

            }
            else
            {
                MessageBox.Show("积分不够了，下次再来吧！");
            }

        }
        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            timer1.Enabled = true;
        }

    }
}
