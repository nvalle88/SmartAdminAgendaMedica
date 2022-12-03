using System;
using System.Collections.Generic;

namespace SmartAdmin.Seed.ModelsSaludsa
{
    public partial class Banco
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Codigo { get; set; }
        public bool Estado { get; set; }
        public bool? EsDebito { get; set; }
        public bool? EsCredito { get; set; }
    }
}
