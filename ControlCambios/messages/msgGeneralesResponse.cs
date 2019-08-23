using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ControlCambios.messages
{
    public class msgGeneralesResponse
    {
        public msgGeneralesResponseItem[] resultSet1 { get; set; }
    }

    public class msgGeneralesResponseItem
    {
        public String id { get; set; }
        public String descripcion { get; set; }
        public String estado { get; set; }
    }
}