using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using MediatR;
using Npgsql;
using NpgsqlTypes;

namespace Application.ConsultaCPE
{
    /// <summary>
    /// 
    /// </summary>
    public class InsertarDatosConsultaCPECommand : IRequest<List<string>>
    {
        [Required]
        public CpeInfo[] CpeInfos { get; set; }
        [Required]
        public string RucEmpresa { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class CpeInfo
    {
        public int Id { get; set; }
        public string RucEmisor { get; set; }
        public string SiglasDoc { get; set; }
        public int IdTipoCpe { get; set; }
        public string SerieCpe { get; set; }
        public int CorrelativoCpe { get; set; }
        public DateTime FechaEmisionCpe { get; set; }
        public decimal ImporteCpe { get; set; }
        public byte[] DataXml { get; set; }
        public byte[] DataPdf { get; set; }
        public byte[] DataZip { get; set; }
        public string InfoClienteRuc { get; set; }
        public string IdCpe { get; set; }
        public string IdCli { get; set; }
        public string XmlPath { get; set; }
        public string PdfPath { get; set; }
        public string CdrPath { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class InsertarDatosConsultaCPEEventHandler : IRequestHandler<InsertarDatosConsultaCPECommand, List<string>>
    {
        private static NpgsqlConnection GetNewCnn()
        {
            var cnStr = new NpgsqlConnectionStringBuilder
            {
                Host = "localhost",
                Password = "6U3q7YQf",
                Database = "consulta_factesol_cpe",
                Port = 5432,
                Username = "postgres"
            }.ConnectionString;

            return new NpgsqlConnection(cnStr);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<List<string>> Handle(InsertarDatosConsultaCPECommand request, CancellationToken cancellationToken)
        {
            try
            {
                var idVentasOk = new List<string>();
                using (var cnn = GetNewCnn())
                {
                    await cnn.OpenAsync(cancellationToken);
                    var t = await cnn.BeginTransactionAsync(cancellationToken);
                    const string qI = "insert into cpe_info   " +
                                      "(\"RucEmisor\", \"IdTipoCpe\", \"SerieCpe\", \"CorrelativoCpe\", \"FechaEmisionCpe\", \"ImporteCpe\", \"DataZip\", \"InfoClienteRuc\", \"UploadDate\") " +
                                      "values " +
                                      "(@rucEmisor, @tipocpe, @serie, @correlativo, @date, @importe, @zip, @rucCliente, current_timestamp) ";

                    foreach (var cpeInfo in request.CpeInfos)
                    {
                        try
                        {
                            if (cpeInfo.DataXml == cpeInfo.DataPdf)
                                throw new Exception("Se generaron mal los archivos.");

                            var whereQ = " where \"SerieCpe\" = '" + cpeInfo.SerieCpe + "' and \"CorrelativoCpe\" = " + cpeInfo.CorrelativoCpe + " and \"IdTipoCpe\" = " + cpeInfo.IdTipoCpe + " and \"RucEmisor\" = '" + request.RucEmpresa + "'";
                            var qC = "select count(*) from cpe_info" + whereQ;
                            var existe = (await cnn.QueryAsync<int>(qC)).FirstOrDefault() > 0;
                            if (existe)
                            {
                                var qE = "delete from cpe_info" + whereQ;
                                await cnn.ExecuteAsync(qE, transaction: t);
                            }

                            using (var cmd = new NpgsqlCommand(qI, cnn, t))
                            {
                                cmd.Parameters.Add("@rucEmisor", NpgsqlDbType.Varchar).Value = request.RucEmpresa;
                                cmd.Parameters.Add("@tipocpe", NpgsqlDbType.Integer).Value = cpeInfo.IdTipoCpe;
                                cmd.Parameters.Add("@serie", NpgsqlDbType.Varchar).Value = cpeInfo.SerieCpe;
                                cmd.Parameters.Add("@correlativo", NpgsqlDbType.Integer).Value = cpeInfo.CorrelativoCpe;
                                cmd.Parameters.Add("@date", NpgsqlDbType.Date).Value = cpeInfo.FechaEmisionCpe;
                                cmd.Parameters.Add("@importe", NpgsqlDbType.Numeric).Value = cpeInfo.ImporteCpe;
                                cmd.Parameters.Add("@zip", NpgsqlDbType.Bytea).Value = cpeInfo.DataZip;
                                cmd.Parameters.Add("@rucCliente", NpgsqlDbType.Varchar).Value = cpeInfo.InfoClienteRuc;
                                await cmd.ExecuteNonQueryAsync(cancellationToken);
                                idVentasOk.Add(cpeInfo.IdCpe);
                            }
                        }
                        catch (Exception e)
                        {
                            // ignored
                        }
                    }

                    if (cnn.State != System.Data.ConnectionState.Open) await cnn.OpenAsync(cancellationToken);
                    await t.CommitAsync(cancellationToken);
                }

                return idVentasOk;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
