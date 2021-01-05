using Entities;
using MediatR;
using Persistence;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.SeguimientoCRUD
{
    public class SeguimientoCreate
    {

        public class Create : IRequest
        {
            public string Rucempresa { get; set; }

            public int Tipodocumentoid { get; set; }

            public string Serie { get; set; }

            public string Correlativo { get; set; }

            public int Monedaid { get; set; }

            public decimal Importe { get; set; }
            
            public int Documentosemitidos { get; set; }
        }

        public class Handler : IRequestHandler<Create>
        {
            private readonly BdContext _bdContext;

            public Handler(BdContext bdContext)
            {
                _bdContext = bdContext;
            }
            public async Task<Unit> Handle(Create request, CancellationToken cancellationToken)
            {

                var seguimiento = new Seguimientos
                {
                    Rucempresa = request.Rucempresa,
                    Tipodocumentoid = request.Tipodocumentoid,
                    Serie = request.Serie,
                    Correlativo = request.Correlativo,
                    Monedaid = request.Monedaid,
                    Importe = request.Importe,
                    Documentosemitidos = request.Documentosemitidos,
                    Fechaemision = DateTime.Now,
                    Fechaenvio = DateTime.Now
                };

                    _bdContext.Seguimiento.Add(seguimiento);

                    var valor = await _bdContext.SaveChangesAsync();

                    if (valor > 0)
                    {
                        return Unit.Value;
                    }

                    throw new Exception("No se guardaron los cambios");
            }
        }

    }
}
