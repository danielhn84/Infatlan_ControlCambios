using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ControlCambios.messages
{
    public class msgInfoCalendarios
    {
        public String tipo { get; set; }
        public String idcalendario { get; set; }
        public String descripcion { get; set; }
        public String fechaventana { get; set; }
        public String ventanainicio { get; set; }
        public String ventanafin { get; set; }
        public String denegacioninicio { get; set; }
        public String denegacionfin { get; set; }
        public String usuario { get; set; }

    }
    public class msgInfoCalendariosCreateResponse
    {
        public msgInfoCalendariosCreateResponseItem[] resultSet1 { get; set; }
        public String updateCount1 { get; set; }
    }
    public class msgInfoCalendariosCreateResponseItem
    {
        public String idCalendario { get; set; }
    }

    public class msgInfoCalendariosQueryResponse
    {
        public msgInfoCalendariosQueryResponseItem[] resultSet1 { get; set; }
    }

    public class msgInfoCalendariosQueryResponseItem
    {
        public String idCalendario { get; set; }
        public String horaVentanaInicio { get; set; }
        public String horaVentanaFin { get; set; }
        public String horaDenegacionInicio { get; set; }
        public String horaDenegacionFin { get; set; }
    }
}