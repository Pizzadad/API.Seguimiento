using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    public enum TipoDocumento
    {
        /// <summary>
        /// FACTURA
        /// </summary>
        FAC = 1,
        /// <summary>
        /// NOTA DE CRÉDITO
        /// </summary>
        NCR = 2,
        /// <summary>
        /// NOTA DE DEBITO
        /// </summary>
        NDB = 3,
        /// <summary>
        /// RETENCION
        /// </summary>
        RET = 4,
        /// <summary>
        /// GUIA DE REMISION
        /// </summary>
        GRM = 5,
        /// <summary>
        /// RESUMEN
        /// </summary>
        RES = 6
    }

    public enum EstadoSunat : byte
    {
        /// <summary>
        /// No Enviado u Ocurrio Errores en el Envio.
        /// Tiene que volver a Enviar el Documento.
        /// </summary>
        PENDIENTE,
        /// <summary>
        /// CDR Aceptado
        /// </summary>
        CDR_ACEPTADO,
        /// <summary>
        /// CDR - Aceptado pero con Observaciones
        /// </summary>
        CDR_ACEPTADO_CON_OBSERV,
        /// <summary>
        /// CDR - Rechazado (Tiene que Enviar otro Correlativo)
        /// </summary>
        CDR_RECHAZADO,
        /// <summary>
        /// Comunciacion de Baja enviado.
        /// </summary>
        CONSULTAR_BAJA,
        /// <summary>
        /// Comunciacion de Baja con CDR-ACEPTADO
        /// </summary>
        DE_BAJA_CDR_ACEPTADO,
        /// <summary>
        /// Comunciacion de Baja con (CDR-RECHAZADO).
        /// Tiene que Volver ha Enviarlo.
        /// </summary>
        DE_BAJA_CDR_RECHAZADO,
        /// <summary>
        /// Estado utilizado cuando se envia un Resumen Diario
        /// </summary>
        ENVIADO
    }
}
