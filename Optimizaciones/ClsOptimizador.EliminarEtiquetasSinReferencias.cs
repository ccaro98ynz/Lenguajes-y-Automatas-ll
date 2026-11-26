using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace LP1.Optimizaciones
{
    public partial class ClsOptimizador
    {
        private List<string> EliminarEtiquetasSinReferencias(
            List<string> codigo)
        {
            var referencias = new HashSet<string>(
                StringComparer.OrdinalIgnoreCase);

            var resultado = new List<string>();

            // Identificar las etiquetas utilizadas.
            foreach (string instruccion in codigo)
            {
                string linea = instruccion.Trim();

                Match match = Regex.Match(
                    linea,
                    @"^(?:GOTO|IF_FALSE\s+\S+\s+GOTO)\s+(\w+)$"
                );

                if (match.Success)
                    referencias.Add(match.Groups[1].Value);
                else if (linea.StartsWith("GOTO ") ||
                         linea.StartsWith("IF_FALSE "))
                    return codigo;
            }

            // Eliminar etiquetas sin referencias.
            foreach (string instruccion in codigo)
            {
                string linea = instruccion.Trim();

                if (linea.StartsWith("LABEL "))
                {
                    string etiqueta = linea.Substring(6).Trim();

                    if (!referencias.Contains(etiqueta))
                    {
                        Depuracion?.Invoke(
                            "[Etiqueta sin referencias] " + linea
                        );

                        continue;
                    }
                }

                resultado.Add(instruccion);
            }

            return resultado;
        }
    }
}