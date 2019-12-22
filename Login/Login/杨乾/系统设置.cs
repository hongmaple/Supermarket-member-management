using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Login;

namespace 会员管理系统
{
    public partial class 系统设置 : Form
    {
        DBTools db = new DBTools();
        DataView dv;
        DataSet ds;
        public 系统设置()
        {
            InitializeComponent();
        }
        public void zkk()
        {
            string sql = @"select * from CardType";
            ds = db.QuerByAdapter(sql, "zkk");
            dv = new DataView(ds.Tables["zkk"]);
            dataGridView1.DataSource = dv;
        }

        private void huiyuanka_Load(object sender, EventArgs e)
        {
            zkk();
        }

        private void button22_Click(object sender, EventArgs e)
        {
            zkktianjia tj = new zkktianjia();
            tj.ShowDialog();
            zkk();
        }

        private void button21_Click(object sender, EventArgs e)
        {
            try
            {
                zkktianjia xg = new zkktianjia();
                xg.ID = int.Parse(dataGridView1.CurrentRow.Cells[0].Value.ToString());
                xg.bl = true;
                xg.ShowDialog();
                zkk();
            }
            catch (Exception)
            {
               
            }
            
        }

        private void button20_Click(object sender, EventArgs e)
        {
            int bh = int.Parse(dataGridView1.CurrentRow.Cells[0].Value.ToString());
            DialogResult xz = MessageBox.Show(
              "是否有删除当前记录？",
              "系统提示",
              MessageBoxButtons.YesNo,
              MessageBoxIcon.Question);
            if (xz == DialogResult.Yes)
            {
                string sql = @" DELETE FROM [Member].[dbo].[CardType]
                                WHERE id='" + bh + "'";
                db.DonIntsdf(sql);
                zkk();
            }
        }
        public void splb()
        {
            string sql = "select * from GoodsType";
            ds = db.QuerByAdapter(sql, "leibie");
            dataGridView4.DataSource=ds.Tables["leibie"];
        }
        public void dhsp()
        {
            string sql = @"select Goods.goodsId,Goods.goodsName,Goods.goodsPoint,Goods.goodsCount,
 GoodsType.GoodsTypeName,Goods.goodsprice
 from Goods
 inner join GoodsType on Goods.goodsTypeId=GoodsType.GoodsTypeId";
            ds = db.QuerByAdapter(sql, "duihuan");
            dataGridView2.DataSource = ds.Tables["duihuan"];
        }

        public void spmc()
        {
            string sql = @"select orderItem.oiId,Goods.goodsName,orderItem.price,
orderItem.count,GoodsType.GoodsTypeName
from orderItem
inner join Goods on Goods.goodsId=orderItem.goodsId
inner join GoodsType on GoodsType.GoodsTypeId=orderItem.goodsId";
            ds = db.QuerByAdapter(sql, "mingcheng");
            dv = new DataView(ds.Tables["mingcheng"]);
            dataGridView3.DataSource = dv;
        }

        public void cx()
        {
            string sql = "select * from GoodsType";
            ds = db.QuerByAdapter(sql, "leibie");
            dv = new DataView(ds.Tables["leibie"]);
            //加载下拉框
            DataRow row = ds.Tables["leibie"].NewRow();
            row[0] = 0;
            row[1] = "所有类别";
            ds.Tables["leibie"].Rows.InsertAt(row, 0);
            comboBox1.DisplayMember = "GoodsTypeName";
            comboBox1.ValueMember = "GoodsTypeId";
            comboBox1.DataSource = ds.Tables["leibie"];
        }
        private void 系统设置_Load(object sender, EventArgs e)
        {
            zkk();
            splb();
            dhsp();
            spmc();
            cx();
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            spmc();
            if (comboBox1.Text != "所有类别")
            {
                dv.RowFilter = "GoodsTypeName='" + comboBox1.Text + "'";
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
            int bh = int.Parse(dataGridView4.SelectedRows[0].Cells[0].Value.ToString());
            DialogResult xz = MessageBox.Show(
              "是否有删除当前记录？",
              "系统提示",
              MessageBoxButtons.YesNo,
              MessageBoxIcon.Question);
            if (xz == DialogResult.Yes)
            {
                string sql = @"DELETE FROM [Member].[dbo].[GoodsType]
                        WHERE GoodsTypeId='" + bh + "'";
                db.DonIntsdf(sql);
                splb();
            }
            }
            catch (Exception ex)
            {
                MessageBox.Show("出现异常！"+ex.Message);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text.Trim() != "")
            {
                dv.RowFilter = "oiId='" + textBox1.Text + "'";
            }
            else
            {
                dv.RowFilter = "";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            lbtianjia tj = new lbtianjia();
            tj.bh = int.Parse(dataGridView4.CurrentRow.Cells[0].Value.ToString());
            tj.ShowDialog();
            splb();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            lbtianjia xg = new lbtianjia();
            xg.bh = int.Parse(dataGridView4.CurrentRow.Cells[0].Value.ToString());
            xg.bl = true;
            xg.ShowDialog();
            splb();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            sptj tj = new sptj();
            tj.bh = int.Parse(dataGridView2.CurrentRow.Cells["dataGridViewTextBoxColumn1"].Value.ToString());
            tj.ShowDialog();
            dhsp();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            sptj xg = new sptj();
            xg.bh = int.Parse(dataGridView2.CurrentRow.Cells["dataGridViewTextBoxColumn1"].Value.ToString());
            xg.bl = true;
            xg.ShowDialog();
            dhsp();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            int bh = int.Parse(dataGridView2.CurrentRow.Cells["dataGridViewTextBoxColumn1"].Value.ToString());
            DialogResult xz = MessageBox.Show(
              "是否有删除当前记录？",
              "系统提示",
              MessageBoxButtons.YesNo,
              MessageBoxIcon.Question);
            if (xz == DialogResult.Yes)
            {
                string sql = @"DELETE FROM [Member].[dbo].[Goods]
                             WHERE goodsId='" + bh + "'";
                db.DonIntsdf(sql);
                dhsp();
            }
        }
        private void button10_Click(object sender, EventArgs e)
        {
            int bh = int.Parse(dataGridView3.CurrentRow.Cells["dataGridViewTextBoxColumn4"].Value.ToString());
            DialogResult xz = MessageBox.Show(
              "是否有删除当前记录？",
              "系统提示",
              MessageBoxButtons.YesNo,
              MessageBoxIcon.Question);
            if (xz == DialogResult.Yes)
            {
                string sql = @"DELETE FROM [Member].[dbo].[orderItem]
                        WHERE oiId='" + bh + "'";
                db.DonIntsdf(sql);
                spmc();
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 0;
        }

        private void button12_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 1;
        }
    }
}
