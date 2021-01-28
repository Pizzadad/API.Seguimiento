using MediatR;
using Persistence;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Common;
using Newtonsoft.Json;
using Persistence.Context;
using Persistence.Domain;

namespace Application.SeguimientoCRUD
{
    public class SeguimientoCreate : IRequest<int>
    {
        /// <summary>
        /// Correlativo del documento electronico
        /// </summary>
        [MaxLength(8, ErrorMessage = "Máximo de 8 caracteres")]
        public string correlativo { get; set; }
        /// <summary>
        /// Cantidad de CPE que se incluyen en el archivo XML. Ejem: si es factura es 1, si es guia remision es 1, si es resumen va las boletas que estan dentro del resumen
        /// </summary>
        [Required(ErrorMessage = "Campo requerido", AllowEmptyStrings = false)]
        [Range(0, int.MaxValue)]
        public int documentosemitidos { get; set; }
        /// <summary>
        /// Fecha de emisión del documento electronico
        /// </summary>
        [DataType(DataType.DateTime)]
        public DateTimeOffset? fechaemision { get; set; }
        /// <summary>
        /// Importe que represetna, si es resumen o un cpe que no tenga importe dejar en blanco
        /// </summary>
        public decimal? importe { get; set; }

        /// <summary>
        /// 1=PEN, 2=USD
        /// </summary>
        [Range(0, int.MaxValue)]
        public int? monedaid { get; set; }
        /// <summary>
        /// (Opcional) Nombre del pc desde donde se emitio el cpe.
        /// </summary>
        [MaxLength(300, ErrorMessage = "Máximo de 300 caracteres")]
        public string pcname { get; set; }
        /// <summary>
        /// RUC de la empresa emisora del documento
        /// </summary>
        [Required(ErrorMessage = "Campo requerido", AllowEmptyStrings = false)]
        [MaxLength(11, ErrorMessage = "Máximo de 11 caracteres")]
        public string rucempresa { get; set; }
        /// <summary>
        /// Serie del documento electronico
        /// </summary>
        [MaxLength(4, ErrorMessage = "Máximo de 4 caracteres")]
        public string serie { get; set; }
        /// <summary>
        /// Tipo de documento electrónico.
        /// </summary>
        [Required(ErrorMessage = "Campo requerido", AllowEmptyStrings = false)]
        public TipoDocumento tipodoc { get; set; }
        /// <summary>
        /// URL del servicio web a donde se envía el XML.
        /// </summary>
        [MaxLength(500, ErrorMessage = "Máximo de 500 caracteres")]
        public string urlservice { get; set; }
        /// <summary>
        /// (Opcional) Nombre o identificador unico del usuario que ejecuta la accion de envio
        /// </summary>
        [MaxLength(250, ErrorMessage = "Máximo de 250 caracteres")]
        public string username { get; set; }
        /// <summary>
        /// Archivo XML que se esta enviando.
        /// </summary>
        [Length(1024, ErrorMessage = "Peso máximo 1MB")]
        public byte[] xmlbinary { get; set; }
        /// <summary>
        /// Estado de respuesta de Sunat/OSE.
        /// </summary>
        [Required]
        public EstadoSunat Estado { get; set; }
        /// <summary>
        /// Nombre del archivo enviado.
        /// </summary>
        public string nombrearchivo { get; set; }
        [JsonIgnore]
        public bool esdeintegracion { get; set; }
        /// <summary>
        /// Indica el tipo de servicio por el cual el cliente envio el CPE. EJEM: Contasol, Factesol Web, Factesol Movil, Integracion, etc.
        /// </summary>
        [MaxLength(150, ErrorMessage = "Máximo de 150 caracteres")]
        public string tiposervicio { get; set; }

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

                e.tipodocumentoid = request.tipodoc.ToString();
                e.fechaenvio = DateTime.UtcNow - TimeSpan.FromHours(5);
                e.estadoenvio = request.Estado.ToString();
                if ((e.nombrearchivo ?? "").Length > 150)
                    e.nombrearchivo = (e.nombrearchivo ?? "").Substring(0, 150);

                await _bdContext.seguimiento.AddAsync(e, cancellationToken);

                var valor = await _bdContext.SaveChangesAsync(cancellationToken);

                if (valor > 0)
                {
                    return e.id;
                }

                throw new Exception("No se guardaron los cambios");
            }
        }
    }
}