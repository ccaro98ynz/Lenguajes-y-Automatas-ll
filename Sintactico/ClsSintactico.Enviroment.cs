using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LP1
{
    public partial class ClsSintactico:ClsPrincipal
    {
        public string lexema;
        public int token, linea, columna, indice, tokenActual;
        Stack<string> pilaControl = new Stack<string>();

        public Action<string, int, string, int, int> filaError;
        public ClsSintactico(string lexema, int token, int linea, int columna)
        {
            this.lexema = lexema;
            this.token = token;
            this.linea = linea;
            this.columna = columna;
        }
        List<ClsSintactico> li_tokens = new List<ClsSintactico>();
        private ClsTablaVariables variablesDeclaradas = new ClsTablaVariables();
        public ClsSintactico(List<ClsSintactico> li_tokens, ClsTablaVariables tablaVariables)
        {
            this.li_tokens = li_tokens; this.variablesDeclaradas = tablaVariables; indice = 0;

        }
        ClsSintactico aux;
        bool Continuar = true;
        private void Inc()
        {
            if (!Continuar) return;
            if (indice < li_tokens.Count - 1)
            {
                indice++;
                tokenActual = li_tokens[indice].token;//actualizar el token actual
                aux = li_tokens[indice];//y sus propiedades  
            }
            else { Continuar = false; return; }
        }
        int ultimoToken = 0;
        public void MostrarError(string descripcion, int tokenError)
        {
            if (ultimoToken != tokenError) filaError(aux.lexema, tokenError, descripcion, aux.linea, aux.columna);
            ultimoToken = tokenError;
            if (!Continuar) ultimoToken = 0;
        }
        bool EsTipoVariable(int token, int caso = 0)
        {
            switch (caso)
            {
                case 0://Inicio de linea
                    if (token >= 205 && token <= 208) return true;//reservadas de tipo de dato
                    else if (token >= 100 && token <= 103) return true;//identificadores y numeros
                    else if (token == 106) return true;//cadena
                    else if (token == 218 || tokenActual == 219) return true;//true or flase
                    else return false;
                default: return false;
            }
        }
        bool EsComparadorLogico(int token)
        {
            return token >= 108 && token <= 113;
        }
        bool EsCompuertaLogica(int token)
        {
            return token == 114 || token == 115;
        }
        bool EsIdentificador(int token)
        {
            return token == 100;
        }
        bool EsUnaPalabraReservada(int token)
        {
            if (diReservadas.ContainsValue(token)) return true;
            else return false;
        }
        bool EsOperadorAritmetico(int token)
        {
            if (token >= 116 && token <= 120||token==167) return true;
            else return false;
        }
        int tipoRetornoActual = 0;
        public void AnalisisSintactico()
        {
            bool existeMain = false;
            for (indice = 0; indice < li_tokens.Count; indice++)
            {
                tokenActual = li_tokens[indice].token;
                aux = li_tokens[indice];
                switch (tokenActual)
                {
                    default: MostrarError("Lexema no valido en el contexto actual", 550); break;
                    case 201:                          //Metodo Principal
                        if (existeMain)
                        {
                            MostrarError("Método principal ya definido", 551);
                        }
                        else
                        {
                            existeMain = true;
                            pilaControl.Push($"{aux.lexema},Metodo principal sin cerrar,{aux.linea},{aux.columna}");
                            ParametrosDeMetodo(true, true);
                            CuerpoDelMetodo(220);
                        }
                        break;
                    case 202:
                        Inc();
                        if (!EsIdentificador(tokenActual)) MostrarError("Identificador esperado", 552);
                        pilaControl.Push($"{aux.lexema},Cuerpo de metodo sin cerrar,{aux.linea},{aux.columna}");
                        ParametrosDeMetodo(true);
                        CuerpoDelMetodo(203);
                        break;
                    case 205:
                    case 206:
                    case 207:
                    case 208://Declaracion de Variables Globales
                        tipoRetornoActual = tipoVariable(tokenActual); 
                        Inc();
                        if (!EsIdentificador(tokenActual)) MostrarError("Identificador esperado", 552);
                        Inc();
                        switch (tokenActual)
                        {
                            case 121: //Si llega un parentesis despues del identificador
                                indice--;
                                pilaControl.Push($"{aux.lexema},Cuerpo de metodo sin cerrar,{aux.linea},{aux.columna}");

                                ParametrosDeMetodo(true);
                                CuerpoDelMetodo(223);
                                Inc();
                                if (!ResultadoEsperado()) MostrarError("El tipado del metodo no coincide con el tipo de retorno",593);
                                break;
                            case 127: ValidarIgualacion("Coma"); break;//Si llega una coma ,
                            case 107: ValidarIgualacion("Valor"); break; //Si llega el signo igual =
                            case 128: break;// Se cierra la sentencia
                            default: MostrarError("Lexema no valido en el contexto actual", 550); break;
                        }
                        break;
                }
            }
            if (pilaControl.Count > 0)
            {
                foreach (var error in pilaControl)
                {
                    if (error != null)
                    {
                        string[] x = error.Split(',');
                        filaError(x[0], ObtenerCodigoError(x[1]), x[1], int.Parse(x[2]), int.Parse(x[3]));
                    }
                }
            }
        }


        private bool ResultadoEsperado()
        {
            int tipoEsperado = tipoRetornoActual;
            tipoRetornoActual = 0;

            Stack<int> pilaTipos = new Stack<int>();
            Stack<string> pilaOperadores = new Stack<string>();

            // Cursor local: no alteramos indice durante el cálculo.
            int cursor = indice;
            bool esperaOperando = true;

            while (cursor < li_tokens.Count &&
                   li_tokens[cursor].token != 128)
            {
                var actual = li_tokens[cursor];
                int token = actual.token;
                string lexema = actual.lexema;

                if (token == 121) // (
                {
                    if (!esperaOperando)
                    {
                        MostrarError("Operador esperado", 554);
                        return false;
                    }

                    pilaOperadores.Push("(");
                }
                else if (token == 122) // )
                {
                    if (esperaOperando)
                    {
                        MostrarError("Expresion incompleta", 554);
                        return false;
                    }

                    while (pilaOperadores.Count > 0 &&
                           pilaOperadores.Peek() != "(")
                    {
                        if (!ResolverTipoRetorno(
                            pilaTipos, pilaOperadores.Pop()))
                            return false;
                    }

                    if (pilaOperadores.Count == 0)
                    {
                        MostrarError("Parentesis inesperado", 557);
                        return false;
                    }

                    pilaOperadores.Pop();
                }
                else if (EsOperadorRetorno(lexema))
                {
                    if (esperaOperando)
                    {
                        MostrarError("Falta operando", 554);
                        return false;
                    }

                    while (pilaOperadores.Count > 0 &&
                           pilaOperadores.Peek() != "(" &&
                           PrioridadRetorno(pilaOperadores.Peek()) >=
                           PrioridadRetorno(lexema))
                    {
                        if (!ResolverTipoRetorno(
                            pilaTipos, pilaOperadores.Pop()))
                            return false;
                    }

                    pilaOperadores.Push(lexema);
                    esperaOperando = true;
                }
                else
                {
                    if (!esperaOperando)
                    {
                        MostrarError("Operador esperado", 554);
                        return false;
                    }

                    int tipo = ObtenerTipoRetorno(token, lexema);

                    if (tipo == 0)
                        return false;

                    pilaTipos.Push(tipo);
                    esperaOperando = false;
                }

                cursor++;
            }

            if (cursor >= li_tokens.Count)
            {
                MostrarError("Falta fin de sentencia", 557);
                return false;
            }

            if (esperaOperando)
            {
                MostrarError("Expresion de retorno incompleta", 554);
                return false;
            }

            while (pilaOperadores.Count > 0)
            {
                string operador = pilaOperadores.Pop();

                if (operador == "(")
                {
                    MostrarError("Parentesis sin cerrar", 557);
                    return false;
                }

                if (!ResolverTipoRetorno(pilaTipos, operador))
                    return false;
            }

            if (pilaTipos.Count != 1)
            {
                MostrarError("Expresion de retorno invalida", 595);
                return false;
            }

            int tipoResultado = pilaTipos.Pop();

            // int puede convertirse a decimal.
            bool compatible =
                tipoResultado == tipoEsperado ||
                (tipoEsperado == 3 && tipoResultado == 2);

            if (!compatible)
            {
                MostrarError(
                    "El tipo del retorno no coincide con el metodo", 593);
                return false;
            }

            // Dejar el cursor en el punto y coma.
            // El for principal avanzará al siguiente token.
            indice = cursor;
            tokenActual = li_tokens[indice].token;
            aux = li_tokens[indice];

            return true;
        }

        private int ObtenerTipoRetorno(int token, string lexema)
        {
            switch (token)
            {
                case 100: // Variable
                    var variable = variablesDeclaradas.Buscar(lexema);

                    if (variable == null)
                    {
                        MostrarError("Variable no declarada", 591);
                        return 0;
                    }

                    return variable.TipoToken;

                case 101:
                    return 2; // int

                case 102:
                case 103:
                    return 3; // decimal

                case 106:
                    return 1; // string

                case 218:
                case 219:
                    return 4; // bool

                default:
                    MostrarError("Valor no valido para retorno", 554);
                    return 0;
            }
        }

        private bool EsOperadorRetorno(string operador)
        {
            return operador == "+" ||
                   operador == "-" ||
                   operador == "*" ||
                   operador == "/" ||
                   operador == "%" ||
                   operador == "==" ||
                   operador == "!=" ||
                   operador == ">" ||
                   operador == "<" ||
                   operador == ">=" ||
                   operador == "<=" ||
                   operador == "&&" ||
                   operador == "||";
        }

        private int PrioridadRetorno(string operador)
        {
            switch (operador)
            {
                case "*":
                case "/":
                case "%":
                    return 6;

                case "+":
                case "-":
                    return 5;

                case ">":
                case "<":
                case ">=":
                case "<=":
                    return 4;

                case "==":
                case "!=":
                    return 3;

                case "&&":
                    return 2;

                case "||":
                    return 1;

                default:
                    return -1;
            }
        }

        private bool ResolverTipoRetorno(
            Stack<int> pilaTipos, string operador)
        {
            if (pilaTipos.Count < 2)
            {
                MostrarError("Faltan operandos", 554);
                return false;
            }

            int derecha = pilaTipos.Pop();
            int izquierda = pilaTipos.Pop();

            bool numericos =
                (izquierda == 2 || izquierda == 3) &&
                (derecha == 2 || derecha == 3);

            switch (operador)
            {
                case "+":
                    // Concatenacion: string + string
                    if (izquierda == 1 && derecha == 1)
                    {
                        pilaTipos.Push(1);
                    }
                    else if (numericos)
                    {
                        pilaTipos.Push(
                            izquierda == 3 || derecha == 3 ? 3 : 2);
                    }
                    else
                    {
                        MostrarError("Tipos incompatibles en suma", 595);
                        return false;
                    }
                    break;

                case "-":
                case "*":
                case "/":
                case "%":
                    if (!numericos)
                    {
                        MostrarError(
                            "Operacion aritmetica incompatible", 595);
                        return false;
                    }

                    pilaTipos.Push(
                        izquierda == 3 || derecha == 3 ? 3 : 2);
                    break;

                case ">":
                case "<":
                case ">=":
                case "<=":
                    if (!numericos)
                    {
                        MostrarError(
                            "Comparacion incompatible", 595);
                        return false;
                    }

                    pilaTipos.Push(4);
                    break;

                case "==":
                case "!=":
                    if (izquierda != derecha && !numericos)
                    {
                        MostrarError(
                            "Tipos incompatibles en igualdad", 595);
                        return false;
                    }

                    pilaTipos.Push(4);
                    break;

                case "&&":
                case "||":
                    if (izquierda != 4 || derecha != 4)
                    {
                        MostrarError(
                            "Operacion logica requiere booleanos", 595);
                        return false;
                    }

                    pilaTipos.Push(4);
                    break;

                default:
                    MostrarError("Operador desconocido", 554);
                    return false;
            }

            return true;
        }


        private int ObtenerCodigoError(string descripcion)
        {
            switch (descripcion)
            {
                case "Parentesis principal sin cerrar": return 564;
                case "Grupo de parametros sin cerrar": return 565;
                case "Cuerpo de metodo sin cerrar": return 566;
                case "Condicional no cerrada": return 567;
                case "Cuerpo For sin cerrar": return 568;
                case "Cuerpo While sin cerrar": return 569;
                case "Caso sin cerrar": return 570;
                case "Cuerpo if sin cerrar": return 571;
                case "Cuerpo else sin cerrar": return 572;
                default: return 599; // Por si se cuela otro error desconocido
            }
        }
    }
}
