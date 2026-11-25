using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LP1
{
    public partial class ClsSintactico
    {
        private void BloqueFor()
        {
            Inc();
            if (tokenActual != 121) MostrarError("Parentesis esperado", 558);
            else
            {
                pilaControl.Push($"{aux.lexema},Parentesis sin cerrar,{aux.linea},{aux.columna}");
                Inc();
                if (tokenActual != 100) MostrarError("Variable esperada", 559);
                Inc();
                if (tokenActual != 107) MostrarError("Operador de asignacion esperado", 560);
                Inc();
                if (!EsTipoVariable(tokenActual)) MostrarError("Valor Esperado", 560);
                Inc();
                if (tokenActual != 127) MostrarError("Se esperaba una coma", 561);
                Inc();
                if (!EsTipoVariable(tokenActual)) MostrarError("Se esperaba un limite numerico", 554);
                Inc();
                if (tokenActual != 127) MostrarError("Se esperaba una coma", 561);
                Inc();
                switch (tokenActual)
                {
                    case 109: case 111: break;
                    default: MostrarError("Operador de incremento o decremento esperado", 554); break;
                }
                Inc();
                if (tokenActual != 122) MostrarError("Parametros de ciclo for sin cerrar", 562);
                else pilaControl.Pop();
                Inc();
                if (tokenActual != 123) MostrarError("Operador de apertura de bloque esperado", 563);
                else
                {
                    pilaControl.Push($"{aux.lexema},Cuerpo For sin cerrar,{aux.linea},{aux.columna}");
                    CuerpoDelMetodo(124);
                }
            }
        }
        private void BloqueWhile()
        {
            ValidarCondicional(true);
            Inc();
            if (tokenActual != 123) MostrarError("Operador de apertura de bloque esperado", 563);
            else
            {
                pilaControl.Push($"{aux.lexema},Cuerpo While sin cerrar,{aux.linea},{aux.columna}");
                CuerpoDelMetodo(124);
            }
        }
        private void BloqueSwitchCase(bool inicio = false)
        {
            if (inicio)
            {
                Inc();
                if (!EsTipoVariable(tokenActual)) MostrarError("Variable esperada", 559);
                else BloqueSwitchCase();
            }
            else
            {
                Inc();
                if (tokenActual == 123)
                {
                    bool defaultactivo = false;
                    while (tokenActual != 124 && Continuar)
                    {
                        Inc();
                        if (tokenActual == 212 || tokenActual == 213)
                        {
                            if (tokenActual == 213)
                            {
                                if (defaultactivo) MostrarError("Solo puede haber un caso default", 564);
                                else defaultactivo = true;
                            }
                            pilaControl.Push($"{aux.lexema},Caso sin cerrar,{aux.linea},{aux.columna}");
                            Inc();
                            if (EsTipoVariable(tokenActual))
                            {
                                CuerpoDelMetodo(214);
                                if (tokenActual != 214) MostrarError("Cuerpo del caso sin cerrar", 562);
                            }
                            else MostrarError("Variable esperada", 559);
                        }
                    }
                }
                else MostrarError("Operador de apertura de bloque esperado", 563);
            }
        }
        private void BloqueIf()
        {
            ValidarCondicional(true);
            Inc();
            if (tokenActual != 123) MostrarError("Operador de apertura de bloque esperado", 563);
            else
            {
                pilaControl.Push($"{aux.lexema},Cuerpo if sin cerrar,{aux.linea},{aux.columna}");
                CuerpoDelMetodo(124);
                Inc();
                if (tokenActual == 210)
                {
                    Inc();
                    if (tokenActual != 123) MostrarError("Operador de apertura de bloque esperado", 563);
                    else
                    {
                        pilaControl.Push($"{aux.lexema},Cuerpo else sin cerrar,{aux.linea},{aux.columna}");
                        CuerpoDelMetodo(124);
                    }
                }
                else indice--;
            }
        }
    }
}

