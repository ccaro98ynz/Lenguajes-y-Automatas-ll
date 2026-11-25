
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace LP1
{
    public class ClsCodigoIntermedio
    {
        private readonly List<string> postfijo;
        private readonly List<string> instrucciones;

        private int contadorTemporal;

        public List<string> Instrucciones
        {
            get { return instrucciones; }
        }

        public ClsCodigoIntermedio(List<string> entrada)
        {
            if (entrada == null)
                throw new ArgumentNullException("entrada");

            postfijo = entrada;
            instrucciones = new List<string>();
        }

        private void Emitir(string instruccion)
        {
            instrucciones.Add(instruccion);
        }

        private string NuevoTemporal()
        {
            contadorTemporal++;
            return "t" + contadorTemporal;
        }

        private bool EsOperador(string valor)
        {
            switch (valor)
            {
                case "+":
                case "-":
                case "*":
                case "/":
                case "%":
                case "==":
                case "!=":
                case "<":
                case ">":
                case "<=":
                case ">=":
                case "&&":
                case "||":
                case "=":
                    return true;

                default:
                    return false;
            }
        }

        // Respeta literales como "Hola Mundo".
        private List<string> SepararTokens(string linea)
        {
            List<string> resultado = new List<string>();
            StringBuilder actual = new StringBuilder();

            bool dentroCadena = false;
            char comilla = '\0';
            bool escape = false;

            foreach (char caracter in linea)
            {
                if (dentroCadena)
                {
                    actual.Append(caracter);

                    if (escape)
                        escape = false;
                    else if (caracter == '\\')
                        escape = true;
                    else if (caracter == comilla)
                        dentroCadena = false;
                }
                else if (caracter == '"' || caracter == '\'')
                {
                    dentroCadena = true;
                    comilla = caracter;
                    actual.Append(caracter);
                }
                else if (char.IsWhiteSpace(caracter))
                {
                    if (actual.Length > 0)
                    {
                        resultado.Add(actual.ToString());
                        actual.Clear();
                    }
                }
                else
                {
                    actual.Append(caracter);
                }
            }

            if (actual.Length > 0)
                resultado.Add(actual.ToString());

            if (dentroCadena)
                throw new InvalidOperationException(
                    "Cadena sin cerrar en codigo postfijo.");

            return resultado;
        }

        private string ResolverExpresion(List<string> tokens)
        {
            Stack<string> pila = new Stack<string>();

            foreach (string token in tokens)
            {
                if (!EsOperador(token))
                {
                    pila.Push(token);
                    continue;
                }

                if (pila.Count < 2)
                    throw new InvalidOperationException(
                        "Faltan operandos para: " + token);

                string derecha = pila.Pop();
                string izquierda = pila.Pop();

                if (token == "=")
                {
                    Emitir(izquierda + " = " + derecha);
                    pila.Push(izquierda);
                }
                else
                {
                    string temporal = NuevoTemporal();

                    Emitir(temporal + " = " +
                           izquierda + " " + token +
                           " " + derecha);

                    pila.Push(temporal);
                }
            }

            if (pila.Count != 1)
                throw new InvalidOperationException(
                    "Expresion postfija invalida.");

            return pila.Pop();
        }

        private bool EsEtiqueta(string token, string prefijo)
        {
            return Regex.IsMatch(
                token,
                "^" + prefijo + "[0-9]+$");
        }

        private string ObtenerNumero(string etiqueta, string prefijo)
        {
            return etiqueta.Substring(prefijo.Length);
        }

        private void ProcesarCondicion(
            List<string> tokens, string etiqueta, bool esWhile)
        {
            if (tokens.Count < 2)
                throw new InvalidOperationException(
                    "Condicion vacia: " + etiqueta);

            if (esWhile)
                Emitir("LABEL " + etiqueta);

            List<string> expresion =
                tokens.GetRange(1, tokens.Count - 1);

            string resultado = ResolverExpresion(expresion);

            string destino = esWhile
                ? "fin" + etiqueta
                : "else" + ObtenerNumero(etiqueta, "if");

            Emitir("IF_FALSE " + resultado +
                   " GOTO " + destino);
        }

        public void Analizar()
        {
            instrucciones.Clear();
            contadorTemporal = 0;

            foreach (string linea in postfijo)
            {
                if (string.IsNullOrWhiteSpace(linea))
                    continue;

                List<string> tokens = SepararTokens(linea);

                if (tokens.Count == 0)
                    continue;

                string primero = tokens[0];

                // Inicio y fin del metodo principal.
                if (primero == "mainF")
                {
                    Emitir("PROC MAIN");
                    continue;
                }

                if (primero == "mainFin")
                {
                    Emitir("ENDPROC MAIN");
                    continue;
                }

                // Metodos auxiliares.
                if (primero == "F" && tokens.Count >= 2)
                {
                    Emitir("PROC " + tokens[1]);
                    continue;
                }

                if (primero == "Finally")
                {
                    Emitir("ENDPROC");
                    continue;
                }

                // Condicional.
                if (EsEtiqueta(primero, "if"))
                {
                    ProcesarCondicion(tokens, primero, false);
                    continue;
                }

                if (EsEtiqueta(primero, "finif"))
                {
                    string numero = ObtenerNumero(primero, "finif");

                    Emitir("GOTO finelse" + numero);
                    continue;
                }

                if (EsEtiqueta(primero, "else"))
                {
                    Emitir("LABEL " + primero);
                    continue;
                }

                if (EsEtiqueta(primero, "finelse"))
                {
                    Emitir("LABEL " + primero);
                    continue;
                }

                // Ciclos.
                if (EsEtiqueta(primero, "while"))
                {
                    ProcesarCondicion(tokens, primero, true);
                    continue;
                }

                if (EsEtiqueta(primero, "finwhile"))
                {
                    string numero =
                        ObtenerNumero(primero, "finwhile");

                    Emitir("GOTO while" + numero);
                    Emitir("LABEL " + primero);
                    continue;
                }

                // Declaraciones.
                if (primero == "int" ||
                    primero == "str" ||
                    primero == "dec" ||
                    primero == "bool")
                {
                    if (tokens.Count < 2)
                        throw new InvalidOperationException(
                            "Declaracion sin identificador.");

                    Emitir("DECL " + primero + " " + tokens[1]);

                    if (tokens.Count > 2)
                    {
                        ResolverExpresion(
                            tokens.GetRange(1, tokens.Count - 1));
                    }

                    continue;
                }

                // Asignaciones y expresiones normales.
                ResolverExpresion(tokens);
            }
        }
    }
}
