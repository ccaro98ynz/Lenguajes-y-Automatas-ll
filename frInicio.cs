using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LP1
{
    public partial class frInicio : Form
    {
        #region Bloc_de_notas
        const string rutaCarpeta = "C:\\Users\\Carlitos\\Documents";
        const string rutaTipografia = "C:\\Users\\Carlitos\\Documents\\tipografia_inicial.txt";
        List<ClsSintactico> li_sintactica = new List<ClsSintactico>();
        public frInicio()
        {
            InitializeComponent();
            rtxtexto.AcceptsTab = true;
            if (File.Exists(rutaTipografia))
            {
                string[] datos_fuente = File.ReadAllText(rutaTipografia).Split('\n');
                string nombre = datos_fuente[0];
                float size = float.Parse(datos_fuente[1]);
                FontStyle estilo = (FontStyle)Enum.Parse(typeof(FontStyle), datos_fuente[2].Trim());
                Font fuenteRecuperada = new Font(nombre, size, estilo);
                rtxtexto.Font = fuenteRecuperada;
            }
        }
        private void rtxtexto_SelectionChanged(object sender, EventArgs e)
        {
            int index = rtxtexto.SelectionStart;
            int linea = rtxtexto.GetLineFromCharIndex(index) + 1;
            int columna = index - rtxtexto.GetFirstCharIndexOfCurrentLine();
            indices.Text = $"Línea: {linea}, Columna: {columna}";
        }
        private void abrirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Archivos de texto (*.txt)|*.txt|Todos los archivos (*.*)|*.*";
            ofd.Title = "Abrir archivo de texto";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    rtxtexto.Text = File.ReadAllText(ofd.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error" + "" + ex);
                }
            }
        }
        private void guardarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frNombres frnombre = new frNombres();
            if (frnombre.ShowDialog() == DialogResult.OK)
            {
                if (!string.IsNullOrWhiteSpace(frnombre.Nombre))
                {
                    string rutaCompleta = Path.Combine(rutaCarpeta, frnombre.Nombre + ".txt");
                    try
                    {
                        StreamWriter buffer = new StreamWriter(rutaCompleta);
                        buffer.Write(rtxtexto.Text);
                        buffer.Close();
                        MessageBox.Show("Guardado Exitosamente");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al guardar: " + ex.Message);
                    }
                }
                else
                {
                    MessageBox.Show("Escriba un nombre válido.");
                }
            }
        }
        private void borrarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rtxtexto.Text = "";
        }
        private void fuenteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FontDialog fontDialog = new FontDialog();
            fontDialog.Font = rtxtexto.Font;
            if (fontDialog.ShowDialog() == DialogResult.OK)
            {
                rtxtexto.Font = fontDialog.Font;
            }
        }
        private void copiarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(rtxtexto.Text);

        }
        private void cortarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rtxtexto.Cut();
        }
        private void pegarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rtxtexto.Paste();
        }
        private void RegistrarFuente()
        {
            Font fuente = rtxtexto.Font;
            string datosFuente =
            $"{fuente.Name}\n{fuente.Size}\n{fuente.Style}\n{fuente.Unit}";
            string rutaCompleta = rutaTipografia;
            if (!File.Exists(rutaCompleta))
            {
                File.Create(rutaCompleta);
                File.WriteAllText(rutaCompleta, datosFuente);
            }
            else File.WriteAllText(rutaCompleta, datosFuente);
        }
        private void frInicio_FormClosing(object sender, FormClosingEventArgs e)
        {
            RegistrarFuente();
        }
        #endregion
        private void analisisLexicoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frLexico formLexico = new frLexico();
            formLexico.Show();
            //ClsPrincipal x = new ClsPrincipal();
            //x.textofinal += (s) => { rtxtexto.Invoke(new Action(() => rtxtexto.Text = s)); };
            //x.Transcribir_Matriz();
        }

       
    }
}
