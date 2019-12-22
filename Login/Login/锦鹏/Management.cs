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
    public partial class Management : Form
    {
        //decimal可以放小数和整数
        GM gm = new GM();
        public Management()
        {
            InitializeComponent();
        }
        //查询会员信息
        DataView dv;
        DataView dw;
        public void cx()
        {
            string sql = @"select  m.id,m.name,c.typeName,m.point,c.sale,m.Balance,'0' as 积累消费 from MemberInfo m
                        inner join CardType c on m.cardType=c.id ";
            DataSet ds = gm.Tycx(sql, "lbcx");
            dv = new DataView(ds.Tables["lbcx"]);
            dw = new DataView(ds.Tables["lbcx"]);
        }
        public void Shaixuan()
        {
            //筛选
            //判断查询内容是否为整型
            if (System.Text.RegularExpressions.Regex.IsMatch(textBox1.Text, @"^[0-9]*$") && textBox1.Text.Trim() != "")
            {
                dv.RowFilter = "id='" + textBox1.Text + "' or name like'%" + textBox1.Text + "%'";
            }
            else
            {
                dv.RowFilter = "name like'%" + textBox1.Text + "%'";
            }
        }
        //查询按钮
        private void button3_Click(object sender, EventArgs e)
        {
            //当面板panel4隐藏时，查询按钮查询文本框textBox1里面的内容。
            if (panel4.Visible == false)
            {
                cx();
                //绑定网格控件dataGridView1
                dataGridView1.DataSource = dv;
                Jf();
                //进行筛选
                Shaixuan();
            }
            //当面板panel4为显示时，查询按钮查询文本框textBox2里面的内容。
            else
            {
                //查询会员信息
                cx();
                //绑定网格控件dataGridView2
                dataGridView2.DataSource = dw;
                Jf();
                //查询所有会员卡的姓名
                if (this.treeView1.Nodes.Count ==0)//count节点计数
                {
                    cxname();
                }
                //自动选中子节点
                if (textBox2.Text.Trim() != "")
                {
                    Ylzx();
                }
                if (textBox2.Text.Trim()=="")
                {
                    //自动选中所有会员
                    treeView1.SelectedNode = treeView1.Nodes[0];
                }
                //视图控件树节点等于1时网格控件dataGridView2内容显示为空
                else if (treeView1.SelectedNode.Level == 1)
                {
                    dw.RowFilter = "id=0";
                }
            }
        }
        //查询积累消费
        public void Jf()
        {
            string sql = @"select memberId,sum([sum]) 积累消费 from OrderInfo
                            group by memberId";
            SqlDataReader reader = gm.reader(sql);
            while (reader.Read())
            {
                int zbh;
                //获取积累费相对应的编号
                int bh = int.Parse(reader["memberId"].ToString());
                if (panel4.Visible == false)
                {
                    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    {
                        //获取网格控件dataGridView1相对应的编号
                       zbh = int.Parse(dataGridView1.Rows[i].Cells[0].Value.ToString());
                       if (zbh == bh)
                       {
                           dataGridView1.Rows[i].Cells[6].Value = reader["积累消费"].ToString();
                       }
                    } 
                 }
                else
                {
                   for (int i = 0; i < dataGridView2.Rows.Count; i++)
                   {
                       //获取网格控件dataGridView2相对应的编号
                      zbh = int.Parse(dataGridView2.Rows[i].Cells[0].Value.ToString());
                      if (zbh == bh)
                      {
                           dataGridView2.Rows[i].Cells[6].Value = reader["积累消费"].ToString();
                      }
                   }
                }
            }
        }
        //刷新时自动选中所选的那一行
        public void zdxz()
        {
            if (dataGridView1.Rows.Count>0)
            {
                //获取选中行的行号
                int hh = int.Parse(dataGridView1.CurrentCell.RowIndex.ToString());
                //刷新
                this.cx();
                dataGridView1.DataSource = dv;
                Jf();
                //自动选中所选的那一行
                dataGridView1.CurrentCell = dataGridView1.Rows[hh].Cells[0];
            }
        }
        //增加按钮
        private void button4_Click(object sender, EventArgs e)
        {
            ZengGai zg = new ZengGai();
            zg.Text = "添加会员";
            int bh = gm.Dyh("select MAX(id) from MemberInfo");
            zg.ShowDialog();
            //当dataGridView1列表为空时，添加会员成功后筛选出刚添加的会员的信息。
            if (dataGridView1.Rows.Count == 0)
            {
                cx();
                dataGridView1.DataSource = dv;
                Jf();
                dv.RowFilter = " id=" + (bh+1);
                return;
            }
            zdxz();
        }
        //修改按钮
        private void button5_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count == 0)
            {
                MessageBox.Show("不能修改会员信息，请选择会员！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }
            ZengGai xg = new ZengGai();
            xg.zbh = int.Parse(dataGridView1.SelectedRows[0].Cells[0].Value.ToString());
            xg.bl = true;
            xg.Text = "修改会员";
            xg.ShowDialog();
            zdxz();
        }
        //双击事件,弹出修改。
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.Rows.Count == 0)
            {
                MessageBox.Show("不能修改会员信息，请选择会员！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }
            ZengGai xg = new ZengGai(); 
            xg.zbh = int.Parse(dataGridView1.SelectedRows[0].Cells[0].Value.ToString());
            xg.bl = true;
            xg.Text = "修改会员";
            xg.ShowDialog();
            zdxz();
        }
        //删除
        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.Rows.Count == 0)
                {
                    MessageBox.Show("不能删除会员信息，请选择会员！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    return;
                }
                DialogResult dr = MessageBox.Show("是否要删除当前记录？", "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    int zbh = int.Parse(dataGridView1.SelectedRows[0].Cells[0].Value.ToString());
                    string sql = string.Format(@"delete OrderInfo
                            where memberId='{0}'
                            delete MemberInfo
                            where id='{1}'", zbh, zbh);
                    int hs = gm.Zsg(sql);
                    if (hs >= 1)
                    {
                        MessageBox.Show("删除成功！", "提示");
                        this.cx();
                        dataGridView1.DataSource = dv;
                        Jf();
                    }
                    else
                    {
                        MessageBox.Show("删除失败！", "提示");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
          
        }
        //会员充值
        private void button7_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count == 0)
            {
                MessageBox.Show("不能续费，请选择会员！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }
            Recharge rh = new Recharge();
            rh.zbh = int.Parse(dataGridView1.SelectedRows[0].Cells[0].Value.ToString());
            rh.ShowDialog();
            zdxz();
        }
        //右击菜单栏会员转账
        private void 会员转账ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count == 0)
            {
                MessageBox.Show("请选择会员！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }
           Transfer tf = new Transfer();
            tf.zbh = int.Parse(dataGridView1.SelectedRows[0].Cells[0].Value.ToString());
            tf.jlxf = double.Parse(dataGridView1.SelectedRows[0].Cells[6].Value.ToString());
            tf.ShowDialog();
            zdxz();
        }
        //右击菜单栏会员卡信息
        private void huiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count == 0)
            {
                MessageBox.Show("请选择会员！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }
            information it = new information();
            it.zbh = int.Parse(dataGridView1.SelectedRows[0].Cells[0].Value.ToString());
            it.jlxf = double.Parse(dataGridView1.SelectedRows[0].Cells[6].Value.ToString());
            it.Show();
        }
        //会员管理
        private void button1_Click(object sender, EventArgs e)
        {
            //面板panel4隐藏
            panel4.Visible = false;
        }
        //会员多级显示
        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView2.Rows.Count>0)
            {
                dataGridView2.DataSource = dw;
                dw.RowFilter = "id=0";
            }
            //面板panel4显示
            panel4.Visible = true;
            //清空
            treeView1.Nodes.Clear();
            textBox2.Text = "";
            label9.Text = "";
            label10.Text = "";
            label11.Text = "";
            label12.Text = "";
            label13.Text = "";
            label14.Text = "";
            label16.Text = "合计：";
            label17.Text = "";
            label18.Text = "";
            label19.Text = "";
            groupBox3.Text = "所有会员";
        }
        //查询所有会员卡的姓名
        public void cxname()
        {
            //if (treeView1.Nodes != null)
            //{
            //    treeView1.Nodes.Clear();
            //}
            TreeNode root = new TreeNode("所有会员");
            string sql = "select * from MemberInfo";
            SqlDataReader reader = gm.reader(sql);
            while (reader.Read())
            {
                TreeNode tn = new TreeNode(reader["name"].ToString());
                tn.Tag = reader["id"];
                root.Nodes.Add(tn);
            }
            reader.Close();
            treeView1.Nodes.Add(root);
            //if (textBox2.Text != "")
            //{
            //    treeView1.SelectedNode = treeView1.Nodes[0].Nodes[0];
            //}
            //treeView1.SelectedNode.Nodes[0].Tag = 1;
            //treeView1.SelectedNode = treeView1.Nodes[0].Nodes[0];
        }
        //用textBox2查询编号时自动选中子节点
        //DataView da;
        //public void zdx()
        //{
        //    //string sql = "select COUNT(*) from dbo.MemberInfo where id<=" + textBox2.Text.Trim();
        //    //int s = gm.Dyh(sql);
        //    string sql = "select ROW_NUMBER() over(order by id) 序号,id from MemberInfo";
        //    DataSet ds = gm.Tycx(sql, "ROW_NUMBER");
        //    da= new DataView(ds.Tables["ROW_NUMBER"]);
        //    int s = int.Parse(textBox2.Text.Trim().ToString());
        //    da.RowFilter = " id=" + s;
        //    int z = int.Parse(ds.Tables["ROW_NUMBER"].Rows[0][0].ToString());
        //    MessageBox.Show(z.ToString());
        //    //treeView1.SelectedNode = treeView1.Nodes[0].Nodes[z - 1];
        //}

        //分组框groupBox2里面所显示的内容
        int bh;
        public void groupBoxEr()
        {
            string sql = @"select  m.id,m.name,c.typeName,m.point from MemberInfo M
                                        inner join CardType c on m.cardType=c.id
                                        where m.id="+bh;
            SqlDataReader reader = gm.reader(sql);
            label14.Text =DateTime.Now.ToLongDateString().ToString();
            if (reader.Read())
            {
                label9.Text = reader["id"].ToString();
                label10.Text = reader["name"].ToString();
                groupBox3.Text = reader["name"].ToString();
                label11.Text = reader["point"].ToString() + "分";
                //label12.Text = reader["积累消费"].ToString() + "元";
                label13.Text = reader["typeName"].ToString();
            }
            label12.Text = "0元";
            sql= "select memberId,sum([sum]) 积累消费 from OrderInfo group by memberId ";
            reader = gm.reader(sql);
            while (reader.Read())
            {
                int memberId =int.Parse(reader["memberId"].ToString());
                if (memberId == bh)
                {
                    label12.Text = reader["积累消费"].ToString() + "元";
                }
            }
            reader.Close();
        }
        //用来自动选中节点
        public void Ylzx()
        {
            string sql = "select ROW_NUMBER() over(order by id) 序号,id,name from MemberInfo";
            DataSet ds = gm.Tycx(sql, "ROW_NUMBER");
            DataView da = new DataView(ds.Tables["ROW_NUMBER"]);
            dataGridView3.DataSource = da;
            if (System.Text.RegularExpressions.Regex.IsMatch(textBox2.Text, @"^[0-9]*$"))
            {
                da.RowFilter = " id='" + textBox2.Text.Trim() + "' or name like '%" + textBox2.Text.Trim() + "%'";
            }
            else
            {
                da.RowFilter = " name like '%" + textBox2.Text.Trim() + "%'";
            }
            if (dataGridView3.Rows.Count>0)
            {
                int z = int.Parse(dataGridView3.SelectedRows[0].Cells[0].Value.ToString());
                //自动选中节点
                treeView1.SelectedNode = treeView1.Nodes[0].Nodes[z - 1];
            }
        }
        //合计
        public void Tongji()
        {
            string sql = @"select COUNT(id) 总人数,sum(point) 总积分,SUM(Balance) 总余额 from MemberInfo
                           select SUM(sum) 积累消费 from OrderInfo";
            SqlDataReader reader = gm.reader(sql);
            if (reader.Read())
            {
                label16.Text = "合计：" + reader["总人数"].ToString();
                label17.Text = reader["总积分"].ToString();
                label18.Text = reader["总余额"].ToString();
            }
            sql = @"select SUM(sum) 积累消费 from OrderInfo";
            reader = gm.reader(sql);
            if (reader.Read())
            {
                label19.Text = reader["积累消费"].ToString();
            }
            reader.Close();
        }
        //清空
        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            
            groupBox3.Text = treeView1.SelectedNode.Text;
            Tongji();
            if (treeView1.SelectedNode.Level == 1)
            {
                bh = int.Parse(treeView1.SelectedNode.Tag.ToString());
                groupBoxEr();
                dw.RowFilter = "id=0";
                label16.Text = "合计：";
                label17.Text = "";
                label18.Text = "";
                label19.Text = "";
                //MessageBox.Show(treeView1.SelectedNode.Index.ToString());
            }
            //清空
            else
            {
                dw.RowFilter = "";
                label9.Text = "";
                label10.Text = "";
                label11.Text = "";
                label12.Text = "";
                label13.Text = "";
                label14.Text = "";
            }
        }
    }
}
