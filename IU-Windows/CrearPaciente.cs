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
        List<Tuple<object,Label>> requeridos = new List<Tuple<object,Label>>();
        public CrearPaciente()
        {
            this.StartPosition = FormStartPosition.CenterScreen;
            InitializeComponent();
            setRequeridos();
            this.comboBoxResponsable.Items.Add("Seleccionar");
            this.comboBoxResponsable.Items.AddRange(new SQLHelper().Select("SELECT Nombre FROM Usuarios ORDER BY Nombre").ToArray());
            this.comboBoxResponsable.SelectedIndex = 0;
        }

        private void cerrar(object sender, EventArgs e)
        {
            this.Close();
        }

        private void acercaDeToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Constants.AcercaDe();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            bool error = false;
            foreach(var x in requeridos)
            {
                if (x.Item1.GetType() == typeof(ComboBox) && x.Item1.GetType() == typeof(TextBox) && String.IsNullOrEmpty((x.Item1 as ComboBox).Text)) 
                {
                    error = true;
                    x.Item2.ForeColor = Color.Red;
                }
                else if (x.Item1.GetType() == typeof(TextBox) && x.Item1.GetType() == typeof(TextBox) && String.IsNullOrEmpty((x.Item1 as TextBox).Text))
                {
                    error = true;
                    x.Item2.ForeColor = Color.Red;
                }
                else if (x.Item1.GetType() == typeof(ComboBox) && x.Item1.GetType() == typeof(ComboBox) && (x.Item1 as ComboBox).Text.Contains("Seleccionar"))
                {
                    error = true;
                    x.Item2.ForeColor = Color.Red;
                }
            }
            if(!error)
            {
                SQLHelper db = new SQLHelper();
                Paciente paciente = new Paciente()
                {
                    Nombre = textBoxNombre.Text,
                    Apellidos = textBoxApellidos.Text,
                    FechaDeNacimiento = Convert.ToDateTime(dateFechaDeNacimiento.Text),
                    Responsable = new SQLHelper().GetUsuario(comboBoxResponsable.Text).SqlId
                };
                db.InsertarPaciente(paciente);
                this.Close();
                MessageBox.Show($"Paciente {paciente.Nombre} {paciente.Apellidos} agregado correctamente");
            }
        }
        private void setRequeridos()
        {
            requeridos.Add(new Tuple<object, Label>(textBoxNombre, lblNombre));
            requeridos.Add(new Tuple<object, Label>(textBoxApellidos, lblApellidos));
            requeridos.Add(new Tuple<object, Label>(comboBoxResponsable, lblResponasble));
        }
    }
}
