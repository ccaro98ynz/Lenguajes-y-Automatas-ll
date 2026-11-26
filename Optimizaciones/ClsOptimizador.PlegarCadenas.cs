using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LP1.CodigoIntermedio;

namespace LP1.Optimizaciones
{
    public partial class ClsOptimizador
    {
		private void PlegarCadenas(ClsInstruccionIntermedia instruccion)
		{
			if (instruccion.Operador != "+") return;

			string izquierda = instruccion.Izquierda;
			string derecha = instruccion.Derecha;

			if (!EsLiteralCadena(izquierda) ||
				!EsLiteralCadena(derecha))
				return;

			string original = instruccion.ToString();

			string contenidoIzquierdo =
				izquierda.Substring(1, izquierda.Length - 2);

			string contenidoDerecho =
				derecha.Substring(1, derecha.Length - 2);

			instruccion.Izquierda =
				"\"" + contenidoIzquierdo + contenidoDerecho + "\"";

			instruccion.Operador = null;
			instruccion.Derecha = null;

			Depuracion?.Invoke(
				"[Plegado de cadenas] " +
				original + " -> " + instruccion
			);
		}

	}
}
