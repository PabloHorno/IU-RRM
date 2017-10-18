using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO.Ports;
using System.Timers;
namespace IU_Windows
{
    public partial class Inicio : Form
    {
        Process p = new Process();
        public Inicio()
        {
            InitializeComponent();
            button1.Click += Button1_Click;
            toolStripMenuItem1.Click += ToolStripMenuItem1_Click;
            toolStripMenuItem2.Click += ToolStripMenuItem2_Click;
            label1.Click += Label1_Click;
            linkLabel1.Click += LinkLabel1_Click;
        }

        private void LinkLabel1_Click(object sender, EventArgs e)
        {
        }

        private void ToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            toolStripDropDownButton1.Text = "COM2";
        }

        private void Label1_Click(object sender, EventArgs e)
        {
            toolStripMenuItem1.Text = "";
        }

        private void ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            toolStripDropDownButton1.Text = "COM1";
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (ProcessHelper.IsProcessRunning(p))
            {
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.FileName = "C:\\Users\\joseangel\\source\\repos\\IU-RRM\\WindowsGame2\\WindowsGame2\\bin\\x86\\Debug\\WindowsGame2.exe";
                p.StartInfo.CreateNoWindow = true;
                p.Start();
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {

        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            MessageBox.Show("Error faltan de rellenar datos","Error", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Error);
        }
    }
    public static class ProcessHelper
    {
        public static bool IsProcessRunning(Process p)
        {
            try
            {
                Process.GetProcessById(p.Id);
            }
            catch (InvalidOperationException)
            {
                return false;
            }
            return true;
        }
    }
}
