using System;
using Newtonsoft.Json;

namespace API.Seguimiento.Models.FactesolMovil
{
    public partial class PlanCompany
    {
        [JsonProperty("i_plancompany")]
        public long IPlancompany { get; set; }

        [JsonProperty("v_rucnumber")]
        public string VRucnumber { get; set; }

        [JsonProperty("i_planid")]
        public long IPlanid { get; set; }

        [JsonProperty("t_fechasolicitud")]
        public string TFechasolicitud { get; set; }

        [JsonProperty("t_fechaexpiracion")]
        public DateTimeOffset TFechaexpiracion { get; set; }
    }
}
