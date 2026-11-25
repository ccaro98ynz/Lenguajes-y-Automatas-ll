using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Lifetime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LP1
{
    public partial class frLexico : Form
    {
        Timer cronn = new Timer();
        bool acces = true;
        ClsTablaSimbolos baseTabla = new ClsTablaSimbolos();
        List<ClsSimbolo> simbolos = new List<ClsSimbolo>();
        List<ClsSintactico> tokens_Entrada = new List<ClsSintactico>();
        ClsSintactico sintacticoActual = null;

        public frLexico()
        {
            InitializeComponent();
            cronn.Interval = 900;
            cronn.Tick += AnalizarCodigo;
        }
        private void AnalizarCodigo(object sender, EventArgs e)
        {
            if (!acces) return; // Salida rápida
            try
            {
                acces = false;
                cronn.Stop();
                InicializarAnalisisLexico();
                dgvErrores.Rows.Clear();
                dgvLexico.Rows.Clear();
                dgvLexico.SuspendLayout();
                dgvErrores.SuspendLayout();
                ClsLexico lexicoo = new ClsLexico();
                lexicoo.filaToken += Tokens;
                lexicoo.filaError += Errores;
                lexicoo.verToken += MostrarToken;
                lexicoo.AnalisisLexico(rtxcode.Text);
                lexicoo.ListadeTokens(out tokens_Entrada);
                sintacticoActual = (tokens_Entrada.Count != 0) ? new ClsSintactico(tokens_Entrada, baseTabla) : null;
                if (sintacticoActual != null)
                {
                    baseTabla.Limpiar();
                    sintacticoActual.filaError = Errores;
                    sintacticoActual.AnalisisSintactico();
                    if (dgvErrores.Rows.Count == 0)
                    {
                        rtxPostfijo.Text = "";
                        ClsSemantico semantico = new ClsSemantico(tokens_Entrada);

                        semantico.Analizar();
                        rtxPostfijo.Lines = semantico.Postfijo.ToArray();
                        ClsCodigoIntermedio intermedio =
                        new ClsCodigoIntermedio(semantico.Postfijo);

                        intermedio.Analizar();

                        // Necesitas crear un RichTextBox llamado rtxIntermedio.
                        rtxIntermedio.Lines =
                            intermedio.Instrucciones.ToArray();
                    }
                }
            }
            finally
            {
                dgvLexico.ResumeLayout();
                dgvErrores.ResumeLayout();
                simbolos = baseTabla.ObtenerSimbolos();
                acces = true;
            }
        }
        private void MostrarToken(string token)
        {
            lock (this)
            {
                lblTtoken.Invoke(new Action(() =>
                {
                    lblTtoken.Text = "Token : " + token;
                }));
            }
        }
        public void InicializarAnalisisLexico()
        {
            if (dgvErrores.Columns.Count == 0)
            {
                dgvErrores.Columns.Add("Lexema", "Lexema");
                dgvErrores.Columns.Add("Token", "Token");
                dgvErrores.Columns.Add("Descripcion del Error", "Descripcion del Error");
                dgvErrores.Columns.Add("Linea", "Linea");
                dgvErrores.Columns.Add("Columna", "Columna");
            }
            if (dgvLexico.Columns.Count == 0)
            {
                dgvLexico.Columns.Add("Palabra", "Palabra");
                dgvLexico.Columns.Add("Token", "Token");
                dgvLexico.Columns.Add("Linea", "Linea");
                dgvLexico.Columns.Add("Columna", "Columna");
            }
        }
        public void Errores(string Palabra, int Token, string Descripcion, int Linea, int Columna)
        {
            dgvErrores.Rows.Add(Palabra, Token, Descripcion, Linea, Columna);
        }
        public void Tokens(string Palabra, int Token, int Linea, int Columna)
        {
            dgvLexico.Rows.Add(Palabra, Token, Linea, Columna);
        }
        private void rtxcode_TextChanged(object sender, EventArgs e)
        {
            cronn.Stop();
            cronn.Start();
        }
        private void frLexico_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
        private void dgvErrores_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private Form ventanaTabla = null;
        private string getTipoVariable(int id)
        {
            switch (id)
            {
                case 1:
                    return "Cadena";
                case 2:
                    return "Entero";
                case 3:
                    return "Decimal";
                case 4:
                    return "Booleano";
                default: return "Desconocido";
            }
        }
        private void btnTablaVariables_Click(object sender, EventArgs e)
        {
            // Si la ventana ya está abierta, la traemos al frente
            if (ventanaTabla != null && !ventanaTabla.IsDisposed)
            {
                ventanaTabla.BringToFront();
                return;
            }

            // 1. Crear la ventana nueva
            ventanaTabla = new Form
            {
                Text = "Tabla de Símbolos y Variables",
                Size = new System.Drawing.Size(500, 350),
                StartPosition = FormStartPosition.CenterParent,
                MinimizeBox = false,
                MaximizeBox = false
            };

            // 2. Crear un DataGridView fresco para esta ventana
            DataGridView dgvSimbolos = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false
            };

            // 3. Agregar las columnas
            dgvSimbolos.Columns.Add("Nombre", "Nombre");
            dgvSimbolos.Columns.Add("Tipo", "Tipo");
            dgvSimbolos.Columns.Add("ValorGuardado", "Valor Guardado");
            dgvSimbolos.Columns.Add("Linea", "Línea");
            dgvSimbolos.Columns.Add("Columna", "Columna");

            // 4. Llenarlo con los datos actuales que analizó tu compilador
            foreach (var simbolo in simbolos)
            {
                dgvSimbolos.Rows.Add(
                    simbolo.Nombre,
                    getTipoVariable(simbolo.TipoToken),
                    simbolo.ValorGuardado ?? "Sin valor",
                    simbolo.Linea,
                    simbolo.Columna
                );
            }

            ventanaTabla.Controls.Add(dgvSimbolos);

            // 5. Limpiar la referencia cuando el usuario cierre la ventana
            ventanaTabla.FormClosed += (s, args) =>
            {
                ventanaTabla = null;
            };

            ventanaTabla.Show(this);
        }

        }
}
