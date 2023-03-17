using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    internal class DBHelper
    {
        private static DBHelper _Instance;
        private SqlConnection _cnn;

        public static DBHelper Instance
        {
            get
            {
                if (_Instance == null)
                {
                    string s = @"Data Source=LAPTOP-CUA-DUY\SQLEXPRESS02;Integrated Security=SSPI;Initial Catalog=QuanLiSinhVien";
                    _Instance = new DBHelper(s);
                }
                return _Instance;

            }

            set => _Instance=value;
        }
        public DBHelper(string s)
        {
            _cnn = new SqlConnection(s);
        }
        public void ExecuteDB(string query, SqlParameter p)
        {
            SqlCommand cmd = new SqlCommand(query,_cnn);
            cmd.Parameters.Add(p);
            _cnn.Open();
            cmd.ExecuteNonQuery();
            _cnn.Close();

        }
        public void ExecuteDB(SqlCommand cmd)
        {
            cmd.Connection = _cnn;
            _cnn.Open();
            cmd.ExecuteNonQuery();
            _cnn.Close();
        }
        public void ExcuteDB(string query)
        {
            SqlCommand cmd = new SqlCommand(query, _cnn);

            _cnn.Open();
            cmd.ExecuteNonQuery();
            _cnn.Close();
        }
        public DataTable GetRecordsx(string s) { 
            DataTable dt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(s,_cnn);
            _cnn.Open();
            adapter.Fill(dt);
            _cnn.Close();
            return dt;
        }
        public DataTable GetRecordsx(SqlCommand cmd)
        {
            DataTable dt = new DataTable();
            cmd.Connection = _cnn;
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            _cnn.Open();
            adapter.Fill(dt);
            _cnn.Close();
            return dt;
        }
    }
}
