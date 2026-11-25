using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LP1
{
    public class ClsSimbolo
    {
        public string Nombre { get; set; }
        public int TipoToken { get; set; } // El token del tipo de dato (ej. int, string)
        public string ValorGuardado { get; set; } // El valor guardado en la variable
        public int Linea { get; set; }
        public int Columna { get; set; }

    }
}
