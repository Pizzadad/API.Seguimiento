using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using API.Seguimiento.FactesolMovilNotifications;
using API.Seguimiento.Models.FactesolMovil;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace API.Seguimiento.Jobs
{
    public class FactesolMovilJob : IHostedService, IDisposable
    {
        private readonly ILogger<FactesolMovilJob> _logger;
        private readonly HttpClient _factesolMovilApi;
        private Timer _timer;

        public FactesolMovilJob(IHttpClientFactory factory, ILogger<FactesolMovilJob> logger)
        {
            _logger = logger;
            _factesolMovilApi = factory.CreateClient("FactesolMovilAPI");
        }

        private async void DoWork(object state)
        {
            try
            {
                var srtPendientes = await _factesolMovilApi.GetStringAsync("/api/v1/Jobs/ObtenerProximosVencimientos");
                var pendientes = JsonConvert.DeserializeObject<ProximosVencimientos>(srtPendientes);
                if(pendientes.HayPendientes)
                {
                    _logger.LogInformation("++++JOB Factesol Móvil: Hay pendientes: " + pendientes.Vencimientos.Count());
                    foreach (var vencimiento in pendientes.Vencimientos)
                    {
                        await PushNotificacions.NotificarEnvioComprobantes(vencimiento);
                        _logger.LogInformation("++++JOB Factesol Móvil: Notificado pendientes a: " + vencimiento.Ruc);
                    }
                }

                var strVencimiento = await _factesolMovilApi.GetStringAsync("/api/v1/Jobs/ObtenerPlanesPorRenovar");
                var vencimientosProximos = JsonConvert.DeserializeObject<List<PlanCompany>>(strVencimiento);
                if (vencimientosProximos.Any())
                {
                    _logger.LogInformation("++++JOB Factesol Móvil: Hay vencimientos servicios: " + vencimientosProximos.Count());
                    foreach (var venc in vencimientosProximos)
                    {
                        await PushNotificacions.NotificarRenovacion(venc);
                        _logger.LogInformation("++++JOB Factesol Móvil: Notificado vencimiento servicio a: " + venc.VRucnumber);
                    }
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
            }
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(DoWork, null, TimeSpan.Zero,
                TimeSpan.FromDays(1));

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
