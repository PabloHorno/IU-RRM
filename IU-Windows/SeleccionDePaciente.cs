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
    public partial class SeleccionDePaciente : Form
    {
        public SeleccionDePaciente(int sqlId)
        {
            InitializeComponent();
            CargarDatos(sqlId);
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
        private void CargarDatos(int sqlId)
        {
            SQLHelper db = new SQLHelper();
            var usurario = db.GetUsuario(sqlId);
            lblNombre.Text = usurario.Nombre;
            lblApellidos.Text = usurario.Apellido;
            lblCorreo.Text = usurario.SqlId.ToString();
        }
    }
}
