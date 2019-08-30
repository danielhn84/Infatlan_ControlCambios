using ControlCambios.messages;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace ControlCambios.classes
{
    public class CalendarConnection
    {
        private static string connectionString = ConfigurationManager.AppSettings["DBConnString"];

        public static List<CalendarEvent> getEvents(DateTime start, DateTime end)
        {

            HttpService vConector = new HttpService();
            List<CalendarEvent> events = new List<CalendarEvent>();
            msgConsultasGenerales vConsultasGeneralesRequest = new msgConsultasGenerales()
            {
                tipo = "1"
            };

            String vResponseConsultasGenerales = "";
            HttpResponseMessage vHttpResponseConsultasGenerales = vConector.PostCalendario(vConsultasGeneralesRequest, ref vResponseConsultasGenerales);

            if (vHttpResponseConsultasGenerales.StatusCode == System.Net.HttpStatusCode.OK)
            {
                msgConsultasGeneralesCalendario vInfoConsultasGeneralesResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<msgConsultasGeneralesCalendario>(vResponseConsultasGenerales);
                if (vInfoConsultasGeneralesResponse.resultSet1.Count() > 0)
                {
                    foreach (msgConsultasGeneralesCalendarioItem item in vInfoConsultasGeneralesResponse.resultSet1)
                    {
                        events.Add(new CalendarEvent()
                        {
                            id = Convert.ToInt32(item.idCambio),
                            title = Convert.ToString(item.mantenimientoNombre),
                            description = Convert.ToString(item.observaciones),
                            start = Convert.ToDateTime(item.FechaInicio),
                            end = Convert.ToDateTime(item.FechaFinal),
                            allDay = Convert.ToBoolean(item.TodoDia),
                            status = Convert.ToInt32(item.estado)
                        });
                    }
                }
            }

            return events;
        }

        public static void updateEvent(int id, String title, String description)
        {
            
        }

        public static void updateEventTime(int id, DateTime start, DateTime end, bool allDay)
        {
            
        }

        public static void deleteEvent(int id)
        {
            
        }

        public static int addEvent(CalendarEvent cevent)
        {
            int key = 0; 
            return key;
        }
    }
}