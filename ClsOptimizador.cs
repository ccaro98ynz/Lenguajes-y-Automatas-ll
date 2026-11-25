
using System;
using System.Collections.Generic;
using System.Linq;
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

        private string SimplificarExpresion(string instruccion)
        {
            Match match = Regex.Match(
                instruccion,
                @"^(\w+)\s*=\s*(\S+)\s*([+\-*/])\s*(\S+)$"
            );

            if (!match.Success) return instruccion;

            string destino = match.Groups[1].Value;
            string izquierda = match.Groups[2].Value;
            string operador = match.Groups[3].Value;
            string derecha = match.Groups[4].Value;

            switch (operador)
            {
                case "+":
                    if (derecha == "0")
                        return destino + " = " + izquierda;
                    if (izquierda == "0")
                        return destino + " = " + derecha;
                    break;

                case "-":
                    if (derecha == "0")
                        return destino + " = " + izquierda;
                    break;

                case "*":
                    if (derecha == "1")
                        return destino + " = " + izquierda;
                    if (izquierda == "1")
                        return destino + " = " + derecha;
                    break;

                case "/":
                    if (derecha == "1")
                        return destino + " = " + izquierda;
                    break;
            }

            return instruccion;
        }

        public List<string> Optimizar()
        {
            List<string> resultado = new List<string>(instrucciones);

            // 1. Resolver operaciones constantes.
            resultado = resultado
                .Select(PlegarConstantes)
                .ToList();

            // 2. Simplificar operaciones algebraicas.
            resultado = resultado
                .Select(SimplificarExpresion)
                .ToList();

            // 3. Propagar resultados entre temporales.
            resultado = PropagarConstantes(resultado);

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

        private List<string> PropagarConstantes(List<string> codigo)
        {
            var constantes = new Dictionary<string, string>();
            var resultado = new List<string>();

            foreach (string instruccion in codigo)
            {
                string linea = instruccion.Trim();

                // No propagamos valores entre bloques de control.
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

                // Reconocer operaciones binarias.
                Match operacion = Regex.Match(linea,
                    @"^(\w+)\s*=\s*(-?\w+)\s*([+\-*/%])\s*(-?\w+)$");

                if (operacion.Success)
                {
                    string destino = operacion.Groups[1].Value;
                    string izquierda = operacion.Groups[2].Value;
                    string operador = operacion.Groups[3].Value;
                    string derecha = operacion.Groups[4].Value;

                    if (constantes.ContainsKey(izquierda))
                        izquierda = constantes[izquierda];

                    if (constantes.ContainsKey(derecha))
                        derecha = constantes[derecha];

                    linea = destino + " = " + izquierda +
                            " " + operador + " " + derecha;

                    linea = PlegarConstantes(linea);
                }

                // Reconocer asignaciones simples.
                Match asignacion = Regex.Match(linea,
                    @"^(\w+)\s*=\s*(-?\w+)$");

                if (asignacion.Success)
                {
                    string destino = asignacion.Groups[1].Value;
                    string origen = asignacion.Groups[2].Value;

                    if (constantes.ContainsKey(origen))
                        origen = constantes[origen];

                    linea = destino + " = " + origen;

                    // Invalidar el valor anterior del destino.
                    constantes.Remove(destino);

                    // Solo recordar temporales numericos.
                    short numero;

                    if (Regex.IsMatch(destino, @"^t\d+$") &&
                        short.TryParse(origen, out numero))
                    {
                        constantes[destino] = origen;
                    }
                }
                else
                {
                    // Si se redefine un temporal sin un valor constante,
                    // no debemos conservar su valor anterior.
                    Match destino = Regex.Match(linea, @"^(\w+)\s*=");

                    if (destino.Success)
                        constantes.Remove(destino.Groups[1].Value);
                }

                resultado.Add(linea);
            }

            return resultado;
        }

    }
}
