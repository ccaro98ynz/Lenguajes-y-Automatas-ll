
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using LP1.CodigoIntermedio;

namespace LP1.Optimizaciones
{
    public partial class ClsOptimizador
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

            // 1. Optimizaciones individuales.
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

            // 2. Optimizaciones hasta punto fijo.
            const int maxIteraciones = 20;
            int iteraciones = 0;

            List<string> anterior;

            do
            {
                anterior = new List<string>(resultado);

                resultado = PropagarConstantes(resultado);
                resultado = SimplificarSaltos(resultado);
                resultado = EliminarCodigoInalcanzable(resultado);
                resultado = EliminarSaltosRedundantes(resultado);
                resultado = EliminarEtiquetasSinReferencias(resultado);
                resultado = EliminarTemporalesMuertos(resultado);

                iteraciones++;

            } while (!anterior.SequenceEqual(resultado) &&
                     iteraciones < maxIteraciones);

            // 3. Estadísticas.
            int optimizadas = resultado.Count;
            int eliminadas = originales - optimizadas;

            double reduccion = originales == 0
                ? 0
                : (double)eliminadas / originales * 100;

            //MessageBox.Show(
            //    $"Originales: {originales}\n" +
            //    $"Optimizadas: {optimizadas}\n" +
            //    $"Eliminadas: {eliminadas}\n" +
            //    $"Reducción: {reduccion:F2}%\n" +
            //    $"Iteraciones: {iteraciones}"
            //);

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



        private bool EsLiteralCadena(string valor)
        {
            return !string.IsNullOrEmpty(valor) &&
                   valor.Length >= 2 &&
                   valor.StartsWith("\"") &&
                   valor.EndsWith("\"");
        }

    }
}
