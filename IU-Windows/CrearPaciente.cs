using System;
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
            InitializeComponent();
            this.comboBox1.Items.AddRange(new SQLHelper().Select("SELECT Nombre FROM Usuarios").ToArray());
        }

        private void cerrar(object sender, EventArgs e)
        {
            this.Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
