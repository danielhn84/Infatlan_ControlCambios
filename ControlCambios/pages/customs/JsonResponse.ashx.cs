using ControlCambios.classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace ControlCambios.pages.customs
{
    /// <summary>
    /// Descripción breve de JsonResponse
    /// </summary>
    public class JsonResponse : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            DateTime start = Convert.ToDateTime(context.Request.QueryString["start"]);
            DateTime end = Convert.ToDateTime(context.Request.QueryString["end"]);

            List<int> idList = new List<int>();
            List<ImproperCalendarEvent> tasksList = new List<ImproperCalendarEvent>();

            //Generate JSON serializable events
            foreach (CalendarEvent cevent in CalendarConnection.getEvents(start, end))
            {
                tasksList.Add(new ImproperCalendarEvent
                {
                    id = cevent.id,
                    title = cevent.title,
                    start = String.Format("{0:s}", cevent.start),
                    end = String.Format("{0:s}", cevent.end),
                    description = cevent.description,
                    allDay = cevent.allDay,
                    status = cevent.status
                });
                idList.Add(cevent.id);
            }

            context.Session["idList"] = idList;

            //Serialize events to string
            System.Web.Script.Serialization.JavaScriptSerializer oSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            string sJSON = oSerializer.Serialize(tasksList);

            //Write JSON to response object
            context.Response.Write(sJSON);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}