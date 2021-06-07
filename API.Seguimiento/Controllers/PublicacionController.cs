using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Application.ConsultaCPE;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Application.SeguimientoCRUD;
using Logger;
using Microsoft.Extensions.Logging;

namespace API.Seguimiento.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublicacionController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<PublicacionController> _logger;

        public PublicacionController(IMediator mediator, ILogger<PublicacionController> apiseguimientoLogger)
        {
            _mediator = mediator;
            _logger = apiseguimientoLogger;
        }

        [HttpPost("Publicar")]
        [ProducesResponseType(typeof(List<string>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult> Publicar(InsertarDatosConsultaCPECommand cmd)
        {
            try
            {
                _logger.LogInformation($"Publicar>> Cantidad: {cmd.CpeInfos.Length} | RUC: {cmd.RucEmpresa}");
                var id = await _mediator.Send(cmd);
                return Ok(id);
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
