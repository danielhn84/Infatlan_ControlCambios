using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ControlCambios.messages
{
    public class msgRelacionesMantenimientos
    {
        public String tipo { get; set; }
        public String subtipo { get; set; }
        public String principal { get; set; }
        public String secundario { get; set; }
    }
    public class msgRelacionesMantenimientoCreateResponse
    {
        public String updateCount1 { get; set; }
    }
}
