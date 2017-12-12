using System;
using System.Management;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Security.Cryptography;
using Newtonsoft.Json;
using System.Drawing;

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
        static public Dictionary<String, decimal> parametrosPorDefecto = new Dictionary<string, decimal>
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
        static public string GetRRMSerialPort()
        {
            ManagementScope connectionScope = new ManagementScope();
            SelectQuery serialQuery = new SelectQuery("SELECT * FROM Win32_SerialPort");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(connectionScope, serialQuery);

            try
            {
                foreach (ManagementObject item in searcher.Get())
                {
                    string desc = item["Description"].ToString();
                    string deviceId = item["DeviceID"].ToString();

                    if (desc.Contains("Arduino"))
                    {
                        return deviceId;
                    }
                }
            }
            catch (ManagementException e)
            {
                /* Do Nothing */
            }
            System.Windows.Forms.MessageBox.Show("No se ha podido encontrar el RRM", "Error", System.Windows.Forms.MessageBoxButtons.OK,System.Windows.Forms.MessageBoxIcon.Error);
            return null;
        }
        private static string AutodetectArduinoPort()
        {
            ManagementScope connectionScope = new ManagementScope();
            SelectQuery serialQuery = new SelectQuery("SELECT * FROM Win32_SerialPort");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(connectionScope, serialQuery);

            try
            {
                foreach (ManagementObject item in searcher.Get())
                {
                    string desc = item["Description"].ToString();
                    string deviceId = item["DeviceID"].ToString();

                    if (desc.Contains("Arduino"))
                    {
                        return deviceId;
                    }
                }
            }
            catch (ManagementException e)
            {
                /* Do Nothing */
            }

            return null;
        }
        public static bool ValidarEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

    }

    public class SQLHelper
    {
        private SqlConnection sql;
        private string sqlStringConnection =
            "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=" + System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\DataBase.mdf;Integrated Security=True";
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
        public void Query(string query, Dictionary<string, object> parameters)
        {
            SqlCommand cmd = new SqlCommand(query, sql);
            foreach (var param in parameters)
            {
                cmd.Parameters.AddWithValue(param.Key, param.Value);
            }
            this.Open();
            cmd.ExecuteNonQuery();
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
            Usuario usuario = null;
            if (reader.HasRows)
                while (reader.Read())
                {
                    usuario = ReaderToUser(reader);
                    this.Close();
                    return usuario;
                }
            this.Close();
            return usuario;
        }
        public Usuario GetUsuario(String nombre)
        {
            Usuario usuario = null;
            SqlCommand cmd = new SqlCommand("SELECT * FROM Usuarios WHERE Nombre = @Nombre", sql);
            cmd.Parameters.AddWithValue("@Nombre", nombre);
            this.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
                while (reader.Read())
                {
                    usuario = ReaderToUser(reader);
                    this.Close();
                    return usuario;
                }
            this.Close();
            return usuario;
        }
        public Paciente GetPaciente(Int32 SqlId)
        {
            Paciente paciente = null;
            SqlCommand cmd = new SqlCommand("SELECT * FROM Pacientes WHERE Id = @SqlId", sql);
            cmd.Parameters.AddWithValue("@SqlId", SqlId);
            this.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
                paciente = ReaderToPaciente(reader);
            this.Close();
            return paciente;
        }
        public List<Paciente> GetPacientesFromUser(Int32 SqlId)
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM Pacientes WHERE Responsable = @RespId", sql);
            cmd.Parameters.AddWithValue("@RespId", SqlId);
            this.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            List<Paciente> pacientes = new List<Paciente>();
            while (reader.Read())
                pacientes.Add(ReaderToPaciente(reader));
            this.Close();
            return pacientes;
        }
        public List<Terapia> GetTerapiasFromPaciente(Paciente paciente)
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM Terapias WHERE SqlIdPaciente = @SqlId", sql);
            cmd.Parameters.AddWithValue("@SqlId", paciente.SqlId);
            this.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            List<Terapia> terapias = new List<Terapia>();
            while (reader.Read())
                terapias.Add(new Terapia
                {
                    PacienteSqlid = Int32.Parse(reader["SqlIdPaciente"].ToString()),
                    Repeticiones = Int32.Parse(reader["Repeticiones"].ToString()),
                    Duracion = TimeSpan.Parse(reader["Duracion"].ToString()),
                    Observaciones = reader["Observaciones"].ToString(),
                    Parametros = JsonConvert.DeserializeObject<Dictionary<string, decimal>>(reader["Parametros"].ToString()),
                    tipoTerapia = (TipoTerapia)Int32.Parse(reader["Tipo"].ToString()),
                    Fecha = DateTime.Parse(reader["Fecha"].ToString())

                });
            this.Close();
            return terapias;
        }
        public TimeSpan GetHorasTerapiasFromPaciente(Paciente paciente)
        {
            SqlCommand cmd = new SqlCommand("SELECT Duracion FROM Terapias WHERE SqlIdPaciente = @SqlId", sql);
            cmd.Parameters.AddWithValue("@SqlId", paciente.SqlId);
            List<TimeSpan> duraciones = new List<TimeSpan>();
            this.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
                duraciones.Add(TimeSpan.Parse(reader["Duracion"].ToString()));
            this.Close();
            TimeSpan horasTotales = new TimeSpan();
            duraciones.ForEach(x => horasTotales += x);
            return horasTotales;
        }
        public List<Paciente> GetPacientesFromUser(Usuario user)
        {
            return GetPacientesFromUser(user.SqlId);
        }
        public int SelectScalar(string query, Dictionary<string, object> parametros)
        {
            SqlCommand cmd = new SqlCommand(query, sql);
            foreach (var param in parametros)
                cmd.Parameters.AddWithValue(param.Key, param.Value);

            this.Open();
            int count = (Int32)cmd.ExecuteScalar();
            this.Close();
            return count;
        }
        public int Count(string tabla)
        {
            return SelectScalar($"SELECT COUNT(*) FROM {tabla}", new Dictionary<string, object>());
        }
        public Dictionary<TipoTerapia, int> GetNumTerapiasRealizadasFromPaciente(Paciente paciente)
        {
            Dictionary<TipoTerapia, int> numTerapias = new Dictionary<TipoTerapia, int>();
            foreach (var terapia in Enum.GetValues(typeof(TipoTerapia)))
            {
                numTerapias.Add((TipoTerapia)terapia, this.SelectScalar("SELECT COUNT(*) FROM Terapias WHERE Tipo = @TipoTerapia AND SqlIdPaciente = @SqlId", new Dictionary<string, object> {
                                                                                                                                                        { "@TipoTerapia", terapia },
                                                                                                                                                        { "@SqlId", paciente.SqlId} }
                ));
            }
            return numTerapias;
        }
        public void InsertarPaciente(Paciente paciente)
        {
            string query = "INSERT INTO Pacientes (Nombre, Apellidos, Responsable, FechaDeNacimiento) VALUES (@Nombre, @Apellidos, @Responsable,@FechaDeNacimiento)";
            SqlCommand cmd = new SqlCommand(query, sql);
            cmd.Parameters.AddWithValue("@Nombre", paciente.Nombre);
            cmd.Parameters.AddWithValue("@Apellidos", paciente.Apellidos);
            cmd.Parameters.AddWithValue("@Responsable", paciente.Responsable);
            cmd.Parameters.AddWithValue("@FechaDeNacimiento", paciente.FechaDeNacimiento);
            this.Open();
            cmd.ExecuteNonQuery();
            this.Close();
        }
        public void EliminarPaciente(Paciente paciente)
        {
            EliminarPaciente(paciente.SqlId);
        }
        public void EliminarPaciente(Int32 SqlId)
        {
            SqlCommand cmd = new SqlCommand("DELETE FROM Pacientes WHERE Id=@SqlId", sql);
            cmd.Parameters.AddWithValue("@SqlId", SqlId);
            this.Open();
            cmd.ExecuteNonQuery();
            this.Close();
        }
        private Usuario ReaderToUser(SqlDataReader reader)
        {
            Usuario usuario = new Usuario
            {
                Nombre = reader["Nombre"].ToString(),
                Apellido = reader["Apellidos"].ToString(),
                SqlId = Int32.Parse(reader["Id"].ToString())
            };
            return usuario;
        }
        private Paciente ReaderToPaciente(SqlDataReader reader)
        {
            Paciente paciente = new Paciente
            {
                Nombre = reader["Nombre"].ToString(),
                Apellidos = reader["Apellidos"].ToString(),
                SqlId = Int32.Parse(reader["Id"].ToString()),
                Observaciones = reader["Observaciones"].ToString()
            };
            return paciente;
        }
    }
}
