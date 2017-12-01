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
using System.Data.SqlClient;
namespace IU_Windows
{
    public partial class Inicio : Form
    {
        List<TextBox> requeridos = new List<TextBox>();
        public Inicio()
        {
            this.StartPosition = FormStartPosition.CenterScreen;
            InitializeComponent();
            setRequeridos();
            //label1.Click += Label1_Click;
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
                Usuario usuario = new SQLHelper().GetUsuario(tBoxUser.Text, tBoxPassword.Text);
                if(usuario != null)
                {
                    this.Hide();
                    SeleccionDePaciente next = new SeleccionDePaciente(usuario.SqlId);
                    next.Show();
                }
                else
                    lblError.Text = "Usuario/Contraseña incorrectos";
            }
        }

        private void LinkLabel1_Click(object sender, EventArgs e)
        {
            this.Hide();
            CrearCuenta crearCuenta = new CrearCuenta();
            crearCuenta.Show();
        }
        private void Button1_Click(object sender, EventArgs e)
        {

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
        private void setRequeridos()
        {
            requeridos.Add(tBoxUser);
            requeridos.Add(tBoxPassword);
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
