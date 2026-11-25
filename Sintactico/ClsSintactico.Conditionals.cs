using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LP1
{
    public partial class ClsSintactico
    {

        private void ValidarCondicional(bool inicio = false, string estado = "")
        {
            if (inicio)
            {
                Inc();

                if (tokenActual != 121)
                {
                    MostrarError("Parentesis esperado", 558);
                    return;
                }

                pilaControl.Push(
                    aux.lexema + ",Condicional no cerrada," +
                    aux.linea + "," + aux.columna);

                Inc();
                estado = "Inicio";
            }

            int profundidad = 1;

            while (Continuar && indice < li_tokens.Count)
            {
                switch (estado)
                {
                    case "Inicio":
                    case "Operando":

                        if (tokenActual == 121)
                        {
                            pilaControl.Push(
                                aux.lexema + ",Parentesis sin cerrar," +
                                aux.linea + "," + aux.columna);

                            profundidad++;
                            Inc();
                            break;
                        }

                        if (tokenActual == 100)
                        {
                            ClsSimbolo variable =
                                variablesDeclaradas.Buscar(aux.lexema);

                            if (variable == null)
                            {
                                MostrarError("Variable inexistente", 567);
                                return;
                            }

                            scopeSemanticoActual = variable.TipoToken;
                        }
                        else if (!EsTipoVariable(tokenActual) &&
                                 tokenActual != 218 &&
                                 tokenActual != 219)
                        {
                            MostrarError("Operando esperado", 554);
                            return;
                        }

                        Inc();
                        estado = "Operador";
                        break;

                    case "Operador":

                        if (tokenActual == 122)
                        {
                            profundidad--;
                            pilaControl.Pop();

                            if (profundidad == 0)
                                return;

                            Inc();
                            break;
                        }

                        if (EsComparadorLogico(tokenActual))
                        {
                            estado = "Comparacion";
                        }
                        else if (EsOperadorAritmetico(tokenActual))
                        {
                            estado = "Aritmetica";
                        }
                        else if (EsCompuertaLogica(tokenActual))
                        {
                            estado = "Logica";
                        }
                        else
                        {
                            MostrarError(
                                "Operador esperado en la condicion", 588);
                            return;
                        }

                        break;

                    case "Comparacion":
                    case "Aritmetica":
                    case "Logica":

                        Inc();
                        estado = "Operando";
                        break;

                    default:
                        MostrarError("Estado sintactico invalido", 554);
                        return;
                }
            }

            MostrarError("Condicional sin cerrar", 557);
        }

    }
}
