using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ControlCambios.messages
{
    public class msgInfoImplementadores
    {
        public String tipo { get; set; }
        public String idcambio { get; set; }
        public String responsable { get; set; }
        public String usuariocrud { get; set; }
    }

    public class msgInfoImplementadoresQueryResponse
    {
        public msgInfoImplementadoresQueryResponseItem[] resultSet1 { get; set; }
    }

    public class msgInfoImplementadoresQueryResponseItem
    {
        public String id { get; set; }
        public String usuario { get; set; }

    }
}