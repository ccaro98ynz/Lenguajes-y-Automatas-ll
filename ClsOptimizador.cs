
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace LP1
{
    public class ClsOptimizador
    {
        private readonly List<string> instrucciones;

        public ClsOptimizador(List<string> instrucciones)
        {
            if (instrucciones == null)
                throw new ArgumentNullException("instrucciones", "No hay Instrucciones para Analizar");
            this.instrucciones = instrucciones;
        }

        public List<string> Optimizar()
        {
            List<string> resultado = new List<string>();

            foreach (string instruccion in instrucciones)
            {
                resultado.Add(PlegarConstantes(instruccion));
            }

            return resultado;
        }

        private string PlegarConstantes(string instruccion)
        {
            Match match = Regex.Match(
                instruccion,
                @"^(\w+)\s*=\s*(-?\d+)\s*([+\-*/%])\s*(-?\d+)$"
            );

            if (!match.Success)
                return instruccion;

            string variable = match.Groups[1].Value;
            int izquierda = int.Parse(match.Groups[2].Value);
            string operador = match.Groups[3].Value;
            int derecha = int.Parse(match.Groups[4].Value);

            int resultado;

            switch (operador)
            {
                case "+":
                    resultado = izquierda + derecha;
                    break;

                case "-":
                    resultado = izquierda - derecha;
                    break;

                case "*":
                    resultado = izquierda * derecha;
                    break;

                case "/":
                    if (derecha == 0) return instruccion;

                    resultado = izquierda / derecha;
                    break;

                case "%":
                    if (derecha == 0) return instruccion;

                    resultado = izquierda % derecha;
                    break;

                default:
                    return instruccion;
            }

            // Los enteros del 8086 son de 16 bits.
            if (resultado < short.MinValue ||
                resultado > short.MaxValue)
                return instruccion;

            return variable + " = " + resultado;
        }
    }
}
