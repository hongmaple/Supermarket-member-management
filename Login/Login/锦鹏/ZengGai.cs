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
    public partial class ZengGai : Form
    {
        public bool bl = false;
        //获得编号
        public int zbh = 0;
        GM gm = new GM();
        public ZengGai()
        {
            InitializeComponent();
        }
        //获得最后一个id的自动标示量并加1，用来显示自动添加的会员卡号。
        public void bhcx()
        {
            //获得最后一个id的自动标示量
            int bh = gm.Dyh("SELECT IDENT_CURRENT('MemberInfo')");
            label8.Text = (bh+1).ToString();
        }
        //窗体加载事件
        private void ZengGai_Load(object sender, EventArgs e)
        {
            bhcx();
            huiyuan();
            if (bl==true)
            {
                xsnr();
            } 
        }
        //添加
        public void tj()
        {
            string name = textBox2.Text.Trim();
            string hykh = comboBox1.SelectedValue.ToString();
            int point = int.Parse(textBox3.Text.Trim().ToString());
            double ye = double.Parse(textBox4.Text.Trim().ToString());
            if (comboBox2.Text == "储款优惠")
            {
                ye = ye + ye * 0.1;
            }
            string sql = string.Format(@"insert MemberInfo
            values ('{0}','{1}','{2}','{3}')",
            name, hykh, point, ye);
            int hs = gm.Zsg(sql);
            if (hs >= 1)
            {
                MessageBox.Show("添加成功", "提示");
                this.Close();
            }
            else
            {
                MessageBox.Show("添加失败", "提示");
                this.Close();
            }
        }
        //点击修改时显示的内容
        public void xsnr()
        {
            string sql = @"select  m.id,m.name,c.typeName,m.point,m.Balance from MemberInfo M
                            inner join CardType c on m.cardType=c.id
                            where m.id=" + zbh;
            SqlDataReader reader = gm.reader(sql);
            label8.Text = zbh.ToString();
            comboBox2.Enabled = false;
            comboBox2.Text = "不优惠";
            if (reader.Read())
            {
                textBox2.Text = reader["name"].ToString();
                comboBox1.Text = reader["typeName"].ToString();
                textBox3.Text = reader["point"].ToString();
                textBox4.Text = reader["Balance"].ToString();
            }
            reader.Close();
        }
        //修改操作
        public void xg()
        {
            string sql = string.Format(@"update MemberInfo
                           set name='{0}',cardType='{1}',point='{2}',Balance='{3}'
                           where id={4}", textBox2.Text.Trim(), comboBox1.SelectedValue.ToString().Trim(),
                                          textBox3.Text.Trim(), textBox4.Text.Trim(),zbh);
            int hs =gm.Zsg(sql);
            if (hs==1)
            {
                MessageBox.Show("修改成功","提示");
                this.Close();
            }
            else
            {
                MessageBox.Show("修改失败", "提示");
            }
        }
        //会员级别查询
        public void huiyuan()
        {
            DataSet ds = gm.Tycx("select * from CardType", "CardType");
            comboBox1.DisplayMember = "typeName";
            //comboBox1.ValueMember = "sale";
            comboBox1.DataSource = ds.Tables["CardType"];
            //自动选中不优惠
            comboBox2.Text = "储款优惠";
        }
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox2.Text == "储款优惠")
            {
                label7.Text = "交费100元实际存入卡中110元";
                label7.Visible = true;
            }
            else
            {
                label7.Visible = false;
            }
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox1.ValueMember = "sale";
            label9.Text = comboBox1.SelectedValue.ToString() + "折";
            if (comboBox1.SelectedValue.ToString() == "1")
            {
                textBox3.Text = "0";
                return;
            }
            if (comboBox1.SelectedValue.ToString() == "0.9")
            {
                textBox3.Text = "50";
                return;
            }
            if (comboBox1.SelectedValue.ToString() == "0.8")
            {
                textBox3.Text = "100";
                return;
            }
            if (comboBox1.SelectedValue.ToString() == "0.7")
            {
                textBox3.Text = "1000";
                return;
            }
            if (comboBox1.SelectedValue.ToString() == "0.6")
            {
                textBox3.Text = "2000";
                return;
            }
        }
        //保存按钮
        private void button1_Click(object sender, EventArgs e)
        {
            //覆盖前面折扣sale的背后隐藏实际值，用会员卡的id代替实际值。
            comboBox1.ValueMember = "id";
            //判断文本textBox必须填的内容是否为空
            if (textBox2.Text.Trim() == "")
            {
                MessageBox.Show("会员姓名不能为空！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }
            if (textBox3.Text.Trim() == "")
            {
                MessageBox.Show("会员积分不能为空！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }
            if (textBox4.Text.Trim() == "")
            {
                MessageBox.Show("储值金额不能为空！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }
            //用来判断是否为数字
            if (System.Text.RegularExpressions.Regex.IsMatch(textBox3.Text.Trim(), @"^[0-9]*$"))
            {
                //用来判断是否为数字或小数
                if (System.Text.RegularExpressions.Regex.IsMatch(textBox4.Text.Trim(), @"[0-9]\d*.\d*|\d*[0-9]\d*"))
                {
                    //如果bool为true执行修改操作
                    if (bl == true)
                    {
                        xg();
                    }
                    //如果bool不为true执行添加操作
                    else
                    {
                        tj();
                    }
                    return;
                }
                MessageBox.Show("储值金额请输入有效值", "错误");
                return;
            }
            MessageBox.Show("会员积分请输入有效值", "错误");

        }
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        //清空操作
        private void button7_Click(object sender, EventArgs e)
        {
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            comboBox1.Text = "无卡会员";
            comboBox2.Text = "不优惠";
        }
    }
}
