using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Entities;
using Application.SeguimientoCRUD;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Seguimiento.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeguimientoController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SeguimientoController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<List<Seguimientos>>> Get()
        {
            return await _mediator.Send(new SeguimientoGetAll.GetAll());
        }

        [HttpPost]
        public async Task<ActionResult<Unit>> Create(SeguimientoCreate.Create data)
        {
            return await _mediator.Send(data);
        }
    }
}
