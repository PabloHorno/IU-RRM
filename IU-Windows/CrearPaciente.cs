﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IU_Windows
{
    public partial class CrearPaciente : Form
    {
        public CrearPaciente()
        {
            this.StartPosition = FormStartPosition.CenterScreen;
            InitializeComponent();
            this.comboBoxResponsable.Items.AddRange(new SQLHelper().Select("SELECT Nombre FROM Usuarios").ToArray());
        }

        private void cerrar(object sender, EventArgs e)
        {
            this.Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {


        }

        private void acercaDeToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Constants.AcercaDe();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
