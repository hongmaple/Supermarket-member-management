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
    public partial class Transfer : Form
    {
        public Transfer()
        {
            InitializeComponent();
        }
        public int zbh;
        public double jlxf;
        GM gm = new GM();
        double Balance;
        //窗体加载时显示的内容
        private void Transfer_Load(object sender, EventArgs e)
        {
            string sql = @"select * from MemberInfo
                           where id=" + zbh + "";
            SqlDataReader reader = gm.reader(sql);
            if (reader.Read())
            {
                label9.Text = reader["id"].ToString();
                label10.Text = reader["name"].ToString();
                Balance = double.Parse(reader["Balance"].ToString());
            }
            label11.Text = Balance.ToString() + "元";
            label12.Text = jlxf.ToString();
            reader.Close();
        }
        //查询按钮
        private void button3_Click(object sender, EventArgs e)
        {
            panel1.Visible = true;
            numericUpDown1.Visible = false;
            string sql = @"select id,name from MemberInfo where id<>"+zbh;
            DataSet ds = gm.Tycx(sql, "MemberInfo");
            DataView dv = new DataView(ds.Tables["MemberInfo"]);
            dataGridView1.DataSource = dv;
            if (System.Text.RegularExpressions.Regex.IsMatch(textBox1.Text, @"^[0-9]*$") && textBox1.Text.Trim() != "")
            {
                dv.RowFilter = "id='" + textBox1.Text + "'";
            }
            else
            {
                dv.RowFilter = "name like'%" + textBox1.Text + "%'";
            }
        }
        int kh;
        public void Xzh()
        {
            //判断dataGridView1列表中是否为空
            if (dataGridView1.CurrentRow == null)
            {
                MessageBox.Show("请选择转入的会员卡", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }
            panel1.Visible = false;
            numericUpDown1.Visible = true;
            //获取网格控件dataGridView1选中的会员卡号
            kh = int.Parse(dataGridView1.SelectedRows[0].Cells["Column1"].Value.ToString());
            string sql = @"select  m.id,m.name,c.typeName,m.point,m.Balance from MemberInfo M
                        inner join CardType c on m.cardType=c.id
                            where m.id='" + kh + "'";
            SqlDataReader reader = gm.reader(sql);
            if (reader.Read())
            {
                label15.Text = reader["id"].ToString();
                label16.Text = reader["name"].ToString();
                label17.Text = reader["Balance"].ToString() + "元";
            }
            label18.Text ="0元";
            sql = @"select memberId,sum([sum]) 积累消费 from OrderInfo
                    group by memberId";
            reader = gm.reader(sql);
            while(reader.Read())
            {
                int memberId=int.Parse(reader["memberId"].ToString());
                if (memberId==kh)
                {
                    label18.Text=reader["积累消费"].ToString()+"元";
                }
            }
            reader.Close();
            panel1.Visible = false;
        }
        private void button4_Click(object sender, EventArgs e)
        {
            Xzh();
        }
        private void button5_Click(object sender, EventArgs e)
        {
            panel1.Visible = false;
            numericUpDown1.Visible = true;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            numericUpDown1.Enabled = false;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            numericUpDown1.Enabled = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (label15.Text.Trim() != "")
            {
                double szye = double.Parse(numericUpDown1.Value.ToString());
                //转账全部余额
                if (radioButton1.Checked == true)
                {
                    string sql = string.Format(@"update MemberInfo
                                        set Balance=Balance+{0}
                                        where id={1}
                                        update MemberInfo
                                        set Balance=0
                                        where id={2}", Balance, kh, zbh);
                    int hs = gm.Zsg(sql);
                    if (hs >= 1)
                    {
                        MessageBox.Show("转账成功！", "系统提示");
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("转账失败！", "系统提示");
                    }
                    return;
                }
                //转账指定余额
                if (Balance >= szye && numericUpDown1.Value>0)
                {
                        string sql = string.Format(@"update MemberInfo
                                            set Balance=Balance+{0}
                                            where id={1}
                                            update MemberInfo
                                            set Balance=Balance-{2}
                                            where id={3}", szye, kh, szye, zbh);
                        int hs = gm.Zsg(sql);
                        if (hs >= 1)
                        {
                            MessageBox.Show("转账成功！", "系统提示");
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("转账失败！", "系统提示");
                        }
                }
                else if (numericUpDown1.Value<=0)
                {
                    MessageBox.Show("请输入大于0的转账金额", "系统提示");
                }
                else
                {
                    MessageBox.Show("指定金额不能大于当前卡内余额", "系统提示");
                }
            }
            else
            {
                MessageBox.Show("请选择转入的会员卡", "系统提示",MessageBoxButtons.OK,MessageBoxIcon.Asterisk);
            }
        }
        private void button2_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }
        //双击网格控件dataGridView1的事件
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            Xzh();
        }
    }
}
