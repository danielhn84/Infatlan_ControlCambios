using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ControlCambios.messages
{
    public class msgCatSistemasResponse
    {
        public msgCatSistemasResponseItem[] resultSet1 { get; set; }
    }

    public class msgCatSistemasResponseItem
    {
        public String idCatEquipo { get; set; }
        public String descripcion { get; set; }
        public String sistema { get; set; }
        public String idCatSistemas { get; set; }
    }
}