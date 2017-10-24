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
            treeView1.DoubleClick += TreeView1_DoubleClick;
        }

        private void TreeView1_DoubleClick(object sender, EventArgs e)
        {
            MessageBox.Show(e.ToString());
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

            treeView1.BeginUpdate();
            treeView1.Nodes.Add("Pacientes");
            treeView1.Nodes[0].Nodes.Add("Paciente 1");
            treeView1.Nodes[0].Nodes.Add("Paciente 2");
            treeView1.Nodes[0].Nodes.Add("Paciente 3");
            treeView1.Nodes[0].Nodes.Add("Paciente 4");
            treeView1.Nodes[0].Nodes.Add("Paciente 5");
            treeView1.Nodes[0].Nodes.Add("Paciente 6");
            treeView1.Nodes[0].Nodes.Add("Paciente 7");
            treeView1.Nodes.Add("Añadir Paciente");
            treeView1.EndUpdate();
        }
    }
}
