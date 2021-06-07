using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Application.SeguimientoCRUD;
using Logger;
using Microsoft.Extensions.Logging;

namespace API.Seguimiento.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeguimientoController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<SeguimientoController> _logger;

        public SeguimientoController(IMediator mediator, ILogger<SeguimientoController> apiseguimientoLogger)
        {
            _mediator = mediator;
            _logger = apiseguimientoLogger;
        }

        [HttpPost("RegistrarSeguimiento")]
        [ProducesResponseType(typeof(int), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult> Create(SeguimientoCreate data)
        {
            try
            {
                _logger.LogInformation($"RegistrarSeguimiento>> {data.rucempresa}-{data.fechaemision}-{data.tipodoc}-{data.serie}-{data.correlativo} ({data.documentosemitidos})");
                var id = await _mediator.Send(data);
                return Ok(id);
            }
            catch (Exception e)
            {
                LocalExceptionLogger.EscribirLog(e);
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpPost("RegistrarSeguimientoIntegracion")]
        [ProducesResponseType(typeof(int), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult> CreateFromIntegracion(SeguimientoCreate data)
        {
            try
            {
                _logger.LogInformation($"RegistrarSeguimientoIntegracion>> {data.rucempresa}-{data.fechaemision}-{data.tipodoc}-{data.serie}-{data.correlativo} ({data.documentosemitidos})");
                data.esdeintegracion = true;
                var id = await _mediator.Send(data);
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
