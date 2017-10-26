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
            treeView1.NodeMouseDoubleClick += TreeView1_NodeMouseDoubleClick;
            treeView1.NodeMouseClick += TreeView1_NodeMouseClick1;
        }

        private void TreeView1_NodeMouseClick1(object sender, TreeNodeMouseClickEventArgs e)
        {
            int SqlId;
            if(e.Node.Nodes.Count > 0)
            {
                MessageBox.Show("Es un padre puesto que tiene hijos");
            }
            else if (int.TryParse(e.Node.Name, out SqlId))
            {
                List<Paciente> paciente = new SQLHelper().GetPacientesFromUser(SqlId);
                if (paciente.Count > 0)
                {
                    lblNombre.Text = paciente[0].Nombre;
                    lblApellidos.Text = paciente[0].Apellidos;
                    lblCorreo.Text = paciente[0].SqlId.ToString();
                }
            }
        }

        private void TreeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            MessageBox.Show("Has hecho click sobre " + e.Node.Text + "Con SqlId " + e.Node.Name);
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
            List<Paciente> pacientes = new SQLHelper().GetPacientesFromUser(sqlId);
            foreach(Paciente paciente in pacientes)
            {
                treeView1.Nodes[0].Nodes.Add(paciente.SqlId.ToString(), paciente.Nombre+" "+paciente.Apellidos);
            }
            treeView1.Nodes.Add("Añadir Paciente");
            treeView1.EndUpdate();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Inicio inicio = new Inicio();
            inicio.Show();
        }

        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void acercaDeToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void acercaDeToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Este es un programa desarollado por\nPablo Horno Pérez", $"Version {Constants.version}",MessageBoxButtons.OK,MessageBoxIcon.Information);
        }

        private void nuevoPacienteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CrearPaciente crearPaciente = new CrearPaciente();
            crearPaciente.Show();
        }
    }
}
