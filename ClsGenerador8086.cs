using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace LP1
{
    // Codigo de tres direcciones -> ensamblador emu8086, .NET Framework 4.8.
    // Soporta int/bool (DW), str (buffer de 256 bytes, terminado en cero),
    // concatenacion de strings, PRINT de int/str, etiquetas y saltos.
    public class ClsGenerador8086
    {
        private readonly List<string> entrada;
        private int contadorEtiqueta;

        private const string Operando = "(?:\"(?:\\\\.|[^\"\\\\])*\"|\\S+)";
        private static readonly Regex Declaracion = new Regex(
            @"^DECL\s+(int|bool|str)\s+([A-Za-z_]\w*)$", RegexOptions.IgnoreCase);
        private static readonly Regex Binaria = new Regex(
            "^([A-Za-z_]\\w*)\\s*=\\s*(" + Operando +
            ")\\s+(==|!=|<=|>=|&&|\\|\\||[+*/%<>-])\\s+(" + Operando + ")$");
        private static readonly Regex Asignacion = new Regex(
            "^([A-Za-z_]\\w*)\\s*=\\s*(" + Operando + ")$");
        private static readonly Regex Identificador = new Regex(@"^[A-Za-z_]\w*$");
        private static readonly Regex Condicional = new Regex(
            @"^IF_FALSE\s+(\S+)\s+GOTO\s+([A-Za-z_]\w*)$");
        private static readonly Regex LlamadaConRetorno =
    new Regex(
        @"^([A-Za-z_]\w*)\s*=\s*CALL\s+([A-Za-z_]\w*)$"
    );

        private static readonly Regex LlamadaSinRetorno =
            new Regex(
                @"^CALL\s+([A-Za-z_]\w*)$"
            );

        private static readonly Regex Retorno =
            new Regex(
                "^RETURN\\s+(" + Operando + ")$"
            );

        public ClsGenerador8086(List<string> instrucciones)
        {
            if (instrucciones == null)
                throw new ArgumentNullException("instrucciones");
            entrada = instrucciones;
        }
        private void EmitirSaltoLinea(StringBuilder sb)
        {
            // Retorno de carro (CR)
            sb.AppendLine("    MOV DL, 13");
            sb.AppendLine("    MOV AH, 02h");
            sb.AppendLine("    INT 21h");

            // Salto de linea (LF)
            sb.AppendLine("    MOV DL, 10");
            sb.AppendLine("    MOV AH, 02h");
            sb.AppendLine("    INT 21h");
        }
        private static bool EsLiteralCadena(string valor)
        {
            return valor.Length >= 2 && valor[0] == '"' &&
                   valor[valor.Length - 1] == '"';
        }

        private static string ContenidoLiteral(string valor)
        {
            if (!EsLiteralCadena(valor))
                throw new InvalidOperationException("Literal no valido: " + valor);
            string contenido = valor.Substring(1, valor.Length - 2);
            if (contenido.Length > 255 || contenido.Any(c => c > 127))
                throw new NotSupportedException(
                    "Solo se admiten cadenas ASCII de hasta 255 caracteres.");
            return contenido;
        }

        private static bool EsEntero16(string valor, out int numero)
        {
            return int.TryParse(valor, NumberStyles.Integer,
                CultureInfo.InvariantCulture, out numero) &&
                numero >= short.MinValue && numero <= short.MaxValue;
        }

        private static void ValidarNombre(string nombre)
        {
            if (!Identificador.IsMatch(nombre))
                throw new InvalidOperationException("Nombre invalido: " + nombre);
        }

        private static string TipoOperando(string valor,
            Dictionary<string, string> tipos)
        {
            if (EsLiteralCadena(valor)) return "str";
            int numero;
            if (EsEntero16(valor, out numero)) return "int";
            if (int.TryParse(valor, out numero))
                throw new NotSupportedException("Fuera de rango de 16 bits: " + valor);
            string tipo;
            if (!tipos.TryGetValue(valor, out tipo))
                throw new InvalidOperationException(
                    "Variable/temporal no declarado: " + valor);
            return tipo;
        }

        private static bool EsNumerico(string tipo)
        {
            return tipo == "int" || tipo == "bool";
        }

        private static string TipoOperacion(string operador, string izquierda,
            string derecha, Dictionary<string, string> tipos)
        {
            string a = TipoOperando(izquierda, tipos);
            string b = TipoOperando(derecha, tipos);
            if (operador == "+" && a == "str" && b == "str")
                return "str";
            if (!EsNumerico(a) || !EsNumerico(b))
                throw new InvalidOperationException(
                    "Operacion no admitida: " + a + " " + operador + " " + b);
            if (operador == "&&" || operador == "||" ||
                operador == "==" || operador == "!=" ||
                operador == ">" || operador == "<" ||
                operador == ">=" || operador == "<=") return "bool";
            if (a != "int" || b != "int")
                throw new InvalidOperationException(
                    "Aritmetica requiere enteros: " + operador);
            return "int";
        }

        private string NuevaEtiqueta(string prefijo)
        {
            return "CI_" + prefijo + "_" + (++contadorEtiqueta);
        }

        private static string SaltoVerdadero(string op)
        {
            switch (op)
            {
                case "==": return "JE";
                case "!=": return "JNE";
                case "<": return "JL";
                case ">": return "JG";
                case "<=": return "JLE";
                case ">=": return "JGE";
                default: throw new NotSupportedException("Comparacion: " + op);
            }
        }

        private static void AgregarLiteral(string operando,
            Dictionary<string, string> literales)
        {
            if (!EsLiteralCadena(operando) || literales.ContainsKey(operando))
                return;
            ContenidoLiteral(operando);
            literales.Add(operando, "CI_STR_" + (literales.Count + 1));
        }

        private static void VerificarDestino(string destino, string tipo,
            Dictionary<string, string> tipos)
        {
            string actual;
            if (!tipos.TryGetValue(destino, out actual) || actual != tipo)
                throw new InvalidOperationException(
                    "Destino " + destino + " no es de tipo " + tipo);
        }

        private static string DireccionCadena(
    string valor,
    Dictionary<string, string> tipos,
    Dictionary<string, string> literales)
        {
            if (EsLiteralCadena(valor))
            {
                string etiqueta;

                if (!literales.TryGetValue(valor, out etiqueta))
                    throw new InvalidOperationException(
                        "Literal de cadena no registrado: " + valor);

                return etiqueta;
            }

            VerificarDestino(valor, "str", tipos);
            return valor;
        }

        // Emite copia con un limite de 255 caracteres y NUL final.
        // CX debe tener la capacidad restante (255 al empezar).
        // SI debe apuntar a la fuente y DI al destino.
        private void EmitirFragmentoCadena(StringBuilder sb)
        {
            string bucle = NuevaEtiqueta("CAT");
            string fin = bucle + "_FIN";
            sb.AppendLine(bucle + ":");
            sb.AppendLine("    MOV AL, [SI]");
            sb.AppendLine("    OR AL, AL");
            sb.AppendLine("    JZ " + fin);
            sb.AppendLine("    MOV [DI], AL");
            sb.AppendLine("    INC SI");
            sb.AppendLine("    INC DI");
            sb.AppendLine("    DEC CX");
            sb.AppendLine("    JZ " + fin);
            sb.AppendLine("    JMP " + bucle);
            sb.AppendLine(fin + ":");
        }

        private void EmitirCopiaCadena(StringBuilder sb, string destino,
            string origen, Dictionary<string, string> tipos,
            Dictionary<string, string> literales)
        {
            string fuente = DireccionCadena(origen, tipos, literales);
            if (destino.Equals(fuente, StringComparison.OrdinalIgnoreCase)) return;
            sb.AppendLine("    LEA SI, " + fuente);
            sb.AppendLine("    LEA DI, " + destino);
            sb.AppendLine("    MOV CX, 255");
            EmitirFragmentoCadena(sb);
            sb.AppendLine("    MOV BYTE PTR [DI], 0");
        }

        private void EmitirConcatenacion(StringBuilder sb, string destino,
            string izquierda, string derecha,
            Dictionary<string, string> tipos,
            Dictionary<string, string> literales)
        {
            string fuente1 = DireccionCadena(izquierda, tipos, literales);
            string fuente2 = DireccionCadena(derecha, tipos, literales);
            if (destino.Equals(fuente1, StringComparison.OrdinalIgnoreCase) ||
                destino.Equals(fuente2, StringComparison.OrdinalIgnoreCase))
                throw new NotSupportedException(
                    "Concatenacion in-place no admitida; usa temporal.");
            sb.AppendLine("    LEA DI, " + destino);
            sb.AppendLine("    MOV CX, 255");
            sb.AppendLine("    LEA SI, " + fuente1);
            EmitirFragmentoCadena(sb);
            sb.AppendLine("    LEA SI, " + fuente2);
            // Si CX esta en 0, no copiar mas caracteres.
            string fin = NuevaEtiqueta("CAT_FIN");
            sb.AppendLine("    JCXZ " + fin);
            EmitirFragmentoCadena(sb);
            sb.AppendLine(fin + ":");
            sb.AppendLine("    MOV BYTE PTR [DI], 0");
        }

        private void EmitirPrintCadena(StringBuilder sb, string origen,
            Dictionary<string, string> tipos,
            Dictionary<string, string> literales)
        {
            string fuente = DireccionCadena(origen, tipos, literales);
            string bucle = NuevaEtiqueta("PRINT");
            string fin = bucle + "_FIN";
            sb.AppendLine("    LEA SI, " + fuente);
            sb.AppendLine(bucle + ":");
            sb.AppendLine("    MOV DL, [SI]");
            sb.AppendLine("    OR DL, DL");
            sb.AppendLine("    JZ " + fin);
            sb.AppendLine("    PUSH SI");
            sb.AppendLine("    MOV AH, 02h");
            sb.AppendLine("    INT 21h");
            sb.AppendLine("    POP SI");
            sb.AppendLine("    INC SI");
            sb.AppendLine("    JMP " + bucle);
            sb.AppendLine(fin + ":");
        }

        private void EmitirPrintNumero(StringBuilder sb, string valor)
        {
            string positivo = NuevaEtiqueta("POSITIVO");
            string digitos = NuevaEtiqueta("DIGITOS");
            string mostrar = NuevaEtiqueta("MOSTRAR");
            sb.AppendLine("    MOV AX, " + valor);
            sb.AppendLine("    CMP AX, 0");
            sb.AppendLine("    JGE " + positivo);
            sb.AppendLine("    PUSH AX");
            sb.AppendLine("    MOV DL, '-'");
            sb.AppendLine("    MOV AH, 02h");
            sb.AppendLine("    INT 21h");
            sb.AppendLine("    POP AX");
            sb.AppendLine("    NEG AX");
            sb.AppendLine(positivo + ":");
            sb.AppendLine("    MOV BX, 10");
            sb.AppendLine("    XOR CX, CX");
            sb.AppendLine(digitos + ":");
            sb.AppendLine("    XOR DX, DX");
            sb.AppendLine("    DIV BX");
            sb.AppendLine("    PUSH DX");
            sb.AppendLine("    INC CX");
            sb.AppendLine("    OR AX, AX");
            sb.AppendLine("    JNZ " + digitos);
            sb.AppendLine(mostrar + ":");
            sb.AppendLine("    POP DX");
            sb.AppendLine("    ADD DL, '0'");
            sb.AppendLine("    PUSH CX");
            sb.AppendLine("    MOV AH, 02h");
            sb.AppendLine("    INT 21h");
            sb.AppendLine("    POP CX");
            sb.AppendLine("    LOOP " + mostrar);
        }

        // Recibe codigo intermedio ya optimizado. Esta clase solo genera ensamblador.
        public string Generar()
        {
            List<string> codigo = new List<string>(entrada);
            var tipos = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            var literales = new Dictionary<string, string>(StringComparer.Ordinal);
            var declaraciones = new List<string>();

            // 1. Declaraciones del usuario.
            foreach (string linea in codigo)
            {
                Match d = Declaracion.Match(linea);
                if (!d.Success) continue;
                string nombre = d.Groups[2].Value;
                string tipo = d.Groups[1].Value.ToLowerInvariant();
                if (tipos.ContainsKey(nombre))
                    throw new InvalidOperationException("Variable duplicada: " + nombre);
                tipos.Add(nombre, tipo);
            }

            // 2. Tipado de los temporales; los temporales pueden ser str.
            foreach (string linea in codigo)
            {
                Match llamada = LlamadaConRetorno.Match(linea);

                if (llamada.Success)
                {
                    string destinoLlamada = llamada.Groups[1].Value;

                    string tipoExistente;

                    if (tipos.TryGetValue(destinoLlamada, out tipoExistente)
                        && tipoExistente != "int")
                    {
                        throw new InvalidOperationException(
                            "Destino incompatible con retorno entero: " +
                            destinoLlamada
                        );
                    }

                    tipos[destinoLlamada] = "int";

                    continue;
                }
                Match b = Binaria.Match(linea);
                Match a = Asignacion.Match(linea);
                string destino = b.Success ? b.Groups[1].Value :
                    a.Success ? a.Groups[1].Value : null;
                if (destino == null) continue;
                if (b.Success)
                {
                    AgregarLiteral(b.Groups[2].Value, literales);
                    AgregarLiteral(b.Groups[4].Value, literales);
                }
                else AgregarLiteral(a.Groups[2].Value, literales);

                if (!Regex.IsMatch(destino, @"^t\d+$", RegexOptions.IgnoreCase))
                    continue;
                string inferido = b.Success
                    ? TipoOperacion(b.Groups[3].Value, b.Groups[2].Value,
                        b.Groups[4].Value, tipos)
                    : TipoOperando(a.Groups[2].Value, tipos);
                string previo;
                if (tipos.TryGetValue(destino, out previo) && previo != inferido)
                    throw new InvalidOperationException("Temporal con tipos diferentes: " + destino);
                tipos[destino] = inferido;
            }

            // 3. Registrar literales de PRINT ANTES de emitir .DATA.
            // Usamos la misma funcion que registra literales de asignaciones
            // y concatenaciones para evitar etiquetas duplicadas.
            foreach (string instruccion in codigo)
            {
                string linea = instruccion.Trim();
                if (!linea.StartsWith("PRINT ", StringComparison.Ordinal))
                    continue;

                string argumento = linea.Substring(6).Trim();
                if (EsLiteralCadena(argumento))
                    AgregarLiteral(argumento, literales);
            }

            foreach (KeyValuePair<string, string> t in tipos)
            {
                if (t.Value == "str") declaraciones.Add(t.Key + " DB 256 DUP(0)");
                else if (t.Value == "int" || t.Value == "bool")
                    declaraciones.Add(t.Key + " DW 0");
                else throw new NotSupportedException("Tipo aun no implementado: " + t.Value);
            }
            foreach (KeyValuePair<string, string> literal in literales)
            {
                string texto = ContenidoLiteral(literal.Key);
                string bytes = string.Join(", ", texto.Select(c =>
                    ((int)c).ToString(CultureInfo.InvariantCulture)));
                declaraciones.Add(literal.Value + " DB " +
                    (bytes.Length > 0 ? bytes + ", " : "") + "0");
            }

            var sb = new StringBuilder();
            sb.AppendLine(".MODEL SMALL");
            sb.AppendLine(".STACK 100h");
            sb.AppendLine(".DATA");
            foreach (string d in declaraciones) sb.AppendLine("    " + d);
            sb.AppendLine(".CODE");
            contadorEtiqueta = 0;
            bool dentroMain = false;
            bool terminoMain = false;
            string metodoAuxiliarActual = null;
            foreach (string linea in codigo)
            {
                if (Declaracion.IsMatch(linea)) continue;
                if (linea == "PROC MAIN")
                {
                    if (dentroMain || terminoMain ||
                        metodoAuxiliarActual != null)
                    {
                        throw new InvalidOperationException(
                            "MAIN duplicado o declarado dentro de otro metodo."
                        );
                    }

                    dentroMain = true;

                    sb.AppendLine("MAIN PROC");
                    sb.AppendLine("    MOV AX, @DATA");
                    sb.AppendLine("    MOV DS, AX");

                    continue;
                }
                // Finalizacion del metodo principal.
                if (linea == "ENDPROC MAIN")
                {
                    if (!dentroMain)
                        throw new InvalidOperationException("MAIN no iniciado.");

                    sb.AppendLine("    MOV AX, 4C00h");
                    sb.AppendLine("    INT 21h");
                    sb.AppendLine("MAIN ENDP");

                    dentroMain = false;
                    terminoMain = true;
                    continue;
                }

                // Inicio de un procedimiento auxiliar.
                if (linea.StartsWith("PROC ", StringComparison.Ordinal))
                {
                    if (dentroMain || metodoAuxiliarActual != null)
                    {
                        throw new InvalidOperationException(
                            "No se permiten procedimientos anidados."
                        );
                    }

                    metodoAuxiliarActual = linea.Substring(5).Trim();

                    ValidarNombre(metodoAuxiliarActual);

                    sb.AppendLine(metodoAuxiliarActual + " PROC");

                    continue;
                }

                // Finalizacion de un procedimiento auxiliar.
                if (linea.StartsWith("ENDPROC ", StringComparison.Ordinal))
                {
                    string nombreMetodo = linea.Substring(8).Trim();

                    if (metodoAuxiliarActual == null ||
                        metodoAuxiliarActual != nombreMetodo)
                    {
                        throw new InvalidOperationException(
                            "Cierre de procedimiento invalido: " +
                            nombreMetodo
                        );
                    }

                    sb.AppendLine("    RET");
                    sb.AppendLine(metodoAuxiliarActual + " ENDP");

                    metodoAuxiliarActual = null;

                    continue;
                }

                // Validar que exista un procedimiento abierto.
                if (!dentroMain && metodoAuxiliarActual == null)
                {
                    throw new InvalidOperationException(
                        "Instruccion fuera de un procedimiento: " + linea
                    );
                }
                // Retorno de un valor.
                Match retorno = Retorno.Match(linea);

                if (retorno.Success)
                {
                    if (metodoAuxiliarActual == null)
                        throw new InvalidOperationException(
                            "RETURN fuera de un metodo auxiliar."
                        );

                    string valor = retorno.Groups[1].Value;

                    string tipo = TipoOperando(valor, tipos);

                    if (tipo != "int")
                        throw new NotSupportedException(
                            "Por ahora solo se admiten retornos enteros."
                        );

                    sb.AppendLine("    MOV AX, " + valor);

                    // RET se genera al procesar ENDPROC.
                    continue;
                }

                // Invocacion con retorno.
                Match llamada = LlamadaConRetorno.Match(linea);

                if (llamada.Success)
                {
                    string destino = llamada.Groups[1].Value;
                    string metodo = llamada.Groups[2].Value;

                    ValidarNombre(metodo);
                    VerificarDestino(destino, "int", tipos);

                    sb.AppendLine("    CALL " + metodo);
                    sb.AppendLine("    MOV " + destino + ", AX");

                    continue;
                }

                // Invocacion sin retorno.
                Match llamadaVoid = LlamadaSinRetorno.Match(linea);

                if (llamadaVoid.Success)
                {
                    string metodo = llamadaVoid.Groups[1].Value;

                    ValidarNombre(metodo);

                    sb.AppendLine("    CALL " + metodo);

                    continue;
                }
                if (linea.StartsWith("LABEL ", StringComparison.Ordinal))
                {
                    string etiqueta = linea.Substring(6).Trim();
                    ValidarNombre(etiqueta);
                    sb.AppendLine(etiqueta + ":");
                    continue;
                }
                if (linea.StartsWith("GOTO ", StringComparison.Ordinal))
                {
                    string destino = linea.Substring(5).Trim();
                    ValidarNombre(destino);
                    sb.AppendLine("    JMP " + destino);
                    continue;
                }
                Match cond = Condicional.Match(linea);
                if (cond.Success)
                {
                    string condicion = cond.Groups[1].Value;
                    if (!EsNumerico(TipoOperando(condicion, tipos)))
                        throw new InvalidOperationException("IF_FALSE requiere booleano/numerico.");
                    sb.AppendLine("    MOV AX, " + condicion);
                    sb.AppendLine("    CMP AX, 0");
                    sb.AppendLine("    JE " + cond.Groups[2].Value);
                    continue;
                }
                if (linea.StartsWith("PRINT ", StringComparison.Ordinal))
                {
                    string origen = linea.Substring(6).Trim();
                    string tipo = TipoOperando(origen, tipos);

                    if (tipo == "str")
                        EmitirPrintCadena(sb, origen, tipos, literales);
                    else if (EsNumerico(tipo))
                        EmitirPrintNumero(sb, origen);
                    else
                        throw new NotSupportedException("PRINT: " + tipo);

                    EmitirSaltoLinea(sb);
                    continue;
                }
                if (linea.StartsWith("READ ", StringComparison.Ordinal))
                    throw new NotSupportedException("READ aun no implementado.");

                Match b = Binaria.Match(linea);
                if (b.Success)
                {
                    string destino = b.Groups[1].Value;
                    string izq = b.Groups[2].Value;
                    string op = b.Groups[3].Value;
                    string der = b.Groups[4].Value;
                    string tipo = TipoOperacion(op, izq, der, tipos);
                    VerificarDestino(destino, tipo, tipos);
                    if (tipo == "str")
                    {
                        EmitirConcatenacion(sb, destino, izq, der, tipos, literales);
                        continue;
                    }
                    sb.AppendLine("    MOV AX, " + izq);
                    sb.AppendLine("    MOV BX, " + der);
                    switch (op)
                    {
                        case "+": sb.AppendLine("    ADD AX, BX"); break;
                        case "-": sb.AppendLine("    SUB AX, BX"); break;
                        case "*": sb.AppendLine("    IMUL BX"); break;
                        case "/": sb.AppendLine("    CWD"); sb.AppendLine("    IDIV BX"); break;
                        case "%": sb.AppendLine("    CWD"); sb.AppendLine("    IDIV BX"); sb.AppendLine("    MOV AX, DX"); break;
                        case "&&":
                            sb.AppendLine("    CMP AX, 0");
                            sb.AppendLine("    MOV AX, 0");
                            // El resultado se normaliza a bool sin cortocircuito.
                            string no = NuevaEtiqueta("AND_NO");
                            string fin = no + "_FIN";
                            sb.AppendLine("    JE " + no);
                            sb.AppendLine("    CMP BX, 0");
                            sb.AppendLine("    JE " + no);
                            sb.AppendLine("    MOV AX, 1");
                            sb.AppendLine("    JMP " + fin);
                            sb.AppendLine(no + ":");
                            sb.AppendLine(fin + ":");
                            break;
                        case "||":
                            string si = NuevaEtiqueta("OR_SI");
                            string finOr = si + "_FIN";
                            sb.AppendLine("    CMP AX, 0");
                            sb.AppendLine("    JNE " + si);
                            sb.AppendLine("    CMP BX, 0");
                            sb.AppendLine("    JNE " + si);
                            sb.AppendLine("    MOV AX, 0");
                            sb.AppendLine("    JMP " + finOr);
                            sb.AppendLine(si + ":");
                            sb.AppendLine("    MOV AX, 1");
                            sb.AppendLine(finOr + ":");
                            break;
                        default:
                            string ok = NuevaEtiqueta("TRUE");
                            string terminar = ok + "_FIN";
                            sb.AppendLine("    CMP AX, BX");
                            sb.AppendLine("    " + SaltoVerdadero(op) + " " + ok);
                            sb.AppendLine("    MOV AX, 0");
                            sb.AppendLine("    JMP " + terminar);
                            sb.AppendLine(ok + ":");
                            sb.AppendLine("    MOV AX, 1");
                            sb.AppendLine(terminar + ":");
                            break;
                    }
                    sb.AppendLine("    MOV " + destino + ", AX");
                    continue;
                }

                Match a = Asignacion.Match(linea);
                if (a.Success)
                {
                    string destino = a.Groups[1].Value;
                    string origen = a.Groups[2].Value;
                    string tipoOrigen = TipoOperando(origen, tipos);
                    string tipoDestino;
                    if (!tipos.TryGetValue(destino, out tipoDestino))
                        throw new InvalidOperationException("Destino no declarado: " + destino);
                    if (tipoDestino == "str" && tipoOrigen == "str")
                        EmitirCopiaCadena(sb, destino, origen, tipos, literales);
                    else if (EsNumerico(tipoDestino) && EsNumerico(tipoOrigen))
                    {
                        sb.AppendLine("    MOV AX, " + origen);
                        sb.AppendLine("    MOV " + destino + ", AX");
                    }
                    else throw new InvalidOperationException(
                        "Asignacion incompatible: " + tipoDestino + " = " + tipoOrigen);
                    continue;
                }
                throw new NotSupportedException(
                    "Instruccion intermedia no implementada: " + linea);
            }
            if (!terminoMain || dentroMain)
            {
                throw new InvalidOperationException(
                    "Falta ENDPROC MAIN."
                );
            }

            if (metodoAuxiliarActual != null)
            {
                throw new InvalidOperationException(
                    "Metodo sin cerrar: " + metodoAuxiliarActual
                );
            }

            sb.AppendLine("END MAIN");

            return sb.ToString();
        }
    }
}
