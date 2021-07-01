using Newtonsoft.Json;

namespace API.Seguimiento.Models.FactesolMovil
{
    public partial class ProximosVencimientos
    {
        [JsonProperty("HayPendientes")]
        public bool HayPendientes { get; set; }

        [JsonProperty("Vencimientos")]
        public Vencimiento[] Vencimientos { get; set; }
    }

    public partial class Vencimiento
    {
        [JsonProperty("Ruc")]
        public string Ruc { get; set; }

        [JsonProperty("RazSocial")]
        public string RazSocial { get; set; }

        [JsonProperty("BoletasSinEnviar")]
        public long BoletasSinEnviar { get; set; }

        [JsonProperty("FacturasSinEnviar")]
        public long FacturasSinEnviar { get; set; }
    }
}