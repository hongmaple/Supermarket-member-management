using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;


namespace Login
{
    class DBTools
    {
        public SqlDataAdapter jiazai;
        private string open = "server=.;uid=sa;pwd=sasa;database=Member";//连接数据库的语句
        public DataSet QuerByAdapter(string sql, string tableName)//断开式查询的方法
        {
            DataSet movie = new DataSet();//创建临时数据库供全局使用
            SqlConnection conn = new SqlConnection(open);
            jiazai = new SqlDataAdapter(sql, conn);
            jiazai.Fill(movie, tableName);
            return movie;
        }
        public SqlDataReader reader(string sql) //连接式操作数据库
        {
            SqlConnection conn = new SqlConnection(open);
            conn.Open();
            SqlCommand cmd = new SqlCommand(sql, conn);
            SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            //CommandBehavior.CloseConnection 提供对查询结果和查询数据库的影响的说明，在执行CloseConnection命令时如果关闭关联的DataReader,也会关闭连接
            return reader;
        }
        public int DonIntsdf(string sql) //用于执行增删改的方法
        {
            int rs = 0;
            SqlConnection conn = new SqlConnection(open);
            SqlCommand cmd = new SqlCommand(sql, conn);
            conn.Open();
            rs = cmd.ExecuteNonQuery();
            conn.Close();
            return rs;
        }
        public int Login(string sql)//用于执行查询的方法
        {
            int login = 0;
            SqlConnection conn = new SqlConnection(open);
            SqlCommand cmd = new SqlCommand(sql, conn);
            conn.Open();
            login = Convert.ToInt32(cmd.ExecuteScalar());
            conn.Close();
            return login;
        }
    }
}
