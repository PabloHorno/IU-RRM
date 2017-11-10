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

        private void HandlerComboBoxTipoParametros(object sender, EventArgs e)
        {
            if (((ComboBox)sender).Name.Contains("comboBoxTipoParametros"))
            {
                string name = ((ComboBox)sender).Name.Replace("comboBoxTipoParametros", "");
                List<string> campos = new List<string> { "numericUpDownAngulo", "numericUpDownTiempo", "numericUpDownVelocidad" };

                //object second = this.GetType().GetField("numericUpDownTiempoCierrePulgar", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(this); 
                foreach (string campo in campos)
                {
                    try
                    {
                        var obj = (this.GetType().GetField(campo + name, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(this) as NumericUpDown);
                        obj.Enabled = ((sender as ComboBox).SelectedIndex == 0);

                        if((sender as ComboBox).SelectedIndex == 1)
                        {
                            obj.Value = Helper.parametrosPorDefecto[obj.Name];
                        }
                        else if((sender as ComboBox).SelectedIndex == 2)
                        {
                            Terapia ultimaTerapia = paciente.GetTerapias().Find(x => (int)x.tipoTerapia == this.comboBoxSeleccionTerapia.SelectedIndex);
                            if(ultimaTerapia == null)
                            {
                                MessageBox.Show($"El paciente {paciente.Nombre.ToUpper()} {paciente.Apellidos.ToUpper()} no ha realizado ninguna terapia del tipo: " + (TipoTerapia)this.comboBoxSeleccionTerapia.SelectedIndex, "Falta terapia", MessageBoxButtons.OK,MessageBoxIcon.Stop);
                                (sender as ComboBox).SelectedIndex = 1;
                                return;
                            }
                        }
                    }
                    catch
                    {
                    }
                }
            }
        }


        private void Inicializacion()
        {
            this.tabControlTerapias.Appearance = TabAppearance.FlatButtons;
            this.tabControlTerapias.ItemSize = new Size(0, 1);
            this.tabControlTerapias.SizeMode = TabSizeMode.Fixed;
            this.lblNombreCuenta.Text = usuario.Nombre;

            this.comboBoxTipoParametrosAperturaIndice.SelectedIndexChanged += HandlerComboBoxTipoParametros;
            this.comboBoxTipoParametrosCierreIndice.SelectedIndexChanged += HandlerComboBoxTipoParametros;
            this.comboBoxTipoParametrosAperturaCorazon.SelectedIndexChanged += HandlerComboBoxTipoParametros;
            this.comboBoxTipoParametrosCierreCorazon.SelectedIndexChanged += HandlerComboBoxTipoParametros;
            this.comboBoxTipoParametrosAperturaAnular.SelectedIndexChanged += HandlerComboBoxTipoParametros;
            this.comboBoxTipoParametrosCierreAnular.SelectedIndexChanged += HandlerComboBoxTipoParametros;
            this.comboBoxTipoParametrosAperturaMeñique.SelectedIndexChanged += HandlerComboBoxTipoParametros;
            this.comboBoxTipoParametrosCierreMeñique.SelectedIndexChanged += HandlerComboBoxTipoParametros;
            this.comboBoxTipoParametrosAperturaPulgar.SelectedIndexChanged += HandlerComboBoxTipoParametros;
            this.comboBoxTipoParametrosCierrePulgar.SelectedIndexChanged += HandlerComboBoxTipoParametros;
            this.comboBoxTipoParametrosAperturaPinza.SelectedIndexChanged += HandlerComboBoxTipoParametros;
            this.comboBoxTipoParametrosCierrePinza.SelectedIndexChanged += HandlerComboBoxTipoParametros;
            this.comboBoxTipoParametrosAperturaCompleta.SelectedIndexChanged += HandlerComboBoxTipoParametros;
            this.comboBoxTipoParametrosCierreCompleta.SelectedIndexChanged += HandlerComboBoxTipoParametros;

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
            groupBoxDatosPaciente.Text = ("[ " + paciente.SqlId + " ]: " + paciente.Nombre + " " + paciente.Apellidos).ToUpper();
            this.listViewHistorialTerapias.Items.Clear();
            foreach (Terapia terapia in paciente.GetTerapias())
            {
                ListViewItem listViewItem = new ListViewItem();
                listViewItem.SubItems.Add(terapia.Nombre);
                listViewItem.SubItems.Add(terapia.TiempoApertura.ToShortTimeString());
                listViewItem.SubItems.Add(terapia.Repeticiones.ToString());
                listViewItem.SubItems.Add(terapia.Observacion);
                this.listViewHistorialTerapias.Items.Add(listViewItem);
            }
            if (this.listViewHistorialTerapias.Items.Count > 0)
            {
                this.listViewHistorialTerapias.Items[0].Selected = true;
                this.listViewHistorialTerapias.Items[0].Focused = true;
            }
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
