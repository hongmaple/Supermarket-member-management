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
    public partial class liulan : Form
    {
        ////第二步：声明一个委托类型的事件  
        //public event setTextValue setFormTextValue;  
        public delegate void Slave2MainDelegate(string[] topmost, int[] sdgfhj, int count); //定义委托   
        public liulan()
        {
            InitializeComponent();
        }
        public float danjia;
        DataSet Linshishujuji = new DataSet();
        DBTools cha = new DBTools();
        DataView shaxuan;
        public void chaxungoodslist()
        {
            string sql = "SELECT [goodsId] ,[goodsName] ,[goodsCount] ,[goodsprice] FROM [Member].[dbo].[Goods]";
            Linshishujuji = cha.QuerByAdapter(sql, "goodlist");
            shaxuan = new DataView(Linshishujuji.Tables["goodlist"]);
            dataGridView1.DataSource = shaxuan;

            string sql2 = @"SELECT [GoodsTypeId]
                          ,[GoodsTypeName]
                      FROM [Member].[dbo].[GoodsType]";

            SqlDataReader reader = cha.reader(sql2);
            while (reader.Read())
            {
                TreeNode Fujiedian = new TreeNode(reader["GoodsTypeName"].ToString());
                Fujiedian.Tag = reader["goodsTypeId"].ToString();

                string sql3 = @"SELECT [goodsId]
                              ,[goodsName]
                              ,[goodsprice]
                          FROM [Member].[dbo].[Goods]
                          where goodsId=" + int.Parse(reader["goodsTypeId"].ToString());
                SqlDataReader reader2 = cha.reader(sql3);
                while (reader2.Read())
                {
                    TreeNode zijiedain = new TreeNode(reader2["goodsId"].ToString() + ":" + reader2["goodsName"].ToString() + String.Format("{0,8:C2}", float.Parse(reader2["goodsprice"].ToString())));//{0,8:C2}格式字符串的格式项
                    zijiedain.Tag = int.Parse(reader2["goodsId"].ToString());
                    Fujiedian.Nodes.Add(zijiedain);
                }
                treeView1.Nodes.Add(Fujiedian);
            }
        }
        private void liulan_Load(object sender, EventArgs e)
        {
            chaxungoodslist();
        }
        public bool IsNumber(string str_number)//判断价格框是否为数字
        {
            return System.Text.RegularExpressions.Regex.IsMatch(str_number, @"^[0-9]*$");
            //return System.Text.RegularExpressions.Regex.IsMatch(str_number, @"[1-9]\d*.\d*|0.\d*[1-9]\d*");//只能允许价格框输入小数和整数
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string tj = "";
            if (textBox1.Text != "")
            {
                if (IsNumber(textBox1.Text))
                {
                    tj = " goodsId=" + int.Parse(textBox1.Text);
                }
                else
                {
                    tj = " goodsName  like '%" + textBox1.Text + "%'";
                }
            }
            else
            {
                tj = "";
            }
            shaxuan.RowFilter = tj;
        }
        float zonjiage = 0;//总价
        float yuanjia = 0;//原价
        public string godsId;
        public string goodsName;
        public string point;//兑换商品所需积分
        public string GoodsTypeName;
        public string goodsprice;
        public string jiag;//折后单价
        public string shulian;//数量
        public float zonpice;//单件商品合计总价
        ListViewItem row;
        public void tianjiashangpin(int goodsid, float sum) 
        {
            string sql = @"SELECT [goodsId]
                          ,[goodsName]
                          ,[goodsPoint]
                          ,[goodsCount]
                          ,GoodsType.GoodsTypeName
                          ,[goodsprice]
                      FROM [Member].[dbo].[Goods]
                      inner join dbo.GoodsType
                      on [Goods].goodsTypeId=GoodsType.GoodsTypeId
                      where goodsId=" + goodsid;
            SqlDataReader reader = cha.reader(sql);
            if (reader.Read())
            {
                godsId = Convert.ToString(reader["goodsId"]);
                goodsName = Convert.ToString(reader["goodsName"]);
                point = Convert.ToString(reader["goodsPoint"]);
                GoodsTypeName = Convert.ToString(reader["GoodsTypeName"]);
                goodsprice = Convert.ToString(reader["goodsprice"]);
                yuanjia = yuanjia + float.Parse(reader["goodsprice"].ToString()) *sum;
                float price = float.Parse(reader["goodsprice"].ToString()) * danjia;//danjia此变量表示折扣
                zonpice = price * sum;
                zonjiage = zonjiage + zonpice;//总价
                jiag = Convert.ToString(price);
                shulian = sum.ToString();
            }
            row = new ListViewItem();
            row.Text = godsId;
            //row.Tag = Convert.ToInt32(reader["oid"]);
            row.SubItems.Add(goodsName);
            row.SubItems.Add(GoodsTypeName);
            row.SubItems.Add(goodsprice);
            row.SubItems.Add(Convert.ToString(jiag));
            row.SubItems.Add(shulian);
            row.SubItems.Add(Convert.ToString(zonpice));
            listView1.Items.Add(row);
            listView1.Focus();
            int count = listView1.Items.Count;
            listView1.Items[count - 1].Selected = true;
            listView1.HideSelection = true; // 失去焦点后仍保持选中状态

            //listView1.FocusedItem.BackColor = SystemColors.Highlight;
            //listView1.setFocusable(false);
            //listView1.setFocusable(true);
            //this.listView1.EndUpdate();  //结束数据处理，UI界面一次性绘制。
            //this.listView1.BeginUpdate();   //数据更新，UI暂时挂起，直到EndUpdate绘制控件，可以有效避免闪烁并大大提高加载速度  
        }
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            //1.用商品编号关于商品查出所有的信息
            if (kkklkk())
            {
            int goodsId = int.Parse(dataGridView1.SelectedRows[0].Cells[0].Value.ToString());
            this.tianjiashangpin(goodsId, float.Parse(numericUpDown1.Value.ToString()));
            }
            
        }
        public bool lskd() //判断库存是否满足
        {
            int count = int.Parse(dataGridView1.SelectedRows[0].Cells[2].Value.ToString());
            shulian = Convert.ToString(numericUpDown2.Value);
            if (int.Parse(shulian) <= count && count > 0)
            {
                panduan = true;
            }
            else
            {
                MessageBox.Show("库存不足");
                panduan = false;
            }
            return panduan;
        }
        private void treeView1_DoubleClick(object sender, EventArgs e)//树型控件的事件
        {
            if (treeView1.SelectedNode.Level == 1)
            {
                if (lskd())
                {
                     int goodsId = int.Parse(treeView1.SelectedNode.Tag.ToString());
                     this.tianjiashangpin(goodsId, float.Parse(numericUpDown2.Value.ToString()));
                }
               
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode.Level == 1)
            {
                if (lskd())
                {
                    int goodsId = int.Parse(treeView1.SelectedNode.Tag.ToString());
                    this.tianjiashangpin(goodsId, float.Parse(numericUpDown2.Value.ToString()));
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)//返回
        {
            this.Close();
        }
        //// 第一步：声明一个委托。（根据自己的需求）  
        //public delegate void setTextValue(string textValue);  
        public Slave2MainDelegate Slave2MainDele;//定义委托实例    
        public void button4_Click(object sender, EventArgs e)
        {
            //setFormTextValue(this.textBox1.Text);
            int i = 0;
            int Count=listView1.Items.Count;
            string[] shulian = new string[Count];
            int[] goodsId = new int[Count];
            for (i = 0; i < Count; i++)
            {
                goodsId[i] = int.Parse(listView1.Items[i].SubItems[0].Text);
                shulian[i] = listView1.Items[i].SubItems[5].Text;
                //Slave2MainDele.Invoke(DateTime.Now.ToString());
                Slave2MainDele.Invoke(shulian, goodsId, Count); 
            }
            if (i==Count)
            {
                this.Close();
            }
        }
        //private List<TNPFriendTagRelation > mOldRelation=new ArrayList<>();

        private void button3_Click(object sender, EventArgs e)//删除
        {
            if (listView1.Items.Count!=0)
            {
                listView1.Items.Remove(listView1.SelectedItems[0]);//删除listView中的一行
                listView1.Focus();
                int count = listView1.Items.Count;
                if (count > 0)
                {
                    listView1.Items[count - 1].Selected = true;
                }
                listView1.HideSelection = true; // 失去焦点后仍保持选中状态
                //listView1.FocusedItem.BackColor = SystemColors.Highlight;
            }
            else
            {
                MessageBox.Show("请选择一行");
            }
        }
        bool panduan = false;
        public bool kkklkk() 
        {
            int count = int.Parse(dataGridView1.SelectedRows[0].Cells[2].Value.ToString());
            shulian = Convert.ToString(numericUpDown1.Value);
            if (int.Parse(shulian) <= count && count>0)
            {
                panduan=true;
            }
            else
            {
                MessageBox.Show("库存不足");
                panduan = false;
            }
            return panduan;
        }
        private void listView1_MouseUp(object sender, MouseEventArgs e)
        //鼠标指针在组件上分时并释放鼠标按钮时发生c# listview 点击空白，也不改变焦点
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

        private void button1_Click(object sender, EventArgs e)
        {
            if (kkklkk())
            {
                int goodsId = int.Parse(dataGridView1.SelectedRows[0].Cells[0].Value.ToString());
                this.tianjiashangpin(goodsId, float.Parse(numericUpDown1.Value.ToString()));
            }
        }

        private void 新增商品ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            XingZengoods xin = new XingZengoods();
            xin.ShowDialog(this);
            chaxungoodslist();
        }
        private int iOld = -1;
        private void listView1_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
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
    }
}
