using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LP1.CodigoIntermedio
{
    internal class ClsParserIntermedio
    {
        public static ClsInstruccionIntermedia Parsear(string linea)
        {
            Match match = Regex.Match(
                linea,
                @"^([a-zA-Z_]\w*)\s*=\s*(-?(?:\d+|[a-zA-Z_]\w*))\s*([+\-*/%])\s*(-?(?:\d+|[a-zA-Z_]\w*))$"
            );

            if (!match.Success)
                return null;

            return new ClsInstruccionIntermedia
            {
                Destino = match.Groups[1].Value,
                Izquierda = match.Groups[2].Value,
                Operador = match.Groups[3].Value,
                Derecha = match.Groups[4].Value
            };
        }
    }
}
