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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgvLexico = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.dgvErrores = new System.Windows.Forms.DataGridView();
            this.label3 = new System.Windows.Forms.Label();
            this.rtxcode = new System.Windows.Forms.RichTextBox();
            this.btnTablaVariables = new System.Windows.Forms.Button();
            this.lblTtoken = new System.Windows.Forms.Label();
            this.rtxPostfijo = new System.Windows.Forms.RichTextBox();
            this.rtxIntermedio = new System.Windows.Forms.RichTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLexico)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvErrores)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvLexico
            // 
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(10)))), ((int)(((byte)(100)))), ((int)(((byte)(205)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Bahnschrift Condensed", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.White;
            this.dgvLexico.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvLexico.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.dgvLexico.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvLexico.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dgvLexico.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.dgvLexico.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgvLexico.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dgvLexico.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgvLexico.ColumnHeadersHeight = 29;
            this.dgvLexico.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.dgvLexico.Location = new System.Drawing.Point(533, 138);
            this.dgvLexico.Name = "dgvLexico";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Bahnschrift", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            this.dgvLexico.RowHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvLexico.RowHeadersWidth = 51;
            this.dgvLexico.RowTemplate.DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(230)))), ((int)(((byte)(150)))));
            this.dgvLexico.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Bahnschrift", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvLexico.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.Color.White;
            this.dgvLexico.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.dgvLexico.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.dgvLexico.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvLexico.RowTemplate.Height = 24;
            this.dgvLexico.Size = new System.Drawing.Size(505, 483);
            this.dgvLexico.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial Rounded MT Bold", 16.2F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(24, 103);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(374, 32);
            this.label1.TabIndex = 1;
            this.label1.Text = "Contenido Del Documento";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial Rounded MT Bold", 16.2F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(36, 541);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(380, 32);
            this.label2.TabIndex = 3;
            this.label2.Text = "Registro Actual de Errores";
            // 
            // dgvErrores
            // 
            this.dgvErrores.AllowUserToAddRows = false;
            this.dgvErrores.AllowUserToDeleteRows = false;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(31)))), ((int)(((byte)(203)))));
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Bahnschrift Condensed", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.LightGray;
            this.dgvErrores.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvErrores.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.dgvErrores.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvErrores.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dgvErrores.BackgroundColor = System.Drawing.SystemColors.ButtonFace;
            this.dgvErrores.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgvErrores.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgvErrores.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvErrores.Location = new System.Drawing.Point(30, 670);
            this.dgvErrores.Name = "dgvErrores";
            this.dgvErrores.RowHeadersWidth = 51;
            this.dgvErrores.RowTemplate.Height = 24;
            this.dgvErrores.Size = new System.Drawing.Size(1062, 120);
            this.dgvErrores.TabIndex = 2;
            this.dgvErrores.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvErrores_CellContentClick);
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Arial Rounded MT Bold", 16.2F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(527, 103);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(266, 32);
            this.label3.TabIndex = 4;
            this.label3.Text = "Tokens y Lexemas";
            // 
            // rtxcode
            // 
            this.rtxcode.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.rtxcode.BackColor = System.Drawing.Color.LightSkyBlue;
            this.rtxcode.Location = new System.Drawing.Point(24, 138);
            this.rtxcode.Name = "rtxcode";
            this.rtxcode.Size = new System.Drawing.Size(464, 483);
            this.rtxcode.TabIndex = 5;
            this.rtxcode.Text = "";
            this.rtxcode.TextChanged += new System.EventHandler(this.rtxcode_TextChanged);
            // 
            // btnTablaVariables
            // 
            this.btnTablaVariables.Location = new System.Drawing.Point(1093, 44);
            this.btnTablaVariables.Name = "btnTablaVariables";
            this.btnTablaVariables.Size = new System.Drawing.Size(152, 56);
            this.btnTablaVariables.TabIndex = 6;
            this.btnTablaVariables.Text = "Mostrar Tabla de Variables";
            this.btnTablaVariables.UseVisualStyleBackColor = true;
            this.btnTablaVariables.Click += new System.EventHandler(this.btnTablaVariables_Click);
            // 
            // lblTtoken
            // 
            this.lblTtoken.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblTtoken.AutoSize = true;
            this.lblTtoken.Font = new System.Drawing.Font("Arial Rounded MT Bold", 16.2F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTtoken.Location = new System.Drawing.Point(539, 32);
            this.lblTtoken.Name = "lblTtoken";
            this.lblTtoken.Size = new System.Drawing.Size(122, 32);
            this.lblTtoken.TabIndex = 9;
            this.lblTtoken.Text = "Token : ";
            // 
            // rtxPostfijo
            // 
            this.rtxPostfijo.Location = new System.Drawing.Point(1216, 609);
            this.rtxPostfijo.Name = "rtxPostfijo";
            this.rtxPostfijo.Size = new System.Drawing.Size(292, 276);
            this.rtxPostfijo.TabIndex = 10;
            this.rtxPostfijo.Text = "";
            // 
            // rtxIntermedio
            // 
            this.rtxIntermedio.Location = new System.Drawing.Point(1093, 138);
            this.rtxIntermedio.Name = "rtxIntermedio";
            this.rtxIntermedio.Size = new System.Drawing.Size(415, 405);
            this.rtxIntermedio.TabIndex = 11;
            this.rtxIntermedio.Text = "";
            // 
            // frLexico
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.ClientSize = new System.Drawing.Size(1520, 897);
            this.Controls.Add(this.rtxIntermedio);
            this.Controls.Add(this.rtxPostfijo);
            this.Controls.Add(this.lblTtoken);
            this.Controls.Add(this.btnTablaVariables);
            this.Controls.Add(this.rtxcode);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dgvErrores);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dgvLexico);
            this.Name = "frLexico";
            this.Text = "frLexico";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frLexico_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.dgvLexico)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvErrores)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvLexico;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridView dgvErrores;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.RichTextBox rtxcode;
        private System.Windows.Forms.Button btnTablaVariables;
        private System.Windows.Forms.Label lblTtoken;
        private System.Windows.Forms.RichTextBox rtxPostfijo;
        private System.Windows.Forms.RichTextBox rtxIntermedio;
    }
}