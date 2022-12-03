using System;
using System.Collections.Generic;

namespace SmartAdmin.Seed.ModelsSaludsa
{
    public partial class Medicamento
    {
        public int Id { get; set; }
        public int IdSolicitud { get; set; }
        public string CodigoProducto { get; set; }
        public string DescripcionProducto { get; set; }
        public string PrincipioActivo { get; set; }
        public short? Cantidad { get; set; }
        public short? Prioridad { get; set; }
        public short? CantidadEntregada { get; set; }
        public short? Valor { get; set; }
        public short? Cobertura { get; set; }

        public Solicitud IdSolicitudNavigation { get; set; }
    }
}
