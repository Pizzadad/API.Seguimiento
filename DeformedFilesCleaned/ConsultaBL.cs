using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeformedFilesCleaned
{
    public class ConsultaBL
    {
        private readonly SqlConnection _connection;
        public ConsultaBL()
        {
            var cnstr = new SqlConnectionStringBuilder
            {
                DataSource = "25.72.206.48",
                UserID = "sa",
                Password = "P@$$w0rdSAMBHS",
                InitialCatalog = "DbSeguimiento"
            };
            _connection = new SqlConnection(cnstr.ConnectionString);
        }

        public async Task<List<ConsultaDocumentosDto>> ObtenerDocumentosCorruptosPorRUC(string ruc, int anio, int mes)
        {
            try
            {
                var q = "select id, xmlbinary, " +
                "mensajesunat " +
                "from seguimiento  " +
                "where  " +
               // $"rucempresa = '{ruc}' and " +
                "tipodocumentoid = 'RES' and " +
                $"year(fechaenvio) = {anio} and " +
                $"month(fechaenvio) = {mes} ";

                var data = await _connection.QueryAsync<ConsultaDocumentosDto>(q);
                return data.ToList();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
