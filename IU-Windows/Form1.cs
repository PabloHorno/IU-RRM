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
