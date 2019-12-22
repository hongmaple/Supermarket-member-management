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
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }
        DataSet HuiYuan = new DataSet();
        DBTools dbtools = new DBTools();

        public bool IsNumber(string str_number)//判断价格框是否为数字
        {
            return System.Text.RegularExpressions.Regex.IsMatch(str_number, @"^[0-9]*$");
            //return System.Text.RegularExpressions.Regex.IsMatch(str_number, @"[1-9]\d*.\d*|0.\d*[1-9]\d*");//只能允许价格框输入小数和整数
        }
        private void button9_Click(object sender, EventArgs e)//查询会员信息
        {
            try
            {
                string sql = @"SELECT [MemberInfo].[id]
                          ,[MemberInfo].[name]
                          ,[CardType].typeName
                          ,[MemberInfo].[Balance]
                      FROM [Member].[dbo].[MemberInfo]
                      inner join [Member].[dbo].[CardType]
                      on [MemberInfo].cardType=[CardType].id";
                if (textBox2.Text == "")
                {
                    HuiYuan = dbtools.QuerByAdapter(sql, "huiyuan3");
                    dataGridView1.DataSource = HuiYuan.Tables["huiyuan3"];
                    panel1.Visible = true;
                }
                else
                {
                    if (this.IsNumber(textBox2.Text))
                    {
                        String sql2 = sql + "  where [MemberInfo].[id]=" + int.Parse(textBox2.Text);
                        HuiYuan = dbtools.QuerByAdapter(sql2, "huiyuan1");
                        dataGridView1.DataSource = HuiYuan.Tables["huiyuan1"];
                        panel1.Visible = true;
                    }
                    else if (this.IsNumber(textBox2.Text) == false)
                    {
                        string sql3 = sql + "  where [MemberInfo].[name] like '%" + textBox2.Text + "%'";
                        HuiYuan = dbtools.QuerByAdapter(sql3, "huiyuan2");
                        dataGridView1.DataSource = HuiYuan.Tables["huiyuan2"];
                        panel1.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
         
        }
        public void chaxunxiaofeijilu() //查询消费记录
        {
            //先清空
            if (listView2.Items!=null)
            {
                listView2.Items.Clear();
            }
            int id = int.Parse(dataGridView1.SelectedRows[0].Cells[0].Value.ToString());//获取会员卡号
            string sql3 = string.Format(@"SELECT [buyDate]
                                              ,[sum]
                                              ,LoginInfo.login
                                              ,OrderInfo.oid
                                          FROM [Member].[dbo].[OrderInfo]
                                          inner join dbo.MemberInfo
                                          on MemberInfo.id=[OrderInfo].memberId
                                          inner join dbo.LoginInfo 
                                          on LoginInfo.id=[OrderInfo].LoginId
                                          where MemberInfo.id={0}  order by [oid] desc", id);
            SqlDataReader reader = dbtools.reader(sql3);
            int jifen = 0;
            float zonjine = 0;
            int zonjifen = 0;
            while (reader.Read())
            {
                ListViewItem gfggh = new ListViewItem();
                gfggh.Text = Convert.ToString(reader["buyDate"]);
                gfggh.Tag = Convert.ToInt32(reader["oid"]);
                gfggh.SubItems.Add(Convert.ToString(reader["sum"]));
                zonjine = float.Parse(reader["sum"].ToString()) + zonjine;//历史总金额
                string sql4 = string.Format(@"SELECT 
                                          [goodsPoint]
                                          ,orderItem.count
                                      FROM [Member].[dbo].[Goods]
                                       inner join dbo.orderItem
                                       on Goods.goodsId=orderItem.goodsId
                                       inner join dbo.OrderInfo
                                       on orderItem.orderId=[OrderInfo].oid
                                       where  memberId={0} and oid={1}", id, Convert.ToInt32(reader["oid"]));
                SqlDataReader  duqu = dbtools.reader(sql4);
                while (duqu.Read())
                {
                    jifen = Convert.ToInt32(float.Parse(reader["sum"].ToString())*0.1);//历史总积分
                }
                zonjifen = zonjifen + jifen;//统计总积分
                gfggh.SubItems.Add(jifen.ToString());
                jifen = 0;//清空积分
                gfggh.SubItems.Add("");
                gfggh.SubItems.Add(Convert.ToString(reader["login"]));
                listView2.Items.Add(gfggh);
            }
            int jilushu = listView2.Items.Count;
            textBox3.Text = jilushu.ToString();//记录数
            textBox4.Text = zonjine.ToString();//历史总金额
            textBox5.Text = zonjifen.ToString();
            zonjifen = 0;
        }
        private void button1_Click(object sender, EventArgs e)//panel1的
        {
            try
            {
                this.button8_Click(sender, e);
                float sum = 0;//总消费金额
                int a = 0;//次数
                if (listView2.Items != null)
                {

                    listView2.Items.Clear();
                }
                if (panel6.Visible == true)
                {
                    panel6.Visible = false;
                }
                int id = int.Parse(dataGridView1.SelectedRows[0].Cells[0].Value.ToString());//获取会员卡号
                //查询卡号，姓名，级别等放在控件中
                string sql = string.Format(@"SELECT [MemberInfo].[id]
                                      ,[name]
                                      ,CardType.typeName
                                      ,[point]
                                  FROM [Member].[dbo].[MemberInfo]
                                  inner join dbo.CardType
                                  on [MemberInfo].cardType=CardType.id
                                  where [MemberInfo].id={0}", id);
                SqlDataReader reader = dbtools.reader(sql);
                if (reader.Read())
                {
                    label40.Text = "查看图片";
                    label22.Text = reader["id"].ToString();//卡号
                    label23.Text = reader["name"].ToString();//姓名
                    label25.Text = reader["typeName"].ToString();//级别
                    label26.Text = reader["point"].ToString();//积分
                }
                reader.Close();
                string sql2 = "select sum from OrderInfo where memberId=" + id;
                SqlDataReader duqu = dbtools.reader(sql2);
                while (duqu.Read())
                {
                    sum = sum + float.Parse(duqu["sum"].ToString());
                    a++;
                }
                label27.Text = sum.ToString();//累计金额
                label28.Text = a.ToString();//总共消费次数
                duqu.Close();

                //label29.Text = reader[""];//状态
                //2.同时查询出会员消费的记录
                chaxunxiaofeijilu();
                listView2.Focus();
                int count = listView2.Items.Count;
                if (count > 0)
                {
                    listView2.Items[0].Selected = true;
                }
                listView2.HideSelection = true; // 失去焦点后仍保持选中状态
                panel1.Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
           

        }

        private void button2_Click(object sender, EventArgs e)//panel1的关闭
        {
            panel1.Visible = false;
        }

        private void button10_Click(object sender, EventArgs e)//浏览
        {
            this.button9_Click(sender,e);
        }

        private void button8_Click(object sender, EventArgs e)//清除
        {
            label40.Text = "";
            pictureBox1.Image = null;
            label22.Text = "";
            label23.Text = "";
            label25.Text = "";
            label26.Text = "";
            label27.Text = "";
            label28.Text = "";
            if (listView1.Items != null)
            {
                listView1.Items.Clear();
            }
            if (listView2.Items!=null)
            {
                listView2.Items.Clear();
            }

            //if (HuiYuan.Tables["xiaofeijilu"]!=null)
            //    {
            //        HuiYuan.Tables["xiaofeijilu"].Clear();
            //    }
        }
        private void button5_Click(object sender, EventArgs e)////查看备注
        {
            if (button5.Text=="查看备注")
            {
                button5.Text = "查看消费";
                panel6.Visible = true;
                label19.Text = "备注信息";
            }
            else
            {
                button5.Text = "查看备注";
                label19.Text = "会员消费详细列表";
                panel6.Visible = false;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            button5_Click(sender,e);
            label19.Text = "会员消费详细列表";
            panel6.Visible = false;
        }
        public void chaxunxianxixiaofei(int oid) 
        {
            try
            {
                if (listView1.Items != null)
                {
                    listView1.Items.Clear();
                }
                string sql = string.Format(@"SELECT 
                                           Goods.goodsName
                                          ,[price]
                                          ,[count]
                                      FROM [Member].[dbo].[orderItem]
                                      inner join dbo.OrderInfo
                                      on [orderItem].orderId=OrderInfo.oid
                                      inner join dbo.Goods 
                                      on Goods.goodsId=[orderItem].goodsId
                                      where OrderInfo.oid={0}", oid);
                //HuiYuan = dbtools.QuerByAdapter(sql,"shangpxiangqin");
                //DataRow row = HuiYuan.Tables["shangpxiangqin"].NewRow();
                //            row[0] = bookleiId;//这里只能用变量来赋值
                //            //row[1] = bookleiName;//
                //           bookxianqin.Tables["bookleibie"].Rows.InsertAt(row, 0);//添加到表的第一行位置
                ////dataGridView3.DataSource = HuiYuan.Tables["shangpxiangqin"];
                SqlDataReader reader = dbtools.reader(sql);
                float heji = 0;//总金额
                while (reader.Read())
                {
                    ListViewItem gfggh = new ListViewItem();
                    gfggh.Text = Convert.ToString(reader["goodsName"]);
                    gfggh.SubItems.Add(Convert.ToString(reader["price"]));
                    gfggh.SubItems.Add(Convert.ToString(reader["count"]));
                    float sum = float.Parse(reader["price"].ToString()) * float.Parse(reader["count"].ToString());
                    gfggh.SubItems.Add(sum.ToString());
                    listView1.Items.Add(gfggh);
                    heji = heji + sum;
                }
                reader.Close();
                textBox6.Text = heji.ToString();
            }
            catch (Exception)
            {

            }
        }
        private void listView2_Click(object sender, EventArgs e)//查询详细消费商品方法
        {
            try
            {
                int oid = int.Parse(listView2.SelectedItems[0].Tag.ToString());//获取会员卡号
                chaxunxianxixiaofei(oid);
            }
            catch (Exception)
            {
            }
           
        }

        private void button11_Click(object sender, EventArgs e)//添加会员
        {
            添加会员 kk = new 添加会员();
            kk.ShowDialog();
        }
        会员查询 jlk = null;
        private void button13_Click(object sender, EventArgs e)//会员查询
        {
            if (jlk == null || jlk.IsDisposed == true)
            {
                jlk = new 会员查询();
                jlk.ShowDialog();
            }
        }
        
        private void button12_Click(object sender, EventArgs e)
        {
           jlk = new 会员查询();
           if (jlk!=null || jlk.IsDisposed==false)
            {
                 jlk.Close();
            }
           
        }

        private void button6_Click(object sender, EventArgs e)//添加消费
        {
            if (label22.Text!="")
            {
                 AddTheConsumption xiaofei = new AddTheConsumption();
                 xiaofei.huiyanbh = int.Parse(label22.Text);
                 xiaofei.jine = float.Parse(label27.Text.ToString());
                 xiaofei.Balance = float.Parse(dataGridView1.SelectedRows[0].Cells[3].Value.ToString());
                 xiaofei.ShowDialog(this);
                 this.button1_Click(sender,e);
            }
            else
            {
                MessageBox.Show("请先选择会员");
            }
           
        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            this.button1_Click(sender,e);
        }

        private void listView2_DoubleClick(object sender, EventArgs e)//组件双击事件
        {
            AddTheConsumption cc = new AddTheConsumption();
            cc.ShowDialog();
        }

        private void 增加消费ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.button6_Click(sender,e);
        }

        private void listView2_MouseUp(object sender, MouseEventArgs e)
            //鼠标指针在组件上分时并释放鼠标按钮时发生c# listview 点击空白，也不改变焦点
            {
                int count = listView2.Items.Count;
                if (e.Button == MouseButtons.Left)
                {
                    if (listView2.SelectedItems.Count > 0)
                    {
                        //listView1.Items[listView1.SelectedIndices[0]].Index

                    }
                    else if (listView1.SelectedItems.Count <= 0)//点击空白区  
                    {
                        if (this.listView2.FocusedItem != null)
                        {
                            ListViewItem item = this.listView1.GetItemAt(e.X, e.Y);
                            if (item == null)
                            {
                                this.listView2.FocusedItem.Selected = true;
                                //listView1.Items[count - 1].BackColor = Color.Blue;
                                //listView1.FocusedItem.BackColor = SystemColors.Highlight;
                                //listView1.FocusedItem.ForeColor = Color.White;
                            }
                        }
                  }
            }
        }

        private void 删除当前消费UToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult rs=MessageBox.Show("确定要删除当前消费记录吗？","警告！",MessageBoxButtons.YesNo,MessageBoxIcon.Asterisk);
            if (rs==DialogResult.Yes)
            {
                if (listView2.Items.Count != 0)
                {
                    int orderId = int.Parse(listView2.SelectedItems[0].Tag.ToString());
                    string sql1 = string.Format(@"DELETE FROM [Member].[dbo].[orderItem]
                                            WHERE [orderItem].orderId={0}", orderId);
                    int jieguo = dbtools.DonIntsdf(sql1);
                    if (jieguo > 0)
                    {
                        string sql2 = string.Format(@"DELETE FROM [Member].[dbo].[OrderInfo]
                                                WHERE [OrderInfo].oid={0}", orderId);
                        int jieguo2 = dbtools.DonIntsdf(sql2);
                        if (jieguo2 > 0)
                        {
                            this.chaxunxiaofeijilu();
                            MessageBox.Show("删除成功");
                            chaxunxianxixiaofei(orderId);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("请选择一行");
                }
            }
        }
        private int iOld = -1;
        private void listView2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView2.SelectedIndices.Count > 0) //若有选中项 
            {
                if (iOld == -1)
                {
                    listView2.Items[listView2.SelectedIndices[0]].BackColor = SystemColors.Highlight; //设置选中项的背景颜色 
                    iOld = listView2.SelectedIndices[0]; //设置当前选中项索引 
                }
                else
                {
                    if (listView2.SelectedIndices[0] != iOld)
                    {
                        listView2.Items[listView2.SelectedIndices[0]].BackColor = SystemColors.Highlight; //设置选中项的背景颜色 
                        listView2.Items[iOld].BackColor = SystemColors.Window;//恢复默认背景色 
                        iOld = listView2.SelectedIndices[0]; //设置当前选中项索引 
                    }
                }
            }
            else //若无选中项 
            {
                listView2.Items[iOld].BackColor = SystemColors.Window;//恢复默认背景色 
                iOld = -1; //设置当前处于无选中项状态 
            }
        }

        private void label40_Click(object sender, EventArgs e)//会员图片查询
        {
            try
            {
                if (label40.Text == "查看图片")
                {
                    pictureBox1.Visible = true;
                    int id = int.Parse(label22.Text);
                    string sql = string.Format(@"SELECT [LuJin]
                                          FROM [Member].[dbo].[MemImg]
                                          where [Memid]={0}", id);
                    SqlDataReader reader = dbtools.reader(sql);
                    if (reader.Read())
                    {
                        string skfg = reader["LuJin"].ToString();
                        if (skfg == "")
                        {
                            MessageBox.Show("没有记录此会员的照片");
                        }
                        else
                        {
                            this.pictureBox1.Image = Image.FromFile(skfg);//pictureBox从文件中加载图片的方式
                            label40.Text = "隐藏图片";
                        }
                       
                    }
                }
                else
                {
                    label40.Text = "查看图片";
                    pictureBox1.Visible = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("出现异常："+ex.Message);
            }

        }
        public void chahuiyuanjifen() 
        {
            string sql = @"SELECT[point]
                          FROM [Member].[dbo].[MemberInfo]
                          where id="+int.Parse(label22.Text);
            SqlDataReader reader = dbtools.reader(sql);
            if (reader.Read())
            {
                label26.Text = reader["point"].ToString();
            }
        }
        private void button7_Click(object sender, EventArgs e)
        {
            if (label22.Text=="")
            {
                MessageBox.Show("请先选择一个会员");
            }
            else
            {
            SpecialOffer hh = new SpecialOffer();
            hh.huiyanbh = int.Parse(label22.Text);
            hh.jine = float.Parse(label27.Text.ToString());
            hh.Balance = float.Parse(dataGridView1.SelectedRows[0].Cells[3].Value.ToString());
            hh.ShowDialog();
            chahuiyuanjifen();
            }
           
        }

        private void 兑换商品HToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            SpecialOffer kk = new SpecialOffer();
            kk.ShowDialog();
        }
    }
}
