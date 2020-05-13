using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Punto_de_venta
{
    class Productos
    {
        private int id;
        private string nombre;
        private string descripcion;
        private string marca;
        private int precio;
        private int existencia;

        public int Id { get => id; set => id = value; }
        public string Nombre { get => nombre; set => nombre = value; }
        public string Descripcion { get => descripcion; set => descripcion = value; }
        public string Marca { get => marca; set => marca = value; }
        public decimal Precio { get => precio; set => precio = Convert.ToInt32(value); }
        //public int Precio { get => precio; set => precio = value; }
        public int Existencia { get => existencia; set => existencia = value; }
    }
}
