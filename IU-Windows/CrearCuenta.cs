using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Sql;

namespace IU_Windows
{
    public partial class CrearCuenta : Form
    {
        List<TextBox> requeridos = new List<TextBox>();
        public CrearCuenta()
        {
            InitializeComponent();
            btnCrearCuenta.Click += BtnCrearCuenta_Click;
            btnVerContraseña.Click += BtnVerContraseña_Click;
            btnAtras.Click += BtnAtras_Click;
            setRequeridos();
            validarContraseña();
        }

        private void BtnAtras_Click(object sender, EventArgs e)
        {
            this.Hide();
            Inicio inicio = new Inicio();
            inicio.Show();
        }

        private void BtnVerContraseña_Click(object sender, EventArgs e)
        {
            inputContraseña.UseSystemPasswordChar = !inputContraseña.UseSystemPasswordChar;
        }

        private void BtnCrearCuenta_Click(object sender, EventArgs e)
        {
            requeridos.ForEach(delegate (TextBox x) {
                if(String.IsNullOrEmpty(x.Text))
                {
                    lblErrorCrearCuenta.Text = "Faltan datos de rellenar";
                    x.BackColor = Color.Red;
                }
                return;
            });

            if(validarContraseña())
            {
                SQLHelper db = new SQLHelper();
                Dictionary<string, object> parametros = new Dictionary<string, object>();
                parametros.Add("@Nombre", inputNombre.Text);
                parametros.Add("@Apellidos", inputApellidos.Text);
                parametros.Add("@Correo", inputCorreo.Text);
                parametros.Add("@FechaDeNacimiento", inputNacimiento.Text);
                parametros.Add("@Contraseña", Helper.encprytPassword(inputContraseña.Text));
                db.Insert("INSERT INTO Usuarios (Nombre,Contraseña,Apellidos,Correo) VALUES (@Nombre, @Contraseña, @Apellidos, @Correo)", parametros);
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click_2(object sender, EventArgs e)
        {

        }
        private void setRequeridos()
        {
            requeridos.Add(inputNombre);
            requeridos.Add(inputCorreo);
            requeridos.Add(inputContraseña);
            requeridos.Add(inputContraseña2);
        }
        private bool validarContraseña()
        {
            return inputContraseña.Text == inputContraseña2.Text;
        }
    }
}
