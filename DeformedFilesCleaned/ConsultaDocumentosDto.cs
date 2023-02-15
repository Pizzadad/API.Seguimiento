using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeformedFilesCleaned
{
    public class ConsultaDocumentosDto
    {
        public int id { get; set; }
        public string mensajesunat { get; set; }
        public byte[] xmlbinary { get; set; }
        public string rutaguardado { get; set; }
        public string error { get; set; }
    }
}
