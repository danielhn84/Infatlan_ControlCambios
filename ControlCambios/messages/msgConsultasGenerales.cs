using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ControlCambios.messages
{
    public class msgConsultasGenerales
    {
        public String tipo { get; set; }
    }
    public class msgConsultasGeneralesCalendario
    {
        public msgConsultasGeneralesCalendarioItem[] resultSet1 { get; set; }
    }
    public class msgConsultasGeneralesCalendarioItem
    {
        public String idCambio { get; set; }
        public String mantenimientoNombre { get; set; }
        public String observaciones { get; set; }
        public String FechaInicio { get; set; }
        public String FechaFinal { get; set; }
        public String TodoDia { get; set; }
        public String estado { get; set; }
    }
}