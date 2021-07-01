using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Seguimiento.Models.FactesolMovil;
using FirebaseAdmin.Messaging;

namespace API.Seguimiento.FactesolMovilNotifications
{
    public static class PushNotificacions
    {
        public static async Task<bool> NotificarEnvioComprobantes(Vencimiento data)
        {
            if (data.FacturasSinEnviar > 0)
            {
                var fmessage = new Message
                {
                    Topic = data.Ruc,
                    Notification = new Notification
                    {
                        Title = $"Acción requerida en {data.Ruc}",
                        Body = $"Tiene {data.FacturasSinEnviar} Facturas sin enviar a SUNAT. Por favor envíelas antes de que expiren."
                    }
                };
                await FirebaseMessaging.DefaultInstance.SendAsync(fmessage);
            }

            await Task.Delay(3000);

            if (data.BoletasSinEnviar > 0)
            {
                var bmessage = new Message
                {
                    Topic = data.Ruc,
                    Notification = new Notification
                    {
                        Title = $"Acción requerida en {data.Ruc}",
                        Body = $"Tiene {data.BoletasSinEnviar} Boletas sin enviar a SUNAT."
                    }
                };
                await FirebaseMessaging.DefaultInstance.SendAsync(bmessage);
            }

            return true;
        }

        public static async Task<bool> NotificarRenovacion(PlanCompany data)
        {
            var message = new Message
            {
                Topic = data.VRucnumber,
                Notification = new Notification
                {
                    Title = "Vencimiento de Servicio",
                    Body = $"Su plan adquirido para {data.VRucnumber} vence el {data.TFechaexpiracion:dd/MM/yyyy}, seleccione una renovación para continuar con el servicio."
                }
            };
            await FirebaseMessaging.DefaultInstance.SendAsync(message);

            return true;
        }

        public static async Task<bool> NotificacionGlobal(string titulo, string body)
        {
            var message = new Message
            {
                Topic = "all",
                Notification = new Notification
                {
                    Title = titulo,
                    Body = body
                }
            };
            await FirebaseMessaging.DefaultInstance.SendAsync(message);

            return true;
        }
    }
}
