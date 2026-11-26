using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace LP1.Optimizaciones
{
    public partial class ClsOptimizador
    {
        private List<string> SimplificarSaltos(List<string> codigo)
        {
            var resultado = new List<string>();

            foreach (string instruccion in codigo)
            {
                Match match = Regex.Match(
                    instruccion.Trim(),
                    @"^IF_FALSE\s+([01])\s+GOTO\s+(\w+)$"
                );

                if (!match.Success)
                {
                    resultado.Add(instruccion);
                    continue;
                }

                string condicion = match.Groups[1].Value;
                string etiqueta = match.Groups[2].Value;

                if (condicion == "0")
                    resultado.Add("GOTO " + etiqueta);

                Depuracion?.Invoke(
                    "[Simplificación de salto] " + instruccion
                );
            }

            return resultado;
        }
    }
}