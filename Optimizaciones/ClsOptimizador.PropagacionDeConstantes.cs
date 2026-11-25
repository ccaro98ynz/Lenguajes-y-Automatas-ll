using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using LP1.CodigoIntermedio;

namespace LP1.Optimizaciones
{
    public partial class ClsOptimizador
    {
		private void PropagarConstantes(
	ClsInstruccionIntermedia instruccion,
	Dictionary<string, string> constantes)
		{
			// Sustituir operandos conocidos.
			if (constantes.TryGetValue(instruccion.Izquierda, out string izquierda))
				instruccion.Izquierda = izquierda;

			if (!string.IsNullOrEmpty(instruccion.Derecha) &&
				constantes.TryGetValue(instruccion.Derecha, out string derecha))
				instruccion.Derecha = derecha;

			// Resolver operaciones aritméticas o concatenaciones.
			if (!string.IsNullOrEmpty(instruccion.Operador))
			{
				PlegarConstantes(instruccion);
				PlegarCadenas(instruccion);
				SimplificarExpresion(instruccion);
			}

			// Invalidar el valor anterior del destino.
			constantes.Remove(instruccion.Destino);

			// Registrar valores conocidos.
			if (!string.IsNullOrEmpty(instruccion.Operador))
				return;

			string valor = instruccion.Izquierda;

			if (short.TryParse(valor, out short numero) ||
				EsLiteralCadena(valor))
			{
				constantes[instruccion.Destino] = valor;
			}
		}

		private List<string> PropagarConstantes(List<string> codigo)
		{
			var constantes = new Dictionary<string, string>(
				StringComparer.OrdinalIgnoreCase);

			var resultado = new List<string>();

			foreach (string instruccion in codigo)
			{
				string linea = instruccion.Trim();

				// Las declaraciones no alteran los temporales conocidos.
				if (linea.StartsWith("DECL ", StringComparison.Ordinal))
				{
					resultado.Add(linea);
					continue;
				}

				// Límites de flujo de control.
				if (linea.StartsWith("LABEL ") ||
					linea.StartsWith("GOTO ") ||
					linea.StartsWith("IF_FALSE ") ||
					linea.StartsWith("PROC ") ||
					linea.StartsWith("ENDPROC"))
				{
					constantes.Clear();
					resultado.Add(linea);
					continue;
				}

				// PRINT no modifica variables.
				if (linea.StartsWith("PRINT ", StringComparison.Ordinal))
				{
					resultado.Add(linea);
					continue;
				}

				ClsInstruccionIntermedia actual =
					ClsParserIntermedio.Parsear(linea);

				// Reconocer asignaciones simples.
				if (actual == null)
				{
					Match match = Regex.Match(
	linea,
	"^([a-zA-Z_]\\w*)\\s*=\\s*" +
	"(\"[^\"]*\"|-?\\d+|[a-zA-Z_]\\w*)$"
);
					if (match.Success)
					{
						actual = new ClsInstruccionIntermedia
						{
							Destino = match.Groups[1].Value,
							Izquierda = match.Groups[2].Value
						};
					}
				}

				// Instrucciones que todavía no comprendemos.
				if (actual == null)
				{
					constantes.Clear();
					resultado.Add(linea);
					continue;
				}

				PropagarConstantes(actual, constantes);
				resultado.Add(actual.ToString());
			}

			return resultado;
		}


	}
}
