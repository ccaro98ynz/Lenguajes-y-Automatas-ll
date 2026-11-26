using System;
using System.Collections.Generic;
using System.Linq;

namespace LP1
{
    public class ClsTablaVariables
    {
        private readonly Stack<string> PilaSemantica =
            new Stack<string>();

        private readonly Stack<List<ClsVariable>> _ambitos =
            new Stack<List<ClsVariable>>();

        private readonly List<ClsVariable> _simbolos =
            new List<ClsVariable>();

        public ClsTablaVariables()
        {
            EntrarAmbito();
        }

        public void EntrarAmbito()
        {
            _ambitos.Push(new List<ClsVariable>());
        }

        public void SalirAmbito()
        {
            // No eliminar el ámbito global.
            if (_ambitos.Count <= 1) return;

            _ambitos.Pop();
        }

        public void Push(string valor)
        {
            PilaSemantica.Push(valor);
        }

        public string Pop()
        {
            return PilaSemantica.Pop();
        }

        public bool Agregar(
            string nombre,
            int tipoToken,
            int linea,
            int columna)
        {
            var ambitoActual = _ambitos.Peek();

            if (ambitoActual.Any(s => s.Nombre == nombre))
                return false;

            var simbolo = new ClsVariable
            {
                Nombre = nombre,
                TipoToken = tipoToken,
                ValorGuardado = null,
                Linea = linea,
                Columna = columna
            };

            ambitoActual.Add(simbolo);
            _simbolos.Add(simbolo);

            return true;
        }

        public ClsVariable Buscar(string nombre)
        {
            foreach (var ambito in _ambitos)
            {
                var variable = ambito.FirstOrDefault(
                    s => s.Nombre == nombre
                );

                if (variable != null)
                    return variable;
            }

            return null;
        }

        public void ActualizarValor(string nombre, string valor)
        {
            var variable = Buscar(nombre);

            if (variable == null) return;

            variable.ValorGuardado = valor;
        }

        public List<ClsVariable> ObtenerSimbolos()
        {
            return new List<ClsVariable>(_simbolos);
        }

        public void Limpiar()
        {
            _ambitos.Clear();
            _simbolos.Clear();
            PilaSemantica.Clear();

            EntrarAmbito();
        }
    }
}