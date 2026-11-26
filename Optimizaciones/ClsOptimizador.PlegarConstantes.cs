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
		private void PlegarConstantes(ClsInstruccionIntermedia instruccion)
		{
			int izquierda, derecha;

			if (!int.TryParse(instruccion.Izquierda, out izquierda)) return;
			if (!int.TryParse(instruccion.Derecha, out derecha)) return;

			long resultado;

			switch (instruccion.Operador)
			{
				case "+":
					resultado = (long)izquierda + derecha;
					break;

				case "-":
					resultado = (long)izquierda - derecha;
					break;

				case "*":
					resultado = (long)izquierda * derecha;
					break;

				case "/":
					if (derecha == 0) return;
					resultado = (long)izquierda / derecha;
					break;

				case "%":
					if (derecha == 0) return;
					resultado = (long)izquierda % derecha;
					break;
				case ">":
					resultado = izquierda > derecha ? 1 : 0;
					break;

				case "<":
					resultado = izquierda < derecha ? 1 : 0;
					break;

				case ">=":
					resultado = izquierda >= derecha ? 1 : 0;
					break;

				case "<=":
					resultado = izquierda <= derecha ? 1 : 0;
					break;

				case "==":
					resultado = izquierda == derecha ? 1 : 0;
					break;

				case "!=":
					resultado = izquierda != derecha ? 1 : 0;
					break;
				default:
					return;
			}

			if (resultado < short.MinValue || resultado > short.MaxValue)
				return;

			string original = instruccion.ToString();

			instruccion.Izquierda = resultado.ToString();
			instruccion.Operador = null;
			instruccion.Derecha = null;

			Depuracion?.Invoke(
				"[Plegado de constantes] " +
				original + " -> " + instruccion
			);
		}
	}
}
