using System;
using System.Collections.Generic;

namespace LP1.Optimizaciones
{
    public partial class ClsOptimizador
    {
        private List<string> EliminarSaltosRedundantes(
            List<string> codigo)
        {
            var resultado = new List<string>();

            for (int i = 0; i < codigo.Count; i++)
            {
                string linea = codigo[i].Trim();

                if (linea.StartsWith("GOTO ") &&
                    i + 1 < codigo.Count)
                {
                    string destino = linea.Substring(5).Trim();
                    string siguiente = codigo[i + 1].Trim();

                    if (string.Equals(
                        siguiente,
                        "LABEL " + destino,
                        StringComparison.OrdinalIgnoreCase))
                    {
                        Depuracion?.Invoke(
                            "[Salto redundante] " + linea
                        );

                        continue;
                    }
                }

                resultado.Add(codigo[i]);
            }

            return resultado;
        }
    }
}