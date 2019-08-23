using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ControlCambios.messages
{
    public class msgInfoPersonal
    {
        public String tipo { get; set; }
        public String idpersonal { get; set; }
        public String nombre { get; set; }
        public String cargo { get; set; }
        public String estado { get; set; }
        public String usuario { get; set; }
    }

    public class msgInfoPersonalCreateResponse
    {
        public msgInfoPersonalCreateResponseItem[] resultSet1 { get; set; }
        public String updateCount1 { get; set; }
    }

    public class msgInfoPersonalCreateResponseItem
    {
        public String idPersonal { get; set; }
    }

    public class msgInfoPersonalQueryResponse
    {
        public msgInfoPersonalQueryResponseItem[] resultSet1 { get; set; }
    }
    public class msgInfoPersonalQueryResponseItem
    {
        public String idPersonal { get; set; }
        public String nombre { get; set; }
        public String cargo { get; set; }
        public String estado { get; set; }
    }
}
