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
        Usuario usuario = new Usuario();
        public SeleccionDePaciente(int sqlId)
        {
            this.StartPosition = FormStartPosition.CenterScreen;
            InitializeComponent();
            Inicializacion();
            CargarDatos(sqlId);
            treeView1.NodeMouseDoubleClick += TreeView1_NodeMouseDoubleClick;
            treeView1.NodeMouseClick += TreeView1_NodeMouseClick1;
        }

        private void Inicializacion()
        {
            this.tabControlTerapias.Appearance = TabAppearance.FlatButtons;
            this.tabControlTerapias.ItemSize = new Size(0, 1);
            this.tabControlTerapias.SizeMode = TabSizeMode.Fixed;
            this.comboBoxSeleccionTerapia.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void TreeView1_NodeMouseClick1(object sender, TreeNodeMouseClickEventArgs e)
        {
            int SqlId;
            if (e.Node.Nodes.Count > 0)
            {
                MessageBox.Show("Es un padre puesto que tiene hijos");
            }
            else if (int.TryParse(e.Node.Name, out SqlId))
            {
                List<Paciente> pacientes = new SQLHelper().GetPacientesFromUser(usuario.SqlId);
                if (pacientes.Count > 0)
                {
                    Paciente paciente = pacientes.Find(x => x.SqlId.ToString() == e.Node.Name);
                    lblNombre.Text = paciente.Nombre + " " + paciente.Apellidos;
                    lblCorreo.Text = paciente.SqlId.ToString();
                }
            }
        }

        private void TreeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if(e.Node.Text.Contains("Añadir Paciente"))
            {
                CrearPaciente crearPaciente = new CrearPaciente();
                crearPaciente.Show();
            }
            else
                MessageBox.Show("Has hecho click sobre " + e.Node.Text + "Con SqlId " + e.Node.Name);
        }


        private void label1_Click(object sender, EventArgs e)
        {

        }
        private void CargarDatos(int sqlId)
        {
            SQLHelper db = new SQLHelper();
            usuario = db.GetUsuario(sqlId);

            treeView1.BeginUpdate();
            treeView1.Nodes.Add("Pacientes");
            List<Paciente> pacientes = new SQLHelper().GetPacientesFromUser(sqlId);
            foreach (Paciente paciente in pacientes)
            {
                treeView1.Nodes[0].Nodes.Add(paciente.SqlId.ToString(), paciente.Nombre + " " + paciente.Apellidos);
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
            Constants.AcercaDe();
        }

        private void nuevoPacienteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CrearPaciente crearPaciente = new CrearPaciente();
            crearPaciente.Show();
        }

        private void lblApellidos_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }

        private void label1_Click_2(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.tabControlTerapias.SelectedIndex = this.comboBoxSeleccionTerapia.SelectedIndex;
        }

        private void tabPage3_Click(object sender, EventArgs e)
        {

        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }
    }
}
