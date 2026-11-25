using DocumentFormat.OpenXml.ExtendedProperties;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace LP1
{
    internal class ClsLexico : ClsPrincipal
    {
        public Action<string, int, int, int> filaToken;
        public Action<string, int, string, int, int> filaError;
        char caracter = '\0';
        StringBuilder Lexema; int indice = 0;
        int renglon = 0, columna = 0;
        bool Reescribir = false;
        char[] Nulos = { '\n', '\t', '\0', ' ' };
        List<ClsSintactico>li_tokens = new List<ClsSintactico>();
        public void AnalisisLexico(string argumento)
        {
            Lexema = new StringBuilder();
            int estado = 0, colmtx = 0;
            renglon = 1; columna = 0;
            for (indice = 0; indice < argumento.Length; indice++)
            {
                caracter = argumento[indice];
                columna++;
                colmtx = Get(caracter);
                if (colmtx < 0)//Caracter no reconocido
                {
                    ManejarCaracterInvalido(estado);
                }
                else
                {
                    estado = MATRIZ[estado, colmtx];
                    ValidarLexema(estado);
                    if (caracter == '\n')
                    {
                        renglon++;
                        columna = 0;
                    }
                    if (Reescribir)
                    {
                        estado = 0;
                    }
                }
            }
            if (Lexema.Length > 0)//Si queda un lexema por procesar
            {
                estado = MATRIZ[estado, Get(' ')];//Un espacio en blanco para forzar el estado de aceptacion
                if (estado == 12)//Significa que no se ha cerrado una cadena
                {
                    filaError(Lexema.ToString(), 502, diErroresLexicos[502], renglon, columna);
                }
                else
                {
                    columna++;//Aumentar columna para anular el retroceso del metodo AggToken
                    ValidarLexema(estado);
                }
                Lexema.Clear();
            }
        }
        private void ValidarLexema(int Token)
        {
            Reescribir = true;
            if (Token == 0) return;//Ignorar espacios en blanco
            else if (Token >= 100 && Token < 500)
            {
                if (Lexema.Length > 0)
                {
                    if (Token == 104 || Token == 105)//Ignorar Parrafos o comentarios
                    {
                        Lexema.Clear();
                        return;
                    }
                    else if (Lexema.Length >= 256)//Error de longitud
                    {
                        filaError(Lexema.ToString(), 505, diErroresLexicos[505], renglon, columna);
                        Lexema.Clear();
                    }
                    else
                    {
                        AgregarToken(Token);
                    }
                }
                else
                {
                    filaToken($"{caracter}", Token, renglon, columna);//Si no hay lexema, es un token simple
                    li_tokens.Add(new ClsSintactico($"{caracter}", Token, renglon, columna));
                }
            }
            else if (Token >= 500)
            {
               AgregarError(Token);
            }
            else
            {
                Lexema.Append(caracter);
                Reescribir = false;
            }
        }
        private void AgregarToken(int Token)
        {
            //Manejo de operadores que requieren 2 caracteres
            //Cuando llega el Token 106 quiere decir que se cierra una cadena
            if (Token == 108 || Token == 110 || Token == 112 || Token == 113 || Token == 106||Token==114||Token==115)
            {
                Lexema.Append(caracter);
                filaToken(Lexema.ToString(), Token, renglon, columna);
                li_tokens.Add(new ClsSintactico(Lexema.ToString(),Token,renglon,columna));
                Lexema.Clear();
            }
            else
            {
                columna--;//si existe un lexema, el estado de aceptacion se da en el siguiente caracter (O.C)
                if (diReservadas.ContainsKey(Lexema.ToString()))
                {
                    filaToken(Lexema.ToString(), diReservadas[Lexema.ToString()], renglon, columna);
                    li_tokens.Add(new ClsSintactico(Lexema.ToString(), diReservadas[Lexema.ToString()], renglon, columna));
                }
                else
                {
                    filaToken(Lexema.ToString(), Token, renglon, columna);
                    li_tokens.Add(new ClsSintactico(Lexema.ToString(), Token, renglon, columna));
                }
                Lexema.Clear();
                if (Nulos.Contains(caracter)) columna++;
                else if(indice>0)indice--;//Si no es un espacio en blanco, retroceder el indice
            }
        }
        private void AgregarError(int Token)
        {
            Lexema.Append(caracter);//Agregar el caracter que activa el estado de error
            if (diErroresLexicos.ContainsKey(Token))
            {
                filaError(Lexema.ToString(), Token, diErroresLexicos[Token], renglon, columna);
            }
            else
            {
                filaError(Lexema.ToString(), Token, $"Error desconocido {Token}", renglon, columna);
            }
            Lexema.Clear();
        }
        private void ManejarCaracterInvalido(int Estado)
        {
            if (Estado >= 8 && Estado <= 12)//Dentro de un comentario o cadena no hay problema
            {
                Lexema.Append(caracter);
            }
            else
            {
                filaError($"{caracter}", 503, diErroresLexicos[503], renglon, columna);
            }
        }
        public void ListadeTokens(out List<ClsSintactico> lista)
        {
            lista = li_tokens;
        }
    }
}

