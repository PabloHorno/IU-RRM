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
    public partial class Form1 : Form
    {
        Process p = new Process();
        public Form1()
        {
            InitializeComponent();
            button1.Click += Button1_Click;
            button2.Click += Button2_Click;
            toolStripMenuItem1.Click += ToolStripMenuItem1_Click;
            toolStripMenuItem2.Click += ToolStripMenuItem2_Click;
            label1.Click += Label1_Click;
            
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

        private void Button2_Click(object sender, EventArgs e)
        {
            if (ProcessHelper.IsProcessRunning(p))
            {
                p.CloseMainWindow();
                p.Close();
            }
            else
                this.Close();
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
