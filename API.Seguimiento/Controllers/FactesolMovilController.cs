using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Application.ConsultaCPE;
using Logger;
using Microsoft.Extensions.Logging;
using API.Seguimiento.FactesolMovilNotifications;

namespace API.Seguimiento.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FactesolMovilController : ControllerBase
    {
        private readonly ILogger<FactesolMovilController> _logger;

        public FactesolMovilController(IHttpClientFactory factory, ILogger<FactesolMovilController> logger)
        {
            _logger = logger;
        }

        [HttpPost("Notificar")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult> Notificar([Required] string titulo, [Required] string body)
        {
            try
            {
                _logger.LogInformation($"Notificar>> {new { titulo, body }}");
                var r = await PushNotificacions.NotificacionGlobal(titulo, body);
                return Ok(r);
            }
            catch (Exception e)
            {
                LocalExceptionLogger.EscribirLog(e);
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
