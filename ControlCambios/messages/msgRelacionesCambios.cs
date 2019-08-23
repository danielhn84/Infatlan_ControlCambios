using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ControlCambios.messages
{
    public class msgRelacionesCambios
    {
        public String tipo { get; set; }
        public String subtipo { get; set; }
        public String principal { get; set; }
        public String secundario { get; set; }
    }
    public class msgRelacionesCambiosQueryResponse
    {
        public msgRelacionesCambiosResponseItem[] resultSet1 { get; set; }
    }

    public class msgRelacionesCambiosResponseItem
    {
        public String idCambio { get; set; }
        public String idRelacion { get; set; }
        public String fechaCreacion { get; set; }
    }
}