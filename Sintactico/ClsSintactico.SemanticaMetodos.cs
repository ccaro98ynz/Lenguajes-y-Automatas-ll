using LP1.Sintactico;
using System.Collections.Generic;

namespace LP1
{
    public partial class ClsSintactico
    {
        private readonly Dictionary<string, ClsMetodo>
            metodosDeclarados = new Dictionary<string, ClsMetodo>();

        private ClsMetodo metodoActual;
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
                    if (EsperandoParametro) MostrarError("Falta nueva Declaracion", 588);
                    pilaControl.Pop();
                    return;
                }
                else if (EsTipoVariable(tokenActual))
                {
                    int tipoParametro = tipoVariable(tokenActual);

                    Inc();

                    if (tokenActual != 100)
                    {
                        MostrarError("Identificador esperado", 552);
                        return;
                    }

                    bool registrado = variablesDeclaradas.Agregar(
                        aux.lexema,
                        tipoParametro,
                        aux.linea,
                        aux.columna
                    );

                    if (!registrado)
                        MostrarError("Parametro ya declarado", 590);

                    metodoActual?.TiposParametros.Add(tipoParametro);

                    Inc();

                    EsperandoParametro = false;

                    switch (tokenActual)
                    {
                        case 122:
                            pilaControl.Pop();
                            break;

                        case 127:
                            EsperandoParametro = true;
                            ParametrosDeMetodo();
                            break;

                        case 107:
                            ValidarIgualacion("Valor", true);
                            break;

                        default:
                            MostrarError(
                                "Lexema no valido en el contexto actual", 554
                            );
                            break;
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


        private bool RegistrarMetodo(string nombre, int tipoRetorno)
        {
            if (metodosDeclarados.ContainsKey(nombre))
                return false;

            metodoActual = new ClsMetodo
            {
                Nombre = nombre,
                TipoRetorno = tipoRetorno
            };

            metodosDeclarados.Add(nombre, metodoActual);

            return true;
        }

        private ClsMetodo BuscarMetodo(string nombre)
        {
            metodosDeclarados.TryGetValue(nombre, out var metodo);

            return metodo;
        }
        private int ValidarLlamadaMetodo()
        {
            string nombre = aux.lexema;

            var metodo = BuscarMetodo(nombre);

            if (metodo == null)
            {
                MostrarError("Metodo no declarado", 591);
                return 0;
            }

            Inc();

            if (tokenActual != 121)
            {
                MostrarError("Parentesis esperado", 553);
                return 0;
            }

            Inc();

            int cantidadArgumentos = 0;

            while (Continuar && tokenActual != 122)
            {
                if (cantidadArgumentos >= metodo.TiposParametros.Count)
                {
                    MostrarError("Demasiados argumentos", 595);
                    return 0;
                }

                int tipoArgumento;

                if (tokenActual == 100 && SiguienteToken() == 121)
                {
                    tipoArgumento = ValidarLlamadaMetodo();
                }
                else
                {
                    tipoArgumento = ObtenerTipoRetorno(
                        tokenActual, aux.lexema
                    );
                }

                if (tipoArgumento == 0) return 0;

                int tipoEsperado =
                    metodo.TiposParametros[cantidadArgumentos];

                bool compatible =
                    tipoArgumento == tipoEsperado ||
                    (tipoEsperado == 3 && tipoArgumento == 2);

                if (!compatible)
                {
                    MostrarError("Tipo de argumento incompatible", 595);
                    return 0;
                }

                cantidadArgumentos++;

                Inc();

                if (tokenActual == 122) break;

                if (tokenActual != 127)
                {
                    MostrarError("Coma o parentesis esperado", 554);
                    return 0;
                }

                Inc();

                if (tokenActual == 122)
                {
                    MostrarError("Argumento esperado", 554);
                    return 0;
                }
            }

            if (tokenActual != 122 || !Continuar)
            {
                MostrarError("Parametros sin cerrar", 557);
                return 0;
            }

            if (cantidadArgumentos != metodo.TiposParametros.Count)
            {
                MostrarError("Cantidad de argumentos incorrecta", 595);
                return 0;
            }

            return metodo.TipoRetorno;
        }
    }
}