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
    public partial class 添加会员 : Form
    {
        public 添加会员()
        {
            InitializeComponent();
        }
        DBTools cha = new DBTools();//工具类
        DataSet Linshishujuji = new DataSet();//数据集
        DataView skj;
        public void chushichaxun()//1.从数据库读取唯一的会员卡号放入会员卡号框
        {
            string sql2 = "select id from dbo.MemberInfo order by [id] desc";
            SqlDataReader reader = cha.reader(sql2);
            if (reader.Read())
            {
                textBox1.Text = int.Parse(reader["id"].ToString()) + 1 + "";
            }
            reader.Close();
            //2.从数据库查询会员卡级别等信息放进下拉框
            string sql = "select * from dbo.CardType";
            Linshishujuji = cha.QuerByAdapter(sql, "jibie");
            comboBox2.DisplayMember = "typeName";
            comboBox2.ValueMember = "id";
            comboBox2.DataSource = Linshishujuji.Tables["jibie"];
        }
        private void Form2_Load(object sender, EventArgs e)//窗体加载
        {
            chushichaxun();
        }
        public bool IsNumber(string str_number)//判断价格框是否为数字
        {
            return System.Text.RegularExpressions.Regex.IsMatch(str_number, @"^[0-9]*$");
            //return System.Text.RegularExpressions.Regex.IsMatch(str_number, @"[1-9]\d*.\d*|0.\d*[1-9]\d*");//只能允许价格框输入小数和整数
        }
        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string text = textBox3.Text;
                string tj = "";
                if (this.IsNumber(text))
                {
                    int id = int.Parse(textBox3.Text);
                    tj = " id=" + id;
                }
                if (this.IsNumber(text)==false)
                {
                    tj = "  name like '%" + text + "%'";
                }
                skj.RowFilter = tj;
            }
            catch (Exception)
            {

            }
        }
        private void comboBox2_SelectionChangeCommitted(object sender, EventArgs e)
        {
            int bh = int.Parse(comboBox2.SelectedValue.ToString())-1;
            label5.Text = Linshishujuji.Tables["jibie"].Rows[bh][2].ToString()+"折";
            if (bh==0)
            {
                textBox5.Text = "100";
                textBox6.Text = "100";
            }
            else if (bh==1)
            {
               textBox5.Text = "150";
               textBox6.Text = "150";
            }
            else if (bh==2)
            {
                textBox5.Text = "200";
                textBox6.Text = "200";
            }
            else if (bh==3)
            {
                textBox5.Text = "250";
                textBox6.Text = "250";
            }
            else if (bh==4)
            {
                textBox5.Text = "300";
                textBox6.Text = "300";
            }
            else if (bh == 5)
            {
                textBox5.Text = "350";
                textBox6.Text = "350";
            }
        }

        private void comboBox3_SelectionChangeCommitted(object sender, EventArgs e)
        {
            string kjgh = comboBox3.Text;
            label13.Visible = true;
            if (kjgh=="不优惠")
            {
                label13.Text = "交费100实际存入卡中100";
            }
            else if (kjgh == "存款优惠")
	        {
                label13.Text = "超过100送10";
                //textBox6.Text = int.Parse(textBox6.Text)+10+ "";
            }
            else if (kjgh == "消费转现金")
            {
                label13.Text = "累计消费100转现金10元";
            }
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)//
        {
            if (checkBox1.Checked==true)
            {
                dateTimePicker1.Enabled=true;
            }
            else
            {

            }
        }

        private void button6_Click(object sender, EventArgs e)//关闭
        {
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string sql3 = @"SELECT [MemberInfo].[id]
                          ,[MemberInfo].[name]
                          ,[CardType].typeName
                          ,[MemberInfo].[Balance]
                      FROM [Member].[dbo].[MemberInfo]
                      inner join [Member].[dbo].[CardType]
                      on [MemberInfo].cardType=[CardType].id";
            Linshishujuji = cha.QuerByAdapter(sql3, "huiyuan");
            skj = new DataView(Linshishujuji.Tables["huiyuan"]);
            dataGridView1.DataSource = skj;
            panel1.Visible = true;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            panel1.Visible = false;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            try
            {
                string bh = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
                textBox15.Text = bh;
                panel1.Visible = false;
            }
            catch (Exception)
            {
                
            }
            
        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                string bh = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
                textBox15.Text = bh;
                panel1.Visible = false;
            }
            catch (Exception)
            {

            }
        }
        string MemLujin = "";
        private void 浏览ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Multiselect = false;//该值确定是否可以选择多个文件
            fileDialog.Title = "请选择文件夹"; //窗体标题
            fileDialog.Filter = "图片文件(*.jpg,*.png)|*.jpg;*.png"; //文件筛选
          
            fileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer);
            if (fileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
               string file = fileDialog.FileName;//文件夹路径
               string lujin = file.Substring(file.LastIndexOf("\\") + 1); //格式化处理，提取文件名
               comboBox8.Text = file;
               this.pictureBox1.ImageLocation = file;
               MemLujin = file;
               file = "";
            }
        }

        private void 清除ToolStripMenuItem_Click(object sender, EventArgs e)//清除图片
        {
            this.pictureBox1.Image = null;
        }
        //private FilterInfoCollection videoDevices;
        string lujin = "";
        public void textChange(string fileName)
        {
            lujin = fileName;
        }
        private void 拍照ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Member_photo_collection paizao = new Member_photo_collection();
                paizao.Slave2MainDele += textChange;  //总之就是先把form2里的这个事件注册为form1里的内容
                paizao.ShowDialog(this);
                this.pictureBox1.Image = Image.FromFile(lujin);
                comboBox8.Text = lujin;
                MemLujin = lujin;
                lujin = "";
            }
            catch (Exception)
            {
            }
        }
        private void button5_Click(object sender, EventArgs e)//确定按钮
        {
            if (textBox2.Text != "")
            {
                string name = textBox2.Text;
                int cardType = int.Parse(comboBox2.SelectedValue.ToString());
                int point = int.Parse(textBox5.Text);
                int Balance = int.Parse(textBox6.Text);
                if (comboBox3.Text == "存款优惠" && Balance >= 100)
                {
                    Balance = Balance + 10;
                }
                string sql = string.Format(@"INSERT INTO [Member].[dbo].[MemberInfo]
                                           ([name]
                                           ,[cardType]
                                           ,[point]
                                           ,[Balance])
                                     VALUES
                                           ('{0}','{1}','{2}','{3}')", name, cardType, point, Balance);
                int jieguo = cha.DonIntsdf(sql);
                int id = int.Parse(textBox1.Text);
                string sql2 = string.Format(@"INSERT INTO [Member].[dbo].[MemImg]
                                               ([Memid]
                                               ,[LuJin])
                                         VALUES({0},'{1}')", id,MemLujin);
                cha.DonIntsdf(sql2);
                if (jieguo == 1)
                {
                     DialogResult rs=MessageBox.Show("添加成功,是否退出","提示",MessageBoxButtons.YesNo,MessageBoxIcon.Asterisk);
                    MemLujin = "";
                    if (rs==DialogResult.Yes)
                    {
                        this.Close();
                    }
                    else
                    {
                        textBox1.Text = "";
                        chushichaxun();
                    }
                }
                else
                {
                    MessageBox.Show("添加失败");
                }
            }
            else
            {
                MessageBox.Show("姓名不能为空");
            }
        }

        private void button2_MouseEnter(object sender, EventArgs e)
        {
            label28.Text = "鼠标右击";
            label28.Visible = true;
        }

        private void button2_MouseLeave(object sender, EventArgs e)
        {
            label28.Visible = false;
        }

        private void label29_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label29_MouseEnter(object sender, EventArgs e)
        {
            label7.Visible = true;
            label7.Text = "关闭";
            label29.ForeColor = Color.Blue;
        }

        private void label29_MouseLeave(object sender, EventArgs e)
        {
            label7.Visible = false;
            label29.ForeColor = Color.White;
        }

        private void label7_Click(object sender, EventArgs e)
        {

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
