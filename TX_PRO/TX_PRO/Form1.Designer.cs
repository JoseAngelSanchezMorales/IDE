namespace TX_PRO
{
    partial class Form1
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.txtBoxSimbolos = new System.Windows.Forms.TextBox();
            this.txtBoxConstantes = new System.Windows.Forms.TextBox();
            this.txtBoxCuadruplos = new System.Windows.Forms.TextBox();
            this.txtBoxLexico = new System.Windows.Forms.TextBox();
            this.txtBoxCodigo = new System.Windows.Forms.TextBox();
            this.btnEjecutar = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtBoxSimbolos
            // 
            this.txtBoxSimbolos.Location = new System.Drawing.Point(502, 511);
            this.txtBoxSimbolos.Multiline = true;
            this.txtBoxSimbolos.Name = "txtBoxSimbolos";
            this.txtBoxSimbolos.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtBoxSimbolos.Size = new System.Drawing.Size(403, 135);
            this.txtBoxSimbolos.TabIndex = 10;
            // 
            // txtBoxConstantes
            // 
            this.txtBoxConstantes.Location = new System.Drawing.Point(502, 377);
            this.txtBoxConstantes.Multiline = true;
            this.txtBoxConstantes.Name = "txtBoxConstantes";
            this.txtBoxConstantes.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtBoxConstantes.Size = new System.Drawing.Size(340, 128);
            this.txtBoxConstantes.TabIndex = 9;
            // 
            // txtBoxCuadruplos
            // 
            this.txtBoxCuadruplos.Location = new System.Drawing.Point(502, 56);
            this.txtBoxCuadruplos.Multiline = true;
            this.txtBoxCuadruplos.Name = "txtBoxCuadruplos";
            this.txtBoxCuadruplos.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtBoxCuadruplos.Size = new System.Drawing.Size(340, 304);
            this.txtBoxCuadruplos.TabIndex = 8;
            // 
            // txtBoxLexico
            // 
            this.txtBoxLexico.Location = new System.Drawing.Point(12, 351);
            this.txtBoxLexico.Multiline = true;
            this.txtBoxLexico.Name = "txtBoxLexico";
            this.txtBoxLexico.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtBoxLexico.Size = new System.Drawing.Size(444, 295);
            this.txtBoxLexico.TabIndex = 7;
            // 
            // txtBoxCodigo
            // 
            this.txtBoxCodigo.Location = new System.Drawing.Point(10, 56);
            this.txtBoxCodigo.Multiline = true;
            this.txtBoxCodigo.Name = "txtBoxCodigo";
            this.txtBoxCodigo.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtBoxCodigo.Size = new System.Drawing.Size(446, 236);
            this.txtBoxCodigo.TabIndex = 6;
            // 
            // btnEjecutar
            // 
            this.btnEjecutar.Location = new System.Drawing.Point(162, 15);
            this.btnEjecutar.Name = "btnEjecutar";
            this.btnEjecutar.Size = new System.Drawing.Size(98, 35);
            this.btnEjecutar.TabIndex = 12;
            this.btnEjecutar.Text = "Ejecutar";
            this.btnEjecutar.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(127, 38);
            this.button1.TabIndex = 11;
            this.button1.Text = "Abrir archivo";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1018, 653);
            this.Controls.Add(this.btnEjecutar);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.txtBoxSimbolos);
            this.Controls.Add(this.txtBoxConstantes);
            this.Controls.Add(this.txtBoxCuadruplos);
            this.Controls.Add(this.txtBoxLexico);
            this.Controls.Add(this.txtBoxCodigo);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtBoxSimbolos;
        private System.Windows.Forms.TextBox txtBoxConstantes;
        private System.Windows.Forms.TextBox txtBoxCuadruplos;
        private System.Windows.Forms.TextBox txtBoxLexico;
        private System.Windows.Forms.TextBox txtBoxCodigo;
        private System.Windows.Forms.Button btnEjecutar;
        private System.Windows.Forms.Button button1;
    }
}

