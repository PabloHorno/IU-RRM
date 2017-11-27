using System;
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
                                obj.Value = ultimaTerapia.Parametros[obj.Name];
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
        }

        private void HandlerHabilitarInicioTerapia(object sender, System.EventArgs e)
        {
            TipoTerapia tipoTerapia = (TipoTerapia)this.comboBoxSeleccionTerapia.SelectedIndex;
            bool enable = true;

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

        private void TreeView1_NodeMouseClick1(object sender, TreeNodeMouseClickEventArgs e)
        {
            int SqlId;
            if(e.Button == MouseButtons.Left)
            {

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
            else if(e.Button == MouseButtons.Right)
            {
                contextMenuStrip1.Show(e.Location);
            }

        }

        private void TreeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Text.Contains("Añadir Paciente"))
            {
                CrearPaciente crearPaciente = new CrearPaciente();
                crearPaciente.Show();
            }
        }

        private void CargarDatos(int sqlId)
        {
            treeView1.BeginUpdate();
            treeView1.Nodes.Add("Pacientes");
            List<Paciente> pacientes = usuario.GetPacientes();
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


        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.tabControlTerapias.SelectedIndex = this.comboBoxSeleccionTerapia.SelectedIndex;
        }

        private void btnGuardarTerapia_Click(object sender, EventArgs e)
        {
            Dictionary<string, decimal> parametros = new Dictionary<string, decimal>();
            parametros.Add("tipoTerapia", comboBoxSeleccionTerapia.SelectedIndex);
            foreach (var field in this.GetType().GetFields(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance))
            {

                if (field.GetValue(this).GetType() == typeof(NumericUpDown))
                {
                    NumericUpDown obj = (NumericUpDown)field.GetValue(this);
                    if(obj.AccessibleDescription!= null && (TipoTerapia)Int32.Parse(obj.AccessibleDescription) == (TipoTerapia)comboBoxSeleccionTerapia.SelectedIndex)
                        parametros.Add(obj.Name, obj.Value);
                }
            }
            MessageBox.Show(JsonConvert.SerializeObject(parametros));
        }

        private void tabPage3_Click(object sender, EventArgs e)
        {

        }

        private void btnActualizarObservaciones_Click(object sender, EventArgs e)
        {
            paciente.Observaciones = this.richTextBoxObservaciones.Text;
            this.btnActualizarObservaciones.Enabled = false;
        }

        private void btnIniciarTerapia_Click(object sender, EventArgs e)
        {
            this.subprocesoTerapia.RunWorkerAsync();
            DateTime tIni = DateTime.Now;
            this.lblComienzoTerapia.Text = DateTime.Now.ToShortTimeString();
            this.btnIniciarTerapia.Enabled = false;
            this.progressBar1.Style = ProgressBarStyle.Continuous;
            tiempoTranscurridoTerapia.Start();
        }
        private void Thread_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.lblTiempoTranscurridoTerapia.Text = tiempoTranscurridoTerapia.Elapsed.ToString(@"mm\:ss");
            this.progressBar1.Value = e.ProgressPercentage > 100 ? 100: e.ProgressPercentage;
        }
        private void Thread_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.btnIniciarTerapia.Enabled = true;
            this.progressBar1.Style = ProgressBarStyle.Marquee;
            tiempoTranscurridoTerapia.Reset();
        }
        private void Thread_DoWork(object sender, DoWorkEventArgs e)
        {
            string portName = Helper.GetRRMSerialPort();
            if (portName != null)
            {
                MessageBox.Show($"RRM encontrado en el puerto {portName}");
                RobotRehabilitacionMano RRM = new RobotRehabilitacionMano(portName);

                Stopwatch marcaTiempo = Stopwatch.StartNew();
                while (marcaTiempo.ElapsedMilliseconds <= 10000)
                {
                    System.Threading.Thread.Sleep(100);
                    subprocesoTerapia.ReportProgress((Int32)(marcaTiempo.ElapsedMilliseconds / 100));
                }
                subprocesoTerapia.ReportProgress(100);
            }
        }
        
    }
}
