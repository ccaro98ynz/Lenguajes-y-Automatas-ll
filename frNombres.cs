using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LP1
{
    public partial class frNombres : Form
    {
        public frNombres()
        {
            InitializeComponent();
        }
        string nombre;
        public string Nombre { get => nombre; set => nombre = value; }
        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Nombre = txtnombre.Text;
                DialogResult = DialogResult.OK;
                Close();
            }
        }
    }
}
