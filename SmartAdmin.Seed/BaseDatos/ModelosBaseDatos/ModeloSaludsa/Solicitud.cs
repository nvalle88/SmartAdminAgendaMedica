using System;
using System.Collections.Generic;

namespace SmartAdmin.Seed.ModelsSaludsa
{
    public partial class Solicitud
    {
        public Solicitud()
        {
            Diagnostico = new HashSet<Diagnostico>();
            Medicamento = new HashSet<Medicamento>();
        }

        public int Id { get; set; }
        public string Receta { get; set; }
        public string SerieFactura { get; set; }
        public string IdCliente { get; set; }
        public string NombreCliente { get; set; }
        public string NombreSucursal { get; set; }
        public string NombreOficina { get; set; }
        public string Autorizacion { get; set; }
        public string ClaveAcceso { get; set; }
        public decimal? ValorCopago { get; set; }
        public DateTime? FechaFactura { get; set; }
        public decimal? TotalFactura { get; set; }
        public string IdPrestador { get; set; }
        public int CodigoContrato { get; set; }
        public string CodigoPlan { get; set; }
        public string CodigoProducto { get; set; }
        public int CodigoPaciente { get; set; }
        public int? CodigoTitular { get; set; }
        public DateTime? FechaAtencion { get; set; }
        public DateTime? FechaExpiracion { get; set; }
        public string Estado { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string Observacion { get; set; }

        public ICollection<Diagnostico> Diagnostico { get; set; }
        public ICollection<Medicamento> Medicamento { get; set; }
    }
}
