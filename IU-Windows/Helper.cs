using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Security.Cryptography;

namespace IU_Windows
{
    static public class Helper
    {
        static public string encprytPassword(string password)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(password);
            byte[] inArray = HashAlgorithm.Create("SHA1").ComputeHash(bytes);

            return Convert.ToBase64String(inArray);
        }
    }
    public class SQLHelper
    {
        private SqlConnection sql;
        private string sqlStringConnection = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\joseangel\\source\\repos\\IU-RRM\\IU-Windows\\DataBase.mdf;Integrated Security=True";
        public SQLHelper()
        {
            sql = new SqlConnection(sqlStringConnection);
        }
        public void Open()
        {
            try
            {
                sql.Open();
            }
            catch (SqlException e)
            {
                throw e;
            }
        }
        public void Close()
        {
            try
            {
                sql.Close();
            }
            catch (SqlException e)
            {
                throw e;
            }

        }
        public void Insert(string query, Dictionary<string, object> parameters)
        {
            SqlCommand cmd = new SqlCommand(query, sql);
            foreach (var param in parameters)
            {
                cmd.Parameters.AddWithValue(param.Key, param.Value);
            }
            this.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            this.Close();
        }
        public List<string> Select(string query)
        {

            return new List<string>();
        }
    }
}
