using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO.Ports;
using System.Timers;
using System.Data;
using System.Data.SqlClient;
namespace IU_Windows
{
    public partial class Inicio : Form
    {
        Process p = new Process();
        public Inicio()
        {
            InitializeComponent();
            toolStripMenuItem1.Click += ToolStripMenuItem1_Click;
            toolStripMenuItem2.Click += ToolStripMenuItem2_Click;
            label1.Click += Label1_Click;
            linkLabel1.Click += LinkLabel1_Click;
            btnIniciar.Click += BtnIniciar_Click;
        }

        private void BtnIniciar_Click(object sender, EventArgs e)
        {
            if(String.IsNullOrEmpty(tBoxUser.Text) && String.IsNullOrEmpty(tBoxPassword.Text))
            {
                lblError.Text = "Faltan datos de rellenar";
            }
            else
            {
                SqlConnection sql = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\joseangel\\source\\repos\\IU-RRM\\IU-Windows\\DataBase.mdf;Integrated Security=True");
                SqlCommand cmd = new SqlCommand($"SELECT * FROM Usuarios WHERE Nombre = @Nombre AND Contraseña = @Contraseña",sql);
                cmd.Parameters.AddWithValue("@Nombre",tBoxUser.Text);
                cmd.Parameters.AddWithValue("@Contraseña", Helper.encprytPassword(tBoxPassword.Text));
                
                sql.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        MessageBox.Show($"ID:{reader.GetSqlInt32(0)}\rNombre:{reader.GetString(1)}\rApellidos: {reader.GetString(3)}\rCorreo:{reader.GetString(4)}");

                    }
                }
                else
                {
                    lblError.Text = "Usuario/Contraseña incorrectos";
                }
                reader.Close();
                sql.Close();
            }
        }

        private void LinkLabel1_Click(object sender, EventArgs e)
        {
            this.Hide();
            CrearCuenta crearCuenta = new CrearCuenta();
            crearCuenta.Show();
        }

        private void ToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            toolStripDropDownButton1.Text = "COM2";
        }

        private void Label1_Click(object sender, EventArgs e)
        {
            toolStripMenuItem1.Text = "";
        }

        private void ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            toolStripDropDownButton1.Text = "COM1";
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (ProcessHelper.IsProcessRunning(p))
            {
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.FileName = "C:\\Users\\joseangel\\source\\repos\\IU-RRM\\WindowsGame2\\WindowsGame2\\bin\\x86\\Debug\\WindowsGame2.exe";
                p.StartInfo.CreateNoWindow = true;
                p.Start();
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {

        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
    public static class ProcessHelper
    {
        public static bool IsProcessRunning(Process p)
        {
            try
            {
                Process.GetProcessById(p.Id);
            }
            catch (InvalidOperationException)
            {
                return false;
            }
            return true;
        }
    }
}
