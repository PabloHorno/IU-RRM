using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Security.Cryptography;
using Newtonsoft.Json;

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
        static public Dictionary<String, int> parametrosPorDefecto = new Dictionary<string, int>
        {
            ///ABRIR CERRAR COMPLETA
            //Abrir
            {"numericUpDownTiempoApertura"+"Completa", 5 },
            {"numericUpDownAnguloApertura"+"Completa", 30 },
            {"numericUpDownVelocidadApertura"+"Completa", 15 },
            //Cerrar
            {"numericUpDownTiempoCierre"+"Completa", 5 },
            {"numericUpDownAnguloCierre"+"Completa", -90 },
            {"numericUpDownVelocidadCierre"+"Completa", 15 },
            ///ABRIR CERRAR DEDOS
            //Pulgar
            //Abrir
            {"numericUpDownTiempoApertura"+"Pulgar", 5 },
            {"numericUpDownAnguloApertura"+"Pulgar", 30 },
            {"numericUpDownVelocidadApertura"+"Pulgar", 15 },
            //Cerrar
            {"numericUpDownTiempoCierre"+"Pulgar", 5 },
            {"numericUpDownAnguloCierre"+"Pulgar", 30 },
            {"numericUpDownVelocidadCierre"+"Pulgar", 15 },
            //Indice
            {"numericUpDownTiempoApertura"+"Indice", 5 },
            {"numericUpDownAnguloApertura"+"Indice", 30 },
            {"numericUpDownVelocidadApertura"+"Indice", 15 },
            //Cerrar
            {"numericUpDownTiempoCierre"+"Indice", 5 },
            {"numericUpDownAnguloCierre"+"Indice", 30 },
            {"numericUpDownVelocidadCierre"+"Indice", 15 },
            //Corazón
            {"numericUpDownTiempoApertura"+"Corazon", 5 },
            {"numericUpDownAnguloApertura"+"Corazon", 30 },
            {"numericUpDownVelocidadApertura"+"Corazon", 15 },
            //Cerrar
            {"numericUpDownTiempoCierre"+"Corazon", 5 },
            {"numericUpDownAnguloCierre"+"Corazon", 30 },
            {"numericUpDownVelocidadCierre"+"Corazon", 15 },
            //Anular
            {"numericUpDownTiempoApertura"+"Anular", 5 },
            {"numericUpDownAnguloApertura"+"Anular", 30 },
            {"numericUpDownVelocidadApertura"+"Anular", 15 },
            //Cerrar
            {"numericUpDownTiempoCierre"+"Anular", 5 },
            {"numericUpDownAnguloCierre"+"Anular", 30 },
            {"numericUpDownVelocidadCierre"+"Anular", 15 },
            //Meñique
            {"numericUpDownTiempoApertura"+"Meñique", 5 },
            {"numericUpDownAnguloApertura"+"Meñique", 30 },
            {"numericUpDownVelocidadApertura"+"Meñique", 15 },
            //Cerrar
            {"numericUpDownTiempoCierre"+"Meñique", 5 },
            {"numericUpDownAnguloCierre"+"Meñique", 30 },
            {"numericUpDownVelocidadCierre"+"Meñique", 15 },

        };
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
        public List<object> Select(string query, Dictionary<string, object> parameters = null)
        {
            if (parameters == null)
                parameters = new Dictionary<string, object>();

            SqlCommand cmd = new SqlCommand(query, sql);
            foreach (var param in parameters)
                cmd.Parameters.AddWithValue(param.Key, param.Value);
            this.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            List<object> datos = new List<object>();
            while (reader.Read())
                for (int i = 0; i < reader.FieldCount; i++)
                    datos.Add(reader[i]);
            this.Close();
            return datos;
        }
        public Usuario GetUsuario(string nombre, string contraseña)
        {
            SqlCommand cmd = new SqlCommand($"SELECT * FROM Usuarios WHERE Nombre = @Nombre AND Contraseña = @Contraseña", sql);
            cmd.Parameters.AddWithValue("@Nombre", nombre);
            cmd.Parameters.AddWithValue("@Contraseña", Helper.encprytPassword(contraseña));
            sql.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            Usuario usuario = null;
            if (reader.HasRows)
                while (reader.Read())
                {
                    //usuario = new Usuario { Nombre = reader["Nombre"].ToString(), Apellido = reader["Apellidos"].ToString(), SqlId = Int32.Parse(reader["Id"].ToString()) };
                    Int32 SqlId = Int32.Parse(reader["Id"].ToString());
                    sql.Close();
                    return GetUsuario(SqlId);
                }
            sql.Close();
            return usuario;
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
            while (reader.Read())
                pacientes.Add(new Paciente { Nombre = reader["Nombre"].ToString(), Apellidos = reader["Apellidos"].ToString(), SqlId = Int32.Parse(reader["Id"].ToString()) });
            this.Close();
            return pacientes;
        }
        public List<Terapia> GetTerapiasFromPaciente(Int32 SqlId)
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM Terapias WHERE Paciente = @SqlId", sql);
            cmd.Parameters.AddWithValue("@SqlId", SqlId);
            this.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            List<Terapia> terapias = new List<Terapia>();
            while (reader.Read())
                terapias.Add(new Terapia
                {
                    PacienteSqlid = Int32.Parse(reader["Paciente"].ToString()),
                    Repeticiones = Int32.Parse(reader["Repeticiones"].ToString()),
                    Duracion = DateTime.Parse(reader["Duracion"].ToString()),
                    Observaciones = reader["Observaciones"].ToString(),
                    Parametros = JsonConvert.DeserializeObject<Dictionary<string, Int32>>(reader["Parametros"].ToString()),
                    tipoTerapia = Int32.Parse(reader["Tipo"].ToString()) == 0 ? TipoTerapia.AbrirCerrarDedos :
                                  Int32.Parse(reader["Tipo"].ToString()) == 1 ? TipoTerapia.AbrirCerrarMano :
                                  Int32.Parse(reader["Tipo"].ToString()) == 2 ? TipoTerapia.PinzaFina : TipoTerapia.PinzaGruesa

                });
            this.Close();
            return terapias;
        }
        public List<Paciente> GetPacientesFromUser(Usuario user)
        {
            return GetPacientesFromUser(user.SqlId);
        }
        public int Count(string query, Dictionary<string, object> parameters)
        {
            SqlCommand cmd = new SqlCommand(query, sql);
            foreach (var param in parameters)
                cmd.Parameters.AddWithValue(param.Key, param.Value);

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
