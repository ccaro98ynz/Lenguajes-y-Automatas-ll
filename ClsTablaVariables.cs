using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LP1
{
    public class ClsTablaVariables
    {
        Stack<string> PilaSemantica;
        private readonly List<ClsVariable> _simbolos = new List<ClsVariable>();
        public ClsTablaVariables()
        {
            PilaSemantica = new Stack<string>();
        }
        public void Push(string valor)
        {
            PilaSemantica.Push(valor);
        }
        public string Pop()
        {
            return PilaSemantica.Pop();
        }
        // Método para agregar una variable validando que no exista previamente
        public bool Agregar(string nombre, int tipoToken, int linea, int columna)
        {
            // Verificar si ya existe en la tabla (asumiendo ámbito global o plano por ahora)
            if (_simbolos.Any(s => s.Nombre == nombre))
            {
                return false; // Ya está declarada (Error semántico)
            }
            var simbolo = new ClsVariable()
            {
                Nombre = nombre,
                TipoToken = tipoToken,
                ValorGuardado = null, // Inicialmente sin valor
                Linea = linea,
                Columna = columna
            };
            _simbolos.Add(simbolo);
            return true; // Agregada con éxito
        }
        public void ActualizarValor(string nombre, string valor)
        {
            // Buscamos el índice del símbolo cuyo nombre coincida
            int indice = _simbolos.FindIndex(s => s.Nombre == nombre);

            if (indice != -1) // Si la variable sí existe en la lista
            {
                _simbolos[indice].ValorGuardado = valor;
                // Mandamos tanto el índice como el nuevo valor (o lo que necesites en el delegado)
            }
        }
        // Método para buscar si una variable existe
        public ClsVariable Buscar(string nombre)
        {
            return _simbolos.FirstOrDefault(s => s.Nombre == nombre);
        }
        // Dentro de ClsTablaVariables
        public List<ClsVariable> ObtenerSimbolos()
        {
            return _simbolos;
        }
        public void Limpiar()
        {
            _simbolos.Clear();
            PilaSemantica.Clear();
        }



    }
}
