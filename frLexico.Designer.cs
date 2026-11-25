namespace LP1
{
    partial class frLexico
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
            this.tbLayoutPrincipal = new System.Windows.Forms.TableLayoutPanel();
            this.tbLayoutDescripcion = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.rtxCodigo = new System.Windows.Forms.RichTextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.rtxPostfijo = new System.Windows.Forms.RichTextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.rtxIntermedio = new System.Windows.Forms.RichTextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.dgvTokens = new System.Windows.Forms.DataGridView();
            this.tbLayoutResumen = new System.Windows.Forms.TableLayoutPanel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.panel6 = new System.Windows.Forms.Panel();
            this.dgvErrores = new System.Windows.Forms.DataGridView();
            this.label6 = new System.Windows.Forms.Label();
            this.rtxEnsamblador = new System.Windows.Forms.RichTextBox();
            this.tbLayoutPrincipal.SuspendLayout();
            this.tbLayoutDescripcion.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTokens)).BeginInit();
            this.tbLayoutResumen.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvErrores)).BeginInit();
            this.SuspendLayout();
            // 
            // tbLayoutPrincipal
            // 
            this.tbLayoutPrincipal.ColumnCount = 1;
            this.tbLayoutPrincipal.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tbLayoutPrincipal.Controls.Add(this.tbLayoutDescripcion, 0, 0);
            this.tbLayoutPrincipal.Controls.Add(this.tbLayoutResumen, 0, 1);
            this.tbLayoutPrincipal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbLayoutPrincipal.Location = new System.Drawing.Point(0, 0);
            this.tbLayoutPrincipal.Name = "tbLayoutPrincipal";
            this.tbLayoutPrincipal.RowCount = 2;
            this.tbLayoutPrincipal.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.tbLayoutPrincipal.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tbLayoutPrincipal.Size = new System.Drawing.Size(1251, 674);
            this.tbLayoutPrincipal.TabIndex = 1;
            // 
            // tbLayoutDescripcion
            // 
            this.tbLayoutDescripcion.ColumnCount = 4;
            this.tbLayoutDescripcion.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tbLayoutDescripcion.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35F));
            this.tbLayoutDescripcion.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tbLayoutDescripcion.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.tbLayoutDescripcion.Controls.Add(this.panel4, 3, 0);
            this.tbLayoutDescripcion.Controls.Add(this.panel3, 2, 0);
            this.tbLayoutDescripcion.Controls.Add(this.panel2, 1, 0);
            this.tbLayoutDescripcion.Controls.Add(this.panel1, 0, 0);
            this.tbLayoutDescripcion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbLayoutDescripcion.Location = new System.Drawing.Point(3, 3);
            this.tbLayoutDescripcion.Name = "tbLayoutDescripcion";
            this.tbLayoutDescripcion.RowCount = 1;
            this.tbLayoutDescripcion.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tbLayoutDescripcion.Size = new System.Drawing.Size(1245, 398);
            this.tbLayoutDescripcion.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.SeaGreen;
            this.panel1.Controls.Add(this.rtxCodigo);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(305, 392);
            this.panel1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 14.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(6, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(147, 23);
            this.label1.TabIndex = 0;
            this.label1.Text = "Codigo Fuente";
            // 
            // rtxCodigo
            // 
            this.rtxCodigo.Location = new System.Drawing.Point(6, 29);
            this.rtxCodigo.Name = "rtxCodigo";
            this.rtxCodigo.Size = new System.Drawing.Size(384, 206);
            this.rtxCodigo.TabIndex = 1;
            this.rtxCodigo.Text = "";
            this.rtxCodigo.TextChanged += new System.EventHandler(this.rtxCodigo_TextChanged);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.MediumSeaGreen;
            this.panel2.Controls.Add(this.dgvTokens);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(314, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(429, 392);
            this.panel2.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial", 14.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(6, 3);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(207, 23);
            this.label2.TabIndex = 0;
            this.label2.Text = "Tokens Reconocidos";
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.LightSeaGreen;
            this.panel3.Controls.Add(this.rtxPostfijo);
            this.panel3.Controls.Add(this.label3);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(749, 3);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(305, 392);
            this.panel3.TabIndex = 3;
            // 
            // rtxPostfijo
            // 
            this.rtxPostfijo.Location = new System.Drawing.Point(6, 29);
            this.rtxPostfijo.Name = "rtxPostfijo";
            this.rtxPostfijo.Size = new System.Drawing.Size(384, 206);
            this.rtxPostfijo.TabIndex = 1;
            this.rtxPostfijo.Text = "";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Arial", 14.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(6, 3);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(234, 23);
            this.label3.TabIndex = 0;
            this.label3.Text = "Representacion Postfija";
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.Turquoise;
            this.panel4.Controls.Add(this.rtxIntermedio);
            this.panel4.Controls.Add(this.label4);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(1060, 3);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(182, 392);
            this.panel4.TabIndex = 2;
            // 
            // rtxIntermedio
            // 
            this.rtxIntermedio.Location = new System.Drawing.Point(6, 29);
            this.rtxIntermedio.Name = "rtxIntermedio";
            this.rtxIntermedio.Size = new System.Drawing.Size(384, 206);
            this.rtxIntermedio.TabIndex = 1;
            this.rtxIntermedio.Text = "";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Arial", 14.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(5, 3);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(183, 23);
            this.label4.TabIndex = 0;
            this.label4.Text = "Codigo Intermedio";
            // 
            // dgvTokens
            // 
            this.dgvTokens.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTokens.Location = new System.Drawing.Point(10, 29);
            this.dgvTokens.Name = "dgvTokens";
            this.dgvTokens.Size = new System.Drawing.Size(416, 206);
            this.dgvTokens.TabIndex = 2;
            // 
            // tbLayoutResumen
            // 
            this.tbLayoutResumen.ColumnCount = 1;
            this.tbLayoutResumen.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tbLayoutResumen.Controls.Add(this.panel6, 0, 1);
            this.tbLayoutResumen.Controls.Add(this.panel5, 0, 0);
            this.tbLayoutResumen.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbLayoutResumen.Location = new System.Drawing.Point(3, 407);
            this.tbLayoutResumen.Name = "tbLayoutResumen";
            this.tbLayoutResumen.RowCount = 2;
            this.tbLayoutResumen.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tbLayoutResumen.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tbLayoutResumen.Size = new System.Drawing.Size(1245, 264);
            this.tbLayoutResumen.TabIndex = 1;
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.Color.MediumSeaGreen;
            this.panel5.Controls.Add(this.rtxEnsamblador);
            this.panel5.Controls.Add(this.label5);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(3, 3);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(1239, 126);
            this.panel5.TabIndex = 3;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Dock = System.Windows.Forms.DockStyle.Left;
            this.label5.Font = new System.Drawing.Font("Arial", 14.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(0, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(127, 23);
            this.label5.TabIndex = 0;
            this.label5.Text = "Codigo ASM";
            // 
            // panel6
            // 
            this.panel6.BackColor = System.Drawing.Color.MediumSeaGreen;
            this.panel6.Controls.Add(this.dgvErrores);
            this.panel6.Controls.Add(this.label6);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel6.Location = new System.Drawing.Point(3, 135);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(1239, 126);
            this.panel6.TabIndex = 4;
            // 
            // dgvErrores
            // 
            this.dgvErrores.AllowUserToAddRows = false;
            this.dgvErrores.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvErrores.Location = new System.Drawing.Point(10, 29);
            this.dgvErrores.Name = "dgvErrores";
            this.dgvErrores.Size = new System.Drawing.Size(416, 206);
            this.dgvErrores.TabIndex = 2;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Arial", 14.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(6, 3);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(81, 23);
            this.label6.TabIndex = 0;
            this.label6.Text = "Errores";
            // 
            // rtxEnsamblador
            // 
            this.rtxEnsamblador.Location = new System.Drawing.Point(10, 27);
            this.rtxEnsamblador.Name = "rtxEnsamblador";
            this.rtxEnsamblador.Size = new System.Drawing.Size(333, 96);
            this.rtxEnsamblador.TabIndex = 1;
            this.rtxEnsamblador.Text = "";
            // 
            // frLexico
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.ClientSize = new System.Drawing.Size(1251, 674);
            this.Controls.Add(this.tbLayoutPrincipal);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "frLexico";
            this.Text = "frLexico";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frLexico_FormClosing);
            this.Load += new System.EventHandler(this.frLexico_Load);
            this.tbLayoutPrincipal.ResumeLayout(false);
            this.tbLayoutDescripcion.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTokens)).EndInit();
            this.tbLayoutResumen.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvErrores)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TableLayoutPanel tbLayoutPrincipal;
        private System.Windows.Forms.TableLayoutPanel tbLayoutDescripcion;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.RichTextBox rtxPostfijo;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RichTextBox rtxCodigo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.RichTextBox rtxIntermedio;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DataGridView dgvTokens;
        private System.Windows.Forms.TableLayoutPanel tbLayoutResumen;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.DataGridView dgvErrores;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.RichTextBox rtxEnsamblador;
        private System.Windows.Forms.Label label5;
    }
}