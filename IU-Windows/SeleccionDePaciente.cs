﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace IU_Windows
{
    public partial class SeleccionDePaciente : Form
    {
        Usuario usuario = new Usuario();
        Paciente paciente = new Paciente();
        BackgroundWorker subprocesoTerapia = new BackgroundWorker();
        Stopwatch tiempoTranscurridoTerapia = new Stopwatch();
        public SeleccionDePaciente(int sqlId)
        {
            this.StartPosition = FormStartPosition.CenterScreen;
            usuario = new SQLHelper().GetUsuario(sqlId);
            InitializeComponent();
            Inicializacion();
            CargarListaPacientes(sqlId);
            treeViewPacientes.NodeMouseDoubleClick += treeViewPacientesDobleClick;
            treeViewPacientes.NodeMouseClick += treeViewPacientesClick;

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

                        if ((sender as ComboBox).SelectedIndex == 1)
                        {
                            obj.Value = Helper.parametrosPorDefecto[obj.Name];
                        }
                        else if ((sender as ComboBox).SelectedIndex == 2)
                        {
                            Terapia ultimaTerapia = paciente.GetTerapias().Find(x => (int)x.tipoTerapia == this.comboBoxSeleccionTerapia.SelectedIndex);
                            if (ultimaTerapia == null)
                            {
                                MessageBox.Show($"El paciente {paciente.Nombre.ToUpper()} {paciente.Apellidos.ToUpper()} no ha realizado ninguna terapia del tipo: " + (TipoTerapia)this.comboBoxSeleccionTerapia.SelectedIndex + " Asignamos parametros por defecto", "Falta terapia", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                (sender as ComboBox).SelectedIndex = 1;
                                return;
                            }
                            else
                            {
                                string str = obj.Name
                                                .Replace("numericUpDown", "")
                                                .Replace("Apertura", "A")
                                                .Replace("Cierre", "C")
                                                .Replace("Angulo", "A")
                                                .Replace("Velocidad", "V")
                                                .Replace("Tiempo", "T")
                                                .Replace("Pulgar", "P")
                                                .Replace("Indice", "I")
                                                .Replace("Corazon", "C")
                                                .Replace("Anular", "A")
                                                .Replace("Meñique", "M"); //Acortamiento para que el buffer del arduino pueda manejar toda la informacion

                                obj.Value = ultimaTerapia.Parametros[str];
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
            this.comboBoxSeleccionTerapia.SelectedIndexChanged += HandlerHabilitarInicioTerapia;

            foreach (var prop in this.GetType().GetFields(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance))
            {
                object obj = prop.GetValue(this);
                if (obj == null)
                    continue;
                if (obj.GetType() == typeof(ComboBox))
                {
                    (obj as ComboBox).SelectedIndex = 0;
                    if ((obj as ComboBox).Name.Contains("Parametros"))
                        (obj as ComboBox).SelectedIndexChanged += HandlerComboBoxTipoParametros;
                }
                else if (obj.GetType() == typeof(NumericUpDown))
                {
                    (obj as NumericUpDown).ValueChanged += HandlerHabilitarInicioTerapia;
                }
            }
            this.richTextBoxObservaciones.TextChanged += RichTextBoxObservaciones_TextChanged;
            this.subprocesoTerapia.DoWork += Thread_DoWork;
            this.subprocesoTerapia.ProgressChanged += Thread_ProgressChanged;
            this.subprocesoTerapia.RunWorkerCompleted += Thread_RunWorkerCompleted;
            this.subprocesoTerapia.WorkerReportsProgress = true;
            this.groupBoxDatosPaciente.Hide();
            this.textBoxBusqueda.TextChanged += TextBoxBusqueda_TextChanged;

        }

        private void TextBoxBusqueda_TextChanged(object sender, EventArgs e)
        {
            CargarListaPacientes();
        }

        private void HandlerHabilitarInicioTerapia(object sender, System.EventArgs e)
        {
            TipoTerapia tipoTerapia = (TipoTerapia)this.comboBoxSeleccionTerapia.SelectedIndex;
            bool enable = true;

            if (numRepeticiones.Value == 0)
                enable = false;

            foreach (var prop in this.GetType().GetFields(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance))
            {
                object obj = prop.GetValue(this);
                if (obj == null)
                    continue;
                if (obj.GetType() == typeof(NumericUpDown))
                    switch (tipoTerapia)
                    {
                        case TipoTerapia.AbrirCerrarDedos:
                            String[] dedosStr = { "Pulgar", "Indice", "Corazon", "Anular", "Meñique" };
                            if (dedosStr.Any((obj as NumericUpDown).Name.Contains))
                            {
                                if ((obj as NumericUpDown).Value == 0)
                                    enable = false;
                            }
                            break;
                        case TipoTerapia.AbrirCerrarMano:
                            if ((obj as NumericUpDown).Name.Contains("Completa"))
                            {
                                if ((obj as NumericUpDown).Value == 0)
                                    enable = false;
                            }
                            break;
                        case TipoTerapia.PinzaFina:
                        case TipoTerapia.PinzaGruesa:
                            if ((obj as NumericUpDown).Name.Contains("Pinza"))
                            {
                                if ((obj as NumericUpDown).Value == 0)
                                    enable = false;
                            }
                            break;
                        default:
                            enable = false;
                            break;
                    }

            }
            this.groupBoxEjecucionTerapia.Enabled = enable;
        }
        private void RichTextBoxObservaciones_TextChanged(object sender, System.EventArgs e)
        {
            this.btnActualizarObservaciones.Enabled = true;
        }

        private void treeViewPacientesClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            int SqlId;

            if (e.Node.Nodes.Count == 0 && int.TryParse(e.Node.Name, out SqlId))
            {
                List<Paciente> pacientes = new SQLHelper().GetPacientesFromUser(usuario.SqlId);
                if (pacientes.Count > 0)
                {
                    paciente = pacientes.Find(x => x.SqlId.ToString() == e.Node.Name);
                    mostrarDatosPaciente(paciente);
                    if (e.Button == MouseButtons.Right)
                    {
                        contextMenuStripPaciente.Items[0].Text = paciente.Nombre + " " + paciente.Apellidos;
                        contextMenuStripPaciente.Show(treeViewPacientes, e.X, e.Y);
                        eliminarPacienteToolStripMenuItem.AccessibleDescription = SqlId.ToString();
                    }
                    this.groupBoxDatosPaciente.Show();
                }
            }
        }

        private void treeViewPacientesDobleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Text.Contains("Añadir Paciente"))
            {
                crearPacienteForm(sender, e);
            }
        }

        private void CargarListaPacientes(int sqlId = -1)
        {
            if (sqlId == -1)
                sqlId = usuario.SqlId;

            treeViewPacientes.BeginUpdate();
            treeViewPacientes.Nodes.Clear();
            treeViewPacientes.Nodes.Add("Pacientes");
            List<Paciente> pacientes = usuario.GetPacientes();
            pacientes.Sort((x, y) => x.Nombre.CompareTo(y.Nombre));
            foreach (Paciente paciente in pacientes)
            {
                if (string.IsNullOrEmpty(textBoxBusqueda.Text)
                   || paciente.Nombre.IndexOf(textBoxBusqueda.Text, StringComparison.OrdinalIgnoreCase) >= 0
                   || paciente.Apellidos.IndexOf(textBoxBusqueda.Text, StringComparison.OrdinalIgnoreCase) >= 0)
                    treeViewPacientes.Nodes[0].Nodes.Add(paciente.SqlId.ToString(), paciente.Nombre + " " + paciente.Apellidos);
            }
            treeViewPacientes.Nodes.Add("Añadir Paciente");
            if (!string.IsNullOrEmpty(textBoxBusqueda.Text))
                treeViewPacientes.ExpandAll();
            treeViewPacientes.EndUpdate();
            treeViewPacientes.ExpandAll();
        }

        private void mostrarDatosPaciente(Paciente paciente)
        {
            groupBoxDatosPaciente.Text = (paciente.Nombre + " " + paciente.Apellidos).ToUpper();

            #region Inicio
            this.lblHorasTerapia.Text = paciente.GetHorasTerapias().ToString();
            var numTerapias = new SQLHelper().GetNumTerapiasRealizadasFromPaciente(paciente);
            this.lblNabrirCerrarDedos.Text = numTerapias[TipoTerapia.AbrirCerrarDedos].ToString();
            this.lblNabrirCerrarMano.Text = numTerapias[TipoTerapia.AbrirCerrarMano].ToString();
            this.lblNpinzaFina.Text = numTerapias[TipoTerapia.PinzaFina].ToString();
            this.lblNpinzaGruesa.Text = numTerapias[TipoTerapia.PinzaGruesa].ToString();
            this.lblTotalTerapiasRealizadas.Text = new SQLHelper().SelectScalar(
                "SELECT COUNT(*) FROM Terapias WHERE SqlIdPaciente = @SqlIdPaciente",
                new Dictionary<string, object>{
                    { "@SqlIdPaciente", paciente.SqlId }}
                ).ToString();
            this.richTextBoxObservaciones.Text = paciente.Observaciones;
            this.btnActualizarObservaciones.Enabled = false;

            #endregion
            #region Historial de Terapias
            this.listViewHistorialTerapias.Items.Clear();
            foreach (Terapia terapia in paciente.GetTerapias())
            {
                ListViewItem listViewItem = new ListViewItem();
                listViewItem.SubItems.Add(terapia.Nombre);
                listViewItem.SubItems.Add(terapia.Duracion.ToString());
                listViewItem.SubItems.Add(terapia.Repeticiones.ToString());
                listViewItem.SubItems.Add(terapia.Observaciones);
                listViewItem.SubItems.Add(terapia.Fecha.ToShortDateString());
                this.listViewHistorialTerapias.Items.Add(listViewItem);
            }
            if (this.listViewHistorialTerapias.Items.Count > 0)
            {
                this.listViewHistorialTerapias.Items[0].Selected = true;
                this.listViewHistorialTerapias.Items[0].Focused = true;
            }
            #endregion
            #region Nueva Terapia
            #endregion
            //Reiniciamos controles terapia al cambiar de Paciente
            foreach (var field in this.GetType().GetFields(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance))
            {
                if (field.GetValue(this) == null)
                    continue;
                if (field.GetValue(this).GetType() == typeof(NumericUpDown))
                    (field.GetValue(this) as NumericUpDown).Value = 0;
                else if (field.GetValue(this).GetType() == typeof(ComboBox))
                    (field.GetValue(this) as ComboBox).SelectedIndex = 0;
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
            Inicio inicio = new Inicio();
            inicio.Show();
        }

        private void acercaDeToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Constantes.AcercaDe();
        }

        private void crearPacienteForm(object sender, EventArgs e)
        {
            CrearPaciente crearPaciente = new CrearPaciente();
            crearPaciente.ShowDialog();
            CargarListaPacientes();
        }


        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.tabControlTerapias.SelectedIndex = this.comboBoxSeleccionTerapia.SelectedIndex;
        }

        Dictionary<string, decimal> GetParametros()
        {
            Dictionary<string, decimal> parametros = new Dictionary<string, decimal>();
            if ((TipoTerapia)comboBoxSeleccionTerapia.SelectedIndex == TipoTerapia.PinzaFina && radioButtonPinzaGruesa.Checked)
                parametros.Add("tipo", (decimal)TipoTerapia.PinzaGruesa);
            else
                parametros.Add("tipo", comboBoxSeleccionTerapia.SelectedIndex);
            parametros.Add("R", numRepeticiones.Value);
            foreach (var field in this.GetType().GetFields(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance))
            {

                if (field.GetValue(this).GetType() == typeof(NumericUpDown))
                {
                    NumericUpDown obj = (NumericUpDown)field.GetValue(this);
                    if (obj.AccessibleDescription != null && (TipoTerapia)Int32.Parse(obj.AccessibleDescription) == (TipoTerapia)comboBoxSeleccionTerapia.SelectedIndex)
                        parametros.Add(obj.Name
                                                .Replace("numericUpDown", "")
                                                .Replace("Apertura", "A")
                                                .Replace("Cierre", "C")
                                                .Replace("Angulo", "A")
                                                .Replace("Velocidad", "V")
                                                .Replace("Tiempo", "T")
                                                .Replace("Pulgar", "P")
                                                .Replace("Indice", "I")
                                                .Replace("Corazon", "C")
                                                .Replace("Anular", "A")
                                                .Replace("Meñique", "M")
                                                , obj.Value);
                }
            }
            return parametros;
        }

        private void btnActualizarObservaciones_Click(object sender, EventArgs e)
        {
            paciente.Observaciones = this.richTextBoxObservaciones.Text;
            this.btnActualizarObservaciones.Enabled = false;
        }

        private void btnIniciarTerapia_Click(object sender, EventArgs e)
        {
            //MessageBox.Show(JsonConvert.SerializeObject(GetParametros()));     //Mostrar parametros por dialogo (Debug)
            this.subprocesoTerapia.WorkerSupportsCancellation = true;
            this.subprocesoTerapia.RunWorkerAsync(GetParametros());
            DateTime tIni = DateTime.Now;
            this.lblComienzoTerapia.Text = DateTime.Now.ToShortTimeString();
            this.btnIniciarTerapia.Enabled = false;
            this.progressBar1.Style = ProgressBarStyle.Continuous;
            this.btnDetenerTerapia.Enabled = true;
            this.btnDetenerTerapia.BackColor = Color.Red;
            tiempoTranscurridoTerapia.Start();
        }
        private void btnDetenerTerapia_Click(object sender, EventArgs e)
        {
            subprocesoTerapia.CancelAsync();
        }
        private void Thread_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.lblTiempoTranscurridoTerapia.Text = tiempoTranscurridoTerapia.Elapsed.ToString(@"mm\:ss");
            this.progressBar1.Value = e.ProgressPercentage > 100 ? 100 : e.ProgressPercentage;
        }
        private void Thread_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled == false)
            {
                Terapia terapia = new Terapia {
                    PacienteSqlid = paciente.SqlId,
                    Duracion = tiempoTranscurridoTerapia.Elapsed,
                    Fecha = DateTime.Now,
                    Parametros = GetParametros(),
                    Repeticiones = (int)GetParametros()["R"],
                    tipoTerapia = (TipoTerapia)GetParametros()["tipo"],
                    Observaciones = ""
                 };

                paciente.GuardarTerapia(terapia);
                MessageBox.Show("Fin de Terapia y ha sido guardada");
            }
            else
            {
                MessageBox.Show("Terapia cancelada. No se guardarán datos.", "Terapia Detenida", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            tiempoTranscurridoTerapia.Reset();
            this.btnIniciarTerapia.Enabled = true;
            this.progressBar1.Style = ProgressBarStyle.Marquee;
            this.btnDetenerTerapia.Enabled = false;
            this.btnDetenerTerapia.BackColor = Color.Transparent;
            subprocesoTerapia.Dispose();
        }
        private void Thread_DoWork(object sender, DoWorkEventArgs e)
        {
            Dictionary<string, decimal> parametros = (Dictionary<string, decimal>)e.Argument;
            string portName = Helper.GetRRMSerialPort();
            if (string.IsNullOrEmpty(portName))
            {
                MessageBox.Show("No se ha podido encontrar el RRM.\n Por favor, verifique la conexion al puerto USB.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                e.Cancel = true;
                return;
            }
            else
            {
                int count = 1;
                RobotRehabilitacionMano RRM = new RobotRehabilitacionMano(portName, parametros);
                while (!RRM.finTerapia)
                {
                    if (subprocesoTerapia.CancellationPending)
                    {
                        RRM.Detener();
                        e.Cancel = true;
                        return;
                    }
                    else
                    {
                        //MessageBox.Show(count.ToString());
                        count++;
                        RRM.RealizarMovimiento();
                        subprocesoTerapia.ReportProgress(RRM.ProgresoRealizado());
                        Thread.Sleep(200);
                    }
                }
                RRM.puertoSerial.Close();
            }
        }

        private void eliminarPacienteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int SqlId = int.Parse((sender as ToolStripMenuItem).AccessibleDescription);
            Paciente paciente = new SQLHelper().GetPaciente(SqlId);
            var respuseta = MessageBox.Show($"¿Esta seguro de que desea eliminar al paciente {paciente.Nombre} {paciente.Apellidos} ?", "Eliminar Paciente", MessageBoxButtons.YesNo);
            if (respuseta == DialogResult.Yes)
            {
                new SQLHelper().EliminarPaciente(paciente);
                groupBoxDatosPaciente.Hide();
                CargarListaPacientes();
            }
        }
    }
}
