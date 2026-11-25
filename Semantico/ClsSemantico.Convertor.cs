using System;
using System.Collections.Generic;

namespace LP1
{
    public partial class ClsSemantico
    {
        private readonly List<ClsSintactico> li_Input;
        private readonly List<string> li_Infijo = new List<string>();
        private readonly List<string> li_Leidos = new List<string>();
        private readonly Stack<string> pilaFlowControl = new Stack<string>();

        private int indice;
        private int contadorIf;
        private int contadorWhile;

        public List<string> Infijo { get { return li_Infijo; } }
        public List<string> Postfijo { get { return li_Leidos; } }

        public ClsSemantico(List<ClsSintactico> input)
        {
            if (input == null) throw new ArgumentNullException("input");
            li_Input = input;
        }

        private bool HayTokens() { return indice < li_Input.Count; }
        private int TokenActual() { return li_Input[indice].token; }
        private string LexemaActual() { return li_Input[indice].lexema; }
        private void Avanzar() { indice++; }

        private bool EsTipoDeDato()
        {
            return HayTokens() && (TokenActual() == 205 || TokenActual() == 206 ||
                                    TokenActual() == 207 || TokenActual() == 208);
        }

        private bool EsOperador(string valor)
        {
            return valor == "+" || valor == "-" || valor == "*" || valor == "/" ||
                   valor == "%" || valor == "=" || valor == "==" || valor == "!=" ||
                   valor == "<" || valor == ">" || valor == "<=" || valor == ">=" ||
                   valor == "&&" || valor == "||";
        }

        private int Prioridad(string operador)
        {
            switch (operador)
            {
                case "!":
                    return 7;

                case "*":
                case "/":
                case "%":
                    return 6;

                case "+":
                case "-":
                    return 5;

                case "<":
                case ">":
                case "<=":
                case ">=":
                    return 4;

                case "==":
                case "<>":
                    return 3;

                case "&&":
                    return 2;

                case "||":
                    return 1;

                case "=":
                    return 0;

                default:
                    return -1;
            }
        }
        public void Analizar()
        {
            indice = 0;
            contadorIf = 0;
            contadorWhile = 0;
            li_Leidos.Clear();
            li_Infijo.Clear();
            pilaFlowControl.Clear();

            while (HayTokens())
            {
                int anterior = indice;

                if (TokenActual() == 201 || TokenActual() == 202)
                    ReescribirMetodo();
                else
                    ProcesarInstruccion();

                if (indice <= anterior)
                    throw new InvalidOperationException("El análisis no avanzó en el token " + indice);
            }
        }

        private void ReescribirMetodo()
        {
            int tokenMetodo = TokenActual();
            int tokenFinal = tokenMetodo == 201 ? 220 : 203;
            string encabezado = LexemaActual();
            Avanzar();

            // En F Nombre(), conserva el identificador del método.
            if (tokenMetodo == 202 && HayTokens() && LexemaActual() != "(")
            {
                encabezado += " " + LexemaActual();
                Avanzar();
            }

            li_Leidos.Add(encabezado);

            // Saltar la lista de parámetros, sin perder sus paréntesis anidados.
            if (HayTokens() && LexemaActual() == "(")
                ConsumirParentesis(null);

            // El método termina con su palabra reservada, no con una llave.
            while (HayTokens() && TokenActual() != tokenFinal)
            {
                int anterior = indice;
                ProcesarInstruccion();
                if (indice <= anterior)
                    throw new InvalidOperationException("No se avanzó en el cuerpo del método");
            }

            if (!HayTokens())
                throw new InvalidOperationException("Falta la palabra de cierre del método " + encabezado);

            li_Leidos.Add(LexemaActual());
            Avanzar();
        }

        private void ProcesarInstruccion()
        {
            if (!HayTokens()) return;

            string lexema = LexemaActual();

            if (lexema == "if" || TokenActual() == 209)
            {
                ReescribirIf();
            }
            else if (lexema == "while")
            {
                ReescribirWhile();
            }
            else if (lexema == "{")
            {
                ProcesarBloque();
            }
            else if (lexema == ";")
            {
                Avanzar();
            }
            else if (lexema == "}" || lexema == "else")
            {
                throw new InvalidOperationException("Cierre o else inesperado: " + lexema);
            }
            else if (TokenActual() == 201 || TokenActual() == 202)
            {
                ReescribirMetodo();
            }
            else
            {
                ReescribirInstruccionPostFijo();
            }
        }

