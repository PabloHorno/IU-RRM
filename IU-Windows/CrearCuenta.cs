﻿using System;
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
        List<Tuple<TextBox, Label>> requeridos = new List<Tuple<TextBox, Label>>();
        public CrearCuenta()
        {
            this.StartPosition = FormStartPosition.CenterScreen;
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

        private void BtnCrearCuenta_Click(object sender, EventArgs eventArgs)
        {
            bool error = false;
            requeridos.ForEach(delegate (Tuple<TextBox, Label> x)
            {
                if (String.IsNullOrEmpty(x.Item1.Text))
                {
                    lblErrorCrearCuenta.Text = "Faltan datos de rellenar";
                    x.Item2.ForeColor = Color.Red;
                    error = true;
                }
                else
                    x.Item2.ForeColor = Color.Black;
                return;
            });
            if (error)
                return;
            if (!Helper.ValidarEmail(inputCorreo.Text))
            {
                lblErrorCrearCuenta.Text = "El email no es valido";
                return;
            }
            if (!validarContraseña())
            {
                lblErrorCrearCuenta.Text = "Las contraseñas no coinciden";
                return;
            }
            SQLHelper db = new SQLHelper();
            Dictionary<string, object> parametros = new Dictionary<string, object>();
            parametros.Add("@Nombre", inputNombre.Text);
            parametros.Add("@Apellidos", inputApellidos.Text);
            parametros.Add("@Correo", inputCorreo.Text);
            parametros.Add("@FechaDeNacimiento", inputNacimiento.Text);
            parametros.Add("@Contraseña", Helper.encprytPassword(inputContraseña.Text));
            try
            {
                db.Query("INSERT INTO Usuarios (Nombre,Contraseña,Apellidos,Correo) VALUES (@Nombre, @Contraseña, @Apellidos, @Correo)", parametros);
            }
            catch (System.Data.SqlClient.SqlException e)
            {
                if(e.Number == 2627)
                    lblErrorCrearCuenta.Text = "El nombre de usuario '" + inputNombre.Text + "' ya esta en uso. Utiliza otro nombre";
                return;
            }

            MessageBox.Show($"Usuario {inputNombre.Text} creado correctamente.\nAhora puede iniciar sesion con su usuario y contraseña", "Creacion Correcta", MessageBoxButtons.OK);
            this.Close();
            new Inicio().Show();
        }

        private void setRequeridos()
        {
            requeridos.Add(new Tuple<TextBox, Label>(inputNombre, lblNombre));
            requeridos.Add(new Tuple<TextBox, Label>(inputCorreo, lblCorreo));
            requeridos.Add(new Tuple<TextBox, Label>(inputContraseña, lblContraseña));
            requeridos.Add(new Tuple<TextBox, Label>(inputContraseña2, lblContraseña2));
        }
        private bool validarContraseña()
        {
            return inputContraseña.Text == inputContraseña2.Text;
        }

    }
}
