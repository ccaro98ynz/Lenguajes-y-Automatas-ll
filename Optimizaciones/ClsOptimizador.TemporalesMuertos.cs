using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LP1.Optimizaciones
{
    public partial class ClsOptimizador
    {
		private List<string> EliminarTemporalesMuertos(
	List<string> codigo)
		{
			var resultado = new List<string>(codigo);

			bool huboCambios;

			do
			{
				huboCambios = false;

				for (int i = resultado.Count - 1; i >= 0; i--)
				{
					Match asignacion = Regex.Match(
						resultado[i].Trim(),
						@"^(t\d+)\s*=\s*(""?[^""]*""?)$"
					);

					if (!asignacion.Success) continue;

					string temporal = asignacion.Groups[1].Value;
					string valor = asignacion.Groups[2].Value.Trim();

					// Solo eliminar asignaciones de constantes.
					if (!Regex.IsMatch(valor, @"^(?:-?\d+|""[^""]*"")$"))
						continue;

					bool utilizado = false;

					for (int j = 0; j < resultado.Count; j++)
					{
						if (i == j) continue;

						string linea = Regex.Replace(
							resultado[j],
							@"""[^""]*""",
							""
						);

						if (!Regex.IsMatch(
							linea,
							$@"\b{Regex.Escape(temporal)}\b",
							RegexOptions.IgnoreCase))
							continue;

						utilizado = true;
						break;
					}

					if (utilizado) continue;

					resultado.RemoveAt(i);
					huboCambios = true;
				}

			} while (huboCambios);

			return resultado;
		}

	}
}
