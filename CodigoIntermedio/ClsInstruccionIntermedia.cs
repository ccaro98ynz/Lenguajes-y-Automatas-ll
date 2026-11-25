using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LP1.CodigoIntermedio
{
    public class ClsInstruccionIntermedia
    {
        public string Destino { get; set; }
        public string Izquierda { get; set; }
        public string Operador { get; set; }
        public string Derecha { get; set; }
        public override string ToString()
        {
            if (string.IsNullOrEmpty(Operador))
                return Destino + " = " + Izquierda;

            return Destino + " = " + Izquierda +
                   " " + Operador + " " + Derecha;
        }
    }
}
