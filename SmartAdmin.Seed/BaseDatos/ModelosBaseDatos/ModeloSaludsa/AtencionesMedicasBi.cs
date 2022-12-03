using System;
using System.Collections.Generic;

namespace SmartAdmin.Seed.ModelsSaludsa
{
    public partial class AtencionesMedicasBi
    {
        public long IdAtencionesMedicasBi { get; set; }
        public string NombreCentroMedico { get; set; }
        public long CentroMedicoId { get; set; }
        public long Id { get; set; }
        public string Especialidad { get; set; }
        public string Canal { get; set; }
        public DateTime? Fecha { get; set; }
        public string Calificacion { get; set; }
        public string ComentarioUsuario { get; set; }
        public string Nombre { get; set; }
        public string Cedula { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public string Edad { get; set; }
        public string DetalleCie10 { get; set; }
        public string Cie10 { get; set; }
        public string TipoResolucion { get; set; }
        public string TipoPaciente { get; set; }
        public string ClasificacionSeguimiento { get; set; }
        public string TipoAtencionEfectuadaCovid { get; set; }
        public string TipoResolucionCovid { get; set; }
        public string Examenes { get; set; }
        public string Farmacia { get; set; }
        public string Zendesk { get; set; }
        public string Region { get; set; }
        public string CodigoProducto { get; set; }
        public string Codigo { get; set; }
        public string CodigoPlan { get; set; }
        public string Numero { get; set; }
        public string NumeroPersona { get; set; }
        public DateTime? FechaProceso { get; set; }
        public bool Estado { get; set; }
        public string CanalAcceso { get; set; }
    }
}
