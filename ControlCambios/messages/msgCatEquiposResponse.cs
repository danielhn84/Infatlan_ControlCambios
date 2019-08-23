using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ControlCambios.messages
{
    public class msgCatEquiposResponse
    {
        public msgCatEquiposResponseItem[] resultSet1 { get; set; }
    }
    public class msgCatEquiposResponseItem
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
