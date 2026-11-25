using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpreadsheetLight;

namespace LP1
{
    public class ClsPrincipal
    {
        public Action<string> textofinal;
        public void Transcribir_Matriz()
        {
            SLDocument excel = new SLDocument("Matrizff.xlsx");
            Task.Factory.StartNew(() =>
            {
                int renglon = 1;
                int columna = 2;
                StringBuilder str_valor = new StringBuilder();
                string celda = "";
                str_valor.Append(" char[] indices = { ");
                List<string> li_char = new List<string>();
                do
                {
                    celda = excel.GetCellValueAsString(renglon, columna);
                    li_char.Add($"'{celda}'");
                    columna++;
                } while (!string.IsNullOrEmpty(excel.GetCellValueAsString(renglon, columna)));
                str_valor.Append(string.Join(",", li_char) + "} ;" + "\n");
                li_char.Clear();
                textofinal(str_valor.ToString());
                str_valor.Clear();
                str_valor.Append("int [,] matriz = {\n");
                columna = 2;
                renglon = 2;
                while (!string.IsNullOrEmpty(excel.GetCellValueAsString(renglon, columna)))
                {
                    str_valor.Append("{");
                    while (!string.IsNullOrEmpty(excel.GetCellValueAsString(renglon, columna)))
                    {
                        celda = excel.GetCellValueAsString(renglon, columna);
                        li_char.Add(celda);
                        columna++;
                    }
                    str_valor.Append(string.Join(",", li_char) + $"}},\n");
                    li_char.Clear();
                    columna = 2;
                    renglon++;
                }
                str_valor.Append("};");
                textofinal(str_valor.ToString());
                str_valor.Clear();
            });
        }
        //Valores de la columna 2 en adelante
        protected char[] indices = { 'e', '+', '-', '*', '/','^', '%', '=', '<', '>', '(', ')', '{', '}', '.', ':', ',', ';', '&', '|', '!', '"', '\n', '\t', ' ', '\0' };
        //Primeras 2 columnas son Letra y Digito, las siguientes son los tokens de caracteres especiales
        protected int[,] MATRIZ = {
{1,2,1,116,117,118,119,167,120,13,15,14,121,122,123,124,125,126,127,128,16,17,8,12,0,0,0,503},
{1,1,1,100,100,100,100,100,100,100,100,100,100,100,100,100,100,100,100,100,100,100,100,100,100,100,100,100},
{101,2,5,101,101,101,101,101,101,101,101,101,101,101,101,101,3,101,101,101,101,101,101,101,101,101,101,101},
{500,4,500,500,500,500,500,500,500,500,500,500,500,500,500,500,500,500,500,500,500,500,500,500,500,500,500,500},
{102,4,5,102,102,102,102,102,102,102,102,102,102,102,102,102,102,102,102,102,102,102,102,102,102,102,102,102},
{501,7,501,6,6,501,501,501,501,501,501,501,501,501,501,501,501,501,501,501,501,501,501,501,501,501,501,501},
{501,7,501,501,501,501,501,501,501,501,501,501,501,501,501,501,501,501,501,501,501,501,501,501,501,501,501,501},
{103,7,103,103,103,103,103,167,103,103,103,103,103,103,103,103,103,103,103,103,103,103,103,103,103,103,103,103},
{9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,10,9,104,9,9,9},
{9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,104,9,9,9},
{10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,11,10,10,10,10,10},
{10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,105,10,10,10,10,10},
{12,12,12,12,12,12,12,12,12,12,12,12,12,12,12,12,12,12,12,12,12,12,12,106,502,12,12,12},
{107,107,107,107,107,107,107,107,107,108,107,107,107,107,107,107,107,107,107,107,107,107,107,107,107,107,107,107},
{109,109,109,109,109,109,109,109,109,110,109,109,109,109,109,109,109,109,109,109,109,109,109,109,109,109,109,109},
{111,111,111,111,111,111,111,111,111,112,111,113,111,111,111,111,111,111,111,111,111,111,111,111,111,111,111,111},
{504,504,504,504,504,504,504,504,504,504,504,504,504,504,504,504,504,504,504,504,114,504,504,504,504,504,504,504},
{504,504,504,504,504,504,504,504,504,504,504,504,504,504,504,504,504,504,504,504,504,115,504,504,504,504,504,504},
}; protected int Get(char caracter)
        {
            if (indices.Contains(caracter))
            {
                return Array.IndexOf(indices, caracter) + 2;
            }
            else if (char.IsLetter(caracter) || caracter == '_')
            {
                return 0;
            }
            else if (char.IsDigit(caracter))
            {
                return 1;
            }
            return -1;

        }
        protected Dictionary<int, string> diErroresLexicos = new Dictionary<int, string>()
        {
            {500, "Se esperaba un numero"},
            {501, "Solo numeros como exponente"},
            {502, "Comillas sin cerrar"},
            {503, "Caracter no Reconocido"},
            {504, "Un operador logico debe concatenarse consigo mismo"},
            {505, "Lexema demasiado grande, use menos de 255 caracteres"},
        };
        protected Dictionary<string, int> diReservadas = new Dictionary<string, int>()
        {
            {"mainF",201},{"Fn",202},{"End",203 },
            {"str",205},{"int",206},{"dec",207},{"bool",208},
            {"if",209},{"else",210},
            {"compare",211},{"case",212},{"default",213 },{"fin",214},
            {"while",215},{"for",216},//while,for
            {"cutt",217},//breakloop
            {"true",218},{"false",219},
            {"mainFin",220},// Fin MainVoid
            {"show",221},{"input",222},// print, read
            {"return",223 }//PAra metodos con retorno
        };

        

    }
}
