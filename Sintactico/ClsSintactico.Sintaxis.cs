using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LP1
{
    public partial class ClsSintactico
    {
        bool EsperandoParametro = false;
        private void ParametrosDeMetodo(bool inicio = false, bool esPrincipal = false)
        {
            Inc();
            if (inicio)
            {
                if (esPrincipal)
                {
                    if (tokenActual != 121) MostrarError("Se esperaba parentesis principal", 553);
                    else
                    {
                        pilaControl.Push($"{aux.lexema},Parentesis principal sin cerrar,{aux.linea},{aux.columna}");
                        ParametrosDeMetodo();
                    }
                }
                else
                {
                    if (tokenActual != 121) MostrarError("Se esperaba parentesis", 553);
                    else
                    {
                        pilaControl.Push($"{aux.lexema},Parametros sin cerrar,{aux.linea},{aux.columna}");
                        ParametrosDeMetodo();
                    }

                }
            }
            else
            {
                if (tokenActual == 122)//Cierre de parentesis
                {
                    if (EsperandoParametro) MostrarError("Falta nueva Declaracion",588);
                    pilaControl.Pop();
                    return;
                }
                else if (EsTipoVariable(tokenActual))//Llega int bool string ?
                {
                    Inc();
                    if (tokenActual != 100) MostrarError("Identificador esperado", 552);
                    Inc();
                    EsperandoParametro = false;
                    switch (tokenActual)
                    {
                        case 122: pilaControl.Pop();break;//Cierre de parentesis
                        case 127: //Si llega coma
                            EsperandoParametro = true; 
                            ParametrosDeMetodo(); 
                            break;
                        case 107: ValidarIgualacion("Valor", true); break;//si llega el =
                        default: MostrarError("Lexema no valido en el contexto actual", 554); break;
                    }
                }
                else { MostrarError("Tipo de variable no valido", 555); }
            }
        }
        private void CuerpoDelMetodo(int final)
        {
            Inc();
            while (tokenActual != final && Continuar)
            {

                if (EsTipoVariable(tokenActual))//Si es una variable
                {
                    if (tokenActual == 100) ValidarIgualacion("Reasignacion");
                    else ValidarIgualacion();
                }
                else if (EsUnaPalabraReservada(tokenActual))//Si es una reservada del Lenguaje
                {
                    switch (tokenActual)
                    {
                        case 202:
                            MostrarError("Declaracion de metodo Invalida", 556); 
                            Inc(); 
                            continue;
                        case 209: BloqueIf(); break;//Bloque if
                        case 211: BloqueSwitchCase(true); break; //Bloque switch
                        case 215: BloqueWhile(); break; //Bloque while
                        case 216: BloqueFor(); break;//Bloque for
                        case 221: InstruccionImprimir(true); break; // Imprimir en consola
                        case 222: InstruccionLeer(); break; //Leer input
                    }
                }
                else MostrarError("Lexema no valido en el contexto actual", 554);
                Inc();
            }
            if (tokenActual == final) pilaControl.Pop();
        }
        private void InstruccionLeer()
        {
            Inc();
            if (tokenActual != 128) MostrarError("Punto y coma esperado", 555);
        }
        private void InstruccionImprimir(bool inicio = false)
        {
            if (inicio)
            {
                Inc();
                if (tokenActual != 121) MostrarError("Parentesis esperado", 558);
                else
                {
                    pilaControl.Push($"{aux.lexema},Imprimir sin cerrar,{aux.linea},{aux.columna}");
                    InstruccionImprimir();
                }
                Inc();
                if (tokenActual != 128) MostrarError("Punto y coma esperado", 555);
            }
            else
            {
                while (tokenActual != 122 && Continuar)
                {
                    Inc();
                    if (diReservadas.ContainsValue(tokenActual)) MostrarError("Lexema no valido en el contexto actual", 554);
                    else if (EsTipoVariable(tokenActual))
                    {
                        switch (tokenActual)
                        {
                            default: MostrarError("Lexema no valido en el contexto actual", 554); break;
                            case 106:
                            case 100:
                                Inc();
                                if (tokenActual == 122) { pilaControl.Pop(); return; }//si llega el parentesis de cierre
                                else if (tokenActual != 127) MostrarError("Lexema no valido en el contexto actual", 554);
                                break;
                        }
                    }
                }
                if (tokenActual == 122) pilaControl.Pop();//si llega el parentesis de cierre
            }
        }

    }
}



