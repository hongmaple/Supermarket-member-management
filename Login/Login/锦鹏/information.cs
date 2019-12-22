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
    public partial class information : Form
    {
        public information()
        {
            InitializeComponent();
        }
        public int zbh;
        public double jlxf;
        GM gm = new GM();
        private void information_Load(object sender, EventArgs e)
        {
            string sql = @"select  m.id,m.name,c.typeName,m.point,m.Balance from MemberInfo M
                            inner join CardType c on m.cardType=c.id
                            where  m.id=" + zbh;
            SqlDataReader reader = gm.reader(sql);
            if (reader.Read())
            {
                label8.Text = reader["id"].ToString();
                label9.Text = reader["name"].ToString();
                label11.Text = reader["typeName"].ToString();
                label12.Text = reader["Balance"].ToString() + "元";
            }
            label13.Text =  jlxf.ToString() + "元";
            reader.Close();
        }
    }
}
