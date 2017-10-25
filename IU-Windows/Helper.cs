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
        public Usuario GetUsuario(Int32 SqlId)
        {

            SqlCommand cmd = new SqlCommand($"SELECT * FROM Usuarios WHERE Id = @Id", sql);
            cmd.Parameters.AddWithValue("@Id", SqlId);
            this.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
                while (reader.Read())
                {
                    Usuario usuario = new Usuario { Nombre = reader["Nombre"].ToString(), Apellido = reader["Apellidos"].ToString(), SqlId = Int32.Parse(reader["Id"].ToString()) };
                    this.Close();
                    return usuario;
                }
            this.Close();
            return new Usuario();
        }
        public List<Paciente> GetPacientesFromUser(Int32 SqlId)
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM Pacientes WHERE Responsable = @RespId", sql);
            cmd.Parameters.AddWithValue("@RespId", SqlId);
            this.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            List<Paciente> pacientes = new List<Paciente>();
            while(reader.Read())
                pacientes.Add(new Paciente { Nombre = reader["Nombre"].ToString(), Apellidos = reader["Apellidos"].ToString(), SqlId = Int32.Parse(reader["Id"].ToString()) });
            this.Close();
            return pacientes;
        }
        public List<Paciente> GetPacientesFromUser(Usuario user)
        {
            return GetPacientesFromUser(user.SqlId);
        }
        public int Count(string query, Dictionary<string, object> parameters)
        {
            SqlCommand cmd = new SqlCommand(query, sql);
            foreach (var param in parameters)
            {
                cmd.Parameters.AddWithValue(param.Key, param.Value);
            }
            this.Open();
            int count = (Int32)cmd.ExecuteScalar();
            this.Close();
            return count;
        }
        public int Count(string tabla)
        {
            return Count($"SELECT COUNT(*) FROM {tabla}", new Dictionary<string, object>());
        }
    }
}
