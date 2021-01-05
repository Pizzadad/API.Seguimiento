using System;
using System.Collections.Generic;
using System.Text;

namespace Entities
{
    public class Seguimientos
    {
        public int Id { get; set; }

        public string Rucempresa { get; set; }

        public int Tipodocumentoid { get; set; }

        public string Serie { get; set; }

        public string Correlativo { get; set; }

        public int Monedaid { get; set; }

        public decimal Importe { get; set; }
        public DateTime Fechaemision { get; set; }

        public DateTime Fechaenvio { get; set; }

        public int Documentosemitidos { get; set; }
    }
}
