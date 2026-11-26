using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace LP1.Optimizaciones
{
    public partial class ClsOptimizador
    {
        private List<string> EliminarCodigoInalcanzable(
            List<string> codigo)
        {
            if (codigo.Count == 0) return codigo;

            var etiquetas = new Dictionary<string, int>(
                StringComparer.OrdinalIgnoreCase);

            var visitadas = new HashSet<int>();
            var pendientes = new Stack<int>();
            var resultado = new List<string>();

            // Identificar las posiciones de las etiquetas.
            for (int i = 0; i < codigo.Count; i++)
            {
                string linea = codigo[i].Trim();

                if (!linea.StartsWith("LABEL ")) continue;

                string etiqueta = linea.Substring(6).Trim();

                etiquetas[etiqueta] = i;
            }

            // Considerar cada procedimiento como punto de entrada.
            pendientes.Push(0);

            for (int i = 0; i < codigo.Count; i++)
            {
                if (codigo[i].Trim().StartsWith("PROC "))
                    pendientes.Push(i);
            }

            // Recorrer el flujo de ejecución.
            while (pendientes.Count > 0)
            {
                int indice = pendientes.Pop();

                if (indice < 0 || indice >= codigo.Count)
                    continue;

                if (!visitadas.Add(indice))
                    continue;

                string linea = codigo[indice].Trim();

                if (linea.StartsWith("ENDPROC"))
                    continue;

                // Salto incondicional.
                if (linea.StartsWith("GOTO "))
                {
                    string destino = linea.Substring(5).Trim();

                    if (!etiquetas.ContainsKey(destino))
                        return codigo;

                    pendientes.Push(etiquetas[destino]);

                    continue;
                }

                // Salto condicional.
                if (linea.StartsWith("IF_FALSE "))
                {
                    Match match = Regex.Match(
                        linea,
                        @"^IF_FALSE\s+\S+\s+GOTO\s+(\w+)$"
                    );

                    if (!match.Success)
                        return codigo;

                    string destino = match.Groups[1].Value;

                    if (!etiquetas.ContainsKey(destino))
                        return codigo;

                    pendientes.Push(etiquetas[destino]);
                    pendientes.Push(indice + 1);

                    continue;
                }

                // Flujo secuencial.
                pendientes.Push(indice + 1);
            }

            // Conservar las instrucciones alcanzables
            // y las declaraciones estructurales.
            for (int i = 0; i < codigo.Count; i++)
            {
                string linea = codigo[i].Trim();

                if (visitadas.Contains(i) ||
                    linea.StartsWith("DECL ") ||
                    linea.StartsWith("PROC ") ||
                    linea.StartsWith("ENDPROC"))
                {
                    resultado.Add(codigo[i]);
                    continue;
                }

                Depuracion?.Invoke(
                    "[Código inalcanzable] " + codigo[i]
                );
            }

            return resultado;
        }
    }
}