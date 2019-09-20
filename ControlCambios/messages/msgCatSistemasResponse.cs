using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ControlCambios.messages
{

    public class msgCatSistemas
    {
        public String tipo { get; set; }
        public String parametro { get; set; }
        public String idequipo { get; set; }
        public String nombre { get; set; }
        public String descripcion { get; set; }
    }

    public class msgCatSistemasQueryResponse
    {
        public msgCatSistemasQueryResponseItem[] resultSet1 { get; set; }
    }

    public class msgCatSistemasQueryResponseItem
    {
        public String idCatEquipo { get; set; }
        public String descripcion { get; set; }
        public String sistema { get; set; }
        public String idCatSistemas { get; set; }
    }
}