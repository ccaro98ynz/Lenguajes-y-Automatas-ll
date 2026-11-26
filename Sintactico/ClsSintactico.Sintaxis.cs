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
                        Inc();
                        if (tokenActual == 122) { pilaControl.Pop(); return; }//si llega el parentesis de cierre
                        else if (tokenActual != 127) MostrarError("Lexema no valido en el contexto actual", 554);
                        break;
                    }
                }
                if (tokenActual == 122) pilaControl.Pop();//si llega el parentesis de cierre
            }
        }

    }
}



