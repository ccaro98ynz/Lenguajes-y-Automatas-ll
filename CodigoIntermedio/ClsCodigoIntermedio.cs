
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
        private string metodoActual = null;
        private bool ProcesarMetodo(List<string> tokens)
        {
            string primero = tokens[0];
            if (primero == "FUNC")
            {
                if (tokens.Count != 3)
                    throw new InvalidOperationException(
                        "Declaracion de funcion invalida."
                    );

                if (metodoActual != null)
                    throw new InvalidOperationException(
                        "No se permiten metodos anidados."
                    );

                metodoActual = tokens[2];

                Emitir("PROC " + metodoActual);

                return true;
            }

            if (primero == "ENDFUNC")
            {
                if (metodoActual == null)
                    throw new InvalidOperationException(
                        "No existe una funcion abierta."
                    );

                Emitir("ENDPROC " + metodoActual);

                metodoActual = null;

                return true;
            }
            // Inicio de un metodo auxiliar.
            if (primero == "fn" || primero == "F")
            {
                if (tokens.Count < 2)
                    throw new InvalidOperationException(
                        "Metodo sin identificador.");

                if (metodoActual != null)
                    throw new InvalidOperationException(
                        "No se permiten metodos anidados.");

                metodoActual = tokens[1];

                Emitir("PROC " + metodoActual);

                return true;
            }

            // Finalizacion de un metodo auxiliar.
            if (primero == "end" || primero == "Finally")
            {
                if (metodoActual == null)
                    throw new InvalidOperationException(
                        "No existe un metodo abierto.");

                Emitir("ENDPROC " + metodoActual);

                metodoActual = null;

                return true;
            }

            return false;
        }
        private bool ProcesarRetorno(List<string> tokens)
        {
            if (tokens[0] != "return")
                return false;

            if (metodoActual == null)
                throw new InvalidOperationException(
                    "Return fuera de un metodo.");

            if (tokens.Count < 2)
                throw new InvalidOperationException(
                    "Return sin valor.");

            List<string> expresion =
                tokens.GetRange(1, tokens.Count - 1);

            string resultado = ResolverExpresion(expresion);

            Emitir("RETURN " + resultado);

            return true;
        }
        public List<string> Instrucciones
        {
            get { return instrucciones; }
        }
        private void ProcesarEntradaSalida(List<string> tokens)
        {
            string instruccion = tokens[0];

            if (tokens.Count < 2)
                throw new InvalidOperationException(
                    "La instruccion " + instruccion +
                    " necesita un argumento.");

            List<string> argumentos =
                tokens.GetRange(1, tokens.Count - 1);

            if (instruccion == "show")
            {
                Emitir("PRINT " + string.Join(" ", argumentos));
            }
            else if (instruccion == "input")
            {
                Emitir("READ " + string.Join(" ", argumentos));
            }
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
                if (token.StartsWith("CALL:"))
                {
                    string[] partes = token.Split(':');

                    if (partes.Length != 3 || partes[2] != "0")
                        throw new InvalidOperationException(
                            "Llamada a metodo invalida o no soportada."
                        );

                    string nombreMetodo = partes[1];

                    string temporal = NuevoTemporal();

                    Emitir(temporal + " = CALL " + nombreMetodo);

                    pila.Push(temporal);

                    continue;
                }
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
            {
                throw new InvalidOperationException(
                    "Expresion postfija invalida.\n" +
                    "Tokens: " + string.Join(" ", tokens) + "\n" +
                    "Elementos en pila: " + pila.Count + "\n" +
                    "Contenido: " + string.Join(", ", pila.ToArray())
                );
            }

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
            metodoActual = null;
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
                if (ProcesarMetodo(tokens))
                    continue;
                if (ProcesarRetorno(tokens))
                    continue;
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
                // Inicio del ciclo for.
                if (EsEtiqueta(primero, "for"))
                {
                    ProcesarCondicion(tokens, primero, true);
                    continue;
                }

                // Finalizacion del ciclo for.
                if (EsEtiqueta(primero, "finfor"))
                {
                    string numero =
                        ObtenerNumero(primero, "finfor");

                    Emitir("GOTO for" + numero);
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
                if (tokens[0] == "show" || tokens[0] == "input")
                {
                    ProcesarEntradaSalida(tokens);
                    continue;
                }
                    ResolverExpresion(tokens);
                
            }
        }
    }
}
