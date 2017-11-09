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

           /* this.comboBoxAnguloAperturaCompleta.SelectedIndexChanged += ComboBoxAnguloAperturaCompleta_SelectedIndexChanged;
            this.comboBoxAnguloCierreCompleta.SelectedIndex = 0;
            this.comboBoxSeleccionTerapia.SelectedIndex = 0;
            this.comboBoxTiempoVelocidadAperturaCompleta.SelectedIndex = 0;
            this.comboBoxTiempoVelocidadCierreCompleta.SelectedIndex = 0;
            this.comboBoxAnguloAperturaAnular.SelectedIndex = 0;
            this.comboBoxAnguloAperturaCompleta.SelectedIndex = 0;
            this.comboBoxAnguloAperturaCorazon.SelectedIndex = 0;
            this.comboBoxAnguloAperturaIndice.SelectedIndex = 0;
            this.comboBoxAnguloAperturaMeñique.SelectedIndex = 0;
            this.comboBoxAnguloAperturaPulgar.SelectedIndex = 0;
            this.comboBoxAnguloCierreAnular.SelectedIndex = 0;
            this.comboBoxAnguloCierreCompleta.SelectedIndex = 0;
            this.comboBoxAnguloCierreCorazon.SelectedIndex = 0;
            this.comboBoxAnguloCierreIndice.SelectedIndex = 0;
            this.comboBoxAnguloCierreMeñique.SelectedIndex = 0;
            this.comboBoxAnguloCierrePulgar.SelectedIndex = 0;
            this.comboBoxTeimpoVelocidadAperturaIndice.SelectedIndex = 0;
            this.comboBoxTiempoVelocidadAperturaAnular.SelectedIndex = 0;
            this.comboBoxTiempoVelocidadAperturaCompleta.SelectedIndex = 0;
            this.comboBoxTiempoVelocidadAperturaCorazon.SelectedIndex = 0;
            this.comboBoxTiempoVelocidadAperturaMeñique.SelectedIndex = 0;
            this.comboBoxTiempoVelocidadAperturaPulgar.SelectedIndex = 0;
            this.comboBoxTiempoVelocidadCierreAnular.SelectedIndex = 0;
            this.comboBoxTiempoVelocidadCierreCompleta.SelectedIndex = 0;
            this.comboBoxTiempoVelocidadCierreCorazon.SelectedIndex = 0;
            this.comboBoxTiempoVelocidadCierreIndice.SelectedIndex = 0;
            this.comboBoxTiempoVelocidadCierreMeñique.SelectedIndex = 0;
            this.comboBoxTiempoVelocidadCierrePulgar.SelectedIndex = 0;*/
            this.comboBoxTipoParametrosCierrePulgar.SelectedIndexChanged += ComboBoxTiempoVelocidadCierrePulgar_SelectedIndexChanged;
        }

        private void ComboBoxAnguloAperturaCompleta_SelectedIndexChanged(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void ComboBoxTiempoVelocidadCierrePulgar_SelectedIndexChanged(object sender, EventArgs e)
        {
            string name = ((ComboBox)sender).Name.Replace("comboBoxTipoParametros", "");

            if ((sender as ComboBox).SelectedIndex == 0)
            {
                //Enable
                //object second = this.GetType().GetField("numericUpDownTiempoCierrePulgar", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(this); 
                (this.GetType().GetField("numericUpDownAngulo"+ name, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(this) as NumericUpDown).Enabled = true;
                (this.GetType().GetField("numericUpDownTiempo"+ name, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(this) as NumericUpDown).Enabled = true;
                (this.GetType().GetField("numericUpDownVeclocidad"+ name, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(this) as NumericUpDown).Enabled = true;


            }
            else
            {
                (this.GetType().GetField("numericUpDownAngulo" + name, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(this) as NumericUpDown).Enabled = false;
                (this.GetType().GetField("numericUpDownTiempo" + name, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(this) as NumericUpDown).Enabled = false;
                (this.GetType().GetField("numericUpDownVeclocidad" + name, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(this) as NumericUpDown).Enabled = false;
            }

          
        }

        private void Inicializacion()
        {
            this.tabControlTerapias.Appearance = TabAppearance.FlatButtons;
            this.tabControlTerapias.ItemSize = new Size(0, 1);
            this.tabControlTerapias.SizeMode = TabSizeMode.Fixed;
            this.lblNombreCuenta.Text = usuario.Nombre;

            foreach (var prop in this.GetType().GetFields(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance))
            {
                object obj = prop.GetValue(this);
                if (obj.GetType() == typeof(ComboBox))
                    (obj as ComboBox).SelectedIndex = 0;
            }

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

        private void numericUpDown5_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}
