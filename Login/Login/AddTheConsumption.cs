using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;
namespace Login
{
    public partial class AddTheConsumption : Form
    {
        public float Balance;//会员余额
        public int goodsId;
        public int huiyanbh;//卡号
        public float jine;//累计消费金额
        public AddTheConsumption()
        {
            InitializeComponent();
        }
        DBTools cha = new DBTools();
        DataSet goodslist = new DataSet();
        float danjia = 0;//折扣
        private void AddTheConsumption_Load(object sender, EventArgs e)
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
                label11.Text = reder["sale"].ToString()+"折";
                danjia = float.Parse(reder["sale"].ToString());//折扣
                label12.Text = jine.ToString();
            }
            reder.Close();
        }
        public bool IsNumber(string str_number)//判断价格框是否为数字
        {
            return System.Text.RegularExpressions.Regex.IsMatch(str_number, @"^[0-9]*$");
            //return System.Text.RegularExpressions.Regex.IsMatch(str_number, @"[1-9]\d*.\d*|0.\d*[1-9]\d*");//只能允许价格框输入小数和整数
        }
        private void button1_Click(object sender, EventArgs e)//查找
        {  
            string sql = "SELECT [goodsId] ,[goodsName] ,[goodsCount] ,[goodsprice] FROM [Member].[dbo].[Goods]";
            panel1.Visible = true;
            goodslist = cha.QuerByAdapter(sql,"list");
            DataView sjfgh = new DataView( goodslist.Tables["list"]);
            dataGridView1.DataSource = sjfgh;
            string tj = "";
            if (textBox1.Text != "")
            {
                if (IsNumber(textBox1.Text))
                {
                    tj = " goodsId=" + int.Parse(textBox1.Text);
                }
                else
                {
                    tj = " goodsName  like '%"+textBox1.Text+"%'";
                }
                
            }
            else
            {
                tj = "";
            }
            sjfgh.RowFilter = tj;
        }

        private void button6_Click(object sender, EventArgs e)//退出
        {
            panel1.Visible = false;
        }
        float zonjiage = 0;//总价
        float yuanjia = 0;//原价
        public string godsId;
        public string goodsName;
        public string GoodsTypeName;
        public string goodsprice;
        public string jiag;//折后单价
        public string shulian;//数量
        public float zonpice;//单件商品合计总价
        public void jishuan()//进行统计的方法
        {
            for (int i = 0; i < listView1.Items.Count; i++)
            {
                zonjiage = zonjiage + float.Parse(listView1.Items[i].SubItems[5].Text) * float.Parse(listView1.Items[i].SubItems[4].Text);
                yuanjia = yuanjia + float.Parse(listView1.Items[i].SubItems[3].Text) * float.Parse(listView1.Items[i].SubItems[5].Text);
            }
            label18.Text = zonjiage.ToString();
            textBox2.Text = label18.Text;
            textBox3.Text = yuanjia.ToString();
            label21.Text = yuanjia - zonjiage + "";
            yuanjia = 0;
            label24.Text = Convert.ToString(Convert.ToInt32(zonjiage * 0.1));
            zonjiage = 0;
        }
        public void chaxun(int goodsId, string shulian)  //1.用商品编号关于商品查出所有的信息.添加到listView中
        {
            string sql = @"SELECT [goodsId]
                          ,[goodsName]
                          ,GoodsType.GoodsTypeName
                          ,[goodsprice]
                      FROM [Member].[dbo].[Goods]
                      inner join dbo.GoodsType
                      on [Goods].goodsTypeId=GoodsType.GoodsTypeId
                      where Goods.goodsId=" + goodsId;
            SqlDataReader reader = cha.reader(sql);
            if (reader.Read())
            {
                godsId = Convert.ToString(reader["goodsId"]);
                goodsName = Convert.ToString(reader["goodsName"]);
                GoodsTypeName = Convert.ToString(reader["GoodsTypeName"]);
                goodsprice = Convert.ToString(reader["goodsprice"]);
                float price = float.Parse(reader["goodsprice"].ToString()) * danjia;//danjia此变量表示折扣
                zonpice = price * float.Parse(shulian);
                jiag = Convert.ToString(price);
            }

            ListViewItem row = new ListViewItem();
            row.Text = godsId;
            //row.Tag = Convert.ToInt32(reader["oid"]);
            row.SubItems.Add(goodsName);
            row.SubItems.Add(GoodsTypeName);
            row.SubItems.Add(goodsprice);
            row.SubItems.Add(Convert.ToString(jiag));
            row.SubItems.Add(shulian);
            row.SubItems.Add(Convert.ToString(zonpice));
            listView1.Items.Add(row);
            //this.listView1.EndUpdate();  //结束数据处理，UI界面一次性绘制。
            //this.listView1.BeginUpdate();   //数据更新，UI暂时挂起，直到EndUpdate绘制控件，可以有效避免闪烁并大大提高加载速度  
            listView1.Focus();
            int count = listView1.Items.Count;
            listView1.Items[count-1].Selected = true;
            jishuan();
        }
        public void button5_Click(object sender, EventArgs e)//确定
        {
            goodsId = int.Parse(dataGridView1.SelectedRows[0].Cells[0].Value.ToString());
            int count = int.Parse(dataGridView1.SelectedRows[0].Cells[2].Value.ToString());
            shulian = Convert.ToString(numericUpDown1.Value);
            if (int.Parse(shulian)<=count&&count>0)
            {
                chaxun(goodsId, shulian);
                listView1.HideSelection = true;
            }
            else
            {
                MessageBox.Show("库存不足");
            }
        }
        private void button7_Click(object sender, EventArgs e)//修改数量
        {
            float count = float.Parse(numericUpDown2.Value.ToString());
            float pirce = float.Parse(listView1.SelectedItems[0].SubItems[4].Text);
            float hejijine = count * pirce;
            listView1.SelectedItems[0].SubItems[5].Text = count.ToString();
            listView1.SelectedItems[0].SubItems[6].Text = hejijine.ToString();
            jishuan();
        }
        string[] shanpinshulian = null;//商品数量
        int[] shangpinid = null;
        int tiaoshu = 0;
        public void textChange(string [] msg,int [] number,int count)
        {
            shanpinshulian = new string[count];
            shangpinid = new int[count];
            tiaoshu = count;
            for (int i = 0; i < count; i++)
            {
                shanpinshulian[i] = msg[i];
                shangpinid[i] = number[i];
            }
           
        }
        
        private void button2_Click(object sender, EventArgs e)
        {
            liulan open = new liulan();
            open.danjia = danjia;
            open.Slave2MainDele += textChange;  //总之就是先把form2里的这个事件注册为form1里的内容
            open.ShowDialog(this);
            for (int i = 0; i < tiaoshu; i++)
            {
                this.chaxun(shangpinid[i], shanpinshulian[i]);
            }
        }
        private void button9_Click(object sender, EventArgs e)//确认消费
        {
            try
            {
                DateTime dt = DateTime.Now;//获取当前日期的方法
                string sdfg = dt.ToLongTimeString();
                string sdfk = dt.ToLongDateString();
                int huiyid = int.Parse(label7.Text);//会员卡号
                float molye = float.Parse(textBox2.Text);//消费金额
                //string dataTime = dateTimePicker1.Value.ToString("yyyy-MM-dd")+sdfg;//日期
                string dataTime = sdfg + " " + sdfk;
                //从类的静态属性拿到操作员的用户名
                string loginName = StaticPropertiesOfClasses.Id;
                string loginPwd = StaticPropertiesOfClasses.pwd;
                string login =string.Format(@"SELECT [id]
                          FROM [Member].[dbo].[LoginInfo]
                          where login='{0}' and pass='{1}'", loginName, loginPwd);
                SqlDataReader reader = cha.reader(login);
                int Loginid = -1;//操作员的编号
                if (reader.Read())
                {
                    Loginid = int.Parse(reader["id"].ToString());
                }
               
                if (molye <= Balance)
                {
                    string sql = string.Format(@"INSERT INTO [Member].[dbo].[OrderInfo]
                                           ([memberId]
                                           ,[sum]
                                           ,[buyDate]
                                           ,[LoginId])
                          VALUES({0},{1},'{2}',{3} )", huiyid, molye, dataTime, Loginid);
                    int jieguo1 = cha.DonIntsdf(sql);
                    if (jieguo1==1)
                    {
                        int count = listView1.Items.Count;
                        sql = "SELECT max(oid) FROM [Member].[dbo].[OrderInfo]";
                       reader = cha.reader(sql);
                        if (reader.Read())
                        {
                            int orderId = int.Parse(reader[0].ToString());
                            for (int i = 0; i < count; i++)
                            {
                                int goodsid = int.Parse(listView1.Items[i].SubItems[0].Text);
                                int shulian = int.Parse(listView1.Items[i].SubItems[5].Text);
                                float pice = float.Parse(listView1.Items[i].SubItems[4].Text);
                                //decimals一种数据类型支持小数和整数
                                sql = string.Format(@"INSERT INTO [Member].[dbo].[orderItem]
                                           ([goodsId]
                                           ,[orderId]
                                           ,[count]
                                           ,[price])
                                     VALUES({0},{1},{2},{3})", goodsid, orderId, shulian, pice);
                                cha.DonIntsdf(sql);

                                sql = string.Format(@"UPDATE [Member].[dbo].[Goods]
                                                   SET[goodsCount] =[goodsCount]-{0}
                                          WHERE [Goods].goodsId={1}", shulian, int.Parse(listView1.Items[i].SubItems[0].Text));
                                cha.DonIntsdf(sql);
                            }

                            int jifen = int.Parse(label24.Text);//所得积分
                            sql = string.Format(@"UPDATE [Member].[dbo].[MemberInfo]
                                               SET [point] =[point]+{0} 
                                                  ,[Balance] =[Balance]-{1}
                                             WHERE id={2}", jifen, molye, huiyid);
                            cha.DonIntsdf(sql);
                            MessageBox.Show("收费成功");
                            this.Close();
                        }
                    }
                    else
                    {
                        MessageBox.Show("收费失败");
                    }
                 }
                else
                {
                    MessageBox.Show("当前余额为" + Balance + "元，余额不足，请充值");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void button10_Click(object sender, EventArgs e)//取消返回
        {
            this.Close();
        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)//双击组件添加商品
        {
            this.button5_Click(sender, e);
        }

        private void button3_Click(object sender, EventArgs e)//删除
        {
            if (listView1.Items.Count != 0)
            {
                listView1.Items.Remove(listView1.SelectedItems[0]);//删除listView中的一行
                listView1.Focus();
                listView1.Items[listView1.Items.Count - 1].Selected = true;
                jishuan();
            }
            else
            {
                MessageBox.Show("请选择一行");
            }
        }

        private void button4_Click(object sender, EventArgs e)//新增商品
        {
            XingZengoods xin = new XingZengoods();
            xin.ShowDialog(this);
            this.button1_Click(sender,e);
        }

        private void listView1_MouseUp(object sender, MouseEventArgs e)
        {
            int count = listView1.Items.Count;
            if (e.Button == MouseButtons.Left)
            {
                if (listView1.SelectedItems.Count > 0)
                {
                    //listView1.Items[listView1.SelectedIndices[0]].Index

                }
                else if (listView1.SelectedItems.Count <= 0)//点击空白区  
                {
                    if (this.listView1.FocusedItem != null)
                    {
                        ListViewItem item = this.listView1.GetItemAt(e.X, e.Y);
                        if (item == null)
                        {
                            this.listView1.FocusedItem.Selected = true;
                            //listView1.Items[count - 1].BackColor = Color.Blue;
                            //listView1.FocusedItem.BackColor = SystemColors.Highlight;
                            //listView1.FocusedItem.ForeColor = Color.White;
                        }
                    }
                }
            }
        }
        private int iOld = -1;
        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedIndices.Count > 0) //若有选中项 
            {
                if (iOld == -1)
                {
                    listView1.Items[listView1.SelectedIndices[0]].BackColor = SystemColors.Highlight; //设置选中项的背景颜色 
                    iOld = listView1.SelectedIndices[0]; //设置当前选中项索引 
                }
                else
                {
                    if (listView1.SelectedIndices[0] != iOld)
                    {
                        listView1.Items[listView1.SelectedIndices[0]].BackColor = SystemColors.Highlight; //设置选中项的背景颜色 
                        listView1.Items[iOld].BackColor = SystemColors.Window;//恢复默认背景色 
                        iOld = listView1.SelectedIndices[0]; //设置当前选中项索引 
                    }
                }
            }
            else //若无选中项 
            {
                listView1.Items[iOld].BackColor = SystemColors.Window;//恢复默认背景色 
                iOld = -1; //设置当前处于无选中项状态 
            }
        }

        private void label29_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label29_MouseEnter(object sender, EventArgs e)
        {
            label22.Visible = true;
            label22.Text = "关闭";
            label29.ForeColor = Color.Blue;
        }

        private void label29_MouseLeave(object sender, EventArgs e)
        {
            label22.Visible = false;
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
    }
}
