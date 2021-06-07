using System;
using System.Collections.Generic;

namespace Persistence.Domain
{
    public partial class seguimiento
    {
        public int id { get; set; }
        public string rucempresa { get; set; }
        public string username { get; set; }
        public string sysver { get; set; }
        public string pcname { get; set; }
        public string tipodocumentoid { get; set; }
        public string serie { get; set; }
        public string correlativo { get; set; }
        public int? monedaid { get; set; }
        public decimal? importe { get; set; }
        public DateTime? fechaemision { get; set; }
        public DateTime fechaenvio { get; set; }
        public int documentosemitidos { get; set; }
        public byte[] xmlbinary { get; set; }
        public string urlservice { get; set; }
        public string estadoenvio { get; set; }
        public string nombrearchivo { get; set; }
        public bool esdeintegracion { get; set; }
        public string tiposervicio { get; set; }
        public string mensajesunat { get; set; }
    }
}
