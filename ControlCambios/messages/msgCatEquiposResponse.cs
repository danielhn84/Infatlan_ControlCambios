using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ControlCambios.messages
{

    public class msgCatEquipos
    {
        public String tipo { get; set; }
        public String idequipo { get; set; }
        public String nombre { get; set; }
        public String tipoequipo { get; set; }
        public String ip { get; set; }
        public String ubicacion { get; set; }
        public String estado { get; set; }
    }

    public class msgCatEquiposQueryResponse
    {
        public msgCatEquiposQueryResponseItem[] resultSet1 { get; set; }
    }
    public class msgCatEquiposQueryResponseItem
    {
        public String tipoEquipo { get; set; }
        public String idCatEquipo { get; set; }
        public String nombre { get; set; }
        public String servicio { get; set; }
        public String estado { get; set; }
        public String ip { get; set; }
        public String ubicacion { get; set; }

    }
}
