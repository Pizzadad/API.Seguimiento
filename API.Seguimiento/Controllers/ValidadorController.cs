using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using API.Seguimiento.Extensiones;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace UBLObservacionesValidador.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValidadorController : ControllerBase
    {
        public enum TipoArchivo
        {
            FAC = 1,
            RES = 2,
            NCR = 3,
            NDB = 4,
            RET = 5,
            GRM = 6
        }

        public class ResponseValidation
        {
            public bool Success { get; set; }
            public string Resultado { get; set; }

            public string CodError
            {
                get
                {
                    if (Success) return null;
                    if (Resultado == null) return null;
                    var split = Resultado.Split("|");
                    if (split.Length == 5)
                    {
                        return split[1];
                    }

                    return null;
                }
            }

            public string Observacion
            {
                get
                {
                    if (Success) return null;
                    if (string.IsNullOrWhiteSpace(CodError)) return string.Empty;

                    var dic = CodigoRetornos.ObtenerDiccionario();
                    if (dic.ContainsKey(CodError))
                    {
                        var val = dic[CodError];
                        return val;
                    }

                    return string.Empty;
                }
            }

            public string TagErroneo
            {
                get
                {
                    if (Success) return null;
                    if (string.IsNullOrWhiteSpace(CodError)) return string.Empty;
                    var split = Resultado.Split("|");
                    if (split.Length == 5)
                    {
                        return split[3];
                    }

                    return string.Empty;
                }
            }

            public string ValorErroneo
            {
                get
                {
                    if (Success) return null;
                    if (string.IsNullOrWhiteSpace(CodError)) return string.Empty;
                    var split = Resultado.Split("|");
                    if (split.Length == 5)
                    {
                        return split[4];
                    }

                    return string.Empty;
                }
            }
        }

        private readonly ILogger<ValidadorController> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IConfiguration _configuration;

        public ValidadorController(ILogger<ValidadorController> logger, IWebHostEnvironment webHostEnvironment, IConfiguration configuration)
        {
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
            _configuration = configuration;
        }

        [HttpPost("ValidarCPEContasol")]
        [ProducesResponseType(typeof(ResponseValidation), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult ValidarCPEContasol([Required] TipoArchivo tipodoc, [Required] string nombrearchivo, [Required] byte[] archivo)
        {
            try
            {
                var validar = _configuration[$"OpcionesContasol:{tipodoc}"] == "1";
                if (validar)
                {
                    var resp = Validar(tipodoc, nombrearchivo, archivo);
                    return Ok(resp);
                }

                return Ok(new ResponseValidation
                {
                    Success = true
                });
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpPost("ValidarCPEIntegracion")]
        [ProducesResponseType(typeof(ResponseValidation), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult ValidarCPEIntegracion([Required] TipoArchivo tipodoc, [Required] string nombrearchivo, [Required] byte[] archivo)
        {
            try
            {
                var validar = _configuration[$"OpcionesIntegracion:{tipodoc}"] == "1";
                if (validar)
                {
                    var resp = Validar(tipodoc, nombrearchivo, archivo);
                    return Ok(resp);
                }

                return Ok(new ResponseValidation
                {
                    Success = true
                });
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpPost("ValidarCPE")]
        [ProducesResponseType(typeof(ResponseValidation), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult ValidarCPE([Required] TipoArchivo tipodoc, [Required] string nombrearchivo, [Required] byte[] archivo)
        {
            try
            {
                var resp = Validar(tipodoc, nombrearchivo, archivo);
                return Ok(resp);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private ResponseValidation Validar(TipoArchivo tipodoc, string nombrearchivo, byte[] archivo)
        {
            try
            {
                AppContext.SetSwitch("Switch.System.Xml.AllowDefaultResolver", true);
                var root = _webHostEnvironment.WebRootPath;
                var xslpath = Path.Combine(root, ObtenerXslPorTipoArchivo(tipodoc));
                var xmlpath = Path.Combine(root, "temp", nombrearchivo);
                var catalogospath = Path.Combine(root, "sunat_archivos/sfs/VALI/commons/cpe/catalogo/");

                System.IO.File.WriteAllBytes(xmlpath, archivo);

                var validator = new XslValidator(xslpath, catalogospath);

                using (var stream = System.IO.File.Open(xmlpath, FileMode.Open))
                {
                    var msg = validator.Validate(nombrearchivo, stream);

                    if (msg != null)
                    {
                        return new ResponseValidation()
                        {
                            Success = false,
                            Resultado = msg
                        };
                    }

                    return new ResponseValidation
                    {
                        Success = true
                    };
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private string ObtenerXslPorTipoArchivo(TipoArchivo tipodoc)
        {
            switch (tipodoc)
            {
                case TipoArchivo.FAC:
                    return "sunat_archivos/sfs/VALI/commons/xsl/validation/2.X/ValidaExprRegFactura-2.0.1.xsl";

                case TipoArchivo.RES:
                    return "sunat_archivos/sfs/VALI/commons/xsl/validation/2.X/ValidaExprRegSummary-1.1.0.xsl";

                case TipoArchivo.NCR:
                    return "sunat_archivos/sfs/VALI/commons/xsl/validation/2.X/ValidaExprRegNC-2.0.1.xsl";

                case TipoArchivo.NDB:
                    return "sunat_archivos/sfs/VALI/commons/xsl/validation/2.X/ValidaExprRegND-2.0.1.xsl";

                case TipoArchivo.GRM:
                    return "sunat_archivos/sfs/VALI/commons/xsl/validation/2.X/ValidaExprRegGuiaRemitente-2.0.1.xsl";

                case TipoArchivo.RET:
                    return "sunat_archivos/sfs/VALI/commons/xsl/validation/1.X/ValidaExprRegRetencion-1.0.3.xsl";
            }

            return null;
        }
    }
}
