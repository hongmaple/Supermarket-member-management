using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Login
{
    public partial class 高级查询 : Form
    {
        public 高级查询()
        {
            InitializeComponent();
        }
        public delegate void Sqltjiaojian(string tj);//定义委托
        private void button2_Click(object sender, EventArgs e)//退出
        {
            this.Close();
        }
        public Sqltjiaojian Slave2MainDele;//定义委托实例  
        string sql = "";
        private void button1_Click(object sender, EventArgs e)
        {
            string and="";
            string tj1="";
            string tj2="";
            string tj3="";
            string tj4="";
            string tj5="";
            string tj6="";
            if (checkBox1.Checked==true)
            {
                tj1 = " id="+int.Parse(textBox1.Text);
                sql = tj1;
            }
            if (checkBox2.Checked==true)
            {
                if (sql != "")
	        {
		        and=" and";
	        }
            else
	        {
                and="";
	        }
                tj2 = and+" name like '%"+textBox2.Text+"' ";
                sql = sql + tj2;
            }
            if (checkBox3.Checked==true)
            {
                if (sql != "")
	        {
		        and=" and";
	        } else
	        {
                and="";
	        }
                tj3 = "";
                sql = sql + tj3; 
            }
            if (checkBox4.Checked == true)
            {
                if (sql != "")
	        {
		        and=" and";
	        } else
	        {
                and="";
	        }
                tj4=and+"";
                sql = sql + tj4;
            }
            if (checkBox5.Checked == true)
            {
                if (sql != "")
	        {
		        and=" and";
	        } else
	        {
                and="";
	        }
                tj5 =and+ " Balance<"+float.Parse(textBox3.Text);
                sql = sql + tj5;
            }
            if (checkBox6.Checked == true)
            {
                if (sql != "")
                {
                    and = "and";
                }
                else
                {
                    and = "";
                }
                tj6 = and+" point>"+int.Parse(textBox4.Text);
                sql = sql + tj6;
            }
                Slave2MainDele.Invoke(sql);
                this.Close();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked==true)
            {
                textBox1.Enabled = true;
            }
            else
            {
                textBox1.Enabled = false;
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked == true)
            {
                textBox2.Enabled = true;
            }
            else
            {
                textBox2.Enabled = false;
            }
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked == true)
            {
                comboBox1.Enabled = true;
            }
            else
            {
                comboBox1.Enabled = false;
            }
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox4.Checked == true)
            {
                comboBox2.Enabled = true;
            }
            else
            {
                comboBox2.Enabled = false;
            }
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox5.Checked == true)
            {
                textBox3.Enabled = true;
            }
            else
            {
                textBox3.Enabled = false;
            }
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox6.Checked == true)
            {
                textBox4.Enabled = true;
            }
            else
            {
                textBox4.Enabled = false;
            }
        }  

    }
}
