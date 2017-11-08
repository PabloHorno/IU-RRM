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
        Paciente paciente = new Paciente();
        public SeleccionDePaciente(int sqlId)
        {
            this.StartPosition = FormStartPosition.CenterScreen;
            usuario = new SQLHelper().GetUsuario(sqlId);
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
            this.lblNombreCuenta.Text = usuario.Nombre;

            this.comboBoxAnguloAperturaCompleta.SelectedIndex = 0;
            this.comboBoxAnguloCierreCompleta.SelectedIndex = 0;
            this.comboBoxSeleccionTerapia.SelectedIndex = 0;
            this.comboBoxTiempoVelocidadAperturaCompleta.SelectedIndex = 0;
            this.comboBoxTiempoVelocidadCierreCompleta.SelectedIndex = 0;
            string str="";

            var fields = typeof(SeleccionDePaciente).GetFields( 
                                                                System.Reflection.BindingFlags.Public | 
                                                                System.Reflection.BindingFlags.NonPublic |
                                                                System.Reflection.BindingFlags.Instance);

            var names = Array.ConvertAll(fields, field => field.Name);

            MessageBox.Show(str);
            /*
            foreach (var prop in this.GetType().GetFields(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance))
            {
                if (prop.GetType() == typeof(ComboBox))
                {
                    foreach (var value in prop.GetType().GetProperties())
                        if (value.Name == "SelectedIndex")
                            value.SetValue(prop, 0);
                }
                MessageBox.Show(prop.Name);
            }*/

            this.groupBoxDatosPaciente.Hide();
        }

        private void TreeView1_NodeMouseClick1(object sender, TreeNodeMouseClickEventArgs e)
        {
            int SqlId;
            if (e.Node.Nodes.Count == 0 && int.TryParse(e.Node.Name, out SqlId))
            {
                List<Paciente> pacientes = new SQLHelper().GetPacientesFromUser(usuario.SqlId);
                if (pacientes.Count > 0)
                {
                    paciente = pacientes.Find(x => x.SqlId.ToString() == e.Node.Name);
                    mostrarDatosPaciente(paciente);
                    this.groupBoxDatosPaciente.Show();
                }
            }

        }

        private void TreeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Text.Contains("Añadir Paciente"))
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

        private void mostrarDatosPaciente(Paciente paciente)
        {
            groupBoxDatosPaciente.Text = (paciente.Nombre + " " + paciente.Apellidos).ToUpper();

            
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

        private void label2_Click_1(object sender, EventArgs e)
        {

        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {

        }

        private void lblNombreCuenta_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click_2(object sender, EventArgs e)
        {

        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
