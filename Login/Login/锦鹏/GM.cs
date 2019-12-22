using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;


namespace 网吧系统
{
    class GM
    {
        string Lj = "server=.;uid=sa;pwd=sasa;database=Member";
        public DataSet Tycx(string sql,string kcneme)
        {
            SqlConnection con = new SqlConnection(Lj);
            SqlDataAdapter da = new SqlDataAdapter(sql,con);
            DataSet ds = new DataSet();
            da.Fill(ds, kcneme);
            return ds;
        }
        public int Zsg(string sql)
        {
            SqlConnection con = new SqlConnection(Lj);
            SqlCommand com = new SqlCommand(sql,con);
            con.Open();
            int rs = com.ExecuteNonQuery();
            con.Close();
            return rs;
        }
        public SqlDataReader reader(string sql)
        {
            SqlConnection con = new SqlConnection(Lj);
            con.Open();
            SqlCommand com = new SqlCommand(sql,con);
            SqlDataReader reader = com.ExecuteReader(CommandBehavior.CloseConnection);
            return reader;
        }

        public int Dyh(string sql)
        {
            SqlConnection con = new SqlConnection(Lj);
            con.Open();
            SqlCommand com = new SqlCommand(sql, con);
            int bh =int.Parse(com.ExecuteScalar().ToString());
            con.Close();
            return bh;
        }
    }
}

