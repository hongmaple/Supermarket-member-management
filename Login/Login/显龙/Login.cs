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
using Login;
using Login.Properties;
namespace 神一样的登录界面
{
    public partial class 神一样的登录界面 : Form
    {
        public 神一样的登录界面()
        {
            InitializeComponent();
        }

        DBTools db = new DBTools();
        DataSet ds = new DataSet();
        int jieguo = 0;
        public void yanzhengma() //产生验证码
        {
            Random random = new Random();
            int num1 = random.Next(10);
            int num2 = random.Next(10);
            string[] yunshuan = { "-", "+", "*" };
            int adf = random.Next(3);
            if (yunshuan[adf]=="-")
            {
                jieguo = num1 - num2;
                label5.Text = num1 + "-" + num2+"=";
            }
            else if (yunshuan[adf] == "+")
            {
                jieguo = num1 + num2;
                label5.Text = num1 + "+" + num2 + "=";
            }
            else
            {
                jieguo = num1 * num2;
                label5.Text = num1 + "X" + num2 + "=";
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            yanzhengma();
        }

        int zz = 0;
        private void button1_Click(object sender, EventArgs e)
        {
            string zh = comboBox1.Text;
            string mm = textBox1.Text;
            
            if (zh.Trim()=="")
            {
                MessageBox.Show("用户名不能为空！");
            }
            else if (mm.Trim() == "")
            {
                MessageBox.Show("密码不能为空！");
            }
            else
            {
                if (zz == 3)
                {
                    string sql = @"update LoginInfo
                                set status = '0'
                                ,unlockTime = DATEADD(HH,24,GETDATE())
                                where login='" + zh + "'";
                    int rs = db.DonIntsdf(sql);
                    if (rs == 1)
                    {
                        MessageBox.Show("登录失败三次，账号已锁定！");
                    }
                    zz = 0;
                }
                else
	            {
                    string sql = @"select count(*) from LoginInfo 
                               where login='" + zh + "' and unlockTime>GETDATE()";
                    int rs = db.Login(sql);
                    if (rs == 1)
                    {
                        MessageBox.Show("账号已锁定，无法登录！");
                    }
                    else
                    {
                        string sql2 = @"select count(*) from LoginInfo 
                               where login='" + zh + "' and pass='" + mm + "'";
                        int rs2 = db.Login(sql2);
                        if (rs2 == 0)
                        {
                            MessageBox.Show("登录失败!密码错误，您还有" +(3-zz-1)+"次机会");
                            zz++;
                        }
                        else if(textBox2.Text.Trim()==jieguo.ToString())
                        {
                            string sql3 = @"update LoginInfo
                                set status = '1'
                                ,unlockTime = null
                                where login='" + zh + "'";
                            db.DonIntsdf(sql3);
                            Login.Form1 open = new Login.Form1();
                            open.Show();
                            MessageBox.Show(comboBox1.Text + "欢迎您登陆成功");
                            StaticPropertiesOfClasses.Id = zh;
                            StaticPropertiesOfClasses.pwd = mm;
                            this.Hide();
                        }
                        else
                        {
                            MessageBox.Show("验证码不正确");
                        }
                    }
	            }
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {
            Form2 x = new Form2();
            x.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (monthCalendar1.Visible == true)
            {
                monthCalendar1.Visible = false;
            }
            else
            {
                monthCalendar1.Visible = true;
            }
        }

        private void label4_Click(object sender, EventArgs e)
        {
            Form4 x = new Form4();
            x.Show();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {
            yanzhengma();
        }
        
protected override CreateParams CreateParams
        {
            get
            {
 
                CreateParams cp = base.CreateParams;
 
                cp.ExStyle |= 0x02000000;  // Turn on WS_EX_COMPOSITED  
 
                if (this.IsXpOr2003 == true)
                {
                    cp.ExStyle |= 0x00080000;  // Turn on WS_EX_LAYERED
                    this.Opacity = 1;
                }
 
                return cp;
 
            }
 
        }  //防止闪烁

private Boolean IsXpOr2003
{
    get
    {
        OperatingSystem os = Environment.OSVersion;
        Version vs = os.Version;

        if (os.Platform == PlatformID.Win32NT)
            if ((vs.Major == 5) && (vs.Minor != 0))
                return true;
            else
                return false;
        else
            return false;
    }
    }
            private void label11_Click(object sender, EventArgs e)
            {
                this.Close();
            }

            private void label11_MouseEnter(object sender, EventArgs e)
            {
                label10.Visible = true;
                label10.Text = " 关闭 ";
                label11.ForeColor = Color.Blue;
            }

            private void label11_MouseLeave(object sender, EventArgs e)
            {
                label10.Visible = false;
                label11.ForeColor = Color.White;
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
                mouseSet.Offset(mouseOff.X,mouseOff.Y);//设置移动后的坐标
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
