using MediatR;
using Persistence;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Common;
using Persistence.Context;
using Persistence.Domain;

namespace Application.SeguimientoCRUD
{
    public class SeguimientoCreate : IRequest<int>
    {
        [MaxLength(8, ErrorMessage = "Máximo de 8 caracteres")]
        public string correlativo { get; set; }

        [Required(ErrorMessage = "Campo requerido", AllowEmptyStrings = false)]
        [Range(0, int.MaxValue)]
        public int documentosemitidos { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? fechaemision { get; set; }

        [Required(ErrorMessage = "Campo requerido", AllowEmptyStrings = false)]
        [DataType(DataType.DateTime)]
        public DateTime fechaenvio { get; set; }

        public decimal? importe { get; set; }

        [Range(0, int.MaxValue)]
        public int? monedaid { get; set; }

        [MaxLength(300, ErrorMessage = "Máximo de 300 caracteres")]
        public string pcname { get; set; }

        [Required(ErrorMessage = "Campo requerido", AllowEmptyStrings = false)]
        [MaxLength(11, ErrorMessage = "Máximo de 11 caracteres")]
        public string rucempresa { get; set; }

        [MaxLength(4, ErrorMessage = "Máximo de 4 caracteres")]
        public string serie { get; set; }

        [MaxLength(20, ErrorMessage = "Máximo de 20 caracteres")]
        public string sysver { get; set; }

        [Required(ErrorMessage = "Campo requerido", AllowEmptyStrings = false)]
        [Range(0, int.MaxValue)]
        public int tipodocumentoid { get; set; }

        [MaxLength(500, ErrorMessage = "Máximo de 500 caracteres")]
        public string urlservice { get; set; }

        [MaxLength(250, ErrorMessage = "Máximo de 250 caracteres")]
        public string username { get; set; }

        [Length(100)]
        public byte[] xmlbinary { get; set; }

        public class Handler : IRequestHandler<SeguimientoCreate, int>
        {
            private readonly BdContext _bdContext;

            public Handler(BdContext bdContext)
            {
                _bdContext = bdContext;
            }

            public async Task<int> Handle(SeguimientoCreate request, CancellationToken cancellationToken)
            {
                var e = request.MapTo<seguimiento>();

                if (e.documentosemitidos <= 0)
                    e.documentosemitidos = 1;

                await _bdContext.seguimiento.AddAsync(e, cancellationToken);

                var valor = await _bdContext.SaveChangesAsync(cancellationToken);

                if (valor > 0)
                {
                    return valor;
                }

                throw new Exception("No se guardaron los cambios");
            }
        }
    }
}