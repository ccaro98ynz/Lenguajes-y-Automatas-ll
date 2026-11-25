using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LP1
{
    public partial class ClsSintactico
    {
        int tipoVariableActual = 0;
        string nombreVariableActual = "";
        int scopeSemanticoActual = 0;
        bool asignacionFlag = false;

        private int tipoVariable(int Valor)
        {
            switch (Valor)
            {
                case 205: return 1; //string
                case 206: return 2; //int
                case 207: return 3; //dec
                case 208: return 4; //bool
                default:
                    return Valor;
            }
        }
        private bool DetectarIncompatibilidadDeTipos(int tipoVariable)
        {
            switch (scopeSemanticoActual)
            {
                case 1: //string
                    if (tipoVariable == 106) return false;
                    break;
                case 2: //int
                    if (tipoVariable == 101) return false;
                    break;
                case 3: //dec
                    if (tipoVariable == 101 ||
                        tipoVariable == 102 ||
                        tipoVariable == 103) return false;
                    break;
                case 4: //bool
                    if (tipoVariable == 218 ||
                        tipoVariable == 219) return false;
                    break;
            }
            return true;
        }
        private void ValidarIgualacion(string estado = "Inicio", bool EsParametro = false)
        {
            switch (estado)
            {
                case "Reasignacion":
                    asignacionFlag = true;
                    nombreVariableActual = aux.lexema;
                    var variable = variablesDeclaradas.Buscar(nombreVariableActual);
                    if (variable == null) MostrarError("Variable no declarada", 591);
                    else scopeSemanticoActual = variable.TipoToken;
                    Inc();
                    if (tokenActual == 107) ValidarIgualacion("Valor");
                    else MostrarError("Se esperaba el signo igual", 555);
                    asignacionFlag = false;
                    break;
                case "Inicio"://Representa el inicio de la declaracion si la palabra reservada es un identificador o de tipo 
                    if (EsParametro)
                    {
                        Inc();
                        switch (tokenActual)
                        {
                            case 127: ValidarIgualacion("Coma"); break;//Si llega una coma ,
                            case 107: ValidarIgualacion("Valor"); break; //Si llega el signo igual =
                            case 128: return; ;// Se cierra la sentencia
                            default: MostrarError("Lexema no valido en el contexto actual", 554); return;
                        }
                    }
                    else
                    {
                        if (!EsTipoVariable(tokenActual)) MostrarError("Tipo de variable no valido", 556);//Lee Reservada de tipo de dato o identificador
                        tipoVariableActual = tipoVariable(tokenActual);
                        Inc();
                        if (tokenActual != 100)
                        {
                            if (diReservadas.ContainsValue(tokenActual)) MostrarError("Lexema no valido en el contexto actual", 554);
                            else MostrarError("Identificador esperado", 552);
                        }
                        else
                        {
                            nombreVariableActual = aux.lexema;
                            bool PuedeAgregarse = variablesDeclaradas.Agregar(nombreVariableActual, tipoVariableActual, aux.linea, aux.columna);
                            if (!PuedeAgregarse) MostrarError("Variable ya declarada", 590);
                            else scopeSemanticoActual = tipoVariableActual;
                        }
                        Inc();
                        switch (tokenActual)
                        {
                            case 127: ValidarIgualacion("Coma"); break;//Si llega una coma ,
                            case 107: ValidarIgualacion("Valor"); break; //Si llega el signo igual =
                            case 128: return; ;// Se cierra la sentencia
                            default: MostrarError("Lexema no valido en el contexto actual", 554); return;
                        }
                    }
                    break;
                case "Coma"://Declaracion de multiples variables del mismo tipo
                    Inc();
                    if (tokenActual != 100) MostrarError("Identificador esperado", 552);
                    else
                    {
                        nombreVariableActual = aux.lexema;
                        if (asignacionFlag)
                        {
                            var variableflag = variablesDeclaradas.Buscar(nombreVariableActual);
                            if (variableflag == null) MostrarError("Variable no declarada", 591);
                            else scopeSemanticoActual = variableflag.TipoToken;
                            Inc();
                            if (tokenActual == 107) ValidarIgualacion("Valor");
                            else MostrarError("Se esperaba el signo igual", 555);
                            return;
                        }
                        else
                        {
                            bool PuedeAgregarse2 = variablesDeclaradas.Agregar(nombreVariableActual, tipoVariableActual, aux.linea, aux.columna);
                            if (!PuedeAgregarse2) MostrarError("Variable ya declarada", 590);
                            else scopeSemanticoActual = tipoVariableActual;
                        }
                    }
                    Inc();
                    switch (tokenActual)
                    {
                        case 127: ValidarIgualacion("Coma"); break;//Si llega una coma ,
                        case 107: ValidarIgualacion("Valor"); break; //Si llega el signo igual =
                        case 128: return;// Se cierra la sentencia
                        default: MostrarError("Lexema no valido en el contexto actual", 554); return;
                    }
                    break;
                case "Valor":
                    Inc();
                    if (tokenActual == 121)//Si llega un parentesis despues del =
                    {
                        pilaControl.Push($"{aux.lexema},Parentesis sin cerrar,{aux.linea},{aux.columna}");
                        while (tokenActual != 122&&Continuar) { ValidarIgualacion("Valor"); }
                        if (tokenActual != 122) MostrarError("Estructura sin cerrar", 557);
                        else pilaControl.Pop();
                        Inc();
                        if (EsOperadorAritmetico(tokenActual)) ValidarIgualacion("Valor");
                        else if (tokenActual == 128) return;
                        else MostrarError("Lexema no valido en el contexto actual", 554);
                    }
                    else if (EsTipoVariable(tokenActual)) // si llega un tipo o variable despues del =
                    {
                        int tipoValorAsignado = 0;
                        if (tokenActual == 100) // Si es un identificador (otra variable)
                        {
                            var varDerecha = variablesDeclaradas.Buscar(aux.lexema);
                            if (varDerecha == null)
                            {
                                MostrarError("Variable no declarada en la expresión", 591);
                                return;
                            }
                            tipoValorAsignado = varDerecha.TipoToken;
                        }
                        else
                        {
                            tipoValorAsignado = tipoVariable(tokenActual);
                        }

                        if (DetectarIncompatibilidadDeTipos(tipoValorAsignado))
                        {
                            MostrarError("Error semántico: Tipos incompatibles en la asignación", 595);
                        }
                        else
                        {
                            variablesDeclaradas.ActualizarValor(nombreVariableActual, aux.lexema);
                        }
                        Inc();
                        if (tokenActual == 122) return;
                        else if (tokenActual == 127)//si llega ,
                        {
                            if (EsParametro) return;
                            else ValidarIgualacion("Coma");
                        }
                        else if (tokenActual == 128)// si llega ;
                        {
                            if (EsParametro) MostrarError("Lexema no valido en el contexto actual", 554);
                            else return;
                        }
                        else if (EsOperadorAritmetico(tokenActual)) ValidarIgualacion("Valor");
                        else { MostrarError("Lexema no valido en el contexto actual", 554); return; } // si llega otra cosa
                    }
                    else { MostrarError("Tipo de variable no valido", 556); return; }
                    break;
            }
        }

    }
}
