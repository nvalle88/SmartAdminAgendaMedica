using System;
using System.Collections.Generic;

namespace SmartAdmin.Seed.ModelsSaludsa
{
    public partial class CatalogoMedicamento
    {
        public int Id { get; set; }
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
        public string PrincipioActivo { get; set; }
        public string Laboratorio { get; set; }
        public double? Fraccion { get; set; }
        public string TipoProducto { get; set; }
        public double? Pvp { get; set; }
        public double? Pvpfraccion { get; set; }
        public string Tratamiento { get; set; }
    }
}
