using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LP1.Sintactico
{
    internal class ClsMetodo
    {
        public string Nombre { get; set; }

        public int TipoRetorno { get; set; }

        public List<int> TiposParametros { get; set; }
            = new List<int>();
    }
}
