using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ControlCambios.messages
{
    public class msgInfoEquipos
    {
        public String tipo { get; set; }
        public String idequipo { get; set; }
        public String nombre { get; set; }
        public String ip { get; set; }
        public String tipoequipo { get; set; }
        public String usuario { get; set; }
        public String idcatequipo { get; set; }
    }
    public class msgInfoEquiposCreateResponse
    {
        public msgInfoEquiposCreateResponseItem[] resultSet1 { get; set; }
        public String updateCount1 { get; set; }
    }

    public class msgInfoEquiposCreateResponseItem
    {
        public String idEquipo { get; set; }
    }

    public class msgInfoEquiposQueryResponse
    {
        public msgInfoEquiposQueryResponseItem[] resultSet1 { get; set; }
    }
    public class msgInfoEquiposQueryResponseItem
    {
        public String idEquipo { get; set; }
        public String nombre { get; set; }
        public String ip { get; set; }
        public String tipoEquipo { get; set; }
        public String fechaCreacion { get; set; }
        public String idUsuarioCreacion { get; set; }
        public String idCatEquipo { get; set; }
    }
}
