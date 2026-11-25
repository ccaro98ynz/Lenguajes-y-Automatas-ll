
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using LP1.CodigoIntermedio;

namespace LP1
{
    public class ClsOptimizador
    {
        private readonly List<string> instrucciones;
        public Action<string> Depuracion;
        public ClsOptimizador(List<string> instrucciones)
        {
            if (instrucciones == null)
                throw new ArgumentNullException("instrucciones", "No hay Instrucciones para Analizar");
            this.instrucciones = instrucciones;
        }
        public List<string> ProbarOptimizacion()
        {
            int originales = instrucciones.Count;

            List<string> resultado = new List<string>();

            foreach (string linea in instrucciones)
            {
                ClsInstruccionIntermedia instruccion =
                    ClsParserIntermedio.Parsear(linea);

                if (instruccion == null)
                {
                    resultado.Add(linea);
                    continue;
                }

                PlegarConstantes(instruccion);
                PlegarCadenas(instruccion);
                SimplificarExpresion(instruccion);

                resultado.Add(instruccion.ToString());
            }

            resultado = PropagarConstantes(resultado);
            resultado = EliminarTemporalesMuertos(resultado);

            int optimizadas = resultado.Count;
            int eliminadas = originales - optimizadas;

            double reduccion = originales == 0
                ? 0
                : (double)eliminadas / originales * 100;

            MessageBox.Show(
                $"Originales: {originales}\n" +
                $"Optimizadas: {optimizadas}\n" +
                $"Eliminadas: {eliminadas}\n" +
                $"Reducción: {reduccion:F2}%"
            );

            return resultado;
        }
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

        private void SimplificarExpresion(ClsInstruccionIntermedia instruccion)
        {
            string izquierda = instruccion.Izquierda;
            string derecha = instruccion.Derecha;
            string operador = instruccion.Operador;

            string nuevoValor = null;

            switch (operador)
            {
                case "+":
                    if (derecha == "0") nuevoValor = izquierda;
                    else if (izquierda == "0") nuevoValor = derecha;
                    break;

                case "-":
                    if (derecha == "0") nuevoValor = izquierda;
                    break;

                case "*":
                    if (derecha == "1") nuevoValor = izquierda;
                    else if (izquierda == "1") nuevoValor = derecha;
                    break;

                case "/":
                    if (derecha == "1") nuevoValor = izquierda;
                    break;
            }

            if (nuevoValor == null) return;

            string original = instruccion.ToString();

            instruccion.Izquierda = nuevoValor;
            instruccion.Operador = null;
            instruccion.Derecha = null;

            Depuracion?.Invoke(
                "[Simplificacion algebraica] " +
                original + " -> " + instruccion
            );
        }

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

        private bool EsLiteralCadena(string valor)
        {
            return !string.IsNullOrEmpty(valor) &&
                   valor.Length >= 2 &&
                   valor.StartsWith("\"") &&
                   valor.EndsWith("\"");
        }

    }
}
