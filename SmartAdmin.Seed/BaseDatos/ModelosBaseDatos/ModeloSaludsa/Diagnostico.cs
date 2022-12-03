using System;
using System.Collections.Generic;

namespace SmartAdmin.Seed.ModelsSaludsa
{
    public partial class Diagnostico
    {
        public int Id { get; set; }
        public int IdSolicitud { get; set; }
        public string CodigoDiagnostico { get; set; }
        public string DescripcionDiagnostico { get; set; }

        public Solicitud IdSolicitudNavigation { get; set; }
    }
}
