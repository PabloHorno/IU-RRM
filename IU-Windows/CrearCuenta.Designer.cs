namespace IU_Windows
{
    partial class CrearCuenta
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.lblNombre = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.lblApellidos = new System.Windows.Forms.Label();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.lblCorreo = new System.Windows.Forms.Label();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.lblContraseña = new System.Windows.Forms.Label();
            this.lblContraseña2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(12, 43);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(139, 20);
            this.textBox1.TabIndex = 0;
            // 
            // lblNombre
            // 
            this.lblNombre.AutoSize = true;
            this.lblNombre.Location = new System.Drawing.Point(12, 27);
            this.lblNombre.Name = "lblNombre";
            this.lblNombre.Size = new System.Drawing.Size(44, 13);
            this.lblNombre.TabIndex = 3;
            this.lblNombre.Text = "Nombre";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(12, 92);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(139, 20);
            this.textBox2.TabIndex = 4;
            // 
            // lblApellidos
            // 
            this.lblApellidos.AutoSize = true;
            this.lblApellidos.Location = new System.Drawing.Point(9, 76);
            this.lblApellidos.Name = "lblApellidos";
            this.lblApellidos.Size = new System.Drawing.Size(49, 13);
            this.lblApellidos.TabIndex = 5;
            this.lblApellidos.Text = "Apellidos";
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(12, 138);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(139, 20);
            this.textBox3.TabIndex = 6;
            // 
            // lblCorreo
            // 
            this.lblCorreo.AutoSize = true;
            this.lblCorreo.Location = new System.Drawing.Point(12, 122);
            this.lblCorreo.Name = "lblCorreo";
            this.lblCorreo.Size = new System.Drawing.Size(38, 13);
            this.lblCorreo.TabIndex = 7;
            this.lblCorreo.Text = "Correo";
            this.lblCorreo.Click += new System.EventHandler(this.label1_Click);
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(227, 43);
            this.textBox4.Name = "textBox4";
            this.textBox4.PasswordChar = '•';
            this.textBox4.Size = new System.Drawing.Size(139, 20);
            this.textBox4.TabIndex = 8;
            // 
            // textBox5
            // 
            this.textBox5.Location = new System.Drawing.Point(227, 92);
            this.textBox5.Name = "textBox5";
            this.textBox5.PasswordChar = '•';
            this.textBox5.Size = new System.Drawing.Size(139, 20);
            this.textBox5.TabIndex = 9;
            // 
            // lblContraseña
            // 
            this.lblContraseña.AutoSize = true;
            this.lblContraseña.Location = new System.Drawing.Point(224, 27);
            this.lblContraseña.Name = "lblContraseña";
            this.lblContraseña.Size = new System.Drawing.Size(61, 13);
            this.lblContraseña.TabIndex = 10;
            this.lblContraseña.Text = "Contraseña";
            // 
            // lblContraseña2
            // 
            this.lblContraseña2.AutoSize = true;
            this.lblContraseña2.Location = new System.Drawing.Point(224, 76);
            this.lblContraseña2.Name = "lblContraseña2";
            this.lblContraseña2.Size = new System.Drawing.Size(98, 13);
            this.lblContraseña2.TabIndex = 11;
            this.lblContraseña2.Text = "Repetir Contraseña";
            // 
            // CrearCuenta
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(534, 312);
            this.Controls.Add(this.lblContraseña2);
            this.Controls.Add(this.lblContraseña);
            this.Controls.Add(this.textBox5);
            this.Controls.Add(this.textBox4);
            this.Controls.Add(this.lblCorreo);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.lblApellidos);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.lblNombre);
            this.Controls.Add(this.textBox1);
            this.Name = "CrearCuenta";
            this.Text = "CrearCuenta";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label lblNombre;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label lblApellidos;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Label lblCorreo;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.Label lblContraseña;
        private System.Windows.Forms.Label lblContraseña2;
    }
}