        private void ReescribirIf()
        {
            string etiqueta = "if" + (++contadorIf);
            Avanzar(); // if
            List<string> condicion = new List<string>();
            ConsumirParentesis(condicion);

            List<string> postfijo = ConvertirPostfijo(condicion);
            li_Leidos.Add(etiqueta + " " + string.Join(" ", postfijo));

            pilaFlowControl.Push(etiqueta);
            ProcesarBloque();
            CerrarBloque(); // finifN

            string etiquetaElse = "else" + etiqueta.Substring(2);
            li_Leidos.Add(etiquetaElse);

            if (HayTokens() && LexemaActual() == "else")
            {
                Avanzar();
                pilaFlowControl.Push(etiquetaElse);
                ProcesarBloque();
                CerrarBloque(); // finelseN
            }
            else
            {
                // Else vacío obligatorio, aunque no esté en el código fuente.
                li_Leidos.Add("fin" + etiquetaElse);
            }
        }

        private void ReescribirWhile()
        {
            string etiqueta = "while" + (++contadorWhile);
            Avanzar(); // while
            List<string> condicion = new List<string>();
            ConsumirParentesis(condicion);

            li_Leidos.Add(etiqueta + " " + string.Join(" ", ConvertirPostfijo(condicion)));
            pilaFlowControl.Push(etiqueta);
            ProcesarBloque();
            CerrarBloque(); // finwhileN
        }

        // Consume { instrucciones } sin perder el control de bloques anidados.
        private void ProcesarBloque()
        {
            if (!HayTokens() || LexemaActual() != "{")
                throw new InvalidOperationException("Se esperaba '{' al comenzar el bloque");

            Avanzar(); // {
            while (HayTokens() && LexemaActual() != "}")
            {
                int anterior = indice;
                ProcesarInstruccion();
                if (indice <= anterior)
                    throw new InvalidOperationException("El bloque no avanzó en el token " + indice);
            }

            if (!HayTokens())
                throw new InvalidOperationException("Falta '}' para cerrar un bloque");

            Avanzar(); // }
        }

        private void CerrarBloque()
        {
            if (pilaFlowControl.Count == 0)
                throw new InvalidOperationException("No existe un bloque de control para cerrar");

            li_Leidos.Add("fin" + pilaFlowControl.Pop());
        }

        // Lee exclusivamente el contenido entre paréntesis.
        private void ConsumirParentesis(List<string> destino)
        {
            if (!HayTokens() || LexemaActual() != "(")
                throw new InvalidOperationException("Se esperaba '('");

            Avanzar();
            int profundidad = 1;

            while (HayTokens() && profundidad > 0)
            {
                string lexema = LexemaActual();
                if (lexema == "(")
                {
                    profundidad++;
                    if (destino != null) destino.Add(lexema);
                }
                else if (lexema == ")")
                {
                    profundidad--;
                    if (profundidad > 0 && destino != null) destino.Add(lexema);
                }
                else if (destino != null)
                {
                    destino.Add(lexema);
                }
                Avanzar();
            }

            if (profundidad != 0)
                throw new InvalidOperationException("Falta ')' en la expresión");
        }

        private void ReescribirInstruccionPostFijo()
        {
            bool esDeclaracion = EsTipoDeDato();
            List<string> instruccion = new List<string>();

            while (HayTokens() && LexemaActual() != ";" &&
                   LexemaActual() != "{" && LexemaActual() != "}" &&
                   TokenActual() != 220 && TokenActual() != 203)
            {
                instruccion.Add(LexemaActual());
                Avanzar();
            }

            if (HayTokens() && LexemaActual() == ";") Avanzar();
            if (instruccion.Count == 0) return;

            li_Infijo.Add(string.Join(" ", instruccion));

            string tipo = null;
            if (esDeclaracion)
            {
                tipo = instruccion[0];
                instruccion.RemoveAt(0);
            }

            List<string> resultado = ConvertirPostfijo(instruccion);
            if (tipo != null) resultado.Insert(0, tipo);
            li_Leidos.Add(string.Join(" ", resultado));
        }

        private List<string> ConvertirPostfijo(List<string> expresion)
        {
            Stack<string> operadores = new Stack<string>();
            List<string> salida = new List<string>();

            foreach (string lexema in expresion)
            {
                if (lexema == "(") operadores.Push(lexema);
                else if (lexema == ")")
                {
                    while (operadores.Count > 0 && operadores.Peek() != "(")
                        salida.Add(operadores.Pop());
                    if (operadores.Count == 0)
                        throw new InvalidOperationException("Paréntesis de cierre inesperado");
                    operadores.Pop();
                }
                else if (EsOperador(lexema))
                {
                    while (operadores.Count > 0 && operadores.Peek() != "(")
                    {
                        string superior = operadores.Peek();
                        bool extraer = lexema == "=" ?
                            Prioridad(superior) > Prioridad(lexema) :
                            Prioridad(superior) >= Prioridad(lexema);
                        if (!extraer) break;
                        salida.Add(operadores.Pop());
                    }
                    operadores.Push(lexema);
                }
                else salida.Add(lexema);
            }

            while (operadores.Count > 0)
            {
                if (operadores.Peek() == "(")
                    throw new InvalidOperationException("Paréntesis de apertura sin cerrar");
                salida.Add(operadores.Pop());
            }
            return salida;
        }
    }
}
